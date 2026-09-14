using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using NoReturns.Trials;

namespace NoReturns.Editor {
public static class ShipInteriorTrialBuild {
    const string ScenePath="Assets/_NoReturns/Scenes/ShipInteriorTrial.unity";
    const string ModelPath="Assets/_NoReturns/Art/ShipInteriorTrial/Flatbed_InteriorTrial.fbx";
    [MenuItem("NO RETURNS/Trials/Preview Interior In Play Mode")]
    public static void PreviewInterior() {
        if(!Application.isPlaying)throw new InvalidOperationException("Enter Play mode in the trial scene first.");
        var player=UnityEngine.Object.FindFirstObjectByType<ShipInteriorTrial>();
        if(!player)throw new InvalidOperationException("Trial employee missing.");
        player.enabled=false;player.GetComponent<CharacterController>().enabled=false;
        player.transform.SetPositionAndRotation(new Vector3(0,1.035f,-2.6f),Quaternion.identity);
        player.GetComponentInChildren<Camera>().transform.localRotation=Quaternion.identity;
        Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
    }
    [MenuItem("NO RETURNS/Trials/Inspect Flatbed Interior Trial")]
    public static void Inspect() {
        var rows=new List<string>();
        foreach(var r in UnityEngine.Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None))
            if(r.name.Contains("Ramp")||r.name.Contains("Deck")||r.name.Contains("ConsoleBase")||r.name.Contains("Threshold"))rows.Add(r.name+" center="+r.bounds.center+" size="+r.bounds.size);
        File.WriteAllLines(Path.GetFullPath("../artifacts/ship-interior-trial/bounds.txt"),rows);
    }
    [MenuItem("NO RETURNS/Trials/Create Flatbed Interior Trial")]
    public static void Create() {
        AssetDatabase.Refresh();
        var importer=(ModelImporter)AssetImporter.GetAtPath(ModelPath);
        importer.globalScale=1;importer.useFileScale=true;importer.importCameras=false;importer.importLights=false;importer.SaveAndReimport();
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        var model=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(ModelPath));model.name="Flatbed interior candidate";
        model.transform.rotation=Quaternion.Euler(0,180,0); // Blender forward maps to negative Unity Z through this FBX export.
        var materialDir="Assets/_NoReturns/Art/ShipInteriorTrial/Materials";Directory.CreateDirectory(materialDir);
        var materials=new Dictionary<string,Material>();
        foreach(var renderer in model.GetComponentsInChildren<MeshRenderer>()) {
            var old=renderer.sharedMaterials;var replacement=new Material[old.Length];
            for(int i=0;i<old.Length;i++) {
                string name=old[i]?old[i].name:"Trial";
                if(!materials.TryGetValue(name,out var mat)) {
                    mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));mat.name=name;
                    if(old[i]){mat.color=old[i].color;if(old[i].mainTexture)mat.SetTexture("_BaseMap",old[i].mainTexture);}
                    if(name.Contains("Screen")||name.Contains("Cyan")||name.Contains("Lamp")){mat.EnableKeyword("_EMISSION");mat.SetColor("_EmissionColor",mat.color*.6f);}
                    if(name.Contains("Glazing")){
                        mat.SetFloat("_Surface",1);mat.SetFloat("_SrcBlend",5);mat.SetFloat("_DstBlend",10);mat.SetFloat("_ZWrite",0);
                        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");mat.renderQueue=3000;mat.color=new Color(.2f,.3f,.3f,.22f);
                    }
                    var path=materialDir+"/"+name+".mat";var existing=AssetDatabase.LoadAssetAtPath<Material>(path);
                    if(existing){EditorUtility.CopySerialized(mat,existing);UnityEngine.Object.DestroyImmediate(mat);mat=existing;}else AssetDatabase.CreateAsset(mat,path);
                    materials[name]=mat;
                }
                replacement[i]=mat;
            }
            renderer.sharedMaterials=replacement;
        }
        foreach(var mesh in model.GetComponentsInChildren<MeshFilter>()) {
            if(mesh.name.Contains("Display")||mesh.name.Contains("Label")||mesh.name.Contains("Light"))continue;
            // Visual hull triangles are not the walking collision shell. Use the fitted cabin surfaces.
            if(mesh.name=="Tripo_Hull_Reworked"||mesh.name=="Trial_ExteriorRamp")continue;
            if(mesh.name.Contains("Window"))mesh.gameObject.AddComponent<MeshCollider>().sharedMesh=mesh.sharedMesh;
            else {var box=mesh.gameObject.AddComponent<BoxCollider>();box.center=mesh.sharedMesh.bounds.center;box.size=mesh.sharedMesh.bounds.size;}
        }
        var rampCollider=new GameObject("Trial ramp collision");rampCollider.transform.position=new Vector3(0,.46f,-5.5f);
        rampCollider.transform.rotation=Quaternion.Euler(-Mathf.Atan(1f/3f)*Mathf.Rad2Deg,0,0);
        rampCollider.AddComponent<BoxCollider>().size=new Vector3(3,.08f,Mathf.Sqrt(10));
        var ground=GameObject.CreatePrimitive(PrimitiveType.Cube);ground.name="Trial landing ground";ground.transform.position=new Vector3(0,-.16f,0);ground.transform.localScale=new Vector3(25,.3f,25);
        if(materials.TryGetValue("Trial_Graphite",out var groundMaterial))ground.GetComponent<Renderer>().sharedMaterial=groundMaterial;
        var player=new GameObject("Trial employee");player.transform.position=new Vector3(0,.05f,-8);player.AddComponent<ShipInteriorTrial>();
        var parcel=GameObject.CreatePrimitive(PrimitiveType.Cube);parcel.name="Trial carried parcel";parcel.layer=2;parcel.transform.position=new Vector3(.6f,.45f,-7.7f);parcel.transform.localScale=new Vector3(.8f,.65f,.65f);parcel.AddComponent<Rigidbody>().mass=8;
        for(int i=0;i<3;i++){var light=new GameObject("Cabin light").AddComponent<Light>();light.type=LightType.Point;light.transform.position=new Vector3(0,2.94f,-2+i*2);light.range=5;light.intensity=1.3f;}
        var sun=new GameObject("Trial daylight").AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=1.2f;sun.transform.rotation=Quaternion.Euler(40,-30,0);
        RenderSettings.ambientLight=new Color(.3f,.3f,.3f);
        Physics.SyncTransforms();
        var test=new GameObject("Automatic passage probe");var controller=test.AddComponent<CharacterController>();controller.height=1.8f;controller.radius=.34f;controller.center=Vector3.up*.9f;controller.skinWidth=.035f;controller.stepOffset=.32f;
        var results=new List<string>();bool passed=true;
        foreach(float lane in new[]{-.55f,0,.55f}){
            controller.enabled=false;test.transform.position=new Vector3(lane,.05f,-8);controller.enabled=true;Physics.SyncTransforms();
            for(int i=0;i<900&&test.transform.position.z<2.2f;i++)controller.Move(new Vector3(0,-.02f,.06f));
            bool ok=test.transform.position.z>2.1f;passed&=ok;results.Add("lane="+lane+" end="+test.transform.position.ToString("F3")+" pass="+ok);
            if(!ok){var p=test.transform.position;foreach(var hit in Physics.CapsuleCastAll(p+Vector3.up*.34f,p+Vector3.up*1.46f,.34f,Vector3.forward,.3f))if(hit.collider.gameObject!=test)results.Add("obstacle="+hit.collider.name+" distance="+hit.distance);}
        }
        UnityEngine.Object.DestroyImmediate(test);
        EditorSceneManager.SaveScene(scene,ScenePath);AssetDatabase.SaveAssets();
        var root=Path.GetFullPath("..");var output=Path.Combine(root,"artifacts/ship-interior-trial");Directory.CreateDirectory(output);
        File.WriteAllLines(Path.Combine(output,"passage.txt"),results);
        if(!passed)throw new Exception("Interior passage failed: "+string.Join("; ",results));
        Debug.Log("INTERIOR TRIAL PASS "+string.Join("; ",results));
    }
    [MenuItem("NO RETURNS/Trials/Build Flatbed Interior Trial")]
    public static void Build() {
        Create();var output=Path.GetFullPath("../builds/ShipInteriorTrial/NoReturns-InteriorTrial.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
        if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new Exception("Interior trial build failed");
        File.WriteAllText(Path.GetFullPath("../artifacts/ship-interior-trial/build-success.txt"),DateTime.UtcNow.ToString("O")+"\n"+output);
    }
}
}
