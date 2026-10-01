using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoReturns.CarryLab;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace NoReturns.Editor {
public static class CinderSitePropsBuild {
    const string Art = "Assets/_NoReturns/Art/CinderSiteProps01";
    public const string RootName = "Cinder site props";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static Material Surface => AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
    static Transform Root => GameObject.Find(RootName).transform;
    static string SceneCollision() => Newtonsoft.Json.JsonConvert.SerializeObject(EditorSceneManager.GetActiveScene().GetRootGameObjects()
        .Where(g=>g.name!=RootName).Select(g=>new {g.name,state=CinderMapAppearanceBuild.ColliderState(g.transform)}));
    static Vector3 V(float x,float y,float z) => new Vector3(x,y,z);

    // Small native meshes reuse the approved atlas. No provider or new texture is required.
    internal sealed class Shape {
        readonly List<Vector3> vertices=new List<Vector3>();
        readonly List<Vector2> uv=new List<Vector2>();
        readonly List<int> triangles=new List<int>();
        public void Quad(Vector3 a,Vector3 b,Vector3 c,Vector3 d,bool rust,Vector2[] coordinates=null) {
            int i=vertices.Count; vertices.AddRange(new[]{a,b,c,d});
            // Existing graphite/rust rectangles, inset by a pixel to avoid adjacent atlas regions.
            float x=rust?257:385,y=rust?1:129;
            uv.AddRange(coordinates??new[]{new Vector2(x/512,y/512),new Vector2((x+126)/512,y/512),new Vector2((x+126)/512,(y+126)/512),new Vector2(x/512,(y+126)/512)});
            triangles.AddRange(new[]{i,i+1,i+2,i,i+2,i+3});
        }
        public void Triangle(Vector3 a,Vector3 b,Vector3 c,bool rust,Vector2[] coordinates=null) {
            int i=vertices.Count;vertices.AddRange(new[]{a,b,c});
            float x=rust?257:385,y=rust?1:129;
            uv.AddRange(coordinates??new[]{new Vector2((x+63)/512,(y+63)/512),new Vector2(x/512,y/512),new Vector2((x+126)/512,y/512)});
            triangles.AddRange(new[]{i,i+1,i+2});
        }
        public void Box(Vector3 p,Vector3 s,bool rust=false) {
            var h=s/2;var n=p-h;var f=p+h;
            Quad(V(n.x,n.y,n.z),V(n.x,n.y,f.z),V(n.x,f.y,f.z),V(n.x,f.y,n.z),rust);
            Quad(V(f.x,n.y,f.z),V(f.x,n.y,n.z),V(f.x,f.y,n.z),V(f.x,f.y,f.z),rust);
            Quad(V(n.x,n.y,f.z),V(n.x,n.y,n.z),V(f.x,n.y,n.z),V(f.x,n.y,f.z),rust);
            Quad(V(n.x,f.y,n.z),V(n.x,f.y,f.z),V(f.x,f.y,f.z),V(f.x,f.y,n.z),rust);
            Quad(V(f.x,n.y,n.z),V(n.x,n.y,n.z),V(n.x,f.y,n.z),V(f.x,f.y,n.z),rust);
            Quad(V(n.x,n.y,f.z),V(f.x,n.y,f.z),V(f.x,f.y,f.z),V(n.x,f.y,f.z),rust);
        }
        public void Cylinder(float radius,float low,float high,bool rust=false) {
            for(int k=0;k<8;k++) {
                float a=k*Mathf.PI/4,b=(k+1)*Mathf.PI/4;
                var p=V(Mathf.Cos(a)*radius,low,Mathf.Sin(a)*radius);var q=V(Mathf.Cos(b)*radius,low,Mathf.Sin(b)*radius);
                var r=q+Vector3.up*(high-low);var s=p+Vector3.up*(high-low);
                Quad(q,p,s,r,rust);
                Triangle(V(0,low,0),p,q,rust);
                Triangle(V(0,high,0),r,s,rust);
            }
        }
        public Mesh Save(string name,string directory=Art,bool replace=false) {
            string path=directory+"/"+name+".asset";
            var existing=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(existing&&!replace)return existing;
            var mesh=existing?existing:new Mesh{name=name};mesh.Clear();
            mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateBounds();
            if(existing) { EditorUtility.SetDirty(mesh);return mesh; }
            AssetDatabase.CreateAsset(mesh,path);return mesh;
        }
    }
    static void MakeMeshes() {
        Directory.CreateDirectory(Art);AssetDatabase.Refresh();
        var s=new Shape();
        foreach(float x in new[]{-.7f,0,.7f})s.Box(V(x,.065f,0),V(.15f,.13f,1));
        for(int i=0;i<7;i++)s.Box(V(0,.155f,-.42f+i*.14f),V(1.8f,.05f,.12f),true);
        s.Save("Pallet");
        s=new Shape();s.Box(V(0,.86f,0),V(.9f,1.72f,.46f));s.Box(V(0,.85f,-.245f),V(.82f,1.54f,.035f),true);
        for(int i=0;i<6;i++)s.Box(V(0,.3f+i*.075f,-.27f),V(.58f,.025f,.025f));
        s.Box(V(.3f,1.1f,-.27f),V(.05f,.2f,.05f));s.Save("Cabinet");
        s=new Shape();s.Box(V(0,.92f,0),V(2.4f,.12f,.8f));
        foreach(float x in new[]{-1.08f,1.08f})foreach(float z in new[]{-.28f,.28f})s.Box(V(x,.43f,z),V(.1f,.86f,.1f),true);
        s.Box(V(0,.35f,0),V(2.1f,.06f,.58f));s.Save("Workbench");
        s=new Shape();s.Cylinder(.275f,0,.9f,true);s.Cylinder(.29f,.06f,.1f);s.Cylinder(.29f,.8f,.84f);s.Save("Drum");
        s=new Shape();s.Box(V(0,.3f,0),V(1.05f,.6f,.055f),true);
        for(int i=0;i<6;i++)s.Box(V(0,.1f+i*.08f,-.04f),V(.88f,.025f,.04f));s.Save("Vent");
        s=new Shape();s.Box(V(0,.48f,0),V(1.8f,.96f,1.2f));s.Box(V(0,.49f,-.62f),V(1.55f,.74f,.04f),true);
        for(int i=0;i<7;i++)s.Box(V(0,.2f+i*.085f,-.655f),V(1.35f,.03f,.03f));
        s.Box(V(.5f,1.07f,.25f),V(.35f,.22f,.35f),true);s.Save("Roof unit");
        s=new Shape();s.Cylinder(.09f,-1.5f,1.5f);s.Save("Pipe 3m");
        s=new Shape();s.Box(V(0,0,0),V(1.45f,.72f,.04f));s.Save("Legend board");
        AssetDatabase.SaveAssets();
    }
    static Transform Group(string name,Vector3 position,float yaw=0) {
        var g=new GameObject(name).transform;g.SetParent(Root);g.SetPositionAndRotation(position,Quaternion.Euler(0,yaw,0));return g;
    }
    static GameObject Model(string name,Transform group,Vector3 local,bool collision=true,Quaternion? rotation=null) {
        var obj=new GameObject(name);obj.transform.SetParent(group,false);obj.transform.localPosition=local;
        obj.transform.localRotation=rotation??Quaternion.identity;
        var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(Art+"/"+name+".asset");if(!mesh)throw new Exception("Missing prop mesh "+name);
        obj.AddComponent<MeshFilter>().sharedMesh=mesh;obj.AddComponent<MeshRenderer>().sharedMaterial=Surface;
        if(collision){var c=obj.AddComponent<BoxCollider>();c.center=mesh.bounds.center;c.size=mesh.bounds.size;}
        return obj;
    }
    static void Cargo(Transform group,Vector3 local,Vector3 size) {
        var box=FacilityArt.Place("Parcel",group.TransformPoint(local),size,group.eulerAngles.y);
        if(!box)throw new Exception("Missing existing Parcel visual");box.name="Stored freight / static";box.transform.SetParent(group);
    }
    static void Pallet(string name,float x,float z,float yaw=0) {
        var g=Group("Pallet / "+name,V(x,0,z),yaw);Model("Pallet",g,Vector3.zero,false);
        Cargo(g,V(-.43f,.495f,0),V(.75f,.63f,.64f));Cargo(g,V(.43f,.495f,0),V(.75f,.63f,.64f));Cargo(g,V(.43f,1.125f,0),V(.75f,.63f,.64f));
        var c=g.gameObject.AddComponent<BoxCollider>();c.size=V(1.8f,1.44f,1);c.center=V(0,.72f,0);
    }
    static void Rack(string name,float x,float z,float yaw) {
        var g=Group("Rack / "+name,V(x,0,z),yaw);CinderMapAppearanceBuild.PlacePart("Rack_A",g,g.position,yaw);
        foreach(float height in new[]{.495f,1.595f})foreach(float side in new[]{-.65f,.65f})Cargo(g,V(side,height,0),V(.58f,.47f,.47f));
    }
    static void Furniture(string type,string name,float x,float z,float yaw=0) {
        var g=Group(type+" / "+name,V(x,0,z),yaw);Model(type,g,Vector3.zero);
        if(type=="Workbench")Cargo(g,V(-.65f,1.215f,0),V(.58f,.47f,.47f));
        if(type=="Workbench" && (name.StartsWith("office") || name=="bay dispatch")) {
            var position=V(.55f,1.29f,0);var size=V(.55f,.62f,.48f);
            var monitor=FacilityArt.Place("Receipt",g.TransformPoint(position),size,g.eulerAngles.y+180);
            if(!monitor)throw new Exception("Missing existing CRT visual");monitor.name="Desk CRT / static";monitor.transform.SetParent(g);
            var solid=new GameObject("Desk CRT reserved volume");solid.transform.SetParent(g,false);solid.transform.localPosition=position;
            solid.AddComponent<BoxCollider>().size=size;
        }
    }
    static void Drums(string name,float x,float z) {
        var g=Group("Drums / "+name,V(x,0,z));
        foreach(var p in new[]{V(-.36f,0,-.34f),V(.36f,0,-.34f),V(0,0,.34f)})Model("Drum",g,p);
    }
    static void Pipe(string name,float x,float z,float yaw) {
        var g=Group("Pipes / "+name,V(x,3.15f,z),yaw);
        foreach(float y in new[]{0f,.4f})Model("Pipe 3m",g,V(0,y,0),true,Quaternion.Euler(90,0,0));
    }
    internal static void Legend(string label,float x,float y,float z,float yaw,Transform parent=null) {
        var g=parent?new GameObject("Legend / "+label.Replace('\n',' ')).transform:Group("Legend / "+label.Replace('\n',' '),V(x,y,z),yaw);
        if(parent) {g.SetParent(parent);g.SetPositionAndRotation(V(x,y,z),Quaternion.Euler(0,yaw,0));}
        Model("Legend board",g,Vector3.zero,false);
        var canvasObj=new GameObject("Physical facility label",typeof(RectTransform),typeof(Canvas));canvasObj.transform.SetParent(g,false);
        canvasObj.transform.localPosition=V(0,0,-.025f);canvasObj.transform.localScale=Vector3.one*.001f;
        var rect=canvasObj.GetComponent<RectTransform>();rect.sizeDelta=new Vector2(1400,680);
        canvasObj.GetComponent<Canvas>().renderMode=RenderMode.WorldSpace;
        var textObj=new GameObject("English source",typeof(RectTransform),typeof(Text));textObj.transform.SetParent(canvasObj.transform,false);
        var tr=textObj.GetComponent<RectTransform>();tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=new Vector2(55,25);tr.offsetMax=new Vector2(-55,-25);
        var text=textObj.GetComponent<Text>();text.text=label;text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=180;text.resizeTextForBestFit=true;text.resizeTextMinSize=100;text.resizeTextMaxSize=220;
        text.alignment=TextAnchor.MiddleCenter;text.color=new Color(.88f,.81f,.64f);text.raycastTarget=false;
    }
    [MenuItem("NO RETURNS/Trials/Add Cinder Site Props")]
    public static void Create() {
        if(EditorApplication.isPlaying||EditorSceneManager.GetActiveScene().isDirty)throw new Exception("Stop Play and save changes first.");
        if(EditorSceneManager.GetActiveScene().path!=CinderCompactSiteBuild.ScenePath||GameObject.Find(RootName))throw new Exception("Open compact site; preserve an existing prop pass before rebuilding.");
        string collision=SceneCollision();var points=CinderCompactSiteBuild.PosePoints();
        MakeMeshes();var root=new GameObject(RootName);
        try {
            Rack("warehouse west",-20.7f,.3f,90);Rack("warehouse east",-7.6f,4.5f,270);
            Rack("bay sorting",6.6f,17.4f,90);Rack("service",18.9f,-.3f,270);Rack("storage",1.2f,-27.8f,0);
            Pallet("warehouse north",-18.7f,7.8f);Pallet("warehouse east",-9.5f,8.4f);
            Pallet("bay west",8.7f,20.4f);Pallet("bay east",16.8f,21.6f);
            Pallet("storage west",-6.6f,-26.3f);Pallet("storage east",16.8f,-26.1f);
            Pallet("landing freight",-15.3f,-29.4f,90);
            Furniture("Workbench","warehouse pack",-18.9f,-6.7f);
            Furniture("Workbench","office north",-18.7f,22.3f);Furniture("Workbench","office south",-18.7f,18.1f);
            Furniture("Workbench","bay dispatch",17.8f,16.8f,90);Furniture("Workbench","service",9.6f,1.7f);
            // Keep cabinet backs flush with walls; a body-sized rear gap cannot fit diagonal carried cargo.
            Furniture("Cabinet","office",-7.31f,21.3f,90);Furniture("Cabinet","bay",19.24f,10.2f,90);
            Furniture("Cabinet","service west 1",7.46f,-13.8f,270);Furniture("Cabinet","service west 2",7.46f,-10.9f,270);
            Drums("warehouse",-9.3f,-5.8f);Drums("service",17.5f,-14.4f);Drums("storage",6.9f,-26.4f);
            foreach(var p in new[]{V(-.6f,4.3f,19.8f),V(-.6f,4.3f,10.5f),V(0,4.3f,-14.1f),V(-18,4.3f,-14.7f),V(-.6f,4.3f,.6f),V(-6.9f,4.3f,-14.7f)})Model("Roof unit",Group("Roof machinery",p),Vector3.zero);
            Pipe("north control west",-3.87f,11.1f,0);Pipe("north control west rear",-3.87f,20.1f,0);
            Pipe("sorting west",-3.87f,.6f,0);Pipe("power east",3.87f,-12.3f,0);
            Pipe("service west",6.8f,-9.9f,0);Pipe("office east",-6.63f,19.8f,0);
            foreach(var p in new[]{V(-10.3f,2.9f,-8.59f),V(-9.6f,2.9f,15.1f),V(16.5f,2.9f,7.31f),V(17.1f,2.9f,-18.19f),V(11.1f,2.9f,-28.69f)})Model("Vent",Group("High wall vent",p),Vector3.zero);
            Legend("A\nWAREHOUSE",-17.4f,2.65f,12.19f,180);Legend("SIDE\nOFFICE",-17.4f,2.65f,15.11f,0);
            Legend("BAY 04\nDISPATCH",16.2f,2.65f,7.31f,0);Legend("C SERVICE\nPOWER",16.8f,2.65f,-18.19f,0);
            Legend("STORAGE\nFREIGHT",9,2.65f,-28.69f,0);Legend("BAY 04\nNORTH LOOP",-3.87f,2.65f,1.3f,90);
            Physics.SyncTransforms();
            if(collision!=SceneCollision()||points.Count!=CinderCompactSiteBuild.PosePoints().Count)throw new Exception("Approved structure or occupiable route poses changed.");
            File.WriteAllText(Path.Combine(Output,"props-collision-baseline.json"),collision+"\n");
            Validate();EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());AssetDatabase.SaveAssets();
        } catch {UnityEngine.Object.DestroyImmediate(root);throw;}
    }
    public static List<Vector3> PosePoints() {
        var points=CinderCompactSiteBuild.PosePoints();
        foreach(var c in Root.GetComponentsInChildren<BoxCollider>().Where(c=>c.bounds.min.y<.1f)) {
            var b=c.bounds;var center=V(b.center.x,.035f,b.center.z);
            points.Add(center+Vector3.right*(b.extents.x+.525f));points.Add(center-Vector3.right*(b.extents.x+.525f));
            points.Add(center+Vector3.forward*(b.extents.z+.525f));points.Add(center-Vector3.forward*(b.extents.z+.525f));
        }
        return points.Distinct().Where(p=>!Physics.CheckCapsule(p+Vector3.up*.34f,p+Vector3.up*1.46f,.34f,1<<0,QueryTriggerInteraction.Ignore)).ToList();
    }
    [MenuItem("NO RETURNS/Trials/Validate Cinder Site Props")]
    public static void Validate() {
        if(GameObject.Find(CinderBackgroundBuild.RootName)) { CinderBackgroundBuild.Validate(); return; }
        if(GameObject.Find(CinderArchitectureBuild.RootName)) { CinderArchitectureBuild.Validate(); return; }
        if(EditorSceneManager.GetActiveScene().path!=CinderCompactSiteBuild.ScenePath||!GameObject.Find(RootName))throw new Exception("Open dressed compact site.");
        if(File.Exists(Path.Combine(Output,"props-collision-baseline.json"))&&SceneCollision()!=File.ReadAllText(Path.Combine(Output,"props-collision-baseline.json")).TrimEnd())throw new Exception("Approved site collision changed.");
        CinderCompactSiteBuild.ValidateWithPrefix("props-");
        var meshes=Root.GetComponentsInChildren<MeshFilter>();
        foreach(string path in AssetDatabase.FindAssets("t:Mesh",new[]{Art}).Select(AssetDatabase.GUIDToAssetPath)) {
            var m=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(m.uv.Length!=m.vertexCount||m.uv.Any(p=>p.x<0||p.x>1||p.y<0||p.y>1))throw new Exception("Invalid prop UV "+path);
            var v=m.vertices;var indices=m.triangles;
            for(int i=0;i<indices.Length;i+=3)if(Vector3.Cross(v[indices[i+1]]-v[indices[i]],v[indices[i+2]]-v[indices[i]]).sqrMagnitude<1e-12f)throw new Exception("Degenerate prop triangle "+path);
        }
        if(CinderCompactSiteBuild.PosePoints().Count!=159)throw new Exception("A prop occupied an approved route pose.");
        var labels=Root.GetComponentsInChildren<Text>();if(labels.Length!=6||labels.Any(t=>!t.font||t.raycastTarget))throw new Exception("Facility labels are missing or interactive.");
        var result=new{scene=CinderCompactSiteBuild.ScenePath,source_layout_user_approved=true,prop_groups=Root.childCount,
            groups=Root.Cast<Transform>().GroupBy(t=>t.name.Split('/')[0].Trim()).ToDictionary(g=>g.Key,g=>g.Count()),
            placed_meshes=meshes.Length,triangles=meshes.Sum(f=>f.sharedMesh.triangles.Length/3),added_box_colliders=Root.GetComponentsInChildren<BoxCollider>().Length,
            static_freight_visuals=Root.GetComponentsInChildren<Transform>().Count(t=>t.name=="Stored freight / static"),static_desk_crts=Root.GetComponentsInChildren<Transform>().Count(t=>t.name=="Desk CRT / static"),facility_labels=labels.Select(t=>t.text),
            preserved_base_route_poses=159,carrying_pose_positions=PosePoints().Count,interactive_props=false,layout_changed=false,user_prop_quality_review=false};
        File.WriteAllText(Path.Combine(Output,"props-layout-validation.json"),Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");
        Debug.Log("CINDER SITE PROPS PASS: "+Root.childCount+" groups");
    }
}
}
