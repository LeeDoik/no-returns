using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;

namespace NoReturns.Editor {
public static class EmployeeBatonReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxGripError,maxBoneLengthChange,handTravel,chestTravel,maxLateralDrift,maxAxisAngle,forwardTravel;public bool blocked,returns,cancelled;}
    public static void Review(){
        var receipt=new Receipt();string folder=Path.GetFullPath("../artifacts/employee-baton-pulse");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var baked=new Mesh();
        try {
            EmployeeAnimationBuild.ValidateAssets();
            var instance=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));
            preview.AddSingleGO(instance);typeof(EmployeeVisual).GetMethod("Awake",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(instance.GetComponent<EmployeeVisual>(),null);
            var animator=instance.GetComponent<Animator>();var visual=instance.GetComponent<EmployeeVisual>();
            var renderer=instance.GetComponentInChildren<SkinnedMeshRenderer>();
            var art=UnityEngine.Object.Instantiate(Resources.Load<GameObject>("PSXKit01/Baton"));preview.AddSingleGO(art);
            var bounds=art.GetComponentsInChildren<Renderer>().Select(r=>r.bounds).Aggregate((a,b)=>{a.Encapsulate(b);return a;});
            float scale=.68f/bounds.size.y;Vector3 artOffset=-bounds.center*scale+Vector3.up*.18f;
            var upper=animator.GetBoneTransform(HumanBodyBones.RightUpperArm);var lower=animator.GetBoneTransform(HumanBodyBones.RightLowerArm);var hand=animator.GetBoneTransform(HumanBodyBones.RightHand);
            animator.Rebind();animator.Update(0);float a=Vector3.Distance(upper.position,lower.position),b=Vector3.Distance(lower.position,hand.position);
            Vector3 ready=Vector3.zero,readyGrip=Vector3.zero;Quaternion readyChest=Quaternion.identity;
            foreach(string state in new[]{"Idle","Walk"})foreach(float yaw in new[]{0f,90f,135f})for(int frame=-1;frame<=18;frame++){
                instance.transform.rotation=Quaternion.Euler(0,yaw,0);
                animator.Play(state,0,.2f);animator.SetFloat("StrideRate",1);animator.Update(.001f);
                float time=frame/60f,cooldown=frame<0?0:6-time;
                visual.BatonPose(cooldown,true,1);var evidence=visual.Capture();
                visual.BatonGrip(out var grip,out var rotation);
                var localGrip=instance.transform.InverseTransformPoint(grip);
                receipt.maxAxisAngle=Mathf.Max(receipt.maxAxisAngle,Vector3.Angle(rotation*Vector3.up,instance.transform.rotation*EmployeeVisual.BatonReadyRotation*Vector3.up));
                if(frame==-1){ready=hand.position;readyGrip=localGrip;readyChest=evidence.chest;}
                else {receipt.handTravel=Mathf.Max(receipt.handTravel,Vector3.Distance(ready,hand.position));receipt.chestTravel=Mathf.Max(receipt.chestTravel,Quaternion.Angle(readyChest,evidence.chest));
                    var travel=localGrip-readyGrip;receipt.maxLateralDrift=Mathf.Max(receipt.maxLateralDrift,new Vector2(travel.x,travel.y).magnitude);receipt.forwardTravel=Mathf.Max(receipt.forwardTravel,travel.z);}
                receipt.maxGripError=Mathf.Max(receipt.maxGripError,evidence.batonGripError);
                receipt.maxBoneLengthChange=Mathf.Max(receipt.maxBoneLengthChange,Mathf.Abs(a-Vector3.Distance(upper.position,lower.position)),Mathf.Abs(b-Vector3.Distance(lower.position,hand.position)));
                if(evidence.batonGripError>.11f||receipt.maxBoneLengthChange>.0001f||instance.transform.position.sqrMagnitude>.000001f)throw new Exception("Baton pose changed reach/length/root: "+evidence.batonGripError);
                renderer.BakeMesh(baked);if(baked.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Non-finite baton pose mesh");
                receipt.samples++;
                if(yaw==0&&state=="Idle"&&(frame==-1||frame==3||frame==6||frame==12||frame==18)){
                    art.transform.localScale=Vector3.one*scale;art.transform.SetPositionAndRotation(grip+rotation*artOffset,rotation);
                    Render(preview,renderer,baked,folder,frame==-1?"Pulse-Ready":"Pulse-"+frame);
                }
            }
            visual.BatonPose(6,false,1);receipt.blocked=visual.Capture().batonWeight==0&&!visual.Capture().batonSwing;
            animator.Play("Idle",0,.2f);animator.Update(.001f);visual.BatonPose(5.9f,true,1);receipt.cancelled=!visual.Capture().batonSwing&&visual.BatonMotionCooldown==0;
            animator.Play("Idle",0,.2f);animator.Update(.001f);visual.BatonPose(5.7f,true,1);receipt.returns=!visual.Capture().batonSwing;
            if(!receipt.blocked||!receipt.returns||!receipt.cancelled||receipt.forwardTravel<.16f||receipt.maxLateralDrift>.025f||receipt.maxAxisAngle>1||receipt.chestTravel>.1f)throw new Exception("Pulse must preserve the upright grip and return without a lateral swing");
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}
        finally {preview.Cleanup();UnityEngine.Object.DestroyImmediate(baked);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer renderer,Mesh baked,string folder,string name){
        preview.camera.transform.position=new Vector3(2,1.7f,3.5f);preview.camera.transform.LookAt(new Vector3(0,1,.2f));
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
