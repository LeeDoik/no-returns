using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class FirstPersonArmsReview {
    [Serializable] class Receipt {public string status="FAIL",error,worstGrip;public int samples;public float maxGripError,maxBoneLengthError,maxWristBend,baselineStrikeWristBend;public bool overlayDepth,stackRendered;public int overlayPixels;}
    public static void Review(){
        var receipt=new Receipt();var folder=Path.GetFullPath("../artifacts/first-person-arms");Directory.CreateDirectory(folder);
        var preview=new PreviewRenderUtility();var eyeObject=new GameObject("Arm review eye");var eye=eyeObject.AddComponent<Camera>();eye.fieldOfView=80;
        var arms=new FirstPersonArms(eye);
        var model=(GameObject)typeof(FirstPersonArms).GetField("model",BindingFlags.NonPublic|BindingFlags.Instance).GetValue(arms);
        var visual=model.GetComponent<EmployeeVisual>();typeof(EmployeeVisual).GetMethod("Awake",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(visual,null);
        var animator=model.GetComponent<Animator>();var renderers=model.GetComponentsInChildren<SkinnedMeshRenderer>();
        var weapon=new GameObject("Review baton");var art=UnityEngine.Object.Instantiate(Resources.Load<GameObject>("PSXKit01/Baton"),weapon.transform);
        var rendered=art.GetComponentsInChildren<Renderer>();var bounds=rendered[0].bounds;foreach(var r in rendered.Skip(1))bounds.Encapsulate(r.bounds);float scale=.68f/bounds.size.y;art.transform.localScale*=scale;art.transform.localPosition=-bounds.center*scale+Vector3.up*.18f;
        var box=GameObject.CreatePrimitive(PrimitiveType.Cube);box.transform.position=new Vector3(0,-.33f,.9f);box.transform.localScale=new Vector3(.6f,.55f,.55f);var parcel=box.GetComponent<BoxCollider>();
        preview.AddSingleGO(model);preview.AddSingleGO(weapon);preview.AddSingleGO(box);
        try {
            model.transform.SetPositionAndRotation(new Vector3(0,-1.75f,.20f),Quaternion.identity);animator.Play("Idle",0,.2f);animator.Update(.001f);
            visual.PoseGrip(new Vector3(.195f,-.23f,.64f),Quaternion.Euler(-68,180,12),1);receipt.baselineStrikeWristBend=visual.RightWristBend;
            foreach(float fov in new[]{65f,80f,100f})foreach(float pitch in new[]{-45f,0f,45f})foreach(float reach in new[]{.35f,.5f,.65f,.8f,1f})for(int frame=0;frame<=30;frame++){
                eye.fieldOfView=fov;eye.transform.rotation=Quaternion.Euler(pitch,30,0);
                animator.Play("Idle",0,.2f);animator.Update(.001f);
                float cooldown=6-frame/60f;BatonMotion.FirstPerson(cooldown,out var grip,out var rotation);
                var upper=animator.GetBoneTransform(HumanBodyBones.RightUpperArm);var lower=animator.GetBoneTransform(HumanBodyBones.RightLowerArm);var hand=animator.GetBoneTransform(HumanBodyBones.RightHand);
                float a=Vector3.Distance(upper.position,lower.position),b=Vector3.Distance(lower.position,hand.position);
                weapon.transform.SetPositionAndRotation(eye.transform.rotation*new Vector3(grip.x,grip.y,grip.z*reach),eye.transform.rotation*rotation);
                arms.Display(eye,weapon.transform,true,false,parcel,reach);
                var state=arms.Capture();if(state.gripError>receipt.maxGripError)receipt.worstGrip=$"fov={fov} pitch={pitch} reach={reach} frame={frame}";receipt.maxGripError=Mathf.Max(receipt.maxGripError,state.gripError);receipt.maxWristBend=Mathf.Max(receipt.maxWristBend,state.wristBend);
                if(state.wristBend>25.01f)throw new Exception("Wrist flexion exceeds 25 degrees");
                receipt.maxBoneLengthError=Mathf.Max(receipt.maxBoneLengthError,Mathf.Abs(a-Vector3.Distance(upper.position,lower.position)),Mathf.Abs(b-Vector3.Distance(lower.position,hand.position)));
                if(!state.ready||!state.rightVisible||state.leftVisible||Vector3.Distance(state.grip,weapon.transform.position)>.001f||receipt.maxBoneLengthError>.001f)throw new Exception("Detached baton or stretched arm");
                foreach(var r in renderers){var mesh=new Mesh();r.BakeMesh(mesh);if(mesh.vertices.Any(v=>!float.IsFinite(v.x)||!float.IsFinite(v.y)||!float.IsFinite(v.z)))throw new Exception("Invalid arm mesh");UnityEngine.Object.DestroyImmediate(mesh);}
                if(fov==80&&pitch==0&&reach==1&&(frame==0||frame==5||frame==25))Render(preview,renderers,box,folder,frame==0?"Ready":frame==5?"Strike":"Return",eye.transform.rotation,fov);
                receipt.samples++;
            }
            eye.transform.rotation=Quaternion.identity;eye.fieldOfView=80;animator.Play("Idle",0,.2f);animator.Update(.001f);
            weapon.SetActive(false);arms.Display(eye,weapon.transform,false,true,parcel,1);
            if(!arms.Capture().leftVisible||!arms.Capture().rightVisible)throw new Exception("Carry hands missing");Render(preview,renderers,box,folder,"Carry",Quaternion.identity,80);receipt.samples++;
            arms.Display(eye,weapon.transform,false,false,parcel,1);if(arms.Capture().overlayEnabled||arms.Capture().rightVisible||arms.Capture().leftVisible)throw new Exception("Blocked hands visible");receipt.samples++;
            receipt.overlayDepth=eye.GetUniversalAdditionalCameraData().cameraStack.Single().GetUniversalAdditionalCameraData().clearDepth;
            if(!receipt.overlayDepth||(eye.cullingMask&(1<<FirstPersonArms.Layer))!=0)throw new Exception("Overlay isolation failed");
            if(receipt.maxGripError>.12f)throw new Exception("First-person grip exceeds reach allowance: "+receipt.maxGripError);
            // Render the actual Base/Overlay stack offscreen without entering Play.
            eye.scene=preview.camera.scene;eye.clearFlags=CameraClearFlags.SolidColor;eye.backgroundColor=Color.black;eye.nearClipPlane=.06f;eye.farClipPlane=3;
            var overlay=eye.GetUniversalAdditionalCameraData().cameraStack.Single();overlay.scene=eye.scene;
            box.SetActive(false);weapon.SetActive(true);FirstPersonArms.SetLayer(weapon,FirstPersonArms.Layer);
            animator.Play("Idle",0,.2f);animator.Update(.001f);BatonMotion.FirstPerson(0,out var readyGrip,out var readyRotation);
            weapon.transform.SetPositionAndRotation(readyGrip,readyRotation);arms.Display(eye,weapon.transform,true,false,parcel,1);
            var target=new RenderTexture(1280,720,24);target.Create();var pixels=new Texture2D(1280,720,TextureFormat.RGB24,false);var prior=RenderTexture.active;
            try{RenderPipeline.SubmitRenderRequest(eye,new RenderPipeline.StandardRequest{destination=target});RenderTexture.active=target;pixels.ReadPixels(new Rect(0,0,1280,720),0,0);pixels.Apply();
                receipt.overlayPixels=pixels.GetPixels32().Count(c=>c.r>12||c.g>12||c.b>12);receipt.stackRendered=receipt.overlayPixels>3000;
                File.WriteAllBytes(Path.Combine(folder,"Stack-Ready.png"),pixels.EncodeToPNG());if(!receipt.stackRendered)throw new Exception("Actual overlay stack did not render arms/baton");
            }finally{RenderTexture.active=prior;target.Release();UnityEngine.Object.DestroyImmediate(target);UnityEngine.Object.DestroyImmediate(pixels);}
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{preview.Cleanup();UnityEngine.Object.DestroyImmediate(eyeObject);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
    static void Render(PreviewRenderUtility preview,SkinnedMeshRenderer[] arms,GameObject box,string folder,string name,Quaternion rotation,float fov){
        preview.camera.transform.SetPositionAndRotation(Vector3.zero,rotation);preview.camera.nearClipPlane=.015f;preview.camera.farClipPlane=3;preview.camera.fieldOfView=fov;
        preview.camera.clearFlags=CameraClearFlags.SolidColor;preview.camera.backgroundColor=new Color(.15f,.17f,.2f);
        preview.lights[0].intensity=1.2f;preview.lights[0].transform.rotation=Quaternion.Euler(40,210,0);preview.lights[1].intensity=.7f;preview.lights[1].transform.rotation=Quaternion.Euler(340,30,0);
        box.GetComponent<Renderer>().enabled=name=="Carry";
        preview.BeginStaticPreview(new Rect(0,0,1280,720));var baked=new System.Collections.Generic.List<Mesh>();
        foreach(var arm in arms)if(arm.enabled){var mesh=new Mesh();arm.BakeMesh(mesh);arm.enabled=false;preview.DrawMesh(mesh,arm.transform.position,arm.transform.rotation,arm.sharedMaterial,0);baked.Add(mesh);}
        preview.Render(true);foreach(var mesh in baked)UnityEngine.Object.DestroyImmediate(mesh);var texture=preview.EndStaticPreview();File.WriteAllBytes(Path.Combine(folder,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);
    }
}
}
