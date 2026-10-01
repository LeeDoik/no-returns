using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;
using NoReturns.CarryLab;
public static class ControlsUiCheck {
    static int count;
    static void Check(bool ok,string name){if(!ok)throw new Exception(name);count++;}
    public static object Run(){
        count=0;CarryLanguage.Initialize("controls-ui-check");var go=new GameObject("Controls UI check");var keyboard=InputSystem.AddDevice<Keyboard>();
        using(var controls=new CarryControls(false))try{
            Check(controls.Key(4)=="E","default use key");
            controls.Rebind(4);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.F));InputSystem.Update();
            Check(!controls.Rebinding&&controls.Key(4)=="F","native interactive rebinding");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();
            controls.Rebind(4);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Q));InputSystem.Update();
            Check(controls.Key(4)=="F"&&controls.Notice.Contains("Already assigned"),"duplicate key rejected without losing binding");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();
            controls.Rebind(4);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Escape));InputSystem.Update();
            Check(!controls.Rebinding&&controls.Key(4)=="F","Esc cancels binding");controls.Defaults();Check(controls.Key(4)=="E","defaults restore native binding");
            string pref="NoReturns.Controls.1";var keys=new[]{pref,pref+".Sensitivity",pref+".Fov"};var existed=keys.Select(PlayerPrefs.HasKey).ToArray();string original=PlayerPrefs.GetString(pref,"");float sensitivity=PlayerPrefs.GetFloat(keys[1]),fov=PlayerPrefs.GetFloat(keys[2]);
            try{
                InputSystem.QueueStateEvent(keyboard,new KeyboardState());InputSystem.Update();
                using(var saved=new CarryControls(true)){saved.Defaults();saved.Rebind(4);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.F));InputSystem.Update();saved.Sensitivity=.2f;saved.Fov=90;saved.Save();}
                using(var restored=new CarryControls(true))Check(restored.Key(4)=="F"&&Mathf.Abs(restored.Sensitivity-.2f)<.001f&&restored.Fov==90,"bindings and camera preferences survive a fresh instance");
            }finally{PlayerPrefs.SetString(pref,original);PlayerPrefs.SetFloat(keys[1],sensitivity);PlayerPrefs.SetFloat(keys[2],fov);for(int i=0;i<keys.Length;i++)if(!existed[i])PlayerPrefs.DeleteKey(keys[i]);PlayerPrefs.Save();}
            var hud=go.AddComponent<CrewHud>();hud.BeginMenu("check","CONTROLS & SETTINGS","Controls saved");int clicked=0;hud.MenuButton("buy","Buy beacon license",28,150,600,()=>clicked++);
            Canvas.ForceUpdateCanvases();var events=UnityEngine.Object.FindAnyObjectByType<EventSystem>();
            Check(events!=null&&UnityEngine.Object.FindObjectsByType<EventSystem>().Length==1,"exactly one EventSystem");
            var button=go.GetComponentsInChildren<UnityEngine.UI.Button>().Single();var pos=RectTransformUtility.WorldToScreenPoint(null,button.transform.TransformPoint(button.GetComponent<RectTransform>().rect.center));var data=new PointerEventData(events){position=pos,button=PointerEventData.InputButton.Left};
            var hits=new System.Collections.Generic.List<RaycastResult>();events.RaycastAll(data,hits);Check(go.GetComponentInChildren<UnityEngine.UI.GraphicRaycaster>()!=null&&button.targetGraphic.raycastTarget,"interactive canvas has a raycaster and pointer target");
            ExecuteEvents.Execute(button.gameObject,data,ExecuteEvents.pointerClickHandler);Check(clicked==1,"native pointer click invokes action");
            hud.SetMenuButton("buy","OWNED",false);ExecuteEvents.Execute(button.gameObject,data,ExecuteEvents.pointerClickHandler);Check(clicked==1&&!hud.Click("buy"),"disabled purchase does not invoke");
            var s=new CarryState{phase=2,cinderReview=true,players=4,positions=new Vector3[4]};hud.HideMenu();hud.Apply(s,0,true);hud.SetContext("E 화물 들기");
            Check(go.GetComponentsInChildren<UnityEngine.UI.Text>().All(t=>!t.raycastTarget),"gameplay text does not consume pointer");
            Check(controls.Names.Length==13,"all thirteen button bindings available");
            var room=go.AddComponent<CarryRoom>();room.cinderReview=true;var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
            var workers=new CharacterController[4];for(int i=0;i<4;i++){var worker=new GameObject("Test employee "+i);worker.transform.SetParent(go.transform);workers[i]=worker.AddComponent<CharacterController>();worker.transform.position=new Vector3(-20.7f,1.035f,-25-i);}
            typeof(CarryRoom).GetField("workers",flags).SetValue(room,workers);typeof(CarryRoom).GetField("missionPhase",flags).SetValue(room,2);typeof(CarryRoom).GetField("hazard",flags).SetValue(room,true);typeof(CarryRoom).GetField("occupiedMask",flags).SetValue(room,3);
            var danger=new ThreatState{down=new[]{false,true,false,false}};typeof(CarryRoom).GetField("danger",flags).SetValue(room,danger);Physics.SyncTransforms();var canOpen=typeof(CarryRoom).GetMethod("CanOpenShip",flags);
            Check(!(bool)canOpen.Invoke(room,null),"nearby rescue takes priority over ship UI");danger.down[1]=false;Check((bool)canOpen.Invoke(room,null),"aboard use opens ship when no rescue target");danger.down[1]=true;typeof(CarryRoom).GetField("holder",flags).SetValue(room,0);Check((bool)canOpen.Invoke(room,null),"occupied hands can open terminal without attempting rescue");
            var parcel=new GameObject("Preview parcel");parcel.transform.SetParent(go.transform);parcel.transform.position=new Vector3(-20.7f,2,-24);var body=parcel.AddComponent<Rigidbody>();body.isKinematic=true;var collider=parcel.AddComponent<BoxCollider>();collider.size=new Vector3(.8f,.65f,.65f);
            typeof(CarryRoom).GetField("cargo",flags).SetValue(room,body);typeof(CarryRoom).GetField("cargoCollider",flags).SetValue(room,collider);typeof(CarryRoom).GetField("holder",flags).SetValue(room,-1);danger.down[1]=false;Check(!(bool)canOpen.Invoke(room,null),"aimed parcel aboard takes priority over ship UI");typeof(CarryRoom).GetField("holder",flags).SetValue(room,0);typeof(CarryRoom).GetField("active",flags).SetValue(room,true);typeof(CarryRoom).GetField("menu",flags).SetValue(room,false);typeof(CarryRoom).GetMethod("PreviewPlacement",flags).Invoke(room,null);
            var preview=(LineRenderer)typeof(CarryRoom).GetField("placementPreview",flags).GetValue(room);bool edges=preview.positionCount==17;for(int i=1;i<preview.positionCount;i++){var delta=preview.GetPosition(i)-preview.GetPosition(i-1);int axes=(Mathf.Abs(delta.x)>.001f?1:0)+(Mathf.Abs(delta.y)>.001f?1:0)+(Mathf.Abs(delta.z)>.001f?1:0);edges&=axes==1;}Check(edges,"preview traces box edges without diagonal connectors");UnityEngine.Object.DestroyImmediate(preview.sharedMaterial);UnityEngine.Object.DestroyImmediate(preview.gameObject);typeof(CarryRoom).GetField("placementPreview",flags).SetValue(room,null);
            return new {status="PASS",checks=count};
        }finally{InputSystem.RemoveDevice(keyboard);UnityEngine.Object.DestroyImmediate(go);}
    }
}
