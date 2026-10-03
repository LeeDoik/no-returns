using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace NoReturns.CarryLab {
public sealed class FirstPersonArms {
    public const int Layer=31;
    GameObject model;Animator animator;EmployeeVisual visual;
    SkinnedMeshRenderer left,right;Camera overlay;bool stacked;
    [System.Serializable] public class State {public bool ready,rightVisible,leftVisible,carrying,rescuing,beaconCarrying,overlayEnabled,overlayConfigured;public float gripError,clearance,wristBend,rescueProgress;public Vector3 grip;}
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
    public void Display(Camera eye,Transform baton,bool allowed,bool carrying,BoxCollider parcel,float clearance,float rescueProgress=0,Transform beacon=null){
        if(!model)return;
        bool rescuing=rescueProgress>0,beaconCarrying=beacon!=null;
        overlay.fieldOfView=eye.fieldOfView;overlay.enabled=allowed||carrying||rescuing||beaconCarrying;
        // Put shoulders outside the view; only source mesh arm triangles are rendered.
        model.transform.SetPositionAndRotation(eye.transform.position+eye.transform.rotation*new Vector3(0,-1.65f,Mathf.Lerp(-.15f,.05f,Mathf.InverseLerp(.35f,1,carrying||rescuing||beaconCarrying?1:clearance))),eye.transform.rotation);
        animator.SetFloat("Speed",0);animator.speed=1;
        left.enabled=carrying||rescuing||beaconCarrying;right.enabled=allowed||carrying||rescuing||beaconCarrying;
        state=new State{ready=visual.Ready,rightVisible=right.enabled,leftVisible=left.enabled,carrying=carrying,rescuing=rescuing,beaconCarrying=beaconCarrying,rescueProgress=rescueProgress,overlayEnabled=overlay.enabled,overlayConfigured=stacked,clearance=clearance};
        if(rescuing){
            visual.CarryPose(parcel,false,false,1);
            float enter=Mathf.SmoothStep(0,1,rescueProgress/.18f),press=.018f*Mathf.Sin(rescueProgress*Mathf.PI*4)*enter;
            var leftGoal=Vector3.Lerp(new Vector3(-.23f,-.65f,.35f),new Vector3(-.23f,-.34f+press,.56f),enter);
            var rightGoal=Vector3.Lerp(new Vector3(.23f,-.65f,.35f),new Vector3(.23f,-.34f-press,.60f),enter);
            state.gripError=visual.RescueHands(eye.transform.TransformPoint(leftGoal),eye.transform.TransformPoint(rightGoal),eye.transform.up,eye.transform.forward);
            state.wristBend=visual.RescueWristBend;
        }
        else if(beaconCarrying){
            visual.CarryPose(parcel,false,false,1);
            state.gripError=visual.BeaconHands(beacon.position,beacon.right,beacon.up,beacon.forward);
            state.wristBend=visual.RescueWristBend;
        }
        else if(carrying){visual.CarryPose(parcel,true,false,1);state.gripError=Mathf.Max(visual.Capture().leftContactError,visual.Capture().rightContactError);}
        else if(allowed){
            visual.CarryPose(parcel,false,false,1);
            var desired=baton.position;
            // Rotate the cylindrical handle around its own axis independently of the palm.
            visual.PoseGrip(desired,baton.rotation*Quaternion.Euler(0,-90,0),1,true);state.wristBend=visual.RightWristBend;
            if(visual.BatonGrip(out var actual,out var rotation)){
                baton.SetPositionAndRotation(actual,rotation*Quaternion.Euler(0,90,0));state.grip=actual;state.gripError=Vector3.Distance(desired,actual);
            }
        }
    }
}
}
