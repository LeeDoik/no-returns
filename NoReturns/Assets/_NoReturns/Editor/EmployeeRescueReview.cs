using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class EmployeeRescueReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxFootError,maxWristBend,maxBoneLengthChange;public bool release,blocked;}
    public static void Review(){
        var receipt=new Receipt();string folder=Path.GetFullPath("../artifacts/employee-rescue");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var baked=new Mesh();
        try {
            var instance=UnityEngine.Object.Instantiate(Resources.Load<GameObject>("EmployeeLocal/Employee"));preview.AddSingleGO(instance);instance.SendMessage("Awake");
            var animator=instance.GetComponent<Animator>();var visual=instance.GetComponent<EmployeeVisual>();var renderer=instance.GetComponentInChildren<SkinnedMeshRenderer>();
            var bones=new[]{HumanBodyBones.LeftUpperArm,HumanBodyBones.LeftLowerArm,HumanBodyBones.LeftHand,HumanBodyBones.RightUpperArm,HumanBodyBones.RightLowerArm,HumanBodyBones.RightHand}.Select(animator.GetBoneTransform).ToArray();
            Func<float[]> lengths=()=>new[]{Vector3.Distance(bones[0].position,bones[1].position),Vector3.Distance(bones[1].position,bones[2].position),Vector3.Distance(bones[3].position,bones[4].position),Vector3.Distance(bones[4].position,bones[5].position)};
            animator.Rebind();animator.Update(0);var baseline=lengths();
            foreach(string motion in new[]{"Idle","Walk"})foreach(float yaw in new[]{0f,90f,180f,270f})for(int frame=0;frame<10;frame++){
                instance.transform.rotation=Quaternion.Euler(0,yaw,0);animator.Play(motion,0,frame*.1f);animator.SetFloat("StrideRate",1);animator.Update(.001f);
                visual.AttackPose(0,false);visual.RescuePose(.025f+frame*.25f,false,1);var state=visual.Capture();
                receipt.maxFootError=Mathf.Max(receipt.maxFootError,state.rescueFootError);receipt.maxWristBend=Mathf.Max(receipt.maxWristBend,state.rescueWristBend);
                if(state.rescueWeight!=1||state.rescueFootError>.015f||state.rescueWristBend>25.01f)throw new Exception("Rescue feet/wrist failed: "+state.rescueFootError+" / "+state.rescueWristBend);
                var current=lengths();for(int i=0;i<4;i++)receipt.maxBoneLengthChange=Mathf.Max(receipt.maxBoneLengthChange,Mathf.Abs(current[i]-baseline[i]));
                if(receipt.maxBoneLengthChange>.0001f||instance.transform.position.sqrMagnitude>.000001f)throw new Exception("Bone length or root changed");
                renderer.BakeMesh(baked);if(baked.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Invalid mesh");receipt.samples++;
                if(yaw==0&&frame==2)Render(preview,renderer,baked,folder,"Rescue-"+motion);
            }
            animator.Play("Idle",0,0);animator.Update(.001f);visual.RescuePose(0,false,1);receipt.release=visual.Capture().rescueWeight==0;
            visual.RescuePose(1,false,1);visual.RescuePose(1,true,.016f);receipt.blocked=visual.Capture().rescueWeight==0;
            if(!receipt.release||!receipt.blocked)throw new Exception("Rescue reset failed");receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{preview.Cleanup();UnityEngine.Object.DestroyImmediate(baked);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer renderer,Mesh baked,string folder,string name){
        preview.camera.transform.position=new Vector3(2,1.8f,3.5f);preview.camera.transform.LookAt(new Vector3(0,.95f,.2f));preview.camera.fieldOfView=32;preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=20;
        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.18f,.2f,.22f);
        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
        preview.BeginStaticPreview(new Rect(0,0,800,800));renderer.enabled=false;preview.DrawMesh(baked,renderer.transform.position,renderer.transform.rotation,renderer.sharedMaterial,0);preview.Render(true);renderer.enabled=true;
        var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(folder,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
    }
}
}
