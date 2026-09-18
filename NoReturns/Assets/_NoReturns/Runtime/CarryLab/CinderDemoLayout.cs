using UnityEngine;
namespace NoReturns.CarryLab {
// Scene profile: legacy CarryRoom retains its own coordinates when this is absent.
public sealed class CinderDemoLayout : MonoBehaviour {
    public static bool Active {get;private set;}
    static Transform ship;
    public static Vector3 Reception => new Vector3(32.7f,0,15);
    public static Vector3 ReceiptOffset => Active ? Reception-new Vector3(-6,0,9) : Vector3.zero;
    public static void Initialize(){Active=FindFirstObjectByType<CinderDemoLayout>()!=null;ship=Active?GameObject.Find("Reused Ship Interior Trial 05")?.transform:null;}
    public static Vector3 ShipPoint(Vector3 local)=>Active&&ship?ship.TransformPoint(local):local;
    public static Vector3 Spawn(int slot)=>Active?ShipPoint(new Vector3(slot%2==0?-.7f:.7f,1.06f,slot<2?-2.6f:-1.2f)):new Vector3(slot%2==0?-1:1,0,slot<2?-5:-7);
    public static Vector3 Cargo=>Active?ShipPoint(new Vector3(0,1.4f,-.6f)):new Vector3(0,.55f,-3);
    public static bool Aboard(Vector3 p){if(!Active||!ship)return false;var q=ship.InverseTransformPoint(p);return Mathf.Abs(q.x)<2.6f&&q.z> -3.9f&&q.z<3.5f&&q.y>.65f&&q.y<3;}
    public static void Build(){
        var center=Reception+new Vector3(-2.1f,.8f,0);
        FacilityArt.Place("Receipt",center,new Vector3(.75f,1.6f,.65f),180);
        CarryWorld.Box("Receipt terminal collision",center,new Vector3(.75f,1.6f,.65f),CarryWorld.Mat(new Color(.2f,.24f,.23f))).GetComponent<Renderer>().enabled=false;
        new GameObject("Receipt feedback").AddComponent<ReceiptFeedback>();
        RenderSettings.fog=true;RenderSettings.fogMode=FogMode.Linear;RenderSettings.fogStartDistance=30;RenderSettings.fogEndDistance=100;RenderSettings.fogColor=new Color(.12f,.1f,.16f);
        var sun=GameObject.Find("Blockout daylight");if(sun)sun.name="Work light";
    }
}
}
