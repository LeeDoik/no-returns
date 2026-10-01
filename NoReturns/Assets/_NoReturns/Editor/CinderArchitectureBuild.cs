using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shape = NoReturns.Editor.CinderSitePropsBuild.Shape;

namespace NoReturns.Editor {
public static class CinderArchitectureBuild {
    public const string RootName = "Cinder varied architecture";
    const string Art = "Assets/_NoReturns/Art/CinderArchitecture01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static readonly Vector2[] Chamfer = {P(-2.25f,-3.6f),P(2.25f,-3.6f),P(3.15f,-2.7f),P(3.15f,2.7f),P(2.25f,3.6f),P(-2.25f,3.6f),P(-3.15f,2.7f),P(-3.15f,-2.7f)};
    static readonly Vector2[] Step = {P(-3.15f,-8.55f),P(3.15f,-8.55f),P(3.15f,4.95f),P(1.95f,4.95f),P(1.95f,8.55f),P(-3.15f,8.55f)};
    static readonly string[] Replaced = {"Sorting island","North control annex"};
    static readonly Vector3[] GroundCenters = {new Vector3(-.6f,0,.75f),new Vector3(-.6f,0,15.9f)};
    static Vector2 P(float x,float z) => new Vector2(x,z);
    static Vector3 V(Vector2 p,float y) => new Vector3(p.x,y,p.y);
    static Transform Site => GameObject.Find("Cinder compact suppression site").transform;
    static Transform Root => GameObject.Find(RootName).transform;
    static Material Surface => AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
    static float Cross(Vector2 a,Vector2 b) => a.x*b.y-a.y*b.x;
    static List<int> Triangulate(Vector2[] polygon) {
        var remaining=Enumerable.Range(0,polygon.Length).ToList();var triangles=new List<int>();
        while(remaining.Count>2) {
            bool found=false;
            for(int i=0;i<remaining.Count;i++) {
                int a=remaining[(i+remaining.Count-1)%remaining.Count],b=remaining[i],c=remaining[(i+1)%remaining.Count];
                if(Cross(polygon[b]-polygon[a],polygon[c]-polygon[b])<=.00001f)continue;
                if(remaining.Any(n=>n!=a&&n!=b&&n!=c&&Cross(polygon[b]-polygon[a],polygon[n]-polygon[a])>=0&&Cross(polygon[c]-polygon[b],polygon[n]-polygon[b])>=0&&Cross(polygon[a]-polygon[c],polygon[n]-polygon[c])>=0))continue;
                triangles.AddRange(new[]{a,b,c});remaining.RemoveAt(i);found=true;break;
            }
            if(!found)throw new Exception("Invalid architecture polygon");
        }
        return triangles;
    }
    static Vector2[] UV(string region,float width=1,float height=1) {
        var r=region=="wall"?new Rect(1,1,254,510):region=="floor"?new Rect(257,257,254,254):region=="ceiling"?new Rect(257,129,126,126):new Rect(385,129,126,126);
        return new[]{P(r.x,r.y),P(r.x+r.width*width,r.y),P(r.x+r.width*width,r.y+r.height*height),P(r.x,r.y+r.height*height)}.Select(p=>p/512).ToArray();
    }
    static void Tiles(Shape shape,Vector3 a,Vector3 b,Vector3 c,Vector3 d,string region) {
        float width=Vector3.Distance(a,b),height=Vector3.Distance(a,d);
        for(float u=0;u<width-.001f;u+=1.2f)for(float v=0;v<height-.001f;v+=1.2f) {
            float u1=Mathf.Min(u+1.2f,width),v1=Mathf.Min(v+1.2f,height);
            Vector3 At(float x,float y) => a+(b-a)*(x/width)+(d-a)*(y/height);
            shape.Quad(At(u,v),At(u1,v),At(u1,v1),At(u,v1),false,UV(region,(u1-u)/1.2f,(v1-v)/1.2f));
        }
    }
    static Shape Prism(Vector2[] polygon,float height) {
        var shape=new Shape();
        for(int i=0;i<polygon.Length;i++) {
            var a=polygon[i];var b=polygon[(i+1)%polygon.Length];float length=Vector2.Distance(a,b);
            for(float u=0;u<length-.001f;u+=1.2f) {
                var p=Vector2.Lerp(a,b,u/length);var q=Vector2.Lerp(a,b,Mathf.Min(u+1.2f,length)/length);
                float wall=Mathf.Min(4,height);
                shape.Quad(V(q,0),V(p,0),V(p,wall),V(q,wall),false,UV("wall",Vector2.Distance(p,q)/1.2f,wall/4));
                if(height>4)shape.Quad(V(q,4),V(p,4),V(p,height),V(q,height),false,UV("graphite"));
            }
        }
        var indices=Triangulate(polygon);
        for(int i=0;i<indices.Count;i+=3) {
            var a=polygon[indices[i]];var b=polygon[indices[i+1]];var c=polygon[indices[i+2]];
            shape.Triangle(V(a,0),V(b,0),V(c,0),false);
            CapTiles(shape,new[]{a,b,c},height);
        }
        return shape;
    }
    // Clip each top triangle to 1.2m atlas tiles, retaining density on concave and chamfered caps.
    static void CapTiles(Shape shape,Vector2[] triangle,float height) {
        List<Vector2> Clip(List<Vector2> input,int axis,float boundary,bool lower) {
            var output=new List<Vector2>();
            for(int i=0;i<input.Count;i++) {
                var a=input[i];var b=input[(i+1)%input.Count];float da=a[axis]-boundary,db=b[axis]-boundary;
                bool insideA=lower?da>=0:da<=0,insideB=lower?db>=0:db<=0;
                if(insideA)output.Add(a);
                if(insideA!=insideB)output.Add(Vector2.Lerp(a,b,da/(da-db)));
            }
            return output;
        }
        float x0=Mathf.Floor(triangle.Min(p=>p.x)/1.2f)*1.2f,z0=Mathf.Floor(triangle.Min(p=>p.y)/1.2f)*1.2f;
        for(float x=x0;x<triangle.Max(p=>p.x);x+=1.2f)for(float z=z0;z<triangle.Max(p=>p.y);z+=1.2f) {
            var piece=triangle.ToList();piece=Clip(piece,0,x,true);piece=Clip(piece,0,x+1.2f,false);piece=Clip(piece,1,z,true);piece=Clip(piece,1,z+1.2f,false);
            Vector2 Map(Vector2 p) => P(257+(p.x-x)/1.2f*126,129+(p.y-z)/1.2f*126)/512;
            for(int i=1;i<piece.Count-1;i++)if(Mathf.Abs(Cross(piece[i]-piece[0],piece[i+1]-piece[0]))>1e-6f)
                shape.Triangle(V(piece[0],height),V(piece[i+1],height),V(piece[i],height),false,new[]{Map(piece[0]),Map(piece[i+1]),Map(piece[i])});
        }
    }
    static Shape Roof(Vector2[] section,float width) {
        var shape=new Shape();
        Vector3 At(Vector2 p,float x) => new Vector3(x,p.y,p.x);
        for(int i=0;i<section.Length;i++) {
            var a=section[i];var b=section[(i+1)%section.Length];
            Tiles(shape,At(b,-width/2),At(a,-width/2),At(a,width/2),At(b,width/2),"ceiling");
        }
        var indices=Triangulate(section);
        for(int i=0;i<indices.Count;i+=3) {
            var a=section[indices[i]];var b=section[indices[i+1]];var c=section[indices[i+2]];
            shape.Triangle(At(a,-width/2),At(b,-width/2),At(c,-width/2),false);
            shape.Triangle(At(a,width/2),At(c,width/2),At(b,width/2),false);
        }
        return shape;
    }
    static void MakeMeshes(bool replace=false) {
        Directory.CreateDirectory(Art);AssetDatabase.Refresh();
        Prism(Chamfer,4.3f).Save("Chamfer utility",Art,replace);Prism(Step,4.3f).Save("Stepped annex",Art,replace);
        Roof(new[]{P(-3.4f,0),P(3.4f,0),P(3.4f,1.5f)},7.2f).Save("Sawtooth roof",Art,replace);
        var arch=new List<Vector2>{P(-3.6f,0),P(3.6f,0)};
        for(int i=0;i<=8;i++){float angle=i*Mathf.PI/8;arch.Add(P(3.6f*Mathf.Cos(angle),.2f+1.7f*Mathf.Sin(angle)));}
        Roof(arch.ToArray(),7.2f).Save("Vault roof",Art,replace);
        var octagon=Enumerable.Range(0,8).Select(i=>P(Mathf.Cos(i*Mathf.PI/4)*2.4f,Mathf.Sin(i*Mathf.PI/4)*2.4f)).ToArray();
        Prism(octagon,3.4f).Save("Octagonal control tower",Art,replace);
        Prism(new[]{P(-4,-3),P(4,-3),P(4,0),P(0,0),P(0,3),P(-4,3)},2.1f).Save("L upper annex",Art,replace);
        Prism(new[]{P(-1.5f,-2.7f),P(1.5f,-2.7f),P(1.95f,-2.25f),P(1.95f,2.25f),P(1.5f,2.7f),P(-1.5f,2.7f),P(-1.95f,2.25f),P(-1.95f,-2.25f)},2.2f).Save("Raised plant room",Art,replace);
        var s=new Shape();s.Box(new Vector3(0,.15f,0),new Vector3(4.6f,.3f,1.2f),true);
        foreach(float x in new[]{-2.15f,2.15f})s.Box(new Vector3(x,-.17f,.28f),new Vector3(.18f,.64f,.12f));s.Save("Entry hood",Art,replace);
        s=new Shape();s.Cylinder(.68f,0,4,true);s.Cylinder(.83f,3.75f,4);s.Cylinder(.83f,0,.18f);s.Save("Industrial stack",Art,replace);
        s=new Shape();
        foreach(var r in new[]{new Rect(-3.6f,-2.7f,6,6.9f),new Rect(-3.6f,7.5f,6,16.8f)})
            Tiles(s,new Vector3(r.xMin,.002f,r.yMin),new Vector3(r.xMin,.002f,r.yMax),new Vector3(r.xMax,.002f,r.yMax),new Vector3(r.xMax,.002f,r.yMin),"floor");
        s.Save("Notch floor infill",Art,replace);
    }
    static GameObject Place(string mesh,Vector3 position,float yaw=0,bool collision=true) {
        var obj=new GameObject(mesh);obj.transform.SetParent(Root);obj.transform.SetPositionAndRotation(position,Quaternion.Euler(0,yaw,0));
        var asset=AssetDatabase.LoadAssetAtPath<Mesh>(Art+"/"+mesh+".asset");if(!asset)throw new Exception("Missing architecture mesh "+mesh);
        obj.AddComponent<MeshFilter>().sharedMesh=asset;obj.AddComponent<MeshRenderer>().sharedMaterial=Surface;
        if(collision)obj.AddComponent<MeshCollider>().sharedMesh=asset;
        return obj;
    }
    static string ObjectPath(Component c) => string.Join("/",c.GetComponentsInParent<Transform>(true).Reverse().Select(t=>t.name));
    internal static string PreservedState() {
        bool Altered(Component c) => c.GetComponentsInParent<Transform>(true).Any(t=>t.name==RootName||Replaced.Contains(t.name));
        var colliders=UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsInactive.Include).Where(c=>!Altered(c)).OrderBy(c=>ObjectPath(c)+c.bounds.ToString("F6"),StringComparer.Ordinal);
        var lights=UnityEngine.Object.FindObjectsByType<Light>(FindObjectsInactive.Include).OrderBy(l=>ObjectPath(l)+l.transform.position.ToString("F6"),StringComparer.Ordinal);
        return Newtonsoft.Json.JsonConvert.SerializeObject(new {
            colliders=colliders.Select(c=>new{path=ObjectPath(c),c.enabled,c.isTrigger,c.gameObject.layer,box_center=c is BoxCollider box?box.center.ToString("F6"):null,box_size=c is BoxCollider sized?sized.size.ToString("F6"):null,position=c.transform.position.ToString("F6"),rotation=c.transform.rotation.ToString("F6"),scale=c.transform.lossyScale.ToString("F6"),bounds=c.bounds.ToString("F6")}),
            lights=lights.Select(l=>new{path=ObjectPath(l),l.enabled,l.intensity,l.range,color=new[]{l.color.r,l.color.g,l.color.b},position=l.transform.position.ToString("F6"),rotation=l.transform.rotation.ToString("F6")}),
            sky=AssetDatabase.GetAssetPath(RenderSettings.skybox),RenderSettings.fog,RenderSettings.fogStartDistance,RenderSettings.fogEndDistance
        });
    }
    [MenuItem("NO RETURNS/Trials/Add Varied Cinder Architecture")]
    public static void Create() {
        var scene=EditorSceneManager.GetActiveScene();
        if(scene.path!=CinderCompactSiteBuild.ScenePath||scene.isDirty||EditorApplication.isPlaying||GameObject.Find(RootName))throw new Exception("Open saved compact site, stop Play and preserve existing architecture before rebuilding.");
        string baseline=PreservedState();var original=Site.Find("Compact yard and utility roofs").GetComponent<MeshFilter>();var originalMesh=original.sharedMesh;
        MakeMeshes();var root=new GameObject(RootName);
        try {
            for(int i=0;i<Replaced.Length;i++)Site.Find(Replaced[i]).gameObject.SetActive(false);
            var mesh=UnityEngine.Object.Instantiate(originalMesh);mesh.name="Architecture yard and retained roofs";
            var v=mesh.vertices;var indices=mesh.triangles;var keep=new List<int>();
            for(int i=0;i<indices.Length;i+=3) {
                var center=(v[indices[i]]+v[indices[i+1]]+v[indices[i+2]])/3;
                bool roof=center.y>3.9f&&center.x>-3.751f&&center.x<2.551f&&((center.z>-2.851f&&center.z<4.351f)||(center.z>7.349f&&center.z<24.451f));
                if(!roof)keep.AddRange(new[]{indices[i],indices[i+1],indices[i+2]});
            }
            mesh.SetTriangles(keep,0);AssetDatabase.CreateAsset(mesh,Art+"/ArchitectureYard.asset");original.sharedMesh=mesh;
            Place("Chamfer utility",GroundCenters[0]);Place("Stepped annex",GroundCenters[1]);Place("Notch floor infill",Vector3.zero,0,false);
            for(int x=0;x<2;x++)for(int z=0;z<3;z++)Place("Sawtooth roof",new Vector3(-17.7f+x*7.2f,4.3f,-5f+z*6.8f));
            for(int i=0;i<4;i++)Place("Vault roof",new Vector3(-5.55f+i*7.2f,4.3f,-24.9f));
            Place("L upper annex",new Vector3(-14.1f,4.3f,19.8f));Place("Octagonal control tower",new Vector3(15.9f,4.3f,20.7f));
            Place("Raised plant room",new Vector3(-.9f,4.3f,14.1f));Place("Raised plant room",new Vector3(12,4.3f,-8.7f));Place("Raised plant room",new Vector3(17,4.3f,-2.1f),90);
            foreach(var p in new[]{new Vector3(9,4.3f,-14.7f),new Vector3(15.6f,4.3f,-14.7f)})Place("Industrial stack",p);
            foreach(var p in new[]{new Vector3(-14.1f,3.85f,12.95f),new Vector3(-14.1f,3.85f,14.7f),new Vector3(12.75f,3.85f,6.85f),new Vector3(12.75f,3.85f,24.95f),new Vector3(13.35f,3.85f,-18.6f),new Vector3(5.25f,3.85f,-29.3f)})Place("Entry hood",p);
            Physics.SyncTransforms();
            if(PreservedState()!=baseline)throw new Exception("Architecture altered retained colliders, lights or sky.");
            File.WriteAllText(Path.Combine(Output,"architecture-preserved-state.json"),baseline+"\n");
            Validate();AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        } catch {
            original.sharedMesh=originalMesh;foreach(string name in Replaced)Site.Find(name).gameObject.SetActive(true);
            UnityEngine.Object.DestroyImmediate(root);throw;
        }
    }
    public static List<Vector3> PosePoints() {
        var points=CinderSitePropsBuild.PosePoints();
        for(int group=0;group<2;group++) {
            var polygon=group==0?Chamfer:Step;
            for(int i=0;i<polygon.Length;i++) {
                var a=polygon[i];var b=polygon[(i+1)%polygon.Length];var d=(b-a).normalized;var normal=P(d.y,-d.x);
                foreach(float t in new[]{.05f,.5f,.95f})points.Add(GroundCenters[group]+V(Vector2.Lerp(a,b,t)+normal*.525f,.035f));
            }
        }
        return points.Distinct().Where(p=>!Physics.CheckCapsule(p+Vector3.up*.34f,p+Vector3.up*1.46f,.34f,1<<0,QueryTriggerInteraction.Ignore)).ToList();
    }
    [MenuItem("NO RETURNS/Trials/Validate Varied Cinder Architecture")]
    public static void Validate() {
        if(GameObject.Find(CinderBackgroundBuild.RootName)) { CinderBackgroundBuild.Validate();return; }
        if(EditorSceneManager.GetActiveScene().path!=CinderCompactSiteBuild.ScenePath||!GameObject.Find(RootName))throw new Exception("Open architecture-enabled compact site.");
        if(PreservedState()!=File.ReadAllText(Path.Combine(Output,"architecture-preserved-state.json")).TrimEnd())throw new Exception("Retained scene state changed.");
        CinderCompactSiteBuild.ValidateWithPrefix("architecture-");
        var props=GameObject.Find(CinderSitePropsBuild.RootName).transform;
        if(props.childCount!=47||props.GetComponentsInChildren<UnityEngine.UI.Text>().Length!=6)throw new Exception("Retained props or physical labels changed.");
        var meshes=Root.GetComponentsInChildren<MeshFilter>();
        foreach(var f in meshes) {
            var m=f.sharedMesh;var v=m.vertices;var t=m.triangles;
            if(f.transform.lossyScale!=Vector3.one||m.uv.Length!=m.vertexCount||m.uv.Any(p=>p.x<0||p.x>1||p.y<0||p.y>1))throw new Exception("Architecture scale/UV invalid "+f.name);
            for(int i=0;i<t.Length;i+=3)if(Vector3.Cross(v[t[i+1]]-v[t[i]],v[t[i+2]]-v[t[i]]).sqrMagnitude<1e-12f)throw new Exception("Degenerate architecture triangle "+f.name);
            if(f.GetComponent<MeshCollider>()) {
                float volume=0;for(int i=0;i<t.Length;i+=3)volume+=Vector3.Dot(v[t[i]],Vector3.Cross(v[t[i+1]],v[t[i+2]]))/6;
                if(volume<=0)throw new Exception("Architecture winding/volume invalid "+f.name);
            }
        }
        var result=new {scene=CinderCompactSiteBuild.ScenePath,replaced_auxiliary_footprints=2,ground_shapes=new[]{"8-sided chamfer","6-sided stepped outline"},
            meshes=AssetDatabase.FindAssets("t:Mesh",new[]{Art}).Select(AssetDatabase.GUIDToAssetPath),placements=meshes.Length,triangles=meshes.Sum(f=>f.sharedMesh.triangles.Length/3),
            new_static_mesh_colliders=Root.GetComponentsInChildren<MeshCollider>().Length,local_lights=UnityEngine.Object.FindObjectsByType<Light>().Count(l=>l.type!=LightType.Directional),
            retained_state_preserved=true,carrying_pose_positions=PosePoints().Count,highest_architecture_m=8.3f,unit_scale=true,preserved_prop_groups=props.childCount,physical_labels=6,interactive_upper_rooms=false,user_architecture_quality_review=false};
        File.WriteAllText(Path.Combine(Output,"architecture-layout-validation.json"),Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");
        Debug.Log("CINDER ARCHITECTURE PASS: "+meshes.Length+" placements");
    }
}
}
