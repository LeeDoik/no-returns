using UnityEngine;
namespace NoReturns.CarryLab {
// Visual-only dressing. Existing primitive colliders remain the gameplay authority.
public static class FacilityArt {
 public static GameObject Place(string module,Vector3 center,Vector3 size,float yaw=0){
  var prefab=Resources.Load<GameObject>("PSXKit01/"+module);if(!prefab)return null;
  var g=Object.Instantiate(prefab);g.name="PSX "+module;
  var renderers=g.GetComponentsInChildren<Renderer>();var b=new Bounds();bool first=true;
  foreach(var r in renderers){if(first){b=r.bounds;first=false;}else b.Encapsulate(r.bounds);}
  if(first){Object.Destroy(g);return null;}
  var offset=b.center;var scale=new Vector3(size.x/b.size.x,size.y/b.size.y,size.z/b.size.z);
  g.transform.localScale=scale;g.transform.rotation=Quaternion.Euler(0,yaw,0);
  g.transform.position=center-g.transform.rotation*Vector3.Scale(offset,scale);return g;
 }
 public static void Dress(){
  Physics.SyncTransforms();
  var boxes=Object.FindObjectsByType<BoxCollider>(FindObjectsSortMode.None);
  foreach(var box in boxes){
   string n=box.name;var b=box.bounds;
   if(n=="West storage rack"||n=="North sorting stack"){
    if(Place("Rack",b.center,b.size))box.GetComponent<Renderer>().enabled=false;
   }else if(n.EndsWith("wall")||n.Contains("divider")||n=="Narrow passage"||n=="East dogleg"){
    bool alongX=b.size.x>b.size.z;float length=alongX?b.size.x:b.size.z;int count=Mathf.CeilToInt(length/3f);float width=length/count;
    for(int i=0;i<count;i++){
     var p=b.center+(alongX?Vector3.right:Vector3.forward)*(-length/2+width*(i+.5f));
     Place("Wall",p,new Vector3(width,b.size.y,alongX?b.size.z:b.size.x),alongX?0:90);
    }
    if(Resources.Load<GameObject>("PSXKit01/Wall"))box.GetComponent<Renderer>().enabled=false;
   }
  }
  // Flush floor visuals: the original floor remains the continuous collision surface.
  for(int x=0;x<2;x++)for(int z=0;z<4;z++)Place("Floor",new Vector3(-1.5f+x*3,-.12f,-10.5f+z*3),new Vector3(3,.24f,3));
  Place("Door",new Vector3(13,2.05f,28),new Vector3(4.5f,4.3f,.5f));
  for(int i=0;i<4;i++)Place("Lamp",new Vector3(7.66f,3.2f,7+i*4),new Vector3(1.2f,.6f,.14f),90);
  var terminal=Place("Receipt",new Vector3(-8.1f,.8f,9),new Vector3(.75f,1.6f,.65f),180);
  if(terminal){var solid=new GameObject("Receipt terminal collision");solid.transform.position=new Vector3(-8.1f,.8f,9);solid.AddComponent<BoxCollider>().size=new Vector3(.75f,1.6f,.65f);}
  // Corner reinforcement stays inside the existing thick rear boundary collider.
  Place("Corner",new Vector3(-14.4f,1.5f,-14),new Vector3(3,3,.48f));
 }
}
}
