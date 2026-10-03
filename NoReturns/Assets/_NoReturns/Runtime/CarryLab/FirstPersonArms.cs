using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace NoReturns.CarryLab {
public sealed class FirstPersonArms {
    public const int Layer=31;
    GameObject model;Animator animator;EmployeeVisual visual;
    SkinnedMeshRenderer left,right;Camera overlay;bool stacked;
    [System.Serializable] public class State {public bool ready,rightVisible,leftVisible,carrying,overlayEnabled,overlayConfigured;public float gripError,clearance;public Vector3 grip;}
    State state=new State();
    public State Capture()=>state;
    public FirstPersonArms(Camera eye){
        var prefab=Resources.Load<GameObject>("EmployeeLocal/FirstPersonArms");
        if(!prefab){Debug.LogError("Prepare FirstPersonArms locally before building.");return;}
        model=Object.Instantiate(prefab);model.name="Local first-person hands";
        animator=model.GetComponent<Animator>();visual=model.GetComponent<EmployeeVisual>();
        foreach(var renderer in model.GetComponentsInChildren<SkinnedMeshRenderer>()){
            if(renderer.name=="First person left arm")left=renderer;else if(renderer.name=="First person right arm")right=renderer;
            renderer.shadowCastingMode=ShadowCastingMode.Off;renderer.receiveShadows=false;renderer.updateWhenOffscreen=true;
        }
        SetLayer(model,Layer);eye.cullingMask&=~(1<<Layer);
        var cameraObject=new GameObject("First-person arms overlay");cameraObject.transform.SetParent(eye.transform,false);
        overlay=cameraObject.AddComponent<Camera>();overlay.cullingMask=1<<Layer;overlay.nearClipPlane=.015f;overlay.farClipPlane=3;
        overlay.allowHDR=eye.allowHDR;overlay.allowMSAA=eye.allowMSAA;
        var data=overlay.GetUniversalAdditionalCameraData();data.renderType=CameraRenderType.Overlay;data.renderShadows=false;
        // Null graphics clients test poses without initializing the URP GPU renderer.
        stacked=SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null;
        if(stacked)eye.GetUniversalAdditionalCameraData().cameraStack.Add(overlay);
        left.enabled=right.enabled=false;state.ready=visual.Ready&&left&&right;
    }
    public void Dispose(Camera eye){if(overlay){if(stacked)eye.GetUniversalAdditionalCameraData().cameraStack.Remove(overlay);Object.Destroy(overlay.gameObject);}if(model)Object.Destroy(model);}
    public static void SetLayer(GameObject root,int layer){foreach(var t in root.GetComponentsInChildren<Transform>(true))t.gameObject.layer=layer;}
    public void Display(Camera eye,Transform baton,bool allowed,bool carrying,BoxCollider parcel,float clearance){
        if(!model)return;
        overlay.fieldOfView=eye.fieldOfView;overlay.enabled=allowed||carrying;
        // Put shoulders outside the view; only source mesh arm triangles are rendered.
        model.transform.SetPositionAndRotation(eye.transform.position+eye.transform.rotation*new Vector3(0,-1.75f,.20f),eye.transform.rotation);
        animator.SetFloat("Speed",0);animator.speed=1;
        left.enabled=carrying;right.enabled=allowed||carrying;
        state=new State{ready=visual.Ready,rightVisible=right.enabled,leftVisible=left.enabled,carrying=carrying,overlayEnabled=overlay.enabled,overlayConfigured=stacked,clearance=clearance};
        if(carrying){visual.CarryPose(parcel,true,false,1);state.gripError=Mathf.Max(visual.Capture().leftContactError,visual.Capture().rightContactError);}
        else if(allowed){
            visual.CarryPose(parcel,false,false,1);
            var desired=baton.position;
            visual.PoseGrip(desired,baton.rotation,1);
            if(visual.BatonGrip(out var actual,out var rotation)){
                baton.SetPositionAndRotation(actual,rotation);state.grip=actual;state.gripError=Vector3.Distance(desired,actual);
            }
        }
    }
}
}
