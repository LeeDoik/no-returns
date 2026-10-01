using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CrewHud {
    GameObject menuSurface,menuPanel;
    string menuKey;
    readonly Dictionary<string,UnityEngine.UI.Text> menuTexts=new Dictionary<string,UnityEngine.UI.Text>();
    readonly Dictionary<string,UnityEngine.UI.Button> menuButtons=new Dictionary<string,UnityEngine.UI.Button>();
    public UnityEngine.UI.InputField Address {get;private set;}
    public string MenuText(string id)=>menuTexts.TryGetValue(id,out var t)?t.text:"";
    public bool Click(string id){if(!menuButtons.TryGetValue(id,out var b)||!b.IsInteractable()||!b.gameObject.activeInHierarchy)return false;if(!Application.isPlaying){b.onClick.Invoke();return true;}
        Canvas.ForceUpdateCanvases();var r=b.GetComponent<RectTransform>();var data=new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,r.TransformPoint(r.rect.center)),button=UnityEngine.EventSystems.PointerEventData.InputButton.Left};
        var hits=new List<UnityEngine.EventSystems.RaycastResult>();UnityEngine.EventSystems.EventSystem.current.RaycastAll(data,hits);if(hits.Count==0||UnityEngine.EventSystems.ExecuteEvents.GetEventHandler<UnityEngine.UI.Button>(hits[0].gameObject)!=b.gameObject)return false;
        UnityEngine.EventSystems.ExecuteEvents.Execute(b.gameObject,data,UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);return true;}
    public void HideMenu(){if(menuSurface)menuSurface.SetActive(false);}
    public bool BeginMenu(string key,string title,string subtitle){
        if(!menuSurface){
            menuSurface=new GameObject("Terminal UI",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.CanvasScaler),typeof(UnityEngine.UI.GraphicRaycaster));menuSurface.transform.SetParent(transform,false);
            var c=menuSurface.GetComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;c.sortingOrder=20;
            var scale=menuSurface.GetComponent<UnityEngine.UI.CanvasScaler>();scale.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scale.referenceResolution=new Vector2(1280,720);scale.screenMatchMode=UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand;
            if(!FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>()){
                var events=new GameObject("UI events",typeof(UnityEngine.EventSystems.EventSystem),typeof(UnityEngine.InputSystem.UI.InputSystemUIInputModule));events.transform.SetParent(transform,false);events.GetComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>().AssignDefaultActions();
            }
        }
        menuSurface.SetActive(true);if(menuKey==key)return false;menuKey=key;
        if(menuPanel){menuPanel.SetActive(false);Destroy(menuPanel);}menuTexts.Clear();menuButtons.Clear();Address=null;
        menuPanel=new GameObject("Menu "+key,typeof(RectTransform),typeof(UnityEngine.UI.Image));menuPanel.transform.SetParent(menuSurface.transform,false);
        var rect=menuPanel.GetComponent<RectTransform>();rect.anchorMin=rect.anchorMax=rect.pivot=new Vector2(.5f,.5f);rect.sizeDelta=new Vector2(1100,620);
        menuPanel.GetComponent<UnityEngine.UI.Image>().color=new Color(.025f,.035f,.035f,.98f);
        MenuLabel("Title",title,28,28,1044,40,28);MenuLabel("Subtitle",subtitle,28,78,1044,60,18);return true;
    }
    public void MenuLabel(string id,string text,float x,float y,float w,float h,int font=19){var t=Text(menuPanel.GetComponent<RectTransform>(),id,new Vector2(x,-y),new Vector2(w,h),font);t.text=T(text);menuTexts[id]=t;}
    public void SetMenuText(string id,string text){if(menuTexts.TryGetValue(id,out var t))t.text=T(text);}
    public void MenuButton(string id,string text,float x,float y,float w,System.Action clicked){
        var g=new GameObject(id,typeof(RectTransform),typeof(UnityEngine.UI.Image),typeof(UnityEngine.UI.Button));g.transform.SetParent(menuPanel.transform,false);var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=r.pivot=Vector2.up;r.anchoredPosition=new Vector2(x,-y);r.sizeDelta=new Vector2(w,44);
        var image=g.GetComponent<UnityEngine.UI.Image>();image.color=new Color(.12f,.2f,.19f);
        var b=g.GetComponent<UnityEngine.UI.Button>();b.targetGraphic=image;var colors=b.colors;colors.highlightedColor=new Color(.6f,.85f,.8f);colors.selectedColor=colors.highlightedColor;colors.disabledColor=new Color(.45f,.45f,.45f);b.colors=colors;b.onClick.AddListener(()=>clicked());
        var t=Text(r,id+" label",new Vector2(10,0),new Vector2(w-20,44),19);t.text=T(text);t.alignment=TextAnchor.MiddleCenter;menuTexts[id]=t;if(menuButtons.Count==0)FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>()?.SetSelectedGameObject(g);menuButtons[id]=b;
    }
    public void SetSlider(string id,float value){var r=menuPanel.transform.Find(id);if(r)r.GetComponent<UnityEngine.UI.Slider>().SetValueWithoutNotify(value);}
    public void SetMenuButton(string id,string text,bool enabled){SetMenuText(id,text);if(menuButtons.TryGetValue(id,out var b))b.interactable=enabled;}
    public void MenuAddress(string address,System.Action<string> changed){
        var g=new GameObject("LAN address",typeof(RectTransform),typeof(UnityEngine.UI.Image),typeof(UnityEngine.UI.InputField));g.transform.SetParent(menuPanel.transform,false);var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=r.pivot=Vector2.up;r.anchoredPosition=new Vector2(28,-230);r.sizeDelta=new Vector2(650,44);g.GetComponent<UnityEngine.UI.Image>().color=new Color(.1f,.15f,.15f);
        Address=g.GetComponent<UnityEngine.UI.InputField>();Address.textComponent=Text(r,"Address text",new Vector2(10,0),new Vector2(630,44),21);Address.textComponent.alignment=TextAnchor.MiddleLeft;Address.characterLimit=64;Address.text=address;Address.onValueChanged.AddListener(v=>changed(v));
    }
    public void MenuSlider(string id,float x,float y,float value,float min,float max,System.Action<float> changed){
        var g=new GameObject(id,typeof(RectTransform),typeof(UnityEngine.UI.Image),typeof(UnityEngine.UI.Slider));g.transform.SetParent(menuPanel.transform,false);var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=r.pivot=Vector2.up;r.anchoredPosition=new Vector2(x,-y);r.sizeDelta=new Vector2(410,20);g.GetComponent<UnityEngine.UI.Image>().color=new Color(.1f,.2f,.18f);
        var handle=new GameObject("Handle",typeof(RectTransform),typeof(UnityEngine.UI.Image));handle.transform.SetParent(r,false);var h=handle.GetComponent<RectTransform>();h.sizeDelta=new Vector2(18,26);handle.GetComponent<UnityEngine.UI.Image>().color=Ink;
        var slider=g.GetComponent<UnityEngine.UI.Slider>();slider.handleRect=h;slider.targetGraphic=handle.GetComponent<UnityEngine.UI.Image>();slider.minValue=min;slider.maxValue=max;slider.value=value;slider.onValueChanged.AddListener(v=>changed(v));
    }
}
}
