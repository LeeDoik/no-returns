using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;

namespace NoReturns.Editor {
public static class EmployeeCarryReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxNearError,maxBoneLengthChange;public bool release,down;}
    public static void Queue(){EditorApplication.delayCall+=Review;}
    public static void Review(){
        var receipt=new Receipt();string folder=Path.GetFullPath("../artifacts/employee-carry");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var baked=new Mesh();GameObject instance=null,box=null;
        try {
            EmployeeAnimationBuild.ValidateAssets();
            instance=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));
            preview.AddSingleGO(instance);instance.SendMessage("Awake");
            var animator=instance.GetComponent<Animator>();var visual=instance.GetComponent<EmployeeVisual>();
            var renderer=instance.GetComponentInChildren<SkinnedMeshRenderer>();
            box=GameObject.CreatePrimitive(PrimitiveType.Cube);preview.AddSingleGO(box);box.transform.localScale=new Vector3(.8f,.65f,.65f);
            var collider=box.GetComponent<BoxCollider>();
            var material=new Material(Shader.Find("Universal Render Pipeline/Lit"));material.color=new Color(.48f,.29f,.12f);box.GetComponent<Renderer>().sharedMaterial=material;
            var arms=new[]{HumanBodyBones.LeftUpperArm,HumanBodyBones.LeftLowerArm,HumanBodyBones.LeftHand,HumanBodyBones.RightUpperArm,HumanBodyBones.RightLowerArm,HumanBodyBones.RightHand}.Select(animator.GetBoneTransform).ToArray();
            animator.Rebind();animator.Update(0);var lengths=new[]{Vector3.Distance(arms[0].position,arms[1].position),Vector3.Distance(arms[1].position,arms[2].position),Vector3.Distance(arms[3].position,arms[4].position),Vector3.Distance(arms[4].position,arms[5].position)};
            foreach(string state in new[]{"Idle","Walk"})foreach(float distance in new[]{.75f,.8f,1.6f})foreach(float yaw in new[]{0f,90f,180f,-90f})for(int frame=0;frame<5;frame++){
                animator.Play(state,0,frame*.2f);animator.SetFloat("StrideRate",1);animator.Update(.001f);
                box.transform.SetPositionAndRotation(new Vector3(0,1.03f,distance),Quaternion.Euler(0,yaw,0));
                visual.CarryPose(collider,true,false,1);var evidence=visual.Capture();
                float error=Mathf.Max(evidence.leftContactError,evidence.rightContactError);
                if(distance<=.8f&&yaw==0){receipt.maxNearError=Mathf.Max(receipt.maxNearError,error);if(error>.045f)throw new Exception("Near parcel contact gap: "+error);}
                var current=new[]{Vector3.Distance(arms[0].position,arms[1].position),Vector3.Distance(arms[1].position,arms[2].position),Vector3.Distance(arms[3].position,arms[4].position),Vector3.Distance(arms[4].position,arms[5].position)};
                for(int i=0;i<4;i++)receipt.maxBoneLengthChange=Mathf.Max(receipt.maxBoneLengthChange,Mathf.Abs(current[i]-lengths[i]));
                if(receipt.maxBoneLengthChange>.0001f||instance.transform.position.sqrMagnitude>.000001f)throw new Exception("Carry pose changed bone length/root");
                renderer.BakeMesh(baked);if(baked.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Non-finite skinned vertex");
                receipt.samples++;
                if(distance==.8f&&yaw==0&&frame==1)Render(preview,renderer,baked,folder,"Carry-"+state,new Vector3(2,1.8f,3.5f));
                if(distance==.8f&&yaw==0&&frame==1&&state=="Idle")Render(preview,renderer,baked,folder,"Carry-Hands",new Vector3(1.7f,1.6f,-.4f));
                if(distance==1.6f&&yaw==90&&state=="Idle"&&frame==1)Render(preview,renderer,baked,folder,"Carry-ReachLimit",new Vector3(2,1.8f,3.5f));
            }
            visual.CarryPose(collider,false,false,1);receipt.release=visual.Capture().carryWeight==0;
            visual.CarryPose(collider,true,false,1);visual.CarryPose(collider,true,true,.016f);receipt.down=visual.Capture().carryWeight==0;
            if(!receipt.release||!receipt.down)throw new Exception("Carry blend did not clear");
            receipt.status="PASS";UnityEngine.Object.DestroyImmediate(material);
        }catch(Exception e){receipt.error=e.ToString();throw;}
        finally {preview.Cleanup();UnityEngine.Object.DestroyImmediate(baked);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer renderer,Mesh baked,string folder,string name,Vector3 camera){
        preview.camera.transform.position=camera;preview.camera.transform.LookAt(new Vector3(0,.95f,.3f));
        preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=20;preview.camera.fieldOfView=32;
        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.18f,.2f,.22f);
        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);
        preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
        preview.BeginStaticPreview(new Rect(0,0,800,800));renderer.enabled=false;
        preview.DrawMesh(baked,renderer.transform.position,renderer.transform.rotation,renderer.sharedMaterial,0);preview.Render(true);renderer.enabled=true;
        var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(folder,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
    }
}
}
