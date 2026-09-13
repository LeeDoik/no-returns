using UnityEngine;
namespace NoReturns.CarryLab {
// Presentation only. No input, authority, mission progression or deadline disclosure.
public sealed class CrewHud : MonoBehaviour {
    UnityEngine.UI.Text objective,wallet,cue,prompt,notice,center;
    readonly UnityEngine.UI.Text[] rows=new UnityEngine.UI.Text[4];
    GameObject surface;
    static readonly Color Ink=new Color(.84f,.87f,.77f);
    static readonly Color[] Colors={new Color(1,.46f,.17f),new Color(.2f,.85f,.86f),new Color(.85f,.5f,.95f),new Color(.95f,.85f,.3f)};
    static string T(string value)=>CarryLanguage.Text(value);
    RectTransform Box(string name,Vector2 anchor,Vector2 pivot,Vector2 pos,Vector2 size,bool panel){
        var g=new GameObject(name,typeof(RectTransform));g.transform.SetParent(surface.transform,false);
        var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=anchor;r.pivot=pivot;r.anchoredPosition=pos;r.sizeDelta=size;
        if(panel){var bg=g.AddComponent<UnityEngine.UI.Image>();bg.color=new Color(.025f,.035f,.035f,.88f);bg.raycastTarget=false;}
        return r;
    }
    UnityEngine.UI.Text Text(RectTransform parent,string name,Vector2 pos,Vector2 size,int fontSize){
        var g=new GameObject(name,typeof(RectTransform));g.transform.SetParent(parent,false);var r=g.GetComponent<RectTransform>();r.anchorMin=r.anchorMax=r.pivot=new Vector2(0,1);r.anchoredPosition=pos;r.sizeDelta=size;
        var t=g.AddComponent<UnityEngine.UI.Text>();t.font=CarryLanguage.Font;t.fontSize=fontSize;t.color=Ink;t.raycastTarget=false;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Truncate;return t;
    }
    void Build(){
        surface=new GameObject("Crew HUD",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.CanvasScaler));surface.transform.SetParent(transform,false);
        var canvas=surface.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=10;
        var scaler=surface.GetComponent<UnityEngine.UI.CanvasScaler>();scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);scaler.screenMatchMode=UnityEngine.UI.CanvasScaler.ScreenMatchMode.Expand;
        var left=Box("Contract",Vector2.up,Vector2.up,new Vector2(24,-24),new Vector2(550,142),true);
        objective=Text(left,"Objective",new Vector2(16,-12),new Vector2(518,78),20);
        wallet=Text(left,"Wallet",new Vector2(16,-98),new Vector2(518,30),18);
        var crew=Box("Crew status",Vector2.one,Vector2.one,new Vector2(-24,-24),new Vector2(310,154),true);
        for(int i=0;i<4;i++){rows[i]=Text(crew,"Employee "+i,new Vector2(14,-10-i*34),new Vector2(282,32),19);rows[i].color=Colors[i];}
        var bottom=Box("Actions",new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(0,24),new Vector2(1000,112),true);
        cue=Text(bottom,"Suppression cue",new Vector2(16,-8),new Vector2(968,27),18);
        prompt=Text(bottom,"Context controls",new Vector2(16,-38),new Vector2(968,30),21);
        notice=Text(bottom,"Notice",new Vector2(16,-75),new Vector2(968,28),17);
        var cross=Box("Reticle",new Vector2(.5f,.5f),new Vector2(.5f,.5f),Vector2.zero,new Vector2(24,30),false);
        center=Text(cross,"Aim",Vector2.zero,new Vector2(24,30),24);center.text="+";center.alignment=TextAnchor.MiddleCenter;
    }
    public void Apply(CarryState s,int local,bool visible){
        if(surface==null)Build();surface.SetActive(visible);if(!visible)return;
        objective.text="NO RETURNS / "+T(CarryMission.Objective(s.phase));
        if(s.phase<0)objective.text="NO RETURNS / "+T("Move the parcel through the passage and onto the marked floor.");
        wallet.text=string.Format(T("WALLET {0} CR / CREW {1}/4"),s.credits,s.players);
        for(int i=0;i<4;i++){
            bool present=(s.occupiedMask&(1<<i))!=0;string state=!present?"EMPTY":s.danger!=null&&s.danger.IsDown(i)?"DOWN":s.holder==i?"CARGO":s.beaconCarrier==i?"BEACON":s.positions!=null&&CarryMission.Aboard(s.positions[i])?"ABOARD":"IN FIELD";
            rows[i].text=(i+1).ToString("00")+" / "+T(state)+(i==local?" / "+T("YOU"):"");rows[i].color=present?Colors[i]:new Color(.42f,.45f,.43f);
        }
        bool down=s.danger!=null&&s.danger.IsDown(local),hands=s.holder==local||s.beaconCarrier==local;
        string action=down?"DOWN / wait for teammate rescue. All down: emergency recovery.":hands?"[Q] SET DOWN   /   HANDS OCCUPIED":s.phase==3&&!s.receiptCollected?"Collect receipt at terminal [E] / No pay until return":"[E] Interact / Hold E rescue / LMB baton / Tab log / Esc menu";
        prompt.text=T(action);
        if(!down&&s.danger!=null&&s.danger.RescueAt(local)>0)prompt.text=string.Format(T("RESCUING {0}% / keep holding E"),Mathf.Clamp(Mathf.RoundToInt(s.danger.RescueAt(local)/2.5f*100),0,100));
        cue.text=s.hazard&&(s.phase==2||s.phase==3)?T(CarrySuppression.Cue(s.suppressionStage)):T("WASD move / Mouse look / Shift quiet walk");
        // No creature AI state or remaining suppression seconds on the HUD.
        notice.text=s.phase==3&&s.receiptCollected?T("Receipt collected / return aboard to get paid"):T(s.message??"");
    }
}
}
