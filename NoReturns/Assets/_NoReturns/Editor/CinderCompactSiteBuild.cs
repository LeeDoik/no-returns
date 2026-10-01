using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
public static class CinderCompactSiteBuild {
    public const string ScenePath = "Assets/_NoReturns/Scenes/CinderCompactSiteReview.unity";
    const string Art = "Assets/_NoReturns/Art/CinderCompactSite01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static readonly string[] Names = {"A WAREHOUSE","SIDE OFFICE","BAY 04","C SERVICE","STORAGE"};
    static readonly Vector2[] Centers = {new Vector2(-14.1f,1.8f),new Vector2(-14.1f,19.8f),new Vector2(12.75f,15.9f),new Vector2(13.35f,-6.9f),new Vector2(5.25f,-24.9f)};
    static readonly Rect[] Solids = {new Rect(-3.6f,7.5f,6,16.8f),new Rect(-3.6f,-18,7.2f,12),new Rect(-21.3f,-18,8.85f,6.3f),new Rect(-3.6f,-2.7f,6,6.9f),new Rect(5.85f,-4.2f,1.2f,8.4f),new Rect(-9.15f,-18,5.55f,6.3f)};
    static readonly string[] SolidNames = {"North control annex","Power block","West storage bays","Sorting island","Service housings","Power block west wing"};
    public static Vector3 Spawn => P(-20.7f,-20.05f);
    public static readonly Rect Field = new Rect(-27.6f,-34.8f,53.55f,65.4f);
    static Transform Root => GameObject.Find("Cinder compact suppression site").transform;
    static Vector3 P(float x,float z,float y=.035f) => new Vector3(x,y,z);
    static Bounds Floor(string name) => GameObject.Find("Cinder Depot editable primitive blockout").transform.Find(name).Find(name+" floor").GetComponent<Renderer>().bounds;
    static Rect Footprint(Bounds b) => new Rect(b.min.x,b.min.z,b.size.x,b.size.z);
    static readonly Vector2[][] Loops = {
        new[]{new Vector2(-22.95f,-10.05f),new Vector2(-22.95f,13.65f),new Vector2(-5.25f,13.65f),new Vector2(-5.25f,-10.05f),new Vector2(-22.95f,-10.05f)},
        new[]{new Vector2(-5.25f,-4.35f),new Vector2(-5.25f,5.85f),new Vector2(4.125f,5.85f),new Vector2(4.125f,-4.35f),new Vector2(-5.25f,-4.35f)},
        new[]{new Vector2(5.325f,-19.65f),new Vector2(5.325f,-4.35f),new Vector2(4.125f,-4.35f),new Vector2(4.125f,5.85f),new Vector2(21.3f,5.85f),new Vector2(21.3f,-19.65f),new Vector2(5.325f,-19.65f)},
        new[]{new Vector2(-10.8f,-19.65f),new Vector2(21.3f,-19.65f),new Vector2(21.3f,-30.15f),new Vector2(-10.8f,-30.15f),new Vector2(-10.8f,-19.65f)}
    };
    public static Vector3[] Delivery(bool sheltered) => (sheltered
        ? new[]{P(-20.7f,-20.05f),P(-20.7f,-19.65f),P(-22.95f,-19.65f),P(-22.95f,25.95f),P(12.75f,25.95f),P(12.75f,15.9f)}
        : new[]{P(-20.7f,-20.05f),P(-14.1f,-20.05f),P(-14.1f,-24.9f),P(-10.8f,-24.9f),P(-10.8f,-10.05f),P(-5.25f,-10.05f),P(-5.25f,-4.35f),P(4.125f,-4.35f),P(4.125f,5.85f),P(12.75f,5.85f),P(12.75f,15.9f)});
    public static List<(string name,Vector3 a,Vector3 b)> Routes() {
        var routes=new List<(string,Vector3,Vector3)>();
        void Chain(string name,Vector3[] p) {for(int i=1;i<p.Length;i++) routes.Add((name+" "+i,p[i-1],p[i]));}
        for(int i=0;i<Loops.Length;i++) Chain("Site loop "+(i+1),Loops[i].Select(v=>P(v.x,v.y)).ToArray());
        Chain("Sheltered delivery",Delivery(true)); Chain("Central delivery",Delivery(false));
        foreach(string name in Names) {var f=Floor(name);Chain(name+" through both doors",new[]{P(f.center.x,f.min.z-1.65f),P(f.center.x,f.center.z),P(f.center.x,f.max.z+1.65f)});}
        Chain("Office north link",new[]{P(-5.25f,13.65f),P(-5.25f,25.95f)});
        Chain("Bay east link",new[]{P(21.3f,5.85f),P(21.3f,25.95f)});
        Chain("Storage landing gate",new[]{P(-14.1f,-24.9f),P(-10.8f,-24.9f)});
        var ship=GameObject.Find("Reused Ship Interior Trial 05").transform;
        routes.Add(("Ship ramp",ship.TransformPoint(new Vector3(0,0,-8))+.035f*Vector3.up,ship.TransformPoint(new Vector3(0,1.035f,-2.5f))+.035f*Vector3.up));
        return routes;
    }
    public static List<Vector3> PosePoints() {
        var points=new List<Vector3>();
        foreach(var route in Routes().Where(r=>r.name!="Ship ramp")) {
            points.Add(route.a);points.Add(route.b);var mid=(route.a+route.b)/2;points.Add(mid);
            var side=Vector3.Cross((route.b-route.a).normalized,Vector3.up);
            points.Add(mid+side*1.1f);points.Add(mid-side*1.1f);
        }
        points.Add(P(5.325f,-4.675741f)); // Regression: native BoxCast missed this grazing service-housing corner.
        // Only sample physically occupiable shoulders; passage tests separately fail if the route is blocked.
        return points.Distinct().Where(p=>!Physics.CheckCapsule(p+Vector3.up*.34f,p+Vector3.up*1.46f,.34f,1<<0,QueryTriggerInteraction.Ignore)).ToList();
    }
    static string LocalCollision(Transform root) => Newtonsoft.Json.JsonConvert.SerializeObject(root.GetComponentsInChildren<BoxCollider>().Select(c=>new {
        path=c.transform.parent.name+"/"+c.name,c.enabled,c.isTrigger,layer=c.gameObject.layer,position=c.transform.localPosition.ToString("F6"),rotation=c.transform.localRotation.ToString("F6"),scale=c.transform.localScale.ToString("F6"),center=c.center.ToString("F6"),size=c.size.ToString("F6")}));
    static GameObject Place(string part,Transform parent,Vector3 pos,float yaw=0) => CinderMapAppearanceBuild.PlacePart(part,parent,pos,yaw);
    static void Wall(string name,Vector2 a,Vector2 b,Transform parent) {
        var root=new GameObject(name).transform;root.SetParent(parent);var length=Vector2.Distance(a,b);
        root.SetPositionAndRotation(P((a.x+b.x)/2,(a.y+b.y)/2,0),Quaternion.Euler(0,Mathf.Abs(a.x-b.x)<.001f?90:0,0));
        var box=root.gameObject.AddComponent<BoxCollider>();box.center=Vector3.up*2;box.size=new Vector3(length,4,.3f);
        float used=0;
        var widths=new[]{1.2f,.9f,.65f,.35f,.3f,.15f,.05f};
        foreach(float width in widths) while(length-used>=width-.001f) {
            string part=width==1.2f?"Wall_A":width==.15f?"End_A":"Wall_Fill"+Mathf.RoundToInt(width*100).ToString("000");
            var obj=Place(part,root,Vector3.zero);obj.transform.localPosition=new Vector3(-length/2+used+width/2,0,0);obj.transform.localRotation=Quaternion.identity;used+=width;
        }
        if(Mathf.Abs(used-length)>.001f) throw new Exception("Unfilled wall length: "+name);
    }
    static void Gate(string name,float x,float low,float high,float opening,Transform parent) {
        Wall(name+" south",new Vector2(x,low),new Vector2(x,opening-1.9f),parent);
        Wall(name+" north",new Vector2(x,opening+1.9f),new Vector2(x,high),parent);
        var frame=Place("DoorFrame_A",parent,P(x,opening,0),90);frame.name=name+" frame";
        foreach(float side in new[]{-1f,1f}) {var box=frame.AddComponent<BoxCollider>();box.center=new Vector3(side*1.75f,2,0);box.size=new Vector3(.3f,4,.3f);}
        var lintel=frame.AddComponent<BoxCollider>();lintel.center=new Vector3(0,3.65f,0);lintel.size=new Vector3(3.2f,.7f,.3f);
    }
    // Native tile-face sampling keeps the existing atlas and world pixel density, including cropped edge tiles.
    static Vector2[] FaceUV(string part,bool upper) {
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_NoReturns/Prefabs/CinderAppearance01/NR_Cinder_"+part+".prefab");
        var f=prefab.GetComponentInChildren<MeshFilter>();var mesh=f.sharedMesh;var v=mesh.vertices.Select(p=>f.transform.localToWorldMatrix.MultiplyPoint3x4(p)).ToArray();
        float y=upper?v.Max(p=>p.y):v.Min(p=>p.y);var chosen=new List<int>();
        for(int i=0;i<mesh.triangles.Length;i+=3) if(Enumerable.Range(i,3).All(n=>Mathf.Abs(v[mesh.triangles[n]].y-y)<.0001f)) chosen.AddRange(Enumerable.Range(i,3).Select(n=>mesh.triangles[n]));
        var uv=new Vector2[4];int[] order={0,1,3,2};
        foreach(int n in chosen.Distinct()) {int corner=(v[n].x>0?1:0)+(v[n].z>0?2:0);uv[Array.IndexOf(order,corner)]=mesh.uv[n];}
        return uv;
    }
    static IEnumerable<Rect> Subtract(Rect a,Rect b) {
        if(!a.Overlaps(b)){yield return a;yield break;}
        float x0=Mathf.Max(a.xMin,b.xMin),x1=Mathf.Min(a.xMax,b.xMax),z0=Mathf.Max(a.yMin,b.yMin),z1=Mathf.Min(a.yMax,b.yMax);
        if(x0>a.xMin)yield return Rect.MinMaxRect(a.xMin,a.yMin,x0,a.yMax);
        if(x1<a.xMax)yield return Rect.MinMaxRect(x1,a.yMin,a.xMax,a.yMax);
        if(z0>a.yMin)yield return Rect.MinMaxRect(x0,a.yMin,x1,z0);
        if(z1<a.yMax)yield return Rect.MinMaxRect(x0,z1,x1,a.yMax);
    }
    static void Surfaces(Transform parent) {
        var vertices=new List<Vector3>();var uv=new List<Vector2>();var tris=new List<int>();
        void Surface(Rect area,float y,string part,bool upper,Rect[] exclude) {
            var source=FaceUV(part,upper);
            for(float x=area.xMin;x<area.xMax-.001f;x+=1.2f)for(float z=area.yMin;z<area.yMax-.001f;z+=1.2f) {
                var tile=Rect.MinMaxRect(x,z,Mathf.Min(x+1.2f,area.xMax),Mathf.Min(z+1.2f,area.yMax));IEnumerable<Rect> fragments=new[]{tile};
                foreach(var hole in exclude) fragments=fragments.SelectMany(r=>Subtract(r,hole)).ToArray();
                foreach(var r in fragments) {
                    int start=vertices.Count;
                    foreach(var p in new[]{new Vector2(r.xMin,r.yMin),new Vector2(r.xMax,r.yMin),new Vector2(r.xMax,r.yMax),new Vector2(r.xMin,r.yMax)}) {
                        vertices.Add(P(p.x,p.y,y));float u=(p.x-x)/1.2f,v=(p.y-z)/1.2f;
                        uv.Add(Vector2.Lerp(Vector2.Lerp(source[0],source[1],u),Vector2.Lerp(source[3],source[2],u),v));
                    }
                    tris.AddRange((upper?new[]{0,2,1,0,3,2}:new[]{0,1,2,0,2,3}).Select(n=>start+n));
                }
            }
        }
        Surface(new Rect(-26.4f,-34.2f,51,63.6f),0,"Floor_A",true,Names.Select(n=>Footprint(Floor(n))).Concat(Solids).ToArray());
        foreach(var r in Solids) {var roof=Rect.MinMaxRect(r.xMin-.15f,r.yMin-.15f,r.xMax+.15f,r.yMax+.15f);Surface(roof,4,"Ceiling_A",false,Array.Empty<Rect>());Surface(roof,4.3f,"Ceiling_A",true,Array.Empty<Rect>());}
        foreach(var roof in new[]{new Rect(-24.45f,-18.15f,3,42.6f),new Rect(-24.45f,24.45f,47.25f,3)}) {
            Surface(roof,4,"Ceiling_A",false,Array.Empty<Rect>());Surface(roof,4.3f,"Ceiling_A",true,Array.Empty<Rect>());
            var ceiling=new GameObject("Sheltered route ceiling collision");ceiling.transform.SetParent(parent);
            var c=ceiling.AddComponent<BoxCollider>();c.center=P(roof.center.x,roof.center.y,4.15f);c.size=new Vector3(roof.width,.3f,roof.height);
        }
        var mesh=new Mesh{name="Compact tile surfaces"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();mesh.RecalculateBounds();
        string path=Art+"/CompactTileSurfaces.asset";AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(mesh,path);
        var obj=new GameObject("Compact yard and utility roofs");obj.transform.SetParent(parent);obj.AddComponent<MeshFilter>().sharedMesh=mesh;
        obj.AddComponent<MeshRenderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
        var floor=obj.AddComponent<BoxCollider>();floor.center=P(-.9f,-2.4f,-.1f);floor.size=new Vector3(51,.2f,63.6f);
    }
    [MenuItem("NO RETURNS/Trials/Create Compact Cinder Site Review")]
    public static void Create() => CreateScene(false);
    public static void CreateScene(bool replace) {
        if(EditorApplication.isPlaying||EditorSceneManager.GetActiveScene().isDirty) throw new Exception("Stop Play and save current changes first.");
        if(File.Exists(ScenePath)&&!replace)throw new Exception("Compact site exists; preserve manual changes before regenerating.");
        var scene=EditorSceneManager.OpenScene(CinderMapAppearanceBuild.ScenePath);EditorSceneManager.SaveScene(scene,ScenePath);
        var old=GameObject.Find("Cinder Depot editable primitive blockout").transform;
        var map=GameObject.Find("Cinder map structures / original colliders retained").transform;
        var before=Names.ToDictionary(n=>n,n=>LocalCollision(old.Find(n)));
        var shipBefore=LocalCollision(GameObject.Find("Reused Ship Interior Trial 05").transform);
        for(int i=0;i<Names.Length;i++) {
            var b=Floor(Names[i]);var delta=P(Centers[i].x-b.center.x,Centers[i].y-b.center.z,0);old.Find(Names[i]).position+=delta;
            if(i==0){GameObject.Find("Cinder appearance warehouse / original colliders retained").transform.position+=delta;GameObject.Find("Cinder entrance presentation").transform.position+=delta;}
            else map.Find(Names[i]).position+=delta;
        }
        int marker=0,tower=0;var keep=new[]{"Exterior ground 108x86.4m","North boundary","South boundary","West boundary","East boundary"};
        foreach(Transform child in old.Cast<Transform>().ToArray()) {
            if(Names.Contains(child.name)||keep.Contains(child.name))continue;
            if(child.name=="Suppressor placeholder") {var corner=new[]{P(-25.95f,-33.15f,2.5f),P(-25.95f,28.95f,2.5f),P(24.3f,28.95f,2.5f),P(24.3f,-33.15f,2.5f)};child.position=corner[tower++];continue;}
            if(child.name.StartsWith("Listener zone")){var points=new[]{P(-22.95f,1.8f,1.3f),P(4.125f,.75f,1.3f),P(21.3f,-6.9f,1.3f)};child.position=points[marker++];continue;}
            UnityEngine.Object.DestroyImmediate(child.gameObject);
        }
        var ship=GameObject.Find("Reused Ship Interior Trial 05").transform;ship.SetPositionAndRotation(P(-20.7f,-28.05f,0),Quaternion.Euler(0,180,0));
        var root=new GameObject("Cinder compact suppression site").transform;
        for(int i=0;i<Solids.Length;i++) {
            var r=Solids[i];var group=new GameObject(SolidNames[i]).transform;group.SetParent(root);
            Wall("West",new Vector2(r.xMin,r.yMin),new Vector2(r.xMin,r.yMax),group);Wall("East",new Vector2(r.xMax,r.yMin),new Vector2(r.xMax,r.yMax),group);
            Wall("South",new Vector2(r.xMin,r.yMin),new Vector2(r.xMax,r.yMin),group);Wall("North",new Vector2(r.xMin,r.yMax),new Vector2(r.xMax,r.yMax),group);
            foreach(float x in new[]{r.xMin,r.xMax})foreach(float z in new[]{r.yMin,r.yMax})Place("Corner_A",group,P(x,z,0));
            foreach(var box in group.GetComponentsInChildren<BoxCollider>()) UnityEngine.Object.DestroyImmediate(box);
            var volume=group.gameObject.AddComponent<BoxCollider>();volume.center=P(r.center.x,r.center.y,2.15f);volume.size=new Vector3(r.width+.3f,4.3f,r.height+.3f);
        }
        Wall("West site edge",new Vector2(-24.6f,-18),new Vector2(-24.6f,27.6f),root);
        Wall("North site edge",new Vector2(-24.6f,27.6f),new Vector2(22.95f,27.6f),root);
        Wall("South site edge",new Vector2(-12.45f,-31.8f),new Vector2(22.95f,-31.8f),root);
        Gate("East invasion opening",22.95f,-31.8f,27.6f,18,root);
        Gate("Landing storage opening",-12.45f,-31.8f,-18,-24.9f,root);
        Directory.CreateDirectory(Art);AssetDatabase.Refresh();Surfaces(root);
        for(float z=-13.8f;z<25;z+=7.2f)Place("Lamp_A",root,P(-24.449f,z,3.55f),90);
        for(float x=-18.6f;x<22;x+=7.2f)Place("Lamp_A",root,P(x,27.449f,3.55f),180);
        foreach(var r in Solids.Take(4))Place("Lamp_A",root,P(r.center.x,r.yMin-.151f,3.55f),180);
        var fieldObj=new GameObject("Suppression field / visual boundary only");fieldObj.transform.SetParent(root);var line=fieldObj.AddComponent<LineRenderer>();line.useWorldSpace=true;line.loop=true;line.positionCount=4;line.widthMultiplier=.1f;
        line.SetPositions(new[]{P(Field.xMin,Field.yMin,.08f),P(Field.xMin,Field.yMax,.08f),P(Field.xMax,Field.yMax,.08f),P(Field.xMax,Field.yMin,.08f)});
        var material=new Material(Shader.Find("Universal Render Pipeline/Unlit")){name="Suppression field marker"};material.color=new Color(.24f,.68f,.5f);string matPath=Art+"/SuppressionField.mat";AssetDatabase.DeleteAsset(matPath);AssetDatabase.CreateAsset(material,matPath);line.sharedMaterial=material;
        GameObject.Find("Blockout employee").transform.SetPositionAndRotation(Spawn,Quaternion.identity);
        GameObject.Find("Trial carried parcel").transform.position=Spawn+new Vector3(.5f,.315f,.8f);
        if(Names.Any(n=>before[n]!=LocalCollision(old.Find(n)))||shipBefore!=LocalCollision(ship))throw new Exception("Building/ship local collision changed during relocation.");
        File.WriteAllText(Path.Combine(Output,"site-local-collision-baseline.json"),Newtonsoft.Json.JsonConvert.SerializeObject(new {buildings=before,ship=shipBefore},Newtonsoft.Json.Formatting.Indented)+"\n");
        Physics.SyncTransforms();EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();Validate();
    }
    public static List<(string name,Vector3 a,Vector3 b,float width)> CrewLanes() => new List<(string,Vector3,Vector3,float)> {
        ("West warehouse",P(-22.95f,-16.35f),P(-22.95f,10.35f),3),("West office",P(-22.95f,16.95f),P(-22.95f,22.65f),3),
        ("North covered",P(-21.3f,25.95f),P(19.65f,25.95f),3),("Warehouse office link",P(-19.65f,13.65f),P(-8.55f,13.65f),3),
        ("Warehouse east",P(-5.25f,-6.75f),P(-5.25f,10.35f),3),("Warehouse south",P(-19.65f,-10.05f),P(-14.1f,-10.05f),3),
        ("Sorting north",P(-2.25f,5.85f),P(1.05f,5.85f),3),("Sorting south",P(-2.25f,-4.35f),P(1.05f,-4.35f),3),
        ("North annex bay",P(4.125f,9.15f),P(4.125f,22.65f),3.15f),("Sorting service",P(4.125f,-1.05f),P(4.125f,2.55f),3.15f),
        ("Power service",P(5.325f,-16.35f),P(5.325f,-7.65f),3.15f),("Bay service link",P(8.7f,5.85f),P(18,5.85f),3),
        ("Service storage link",P(8.7f,-19.65f),P(18,-19.65f),3),("East perimeter",P(21.3f,-16.35f),P(21.3f,22.65f),3),
        ("Storage south",P(-7.5f,-30.15f),P(18,-30.15f),3),("Storage west",P(-10.8f,-26.85f),P(-10.8f,-20.25f),3),
        ("Freight power link",P(-10.8f,-16.35f),P(-10.8f,-13.35f),3)
    };
    [MenuItem("NO RETURNS/Trials/Validate Compact Cinder Site Review")]
    public static void Validate() => ValidateWithPrefix("site-");
    public static void ValidateWithPrefix(string prefix) {
        if(EditorSceneManager.GetActiveScene().path!=ScenePath)throw new Exception("Open compact site review.");Physics.SyncTransforms();
        var baseline=Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(Path.Combine(Output,"site-local-collision-baseline.json")));
        var original=GameObject.Find("Cinder Depot editable primitive blockout").transform;
        foreach(string n in Names)if((string)baseline["buildings"][n]!=LocalCollision(original.Find(n)))throw new Exception("Local building collision changed: "+n);
        if((string)baseline["ship"]!=LocalCollision(GameObject.Find("Reused Ship Interior Trial 05").transform))throw new Exception("Local ship collision changed");
        if(original.GetComponentsInChildren<BoxCollider>().Length!=59)throw new Exception("Legacy exterior geometry was not replaced cleanly");
        var combined=Root.Find("Compact yard and utility roofs").GetComponent<MeshFilter>().sharedMesh;
        if(combined.uv.Length!=combined.vertexCount||combined.uv.Any(v=>v.x<0||v.x>1||v.y<0||v.y>1))throw new Exception("Combined surface UVs invalid");
        foreach(var filter in Root.GetComponentsInChildren<MeshFilter>())if(Vector3.Distance(filter.transform.lossyScale,Vector3.one)>.0001f)throw new Exception("Stretched site module");
        var results=new List<string>();var probes=new List<CharacterController>();var parcel=GameObject.Find("Trial carried parcel").GetComponent<Collider>();bool enabled=parcel.enabled;parcel.enabled=false;
        CharacterController Probe(){var obj=new GameObject("Compact passage probe");var cc=obj.AddComponent<CharacterController>();cc.height=1.8f;cc.radius=.34f;cc.center=Vector3.up*.9f;cc.skinWidth=.035f;cc.stepOffset=.32f;probes.Add(cc);return cc;}
        void Set(CharacterController cc,Vector3 p){cc.enabled=false;cc.transform.position=p;cc.enabled=true;Physics.SyncTransforms();}
        try {
            var cc=Probe();
            foreach(var route in Routes())foreach(bool reverse in new[]{false,true}) {
                var a=reverse?route.b:route.a;var b=reverse?route.a:route.b;Set(cc,a);
                for(int step=0;step<3000;step++){var d=b-cc.transform.position;d.y=0;if(d.magnitude<.04f)break;cc.Move(Vector3.ClampMagnitude(d,.08f)+Vector3.down*.025f);}
                var error=cc.transform.position-b;error.y=0;if(error.magnitude>.08f)throw new Exception("Blocked compact route: "+route.name+" error "+error.magnitude);
                results.Add("PASS "+route.name+(reverse?" reverse":" forward"));
            }
            UnityEngine.Object.DestroyImmediate(cc.gameObject);probes.Clear();var crew=Enumerable.Range(0,4).Select(_=>Probe()).ToArray();
            foreach(var lane in CrewLanes()) {
                var forward=(lane.b-lane.a).normalized;var across=Vector3.Cross(forward,Vector3.up);float measured=float.PositiveInfinity;
                foreach(var body in crew)body.enabled=false;
                // Door openings widen some cross-sections; measure the minimum between opposed wall faces.
                foreach(float fraction in new[]{.17f,.5f,.83f}) {
                    var mid=Vector3.Lerp(lane.a,lane.b,fraction)+Vector3.up*.9f;
                    if(Physics.Raycast(mid,across,out var left,4,1<<0)&&Physics.Raycast(mid,-across,out var right,4,1<<0))measured=Mathf.Min(measured,left.distance+right.distance);
                }
                if(Mathf.Abs(measured-lane.width)>.001f)throw new Exception("Measured lane width changed: "+lane.name+" / "+measured);
                for(int member=0;member<4;member++)Set(crew[member],lane.a+across*((member-1.5f)*.71f));
                for(int step=0;step<Mathf.CeilToInt(Vector3.Distance(lane.a,lane.b)/.08f)+2;step++)for(int member=0;member<4;member++) {
                    var d=lane.b+across*((member-1.5f)*.71f)-crew[member].transform.position;d.y=0;crew[member].Move(Vector3.ClampMagnitude(d,.08f)+Vector3.down*.025f);
                }
                for(int member=0;member<4;member++){var e=crew[member].transform.position-(lane.b+across*((member-1.5f)*.71f));e.y=0;if(e.magnitude>.08f)throw new Exception("Four bodies blocked: "+lane.name);}
                results.Add("PASS four bodies / "+lane.name+" / clear "+lane.width.ToString("F3")+"m");
            }
        } finally {foreach(var cc in probes)if(cc)UnityEngine.Object.DestroyImmediate(cc.gameObject);parcel.enabled=enabled;}
        if(GameObject.Find("Cinder compact maze partitions"))throw new Exception("Rejected interior maze leaked into site.");
        foreach(string name in Names){var b=Floor(name);if(!Field.Contains(new Vector2(b.min.x,b.min.z))||!Field.Contains(new Vector2(b.max.x,b.max.z)))throw new Exception("Building outside field");}
        File.WriteAllLines(Path.Combine(Output,prefix+"passage-validation.txt"),results);
        var result=new {scene=ScenePath,field_size_m=new[]{Field.width,Field.height},core_site_size_m=new[]{47.55f,59.4f},building_count=5,solid_auxiliary_blocks=Solids.Length,original_building_box_colliders=50,relocated_ship_collision_preserved=true,original_remaining_box_colliders=59,
            building_layout=Names.Select(n=>new{name=n,center=Floor(n).center.ToString("F3"),size=Floor(n).size.ToString("F3")}),standard_alley_width_m=3,slightly_wider_alley_m=3.15f,
            movement_segments=Routes().Count*2,four_body_lanes=CrewLanes().Count,closed_site_loops=Loops.Length,interior_maze_removed=true,field_is_physical_wall=false,
            sheltered_route_m=Delivery(true).Zip(Delivery(true).Skip(1),(a,b)=>Vector3.Distance(a,b)).Sum(),central_route_m=Delivery(false).Zip(Delivery(false).Skip(1),(a,b)=>Vector3.Distance(a,b)).Sum(),
            surfaces_vertices=Root.Find("Compact yard and utility roofs").GetComponent<MeshFilter>().sharedMesh.vertexCount,added_colliders=Root.GetComponentsInChildren<Collider>().Length,added_lights=Root.GetComponentsInChildren<Light>().Length,kit_mesh_instances=Root.GetComponentsInChildren<MeshFilter>().Length-1,
            user_layout_review=false,human_four_player_review=false};
        File.WriteAllText(Path.Combine(Output,prefix+"unity-validation.json"),Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");Debug.Log("COMPACT CINDER SITE PASS: "+results.Count);
    }
}
}
