using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace NoReturns.CarryLab {
// Native Input System bindings; no second input profile or dependency.
public sealed class CarryControls : IDisposable {
    const string Preference="NoReturns.Controls.1";
    readonly bool persist;
    readonly InputActionMap map=new InputActionMap("Employee");
    InputActionRebindingExtensions.RebindingOperation rebind;
    public readonly string[] Names={"Forward","Back","Left","Right","Use / rescue","Release","Place / baton","Rotate item","Jump","Quiet walk","Call","Field log","Reset lab"};
    readonly string[] paths={"<Keyboard>/w","<Keyboard>/s","<Keyboard>/a","<Keyboard>/d","<Keyboard>/e","<Keyboard>/q","<Mouse>/leftButton","<Mouse>/rightButton","<Keyboard>/space","<Keyboard>/leftShift","<Keyboard>/c","<Keyboard>/tab","<Keyboard>/r"};
    public float Sensitivity=.12f,Fov=80;
    public int EscapeConsumedFrame {get;private set;}=-1;
    public bool Rebinding=>rebind!=null;
    public string Notice="";
    public CarryControls(bool persist){
        this.persist=persist;
        for(int i=0;i<Names.Length;i++){
            var action=map.AddAction("Key"+i,InputActionType.Button,paths[i]);var binding=action.bindings[0];
            // Stable binding IDs let native JSON overrides survive process restarts.
            binding.id=new Guid("6e6f7265-7475-726e-7300-"+(i+1).ToString("D12"));action.ChangeBinding(0).To(binding);
        }
        if(persist){try{var json=PlayerPrefs.GetString(Preference,"");if(json!="")map.LoadBindingOverridesFromJson(json);}catch(Exception){map.RemoveAllBindingOverrides();}
            Sensitivity=Mathf.Clamp(PlayerPrefs.GetFloat(Preference+".Sensitivity",.12f),.04f,.3f);Fov=Mathf.Clamp(PlayerPrefs.GetFloat(Preference+".Fov",80),65,100);}
        map.Enable();
    }
    public bool Pressed(int i)=>map.actions[i].WasPressedThisFrame();
    public bool Held(int i)=>map.actions[i].IsPressed();
    public string Key(int i){var path=map.actions[i].bindings[0].effectivePath;var key=path.Substring(path.LastIndexOf('/')+1);return path.StartsWith("<Keyboard>/",StringComparison.Ordinal)&&key.Length==1?key.ToUpperInvariant():map.actions[i].GetBindingDisplayString();}
    public void Rebind(int i){
        if(Rebinding)return;var action=map.actions[i];var old=action.bindings[0].overridePath;map.Disable();Notice="Press a key or mouse button / Esc cancels";
        rebind=action.PerformInteractiveRebinding(0).WithExpectedControlType("Button").OnMatchWaitForAnother(0).WithControlsExcluding("<Mouse>/scroll").WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(op=>{EscapeConsumedFrame=Time.frameCount;Finish(op);}).OnComplete(op=>{
                bool duplicate=false;foreach(var other in map.actions)if(other!=action&&other.bindings[0].effectivePath==action.bindings[0].effectivePath)duplicate=true;
                if(duplicate){if(old==null)action.RemoveBindingOverride(0);else action.ApplyBindingOverride(0,old);Notice="Already assigned / choose another key";}else{Notice="Controls saved";Save();}Finish(op);
            }).Start();
    }
    void Finish(InputActionRebindingExtensions.RebindingOperation op){op.Dispose();rebind=null;map.Enable();}
    public void Cancel(){rebind?.Cancel();Notice="";}
    public void Defaults(){Cancel();map.RemoveAllBindingOverrides();Sensitivity=.12f;Fov=80;Notice="Defaults restored";Save();}
    public void Save(){if(!persist)return;try{PlayerPrefs.SetString(Preference,map.SaveBindingOverridesAsJson());PlayerPrefs.SetFloat(Preference+".Sensitivity",Sensitivity);PlayerPrefs.SetFloat(Preference+".Fov",Fov);PlayerPrefs.Save();}catch(PlayerPrefsException){Notice="Settings apply this session / save failed";}}
    public void Dispose(){Cancel();map.Dispose();}
}
}
