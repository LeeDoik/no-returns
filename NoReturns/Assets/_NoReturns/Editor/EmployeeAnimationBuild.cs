using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace NoReturns.Editor {
public static class EmployeeAnimationBuild {
    public const string Folder="Assets/_NoReturns/Resources/EmployeeLocal";
    public const string PrefabPath=Folder+"/Employee.prefab";
    public static string Evidence=>Path.GetFullPath("../artifacts/employee-unity");
    [Serializable] public class Receipt {public string status="FAIL",error,unity=Application.unityVersion;public bool idleAvatar,walkAvatar;public float idleSeconds,walkSeconds;public int vertices,bones;}

    [MenuItem("NO RETURNS/Art/Prepare Local Employee Animations")]
    public static void PrepareDefault()=>Prepare(Path.GetFullPath("../artifacts/employee-idle/corrected/NR_Employee_Idle_Upright.fbx"),Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads/Walking.fbx"));
    public static void Queue(){EditorApplication.delayCall+=PrepareDefault;}
    public static void Prepare(string idleSource,string walkSource) {
        var receipt=new Receipt();Directory.CreateDirectory(Evidence);
        try {
            if(EditorApplication.isPlaying)throw new InvalidOperationException("Stop Play before importing employee assets.");
            foreach(var file in new[]{idleSource,walkSource})if(!File.Exists(file))throw new FileNotFoundException("Acquire the employee animation locally before preparing it.",file);
            Directory.CreateDirectory(Folder);
            Copy(idleSource,Folder+"/Idle.fbx");Copy(walkSource,Folder+"/Walking.fbx");
            Copy(Path.GetFullPath("../art/player-employee-01/source/spacesuit+3d+model.fbm/spacesuit+3d+model_basecolor.jpg"),Folder+"/Employee.jpg");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Configure(Folder+"/Idle.fbx","Idle");Configure(Folder+"/Walking.fbx","Walk");
            var idle=Clip("Idle");var walk=Clip("Walking");
            var source=AssetDatabase.LoadAssetAtPath<GameObject>(Folder+"/Idle.fbx");
            var idleAvatar=source.GetComponent<Animator>().avatar;
            var walkAvatar=AssetDatabase.LoadAssetAtPath<GameObject>(Folder+"/Walking.fbx").GetComponent<Animator>().avatar;
            if(!idleAvatar||!idleAvatar.isValid||!idleAvatar.isHuman||!walkAvatar||!walkAvatar.isValid||!walkAvatar.isHuman)throw new Exception("Employee Humanoid mapping is invalid.");
            var material=AssetDatabase.LoadAssetAtPath<Material>(Folder+"/Suit.mat");
            if(!material){material=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,Folder+"/Suit.mat");}
            material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Folder+"/Employee.jpg"));
            material.SetColor("_BaseColor",Color.white);material.SetFloat("_Smoothness",.18f);EditorUtility.SetDirty(material);
            var controllerPath=Folder+"/Locomotion.controller";
            var controller=AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if(!controller)controller=AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            controller.parameters=Array.Empty<AnimatorControllerParameter>();
            controller.AddParameter("Speed",AnimatorControllerParameterType.Float);
            controller.AddParameter("StrideRate",AnimatorControllerParameterType.Float);
            var machine=controller.layers[0].stateMachine;
            foreach(var state in machine.states)machine.RemoveState(state.state);
            var idleState=machine.AddState("Idle");idleState.motion=idle;machine.defaultState=idleState;
            var walkState=machine.AddState("Walk");walkState.motion=walk;walkState.speedParameter="StrideRate";walkState.speedParameterActive=true;
            var move=idleState.AddTransition(walkState);move.hasExitTime=false;move.hasFixedDuration=true;move.duration=.15f;move.AddCondition(AnimatorConditionMode.Greater,.12f,"Speed");
            var stop=walkState.AddTransition(idleState);stop.hasExitTime=false;stop.hasFixedDuration=true;stop.duration=.15f;stop.AddCondition(AnimatorConditionMode.Less,.08f,"Speed");
            EditorUtility.SetDirty(controller);
            var instance=UnityEngine.Object.Instantiate(source);instance.name="Employee";
            try {
                var animator=instance.GetComponent<Animator>();animator.runtimeAnimatorController=controller;animator.applyRootMotion=false;animator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
                foreach(var renderer in instance.GetComponentsInChildren<SkinnedMeshRenderer>()){renderer.sharedMaterial=material;renderer.updateWhenOffscreen=true;}
                instance.AddComponent<NoReturns.CarryLab.EmployeeVisual>();
                PrefabUtility.SaveAsPrefabAsset(instance,PrefabPath);
                receipt.vertices=instance.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.vertexCount;
                receipt.bones=instance.GetComponentInChildren<SkinnedMeshRenderer>().bones.Length;
            } finally {UnityEngine.Object.DestroyImmediate(instance);}
            AssetDatabase.SaveAssets();ValidateAssets();
            receipt.idleAvatar=receipt.walkAvatar=true;receipt.idleSeconds=idle.length;receipt.walkSeconds=walk.length;receipt.status="PASS";
        } catch(Exception e){receipt.error=e.ToString();throw;}
        finally {File.WriteAllText(Path.Combine(Evidence,"import.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Copy(string from,string to){if(Path.GetFullPath(from)==Path.GetFullPath(to))throw new ArgumentException("Source must be separate from imported assets.");File.Copy(from,to,true);}
    static void Configure(string path,string name) {
        var importer=(ModelImporter)AssetImporter.GetAtPath(path);
        importer.animationType=ModelImporterAnimationType.Human;importer.avatarSetup=ModelImporterAvatarSetup.CreateFromThisModel;
        importer.importAnimation=true;importer.animationCompression=ModelImporterAnimationCompression.Off;
        importer.optimizeGameObjects=false;importer.isReadable=true;importer.materialImportMode=ModelImporterMaterialImportMode.None;
        var description=importer.humanDescription;
        description.human=Enum.GetValues(typeof(HumanBodyBones)).Cast<HumanBodyBones>()
            .Where(b=>BoneName(b)!=null).Select(b=>new HumanBone{humanName=HumanTrait.BoneName[(int)b],boneName=BoneName(b),limit=new HumanLimit{useDefaultValues=true}}).ToArray();
        description.armStretch=.05f;description.legStretch=.05f;description.upperArmTwist=.5f;description.lowerArmTwist=.5f;description.upperLegTwist=.5f;description.lowerLegTwist=.5f;
        importer.humanDescription=description;
        var clips=importer.defaultClipAnimations;if(clips.Length!=1)throw new Exception("Expected one source motion: "+path);
        clips[0].name=name;clips[0].loopTime=true;clips[0].loopPose=true;
        clips[0].lockRootRotation=true;clips[0].keepOriginalOrientation=true;
        clips[0].lockRootPositionXZ=true;clips[0].keepOriginalPositionXZ=true;
        clips[0].lockRootHeightY=true;clips[0].keepOriginalPositionY=true;
        importer.clipAnimations=clips;importer.SaveAndReimport();
    }
    static string BoneName(HumanBodyBones b) {
        if(b==HumanBodyBones.LastBone||b==HumanBodyBones.LeftEye||b==HumanBodyBones.RightEye||b==HumanBodyBones.Jaw)return null;
        string name=b.ToString();
        foreach(string finger in new[]{"Thumb","Index","Middle","Ring","Little"})
            if(name.Contains(finger))return name.Replace("Proximal","1").Replace("Intermediate","2").Replace("Distal","3");
        return name;
    }
    public static AnimationClip Clip(string file)=>AssetDatabase.LoadAllAssetsAtPath(Folder+"/"+file+".fbx").OfType<AnimationClip>().Single(c=>!c.name.StartsWith("__preview__"));
    [Serializable] class ReviewReceipt {public string status="FAIL",error;public float walkFootTravel,idleFootTravel,minHeight=100,maxHeight,rootTravel;public int samples;}
    public static void QueueReview(){EditorApplication.delayCall+=Review;}
    [MenuItem("NO RETURNS/Art/Review Local Employee Animations")]
    public static void Review() {
        var receipt=new ReviewReceipt();Directory.CreateDirectory(Evidence);ValidateAssets();
        var preview=new PreviewRenderUtility();var baked=new Mesh();
        try {
            var instance=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath));
            preview.AddSingleGO(instance);
            var animator=instance.GetComponent<Animator>();animator.Rebind();animator.SetFloat("StrideRate",1);
            var renderer=instance.GetComponentInChildren<SkinnedMeshRenderer>();
            var start=instance.transform.position;
            foreach(string state in new[]{"Idle","Walk"}) {
                var points=new Vector3[5];
                for(int i=0;i<5;i++) {
                    animator.SetFloat("Speed",state=="Walk"?2.2f:0);animator.Play(state,0,i*.2f);animator.Update(.001f);
                    points[i]=animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
                    renderer.BakeMesh(baked);var pointsWorld=baked.vertices.Select(v=>renderer.transform.rotation*v).ToArray();var height=pointsWorld.Max(v=>v.y)-pointsWorld.Min(v=>v.y);
                    foreach(var p in baked.vertices)if(float.IsNaN(p.x)||float.IsInfinity(p.x)||float.IsNaN(p.y)||float.IsInfinity(p.y)||float.IsNaN(p.z)||float.IsInfinity(p.z))throw new Exception("Non-finite skinned vertex");
                    receipt.minHeight=Mathf.Min(receipt.minHeight,height);receipt.maxHeight=Mathf.Max(receipt.maxHeight,height);receipt.samples++;
                    receipt.rootTravel=Mathf.Max(receipt.rootTravel,Vector3.Distance(start,instance.transform.position));
                    if(i==1||i==3) {
                        preview.camera.transform.position=new Vector3(2,1.15f,3.5f);preview.camera.transform.LookAt(new Vector3(0,.95f,0));
                        preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=20;preview.camera.fieldOfView=32;
                        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.18f,.2f,.22f);
                        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);
                        preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
                        preview.BeginStaticPreview(new Rect(0,0,800,800));
                        renderer.enabled=false;preview.DrawMesh(baked,renderer.transform.position,renderer.transform.rotation,renderer.sharedMaterial,0);preview.Render(true);renderer.enabled=true;
                        var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(Evidence,state+"-"+i+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
                    }
                }
                float travel=points.Max(p=>Vector3.Distance(points[0],p));
                if(state=="Idle")receipt.idleFootTravel=travel;else receipt.walkFootTravel=travel;
            }
            if(receipt.walkFootTravel<.05f||receipt.rootTravel>.001f||receipt.minHeight<1.4f||receipt.maxHeight>2.1f)throw new Exception("Unexpected pose size, root motion or static walking.");
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}
        finally {preview.Cleanup();UnityEngine.Object.DestroyImmediate(baked);File.WriteAllText(Path.Combine(Evidence,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    public static void ValidateAssets() {
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
        if(!prefab)throw new Exception("Prepare local employee animations through NO RETURNS/Art before building.");
        var animator=prefab.GetComponent<Animator>();
        if(!animator||!animator.avatar||!animator.avatar.isValid||!animator.avatar.isHuman||animator.applyRootMotion||!animator.runtimeAnimatorController)throw new Exception("Invalid employee Animator.");
        if(prefab.GetComponentsInChildren<Collider>().Length!=0)throw new Exception("Employee art must not add gameplay colliders.");
        if(!prefab.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.mainTexture)throw new Exception("Employee texture missing.");
        if(!Clip("Idle").humanMotion||!Clip("Walking").humanMotion)throw new Exception("Clips must be Humanoid.");
    }
}
}
