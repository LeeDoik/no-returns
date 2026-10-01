using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Shape = NoReturns.Editor.CinderSitePropsBuild.Shape;

namespace NoReturns.Editor {
public static class CinderBackgroundBuild {
    public const string RootName = "Cinder exterior background";
    const string Art = "Assets/_NoReturns/Art/CinderBackground01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static Transform Root => GameObject.Find(RootName).transform;
    static Vector3 V(float x,float y,float z) => new Vector3(x,y,z);
    static Material Material(string name,Color color) {
        string path=Art+"/"+name+".mat";
        var material=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(!material) {
            var shader=Shader.Find("Universal Render Pipeline/Lit");
            if(!shader||!shader.isSupported)throw new Exception("Supported URP Lit shader required.");
            material=new Material(shader);
            AssetDatabase.CreateAsset(material,path);
        }
        material.color=color;material.SetFloat("_Smoothness",0);
        material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"/Weathered mineral.asset"));EditorUtility.SetDirty(material);
        return material;
    }
    static void MineralTexture() {
        string path=Art+"/Weathered mineral.asset";
        if(AssetDatabase.LoadAssetAtPath<Texture2D>(path))return;
        var texture=new Texture2D(128,128,TextureFormat.RGBA32,true){name="Weathered mineral",wrapMode=TextureWrapMode.Repeat,filterMode=FilterMode.Point};
        var pixels=new Color[128*128];var random=new System.Random(87);
        for(int y=0;y<128;y++)for(int x=0;x<128;x++) {
            float n=Mathf.PerlinNoise(x*.07f+11,y*.07f+27),grain=(float)random.NextDouble();
            float shade=.5f+n*.3f+grain*.16f;
            if(grain<.035f)shade*=.5f;
            pixels[y*128+x]=new Color(shade,shade*.97f,shade*.94f);
        }
        texture.SetPixels(pixels);texture.Apply();AssetDatabase.CreateAsset(texture,path);
    }
    static void Facet(Shape shape,Vector3 a,Vector3 b,Vector3 c) {
        var n=Vector3.Cross(b-a,c-a);n=V(Mathf.Abs(n.x),Mathf.Abs(n.y),Mathf.Abs(n.z));
        Vector2 UV(Vector3 p) => n.y>=n.x&&n.y>=n.z?new Vector2(p.x,p.z)*.35f:n.x>n.z?new Vector2(p.z,p.y)*.35f:new Vector2(p.x,p.y)*.35f;
        shape.Triangle(a,b,c,false,new[]{UV(a),UV(b),UV(c)});
    }
    static void Rock(string name,Vector3 size,int seed) {
        var shape=new Shape();var random=new System.Random(seed);var rings=new Vector3[4,7];
        float[] widths={1,.94f,.66f,.18f},heights={-.25f,.18f,.66f,1};
        for(int layer=0;layer<4;layer++)for(int i=0;i<7;i++) {
            float angle=i*Mathf.PI*2/7,irregular=.82f+(float)random.NextDouble()*.18f;
            rings[layer,i]=V(Mathf.Cos(angle)*size.x*.5f*widths[layer]*irregular+layer*size.x*.045f,
                size.y*(heights[layer]+(layer==0?0:((float)random.NextDouble()-.5f)*.15f)),
                Mathf.Sin(angle)*size.z*.5f*widths[layer]*irregular-layer*size.z*.025f);
        }
        for(int layer=0;layer<3;layer++)for(int i=0;i<7;i++) {
            int j=(i+1)%7;
            Facet(shape,rings[layer,j],rings[layer,i],rings[layer+1,i]);
            Facet(shape,rings[layer,j],rings[layer+1,i],rings[layer+1,j]);
        }
        for(int i=0;i<7;i++) {
            int j=(i+1)%7;
            Facet(shape,V(0,-size.y*.25f,0),rings[0,i],rings[0,j]);
            Facet(shape,V(size.x*.135f,size.y*1.02f,-size.z*.075f),rings[3,j],rings[3,i]);
        }
        shape.Save(name,Art,true);
    }
    static void MakeMeshes() {
        Directory.CreateDirectory(Art);AssetDatabase.Refresh();
        MineralTexture();
        Rock("Basalt crag",V(12,8.5f,9),17);Rock("Split ridge",V(26,13,17),29);
        Rock("Distant peak",V(35,23,29),43);Rock("Loose boulder",V(3.4f,2.1f,2.8f),61);
        var terrain=new Shape();var field=CinderCompactSiteBuild.Field;
        var rectangles=new[]{new Rect(field.xMin-.6f,field.yMin-.6f,field.width+1.2f,field.height+1.2f),new Rect(-62,-54,124,108),new Rect(-105,-95,210,190),new Rect(-150,-140,300,280)};
        var rings=new List<Vector3[]>();
        for(int ring=0;ring<rectangles.Length;ring++) {
            var r=rectangles[ring];var corners=new[]{V(r.xMin,0,r.yMin),V(r.xMin,0,r.yMax),V(r.xMax,0,r.yMax),V(r.xMax,0,r.yMin)};
            var points=new List<Vector3>();
            for(int side=0;side<4;side++)for(int i=0;i<12;i++) {
                var p=Vector3.Lerp(corners[side],corners[(side+1)%4],i/12f);
                p.y=ring==0?-.015f:ring==1?.06f:1.2f+(ring-2)*2+Mathf.PerlinNoise(p.x*.055f+10,p.z*.055f+20)*4;
                points.Add(p);
            }
            rings.Add(points.ToArray());
        }
        for(int ring=0;ring<3;ring++)for(int i=0;i<48;i++) {
            int j=(i+1)%48;
            Facet(terrain,rings[ring][i],rings[ring+1][j],rings[ring][j]);
            Facet(terrain,rings[ring][i],rings[ring+1][i],rings[ring+1][j]);
        }
        terrain.Save("Barren basin",Art,true);
        var s=new Shape();s.Cylinder(3.2f,0,25);s.Cylinder(3.5f,1,1.4f,true);s.Cylinder(3.5f,23.4f,23.8f,true);s.Cylinder(2.4f,25,26);
        foreach(float x in new[]{-2.4f,2.4f})foreach(float z in new[]{-2.4f,2.4f})s.Box(V(x,1,z),V(.5f,2,.5f));
        s.Save("Abandoned silo",Art);
        s=new Shape();s.Box(V(0,2.3f,0),V(13,4.6f,8));s.Box(V(-2,5.2f,0),V(7,1.2f,8.6f),true);
        foreach(float x in new[]{-4f,3f})s.Box(V(x,4.5f,-4.1f),V(2,1,.4f),true);
        s.Save("Distant refinery",Art);
        s=new Shape();foreach(float x in new[]{-4f,4f})s.Box(V(x,9,0),V(.6f,18,.6f),true);
        for(int i=0;i<6;i++) {
            s.Box(V(0,2+i*3,0),V(8,.35f,.6f));
            // ponytail: fixed six-bay silhouette; detailed lattice only if close access is added.
            float y=2+i*3;s.Quad(V(-4,y,.32f),V(4,y+2.6f,.32f),V(4,y+3,.32f),V(-4,y+.4f,.32f),true);
            s.Quad(V(-4,y+.4f,-.32f),V(4,y+3,-.32f),V(4,y+2.6f,-.32f),V(-4,y,-.32f),true);
        }
        s.Save("Derelict gantry",Art);AssetDatabase.SaveAssets();
    }
    static void Place(string mesh,Vector3 position,float yaw,Material material,string label=null) {
        var obj=new GameObject(label??mesh);obj.transform.SetParent(Root);obj.transform.SetPositionAndRotation(position,Quaternion.Euler(0,yaw,0));
        var asset=AssetDatabase.LoadAssetAtPath<Mesh>(mesh);if(!asset)throw new Exception("Missing background mesh "+mesh);
        obj.AddComponent<MeshFilter>().sharedMesh=asset;var renderer=obj.AddComponent<MeshRenderer>();renderer.sharedMaterial=material;
        renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
    }
    static void Model(string mesh,Vector3 p,float yaw,Material material) => Place(Art+"/"+mesh+".asset",p,yaw,material,mesh);
    static string State() => Newtonsoft.Json.JsonConvert.SerializeObject(new {
        retained=CinderArchitectureBuild.PreservedState(),
        architecture=GameObject.Find(CinderArchitectureBuild.RootName).GetComponentsInChildren<MeshFilter>().OrderBy(f=>f.name+f.transform.position.ToString("F6"),StringComparer.Ordinal)
            .Select(f=>new{f.name,mesh=AssetDatabase.GetAssetPath(f.sharedMesh),position=f.transform.position.ToString("F6"),rotation=f.transform.rotation.ToString("F6"),scale=f.transform.lossyScale.ToString("F6"),collision=f.GetComponent<Collider>()?f.GetComponent<Collider>().bounds.ToString("F6"):null}),
        fog_color=new[]{RenderSettings.fogColor.r,RenderSettings.fogColor.g,RenderSettings.fogColor.b},RenderSettings.fogMode,RenderSettings.ambientMode,
        ambient_color=new[]{RenderSettings.ambientLight.r,RenderSettings.ambientLight.g,RenderSettings.ambientLight.b},
        sky_exposure=RenderSettings.skybox.GetFloat("_Exposure"),sky_rotation=RenderSettings.skybox.GetFloat("_Rotation")
    });
    [MenuItem("NO RETURNS/Trials/Add Cinder Exterior Background")]
    public static void Create() {
        var scene=EditorSceneManager.GetActiveScene();
        if(scene.path!=CinderCompactSiteBuild.ScenePath||scene.isDirty||EditorApplication.isPlaying||GameObject.Find(RootName)||!GameObject.Find(CinderArchitectureBuild.RootName))throw new Exception("Open saved architecture site, stop Play and preserve existing background before rebuilding.");
        string baseline=State();MakeMeshes();var root=new GameObject(RootName);
        var guards=new[]{"North boundary","South boundary","West boundary","East boundary"}.Select(n=>GameObject.Find(n).GetComponent<Renderer>()).ToArray();
        var enabled=guards.Select(r=>r.enabled).ToArray();
        try {
            var basalt=Material("Weathered basalt",new Color(.45f,.39f,.43f));var ash=Material("Ash and gravel",new Color(.40f,.32f,.30f));
            var surface=AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
            Model("Barren basin",Vector3.zero,0,ash);
            for(int i=0;i<8;i++)Model(i%3==0?"Split ridge":"Basalt crag",V(-48-(i%2)*4,0,-28+i*9),i*47,basalt);
            for(int i=0;i<8;i++)Model(i%3==1?"Split ridge":"Basalt crag",V(46+(i%2)*5,0,-29+i*9),i*31,basalt);
            for(int i=0;i<6;i++)Model(i%2==0?"Split ridge":"Basalt crag",V(-26+i*11,0,51+(i%2)*5),i*51,basalt);
            foreach(float x in new[]{-43f,-7,6,20,37})Model("Basalt crag",V(x,0,-46),x*8,basalt);
            for(int i=0;i<12;i++) {
                float angle=(i*29+8)*Mathf.Deg2Rad;
                Model("Distant peak",V(Mathf.Cos(angle)*88,2,Mathf.Sin(angle)*85),i*37,basalt);
            }
            for(int i=0;i<20;i++) {
                bool east=i%2==0;float z=-31+(i/2)*6.5f;
                Model("Loose boulder",V(east?29.5f+i%3:-31.5f-i%3,0,z),i*67,i%3==0?ash:basalt);
            }
            Model("Distant refinery",V(42,1,66),12,surface);Model("Distant refinery",V(23,1,72),-8,surface);
            foreach(var p in new[]{V(34,1,61),V(42,1,81),V(51,1,72)})Model("Abandoned silo",p,0,surface);
            Model("Derelict gantry",V(21,1,64),12,surface);
            foreach(var p in new[]{V(34,27,61),V(42,27,81),V(51,27,72)})
                Place("Assets/_NoReturns/Art/CinderArchitecture01/Industrial stack.asset",p,0,surface,"Refinery exhaust");
            foreach(var guard in guards)guard.enabled=false;
            Physics.SyncTransforms();if(State()!=baseline)throw new Exception("Background altered retained scene state.");
            File.WriteAllText(Path.Combine(Output,"background-preserved-state.json"),baseline+"\n");
            Validate();AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        } catch {
            for(int i=0;i<guards.Length;i++)guards[i].enabled=enabled[i];UnityEngine.Object.DestroyImmediate(root);throw;
        }
    }
    [MenuItem("NO RETURNS/Trials/Validate Cinder Exterior Background")]
    public static void Validate() {
        if(EditorSceneManager.GetActiveScene().path!=CinderCompactSiteBuild.ScenePath||!GameObject.Find(RootName)||EditorApplication.isPlaying)throw new Exception("Open background site and stop Play.");
        if(State()!=File.ReadAllText(Path.Combine(Output,"background-preserved-state.json")).TrimEnd())throw new Exception("Retained scene state changed.");
        if(Root.GetComponentsInChildren<Collider>().Length!=0||Root.GetComponentsInChildren<Light>().Length!=0)throw new Exception("Scenery must not alter movement or lighting.");
        var field=CinderCompactSiteBuild.Field;var filters=Root.GetComponentsInChildren<MeshFilter>();
        foreach(var f in filters) {
            var mesh=f.sharedMesh;var vertices=mesh.vertices;var indices=mesh.triangles;
            if(f.transform.lossyScale!=Vector3.one||mesh.uv.Length!=mesh.vertexCount||mesh.uv.Any(p=>float.IsNaN(p.x)||float.IsNaN(p.y)||float.IsInfinity(p.x)||float.IsInfinity(p.y))||!f.GetComponent<Renderer>().sharedMaterial.shader.isSupported)throw new Exception("Invalid background mesh/material "+f.name);
            for(int i=0;i<indices.Length;i+=3) {
                var a=f.transform.TransformPoint(vertices[indices[i]]);var b=f.transform.TransformPoint(vertices[indices[i+1]]);var c=f.transform.TransformPoint(vertices[indices[i+2]]);
                if(Vector3.Cross(b-a,c-a).sqrMagnitude<1e-12f)throw new Exception("Degenerate background triangle "+f.name);
                var bounds=new Rect(Mathf.Min(a.x,b.x,c.x),Mathf.Min(a.z,b.z,c.z),Mathf.Max(a.x,b.x,c.x)-Mathf.Min(a.x,b.x,c.x),Mathf.Max(a.z,b.z,c.z)-Mathf.Min(a.z,b.z,c.z));
                if(bounds.Overlaps(field))throw new Exception("Scenery crosses suppression footprint "+f.name);
                if(f.name=="Barren basin"&&Vector3.Cross(b-a,c-a).y<=0)throw new Exception("Terrain winding points downward.");
            }
        }
        foreach(string name in new[]{"North boundary","South boundary","West boundary","East boundary"})
            if(GameObject.Find(name).GetComponent<Renderer>().enabled||!GameObject.Find(name).GetComponent<Collider>().enabled)throw new Exception("Retain invisible fall guards.");
        CinderCompactSiteBuild.ValidateWithPrefix("background-");
        var result=new {scene=CinderCompactSiteBuild.ScenePath,mesh_assets=8,materials=2,native_mineral_texture_size=128,placements=filters.Length,triangles=filters.Sum(f=>f.sharedMesh.triangles.Length/3),
            rock_placements=59,industrial_placements=9,basin_size_m=new[]{300,280},outside_field=true,unit_scale=true,new_colliders=0,new_lights=0,
            invisible_retained_fall_guards=4,retained_state_preserved=true,carrying_pose_positions=CinderArchitectureBuild.PosePoints().Count,interactive_exterior=false,user_background_quality_review=false};
        File.WriteAllText(Path.Combine(Output,"background-layout-validation.json"),Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");
        Debug.Log("CINDER BACKGROUND PASS: "+filters.Length+" placements");
    }
}
}
