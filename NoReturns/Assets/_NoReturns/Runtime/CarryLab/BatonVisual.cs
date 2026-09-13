using UnityEngine;
namespace NoReturns.CarryLab {
// Pure presentation: the existing host-owned shove cooldown drives both peers.
public sealed class BatonVisual {
    readonly GameObject[] roots=new GameObject[4];
    readonly BatonFeedback[] feedback=new BatonFeedback[4];
    public void Display(Camera eye, CharacterController[] crew, int local, int occupiedMask, bool active,
        int cargoHolder, int beaconHolder, ThreatState danger, float[] yaws) {
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
                Object.Destroy(glove.GetComponent<Collider>());glove.transform.SetParent(roots[i].transform,false);
                glove.transform.localPosition=new Vector3(0,0,-.018f);glove.transform.localScale=new Vector3(.085f,.105f,.09f);
                glove.GetComponent<Renderer>().sharedMaterial=CarryWorld.Mat(new Color(.15f,.12f,.1f));
                feedback[i]=new BatonFeedback(roots[i].transform);
            }
            bool down=danger!=null&&danger.IsDown(i);
            float rescue=danger==null?0:danger.RescueAt(i);
            bool show=active&&((occupiedMask&(1<<i))!=0)&&cargoHolder!=i&&beaconHolder!=i&&!down&&rescue<=0;
            roots[i].SetActive(show);if(!show)continue;
            float cooldown=danger==null?0:danger.CooldownAt(i);
            feedback[i].Display(cooldown);
            float elapsed=6-cooldown;
            // Immediate forward beat matches the immediate gameplay contact; smooth return.
            float strike=cooldown>5.5f?Mathf.Pow(1-Mathf.Clamp01(elapsed/.5f),2):0;
            if(i==local) {
                var rotation=eye.transform.rotation;
                var offset=new Vector3(.27f-.13f*strike,-.34f+.12f*strike,.46f+.18f*strike);
                // Pull the visual in at nearby solid surfaces instead of rendering through them.
                float reach=1;
                foreach(var hit in Physics.RaycastAll(eye.transform.position,rotation*Vector3.forward,.95f))
                    if(hit.collider!=crew[i]&&!hit.collider.isTrigger)reach=Mathf.Min(reach,Mathf.Clamp(hit.distance/.95f,.35f,1));
                roots[i].transform.SetPositionAndRotation(eye.transform.position+rotation*(offset*reach),rotation*Quaternion.Euler(-12-65*strike,180,-18+35*strike));
                roots[i].transform.localScale=Vector3.one*reach;
            } else {
                var rotation=Quaternion.Euler(0,yaws[i],0);
                roots[i].transform.SetPositionAndRotation(crew[i].transform.position+rotation*new Vector3(.32f,1.12f,.28f+.25f*strike),rotation*Quaternion.Euler(45+40*strike,180,-12));
                roots[i].transform.localScale=Vector3.one;
            }
        }
    }
}
}
