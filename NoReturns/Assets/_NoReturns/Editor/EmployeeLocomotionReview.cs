using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;

namespace NoReturns.Editor {
public static class EmployeeLocomotionReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxBoneLengthChange,maxLandingFootError,maxCompression,maxDirectionBoneLengthChange,maxDirectionExcursion;public float minDirectionalStride=100;public int directionSamples;public bool directionCleared;public bool downCleared,returned;}
    public static void Review() {
        var receipt=new Receipt();var folder=Path.GetFullPath("../artifacts/employee-locomotion");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var mesh=new Mesh();
        try {
            EmployeeAnimationBuild.ValidateAssets();
            var model=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));preview.AddSingleGO(model);
            var visual=model.GetComponent<EmployeeVisual>();
            typeof(EmployeeVisual).GetMethod("Awake",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(visual,null);
            var animator=model.GetComponent<Animator>();var renderer=model.GetComponentInChildren<SkinnedMeshRenderer>();
            var bones=new[]{HumanBodyBones.LeftUpperLeg,HumanBodyBones.LeftLowerLeg,HumanBodyBones.LeftFoot,HumanBodyBones.RightUpperLeg,HumanBodyBones.RightLowerLeg,HumanBodyBones.RightFoot}.Select(animator.GetBoneTransform).ToArray();
            animator.Rebind();animator.Update(0);
            float[] lengths={Vector3.Distance(bones[0].position,bones[1].position),Vector3.Distance(bones[1].position,bones[2].position),Vector3.Distance(bones[3].position,bones[4].position),Vector3.Distance(bones[4].position,bones[5].position)};
            foreach(string state in new[]{"Idle","Walk"})foreach(float yaw in new[]{0f,90f,135f})foreach(float phase in new[]{0f,.2f,.4f,.6f,.8f}) {
                model.transform.rotation=Quaternion.Euler(0,yaw,0);
                for(int frame=0;frame<9;frame++) {
                    animator.Play(state,0,phase);animator.SetFloat("StrideRate",1);animator.Update(.001f);
                    var l=bones[2].position;var r=bones[5].position;var hips=animator.GetBoneTransform(HumanBodyBones.Hips);var hip=hips.position;
                    bool ground=frame==0||frame>=4;float vertical=frame==1?4:frame==3?-4:0;
                    visual.LocomotionPose(ground,vertical,false,frame>=4?.055f:.1f);
                    var pose=visual.Capture();
                    if(frame==1&&pose.locomotion!="Rising"||frame==2&&pose.locomotion!="Apex"||frame==3&&pose.locomotion!="Falling")throw new Exception("Incorrect air phase");
                    if(pose.landingWeight>0) {
                        receipt.maxLandingFootError=Mathf.Max(receipt.maxLandingFootError,Vector3.Distance(l,bones[2].position),Vector3.Distance(r,bones[5].position));
                        receipt.maxCompression=Mathf.Max(receipt.maxCompression,Vector3.Distance(hip,hips.position));
                    }
                    var current=new[]{Vector3.Distance(bones[0].position,bones[1].position),Vector3.Distance(bones[1].position,bones[2].position),Vector3.Distance(bones[3].position,bones[4].position),Vector3.Distance(bones[4].position,bones[5].position)};
                    for(int i=0;i<4;i++)receipt.maxBoneLengthChange=Mathf.Max(receipt.maxBoneLengthChange,Mathf.Abs(current[i]-lengths[i]));
                    if(receipt.maxBoneLengthChange>.0001f||receipt.maxLandingFootError>.01f||model.transform.position.sqrMagnitude>.000001f)throw new Exception("Leg length, root or planted-foot error");
                    renderer.BakeMesh(mesh);if(mesh.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Non-finite locomotion mesh");
                    receipt.samples++;
                    if(state=="Idle"&&yaw==0&&phase==.2f&&(frame==1||frame==3||frame==5))Render(preview,renderer,mesh,folder,frame==1?"Jump-Rise":frame==3?"Jump-Fall":"Landing");
                }
            }
            receipt.returned=visual.Capture().locomotion=="Grounded"&&visual.Capture().airWeight==0&&visual.Capture().landingWeight==0;
            visual.LocomotionPose(false,4,false,1);visual.LocomotionPose(false,4,true,.016f);
            receipt.downCleared=visual.Capture().airWeight==0&&visual.Capture().landingWeight==0;
            if(!receipt.returned||!receipt.downCleared||receipt.maxCompression<.06f)throw new Exception("Locomotion did not compress, return or clear on down");
            Vector3 swingPeak=Vector3.zero;
            foreach(float yaw in new[]{0f,90f,135f})foreach(var travel in new[]{Vector3.back,Vector3.left,Vector3.right,new Vector3(-1,0,-1),new Vector3(1,0,-1)})foreach(float velocity in new[]{1f,2.2f,4f})foreach(float phase in new[]{0f,.2f,.4f,.6f,.8f}) {
                model.transform.rotation=Quaternion.Euler(0,yaw,0);model.transform.position=Vector3.zero;
                visual.Animate(Vector3.zero,true,1);visual.Animate(model.transform.rotation*travel.normalized*(velocity*.1f),false,.1f);
                animator.Play("Walk",0,phase);animator.SetFloat("StrideRate",1);animator.Update(.001f);
                var beforeLeft=model.transform.InverseTransformPoint(bones[2].position);var beforeRight=model.transform.InverseTransformPoint(bones[5].position);
                var root=model.transform.position;var hips=animator.GetBoneTransform(HumanBodyBones.Hips).rotation;
                visual.DirectionPose(true);var pose=visual.Capture();
                if(phase==.2f)swingPeak=pose.leftFootPosition;
                if(phase==.8f){
                    float advance=Vector3.Dot(swingPeak-pose.leftFootPosition,travel.normalized);
                    receipt.minDirectionalStride=Mathf.Min(receipt.minDirectionalStride,advance);
                    if(advance<.15f)throw new Exception("Foot swing does not advance in actual movement direction");
                }
                receipt.maxDirectionExcursion=Mathf.Max(receipt.maxDirectionExcursion,Vector3.Distance(beforeLeft,pose.leftFootPosition),Vector3.Distance(beforeRight,pose.rightFootPosition));
                var current=new[]{Vector3.Distance(bones[0].position,bones[1].position),Vector3.Distance(bones[1].position,bones[2].position),Vector3.Distance(bones[3].position,bones[4].position),Vector3.Distance(bones[4].position,bones[5].position)};
                for(int i=0;i<4;i++)receipt.maxDirectionBoneLengthChange=Mathf.Max(receipt.maxDirectionBoneLengthChange,Mathf.Abs(current[i]-lengths[i]));
                if(pose.directionWeight<.9f||pose.leftFootPosition.x>=-.03f||pose.rightFootPosition.x<=.03f||Vector3.Distance(root,model.transform.position)>.00001f||Quaternion.Angle(hips,animator.GetBoneTransform(HumanBodyBones.Hips).rotation)>.01f||receipt.maxDirectionBoneLengthChange>.0001f)throw new Exception("Direction pose lanes/root/torso/length failed");
                renderer.BakeMesh(mesh);if(mesh.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Non-finite direction mesh");
                receipt.directionSamples++;
                if(yaw==0&&velocity==2.2f&&phase==.2f&&travel.sqrMagnitude==1)Render(preview,renderer,mesh,folder,travel.z<0?"Backward":travel.x<0?"Strafe-Left":"Strafe-Right");
            }
            visual.DirectionPose(false);receipt.directionCleared=visual.Capture().directionWeight==0;
            visual.Animate(Vector3.zero,true,1);visual.DirectionPose(true);receipt.directionCleared&=visual.Capture().directionWeight==0;
            if(!receipt.directionCleared||receipt.maxDirectionExcursion<.3f)throw new Exception("Directional stride did not redirect/clear");
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}
        finally {preview.Cleanup();UnityEngine.Object.DestroyImmediate(mesh);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer renderer,Mesh mesh,string folder,string name) {
        preview.camera.transform.position=new Vector3(2,1.7f,3.5f);preview.camera.transform.LookAt(new Vector3(0,.9f,0));preview.camera.nearClipPlane=.01f;preview.camera.farClipPlane=20;preview.camera.fieldOfView=32;
        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.18f,.2f,.22f);
        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
        preview.BeginStaticPreview(new Rect(0,0,800,800));renderer.enabled=false;preview.DrawMesh(mesh,renderer.transform.position,renderer.transform.rotation,renderer.sharedMaterial,0);preview.Render(true);renderer.enabled=true;
        var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(folder,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
    }
}
}
