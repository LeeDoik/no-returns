using UnityEngine;
namespace NoReturns.CarryLab {
// Pure presentation: the existing host-owned shove cooldown drives both peers.
public sealed class BatonVisual {
    readonly GameObject[] roots=new GameObject[4];
    readonly BatonFeedback[] feedback=new BatonFeedback[4];
    readonly GameObject[] gloves=new GameObject[4];
    readonly bool[] attached=new bool[4];
    readonly float[] handDistance=new float[4];
    [System.Serializable] public class State {public bool visible,handAttached;public float handDistance;public Vector3 position;}
    public State Capture(int slot)=>new State{visible=roots[slot]&&roots[slot].activeSelf,handAttached=attached[slot],handDistance=handDistance[slot],position=roots[slot]?roots[slot].transform.position:Vector3.zero};
    public void Display(Camera eye, CharacterController[] crew, int local, int occupiedMask, bool active,
        int cargoHolder, int beaconHolder, ThreatState danger, float[] yaws, EmployeeVisual[] employees) {
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
            if(employees[i])employees[i].BatonPose(cooldown,show,Time.deltaTime);
            roots[i].SetActive(show);attached[i]=false;handDistance[i]=0;if(!show)continue;
            gloves[i].SetActive(i==local||!employees[i]);
            feedback[i].Display(cooldown);
            EmployeeVisual.BatonMotion(employees[i]?employees[i].BatonMotionCooldown:cooldown,out var motion,out var motionRotation,out _);
            if(i==local) {
                var rotation=eye.transform.rotation;
                var offset=new Vector3(.27f,-.34f,.46f)+(motion-EmployeeVisual.BatonReady)*.65f;
                // Pull the visual in at nearby solid surfaces instead of rendering through them.
                float reach=1;
                foreach(var hit in Physics.RaycastAll(eye.transform.position,rotation*Vector3.forward,.95f))
                    if(hit.collider!=crew[i]&&!hit.collider.isTrigger)reach=Mathf.Min(reach,Mathf.Clamp(hit.distance/.95f,.35f,1));
                var swingRotation=motionRotation*Quaternion.Inverse(EmployeeVisual.BatonReadyRotation);
                roots[i].transform.SetPositionAndRotation(eye.transform.position+rotation*(offset*reach),rotation*swingRotation*Quaternion.Euler(-12,180,-18));
                roots[i].transform.localScale=Vector3.one*reach;
            } else if(employees[i]&&employees[i].BatonGrip(out var grip,out var handRotation)) {
                roots[i].transform.SetPositionAndRotation(grip,handRotation);
                roots[i].transform.localScale=Vector3.one;attached[i]=true;
                handDistance[i]=Vector3.Distance(roots[i].transform.position,employees[i].RightHandPosition);
            } else {
                var rotation=Quaternion.Euler(0,yaws[i],0);
                roots[i].transform.SetPositionAndRotation(crew[i].transform.position+rotation*motion,rotation*motionRotation);
                roots[i].transform.localScale=Vector3.one;
            }
        }
    }
}
}
