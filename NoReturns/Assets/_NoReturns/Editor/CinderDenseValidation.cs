using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class CinderDenseValidation {
    static string Dir=>Path.Combine(File.Exists("CarryWorkspace.txt")?File.ReadAllText("CarryWorkspace.txt").Trim():Path.GetFullPath(".."),"artifacts/cinder-dense");
    static readonly Vector2Int[] Steps={Vector2Int.up,Vector2Int.right,Vector2Int.down,Vector2Int.left};
    static HashSet<Vector2Int> grid;
    static Vector3 World(Vector2Int p)=>new Vector3(p.x,0,p.y);
    static bool Clear(Vector3 p)=>!Physics.CheckBox(p+Vector3.up*.95f,new Vector3(.57f,.8f,.57f),Quaternion.identity,~0,QueryTriggerInteraction.Ignore);
    static bool Edge(Vector3 a,Vector3 b)=>!Physics.BoxCast(a+Vector3.up*.95f,new Vector3(.57f,.8f,.57f),(b-a).normalized,Quaternion.identity,Vector3.Distance(a,b),~0,QueryTriggerInteraction.Ignore);
    static Vector2Int Nearest(Vector3 p)=>grid.OrderBy(q=>(World(q)-p).sqrMagnitude).First();
    static List<Vector3> Route(Vector3 a,Vector3 b) {
        var start=Nearest(a);var end=Nearest(b);var queue=new Queue<Vector2Int>();var parents=new Dictionary<Vector2Int,Vector2Int>{{start,start}};queue.Enqueue(start);
        while(queue.Count>0&&!parents.ContainsKey(end)){var p=queue.Dequeue();foreach(var delta in Steps){var n=p+delta;if(!grid.Contains(n)||parents.ContainsKey(n)||!Edge(World(p),World(n)))continue;parents[n]=p;queue.Enqueue(n);}}
        if(!parents.ContainsKey(end))throw new Exception("Disconnected cargo route "+a+" to "+b);
        var result=new List<Vector3>();for(var n=end;n!=start;n=parents[n])result.Add(World(n));result.Add(World(start));result.Reverse();return result;
    }
    [Serializable] class Routes { public Vector3[] delivery; public Vector3[] cells; }
    [MenuItem("NO RETURNS/Demo/Validate Dense Facility")]
    public static void Validate() {
        Directory.CreateDirectory(Dir);Physics.SyncTransforms();CinderDemoBuild.Validate();
        var ground=GameObject.Find("Exterior ground 108x86.4m");if(!ground||Vector3.Distance(ground.transform.localScale,new Vector3(108,.3f,86.4f))>.01f)throw new Exception("Map resized");
        if(!GameObject.Find("Dense facility revision"))throw new Exception("Missing density revision");
        var checks=new List<string>();grid=new HashSet<Vector2Int>();
        for(int x=-52;x<=52;x++)for(int z=-41;z<=41;z++)if(Clear(new Vector3(x,0,z)))grid.Add(new Vector2Int(x,z));
        var start=new Vector3(-32,0,-30);
        // Each distinct room and each reception side must remain connected to the landing.
        var rooms=new[]{new Vector3(-39,0,-16),new Vector3(-45,0,-7),new Vector3(-35,0,-7),new Vector3(-42,0,9),new Vector3(-43,0,19),new Vector3(-35,0,20),new Vector3(-24,0,-8),new Vector3(-16,0,1),new Vector3(-24,0,10),new Vector3(-16,0,11),new Vector3(-24,0,27),new Vector3(-25,0,35),new Vector3(-5,0,20),new Vector3(5,0,20),new Vector3(-5,0,31),new Vector3(5,0,31),new Vector3(0,0,25),new Vector3(29,0,17),new Vector3(33,0,25),new Vector3(39,0,24),new Vector3(29,0,-19),new Vector3(29,0,-10),new Vector3(33,0,-2),new Vector3(-5,0,-30),new Vector3(5,0,-30),new Vector3(15,0,-30),new Vector3(48,0,0),new Vector3(48,0,10),new Vector3(48,0,20)};
        foreach(var target in rooms){if(!Clear(target))throw new Exception("Occupied room sample "+target);var route=Route(start,target);checks.Add("PASS reachable room "+target+" / "+route.Count+" cells");}
        foreach(var pair in new[]{new[]{new Vector3(-30,1.5f,31),new Vector3(32,1.5f,31)},new[]{new Vector3(-50,1.5f,-20),new Vector3(-50,1.5f,35)},new[]{new Vector3(-27,1.5f,-9),new Vector3(-12,1.5f,12)},new[]{new Vector3(26,1.5f,-20),new Vector3(36,1.5f,0)}}){if(!Physics.Linecast(pair[0],pair[1]))throw new Exception("Long sightline still open "+pair[0]);checks.Add("PASS blocked long sightline "+pair[0]);}
        // Delivery detour intentionally enters the dispatch corridor and BAY inspection room.
        var waypoints=new[]{start,new Vector3(-31,0,0),new Vector3(-30,0,17),new Vector3(-6,0,25),new Vector3(11,0,25),new Vector3(21,0,37),new Vector3(33,0,32),new Vector3(33,0,25),new Vector3(33,0,16)};
        var delivery=new List<Vector3>();for(int i=1;i<waypoints.Length;i++)delivery.AddRange(Route(waypoints[i-1],waypoints[i]).Skip(i==1?0:1));
        var probe=new GameObject("Dense movement probe");var cc=probe.AddComponent<CharacterController>();cc.height=1.8f;cc.radius=.34f;cc.center=Vector3.up*.9f;cc.stepOffset=.32f;
        try{cc.enabled=false;probe.transform.position=delivery[0]+Vector3.up*.07f;cc.enabled=true;foreach(var target in delivery){for(int n=0;n<40&&Vector2.Distance(new Vector2(probe.transform.position.x,probe.transform.position.z),new Vector2(target.x,target.z))>.07f;n++){var d=target-probe.transform.position;d.y=0;cc.Move(Vector3.ClampMagnitude(d,.1f)+Vector3.down*.02f);}if(Vector2.Distance(new Vector2(probe.transform.position.x,probe.transform.position.z),new Vector2(target.x,target.z))>.12f)throw new Exception("Controller blocked at "+target);}checks.Add("PASS actual CharacterController delivery path: "+delivery.Count+" cells");}finally{UnityEngine.Object.DestroyImmediate(probe);}
        File.WriteAllText(Path.Combine(Dir,"routes.json"),JsonUtility.ToJson(new Routes{delivery=delivery.ToArray(),cells=grid.Select(World).ToArray()}));
        checks.Add("PASS footprint preserved 108x86.4; cargo clearance 1.14m sampled; no AI/human fun claim");File.WriteAllLines(Path.Combine(Dir,"geometry.txt"),checks);Debug.Log("DENSE PASS: "+checks.Count);
    }
    [MenuItem("NO RETURNS/Demo/Capture Dense Facility")]
    public static void Capture() {
        Directory.CreateDirectory(Dir);var o=new GameObject("Dense review camera");var camera=o.AddComponent<Camera>();camera.farClipPlane=220;camera.nearClipPlane=.1f;camera.backgroundColor=new Color(.07f,.09f,.1f);camera.clearFlags=CameraClearFlags.SolidColor;
        var hidden=new List<Renderer>();
        try{
            camera.orthographic=true;camera.orthographicSize=49;o.transform.SetPositionAndRotation(new Vector3(0,100,0),Quaternion.Euler(90,0,0));Save(camera,"overview-roofs.png");
            camera.orthographic=false;camera.fieldOfView=80;
            o.transform.SetPositionAndRotation(new Vector3(-31,1.65f,-20),Quaternion.Euler(0,12,0));Save(camera,"staff-approach.png");
            o.transform.SetPositionAndRotation(new Vector3(-24,1.65f,-7),Quaternion.Euler(0,20,0));Save(camera,"warehouse-interior.png");
            o.transform.SetPositionAndRotation(new Vector3(12,1.65f,-14),Quaternion.Euler(0,0,0));Save(camera,"freight-court.png");
        }finally{foreach(var r in hidden)if(r)r.enabled=true;UnityEngine.Object.DestroyImmediate(o);}
        CaptureCutaway();
    }
    // URP updates renderer visibility between editor frames, not between Camera.Render calls.
    static void CaptureCutaway() {
        var hidden=UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None)
            .Where(r=>r.enabled&&(r.name=="Roof"||r.name.Contains("canopy")||r.name.Contains("shelter"))).ToArray();
        foreach(var r in hidden)r.enabled=false;
        double ready=EditorApplication.timeSinceStartup+.3;
        void CaptureNextFrame(){
            if(EditorApplication.timeSinceStartup<ready)return;
            EditorApplication.update-=CaptureNextFrame;
            var o=new GameObject("Dense cutaway review camera");
            try {
                var camera=o.AddComponent<Camera>();camera.orthographic=true;camera.orthographicSize=49;
                camera.farClipPlane=220;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.07f,.09f,.1f);
                o.transform.SetPositionAndRotation(new Vector3(0,100,0),Quaternion.Euler(90,0,0));
                Save(camera,"overview-rooms.png");
            } finally {foreach(var r in hidden)if(r)r.enabled=true;UnityEngine.Object.DestroyImmediate(o);}
        }
        EditorApplication.update+=CaptureNextFrame;
    }
    static void Save(Camera camera,string file){var rt=new RenderTexture(1400,1000,24);var old=RenderTexture.active;Texture2D image=null;try{camera.targetTexture=rt;camera.Render();RenderTexture.active=rt;image=new Texture2D(1400,1000,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1400,1000),0,0);image.Apply();File.WriteAllBytes(Path.Combine(Dir,file),image.EncodeToPNG());}finally{camera.targetTexture=null;RenderTexture.active=old;if(image)UnityEngine.Object.DestroyImmediate(image);rt.Release();UnityEngine.Object.DestroyImmediate(rt);}}
}
}
