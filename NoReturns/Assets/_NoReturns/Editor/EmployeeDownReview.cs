using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class EmployeeDownReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxWristBend,maxBoneLengthChange,minMeshHeight=10,maxHandStep;public bool down,recovered,interrupted,reset;}
    public static void Review(){
        var receipt=new Receipt();string folder=Path.GetFullPath("../artifacts/employee-down");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var baked=new Mesh();
        try {
            var instance=UnityEngine.Object.Instantiate(Resources.Load<GameObject>("EmployeeLocal/Employee"));preview.AddSingleGO(instance);instance.SendMessage("Awake");
            var animator=instance.GetComponent<Animator>();var visual=instance.GetComponent<EmployeeVisual>();var renderer=instance.GetComponentInChildren<SkinnedMeshRenderer>();
            var ids=new[]{HumanBodyBones.LeftUpperArm,HumanBodyBones.LeftLowerArm,HumanBodyBones.LeftHand,HumanBodyBones.RightUpperArm,HumanBodyBones.RightLowerArm,HumanBodyBones.RightHand,HumanBodyBones.LeftUpperLeg,HumanBodyBones.LeftLowerLeg,HumanBodyBones.LeftFoot,HumanBodyBones.RightUpperLeg,HumanBodyBones.RightLowerLeg,HumanBodyBones.RightFoot};
            var bones=ids.Select(animator.GetBoneTransform).ToArray();
            Func<float[]> lengths=()=>Enumerable.Range(0,4).SelectMany(i=>new[]{Vector3.Distance(bones[i*3].position,bones[i*3+1].position),Vector3.Distance(bones[i*3+1].position,bones[i*3+2].position)}).ToArray();
            animator.Rebind();animator.Update(0);var baseline=lengths();
            foreach(float yaw in new[]{0f,90f,180f,270f}){
                instance.transform.rotation=Quaternion.Euler(0,yaw,0);visual.DownPose(false,false,0);Vector3 previous=Vector3.zero;
                for(int frame=0;frame<130;frame++){
                    bool down=frame<70;
                    animator.Play("Idle",0,frame/60f);animator.Update(.001f);visual.DownPose(down,true,1f/60);
                    var state=visual.Capture();receipt.maxWristBend=Mathf.Max(receipt.maxWristBend,state.downWristBend);
                    if(state.downWristBend>25.01f)throw new Exception("Wrist limit");
                    var current=lengths();for(int i=0;i<current.Length;i++)receipt.maxBoneLengthChange=Mathf.Max(receipt.maxBoneLengthChange,Mathf.Abs(current[i]-baseline[i]));
                    if(receipt.maxBoneLengthChange>.0001f||instance.transform.position.sqrMagnitude>.000001f)throw new Exception("Bone length or root changed");
                    renderer.BakeMesh(baked);foreach(var v in baked.vertices){if(!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z))throw new Exception("Invalid mesh");receipt.minMeshHeight=Mathf.Min(receipt.minMeshHeight,(renderer.transform.position+renderer.transform.rotation*v).y);}
                    if(frame>0)receipt.maxHandStep=Mathf.Max(receipt.maxHandStep,Vector3.Distance(previous,state.rightHand));previous=state.rightHand;
                    if(frame==69)receipt.down=state.downPhase=="Down"&&state.downAmount==1;
                    if(frame==129)receipt.recovered=state.downPhase=="Standing"&&state.downAmount==0;
                    receipt.samples++;
                    if(yaw==0&&(frame==10||frame==25||frame==69||frame==85||frame==110||frame==129))Render(preview,renderer,baked,folder,"Pose-"+frame);
                }
            }
            animator.Play("Idle",0,0);animator.Update(.001f);visual.DownPose(true,true,.3f);float before=visual.Capture().downAmount;
            animator.Play("Idle",0,0);animator.Update(.001f);visual.DownPose(false,true,.05f);float recovering=visual.Capture().downAmount;
            animator.Play("Idle",0,0);animator.Update(.001f);visual.DownPose(true,true,.05f);receipt.interrupted=recovering<before&&visual.Capture().downAmount>recovering;
            visual.DownPose(true,false,0);receipt.reset=visual.Capture().downAmount==0;
            if(!receipt.down||!receipt.recovered||!receipt.interrupted||!receipt.reset||receipt.minMeshHeight<-.045f||receipt.maxHandStep>.25f)throw new Exception("Transition/ground clearance failed");receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{preview.Cleanup();UnityEngine.Object.DestroyImmediate(baked);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer renderer,Mesh baked,string folder,string name){
        preview.camera.transform.position=new Vector3(2.3f,1.9f,4.1f);preview.camera.transform.LookAt(new Vector3(0,.85f,.2f));preview.camera.fieldOfView=32;preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=20;
        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.18f,.2f,.22f);
        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
        preview.BeginStaticPreview(new Rect(0,0,800,800));renderer.enabled=false;preview.DrawMesh(baked,renderer.transform.position,renderer.transform.rotation,renderer.sharedMaterial,0);preview.Render(true);renderer.enabled=true;
        var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(folder,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
    }
}
}
