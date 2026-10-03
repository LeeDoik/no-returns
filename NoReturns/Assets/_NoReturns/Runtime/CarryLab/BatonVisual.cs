using UnityEngine;
namespace NoReturns.CarryLab {
// Pure presentation: the existing host-owned shove cooldown drives both peers.
public sealed class BatonVisual {
    public static float Strike(float cooldown)=>BatonMotion.Amount(cooldown);
    FirstPersonArms view;float clearance=1;
    public void Dispose(Camera eye)=>view?.Dispose(eye);
    public FirstPersonArms.State Hands=>view?.Capture();
    readonly GameObject[] roots=new GameObject[4];
    readonly BatonFeedback[] feedback=new BatonFeedback[4];
    readonly GameObject[] gloves=new GameObject[4];
    readonly bool[] attached=new bool[4];
    readonly float[] handDistance=new float[4];
    [System.Serializable] public class State {public bool visible,handAttached;public float handDistance;public Vector3 position,axis;}
    public State Capture(int slot)=>new State{visible=roots[slot]&&roots[slot].activeSelf,handAttached=attached[slot],handDistance=handDistance[slot],position=roots[slot]?roots[slot].transform.position:Vector3.zero,axis=roots[slot]?roots[slot].transform.up:Vector3.up};
    public void Display(Camera eye, CharacterController[] crew, int local, int occupiedMask, bool active,
        int cargoHolder, int beaconHolder, ThreatState danger, float[] yaws, EmployeeVisual[] employees,BoxCollider parcel,Transform beacon=null) {
        if(view==null)view=new FirstPersonArms(eye);
        for(int i=0;i<4;i++) {
            if(!roots[i]) {
                var model=Resources.Load<GameObject>("PSXKit01/Baton");if(!model)return;
                roots[i]=new GameObject("Shock baton employee "+i);
                var art=Object.Instantiate(model,roots[i].transform);
                var bounds=new Bounds();bool first=true;
                foreach(var r in art.GetComponentsInChildren<Renderer>()){if(first){bounds=r.bounds;first=false;}else bounds.Encapsulate(r.bounds);}
                // Preserve proportions; orient the long axis up and grip below its midpoint.
                float scale=.68f/bounds.size.y;art.transform.localScale*=scale;
                art.transform.localPosition=-bounds.center*scale+Vector3.up*.18f;
                var glove=GameObject.CreatePrimitive(PrimitiveType.Cube);glove.name="Baton glove";
                gloves[i]=glove;
                Object.Destroy(glove.GetComponent<Collider>());glove.transform.SetParent(roots[i].transform,false);
                glove.transform.localPosition=new Vector3(0,0,-.018f);glove.transform.localScale=new Vector3(.085f,.105f,.09f);
                glove.GetComponent<Renderer>().sharedMaterial=CarryWorld.Mat(new Color(.15f,.12f,.1f));
                feedback[i]=new BatonFeedback(roots[i].transform);
            }
            bool down=danger!=null&&danger.IsDown(i);
            float rescue=danger==null?0:danger.RescueAt(i);
            bool show=active&&((occupiedMask&(1<<i))!=0)&&cargoHolder!=i&&beaconHolder!=i&&!down&&rescue<=0;
            float cooldown=danger==null?0:danger.CooldownAt(i);
            if(employees[i])employees[i].AttackPose(cooldown,show);
            roots[i].SetActive(show);attached[i]=false;handDistance[i]=0;if(!show)continue;
            gloves[i].SetActive(i==local?!view.Capture().ready:!employees[i]);
            feedback[i].Display(cooldown);
            float motionCooldown=employees[i]?employees[i].BatonMotionCooldown:cooldown;
            float strike=Strike(motionCooldown);
            if(i==local) {
                var rotation=eye.transform.rotation;
                BatonMotion.FirstPerson(motionCooldown,out var offset,out var heldRotation);
                float reach=1,tip=offset.z+Mathf.Max(0,(heldRotation*Vector3.up).z)*.56f+.06f;
                foreach(var hit in Physics.RaycastAll(eye.transform.position,rotation*Vector3.forward,tip,~0,QueryTriggerInteraction.Ignore))
                    if(hit.collider!=crew[i]&&!hit.collider.attachedRigidbody)reach=Mathf.Min(reach,Mathf.Clamp((hit.distance-.04f)/tip,.35f,1));
                clearance=reach;
                roots[i].transform.SetPositionAndRotation(eye.transform.position+rotation*new Vector3(offset.x,offset.y,offset.z*reach),rotation*heldRotation);
                roots[i].transform.localScale=Vector3.one;
                FirstPersonArms.SetLayer(roots[i],FirstPersonArms.Layer);
            } else if(employees[i]&&employees[i].BatonGrip(out var grip,out var handRotation)) {
                roots[i].transform.SetPositionAndRotation(grip,handRotation);
                roots[i].transform.localScale=Vector3.one;attached[i]=true;
                handDistance[i]=Vector3.Distance(roots[i].transform.position,employees[i].RightHandPosition);
            } else {
                var rotation=Quaternion.Euler(0,yaws[i],0);
                roots[i].transform.SetPositionAndRotation(crew[i].transform.position+rotation*new Vector3(.32f,1.12f,.28f+.25f*strike),rotation*Quaternion.Euler(45+40*strike,180,-12));
                roots[i].transform.localScale=Vector3.one;
            }
        }
        bool blocked=danger!=null&&(danger.IsDown(local)||danger.RescueAt(local)>0);
        view.Display(eye,roots[local].transform,roots[local].activeSelf,active&&!blocked&&cargoHolder==local,parcel,clearance,active&&danger!=null&&!danger.IsDown(local)&&cargoHolder!=local&&beaconHolder!=local?danger.RescueAt(local):0,!blocked?beacon:null);
    }
}
}
