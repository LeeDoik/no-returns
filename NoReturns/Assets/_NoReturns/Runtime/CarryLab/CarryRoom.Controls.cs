using UnityEngine;
using UnityEngine.InputSystem;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    CarryControls controls;
    bool shipMenu,settingsMenu,confirmReturn;
    CarryInput uiCommands=new CarryInput();
    float carryYaw,carryPitch,carryDistance=.8f;
    LineRenderer placementPreview;
    bool UiOpen=>!active||menu||shipMenu||journalOpen;
    bool AboardLocal=>CarryMission.Aboard(workers[local].transform.position,cinderReview);
    void ClosePanels(){controls.Cancel();menu=false;shipMenu=false;settingsMenu=false;confirmReturn=false;journalOpen=false;SetCursor(active);}
    void OpenShip(){if(!active||missionPhase<0||!AboardLocal)return;ClosePanels();shipMenu=true;SetCursor(false);}
    void TogglePause(){if(controls.Rebinding||controls.EscapeConsumedFrame==Time.frameCount)return;if(UiOpen&&active)ClosePanels();else{menu=true;SetCursor(false);}}
    bool RescueNearby(){
        if((!hazard&&!companionPractice)||danger==null||holder==local||equipment.Carrier==local)return false;
        for(int i=0;i<4;i++)if(i!=local&&Present(i)&&danger.IsDown(i)&&Vector3.Distance(workers[local].transform.position,workers[i].transform.position)<=2&&CarryThreat.Sight(workers[local].transform.position,workers[i].transform.position))return true;
        return false;
    }
    bool ParcelTarget(int who,Quaternion look){
        int phase=hosting&&mission!=null?mission.Phase:missionPhase;
        if(holder>=0||equipment.Carrier==who||(phase>=0&&phase!=2))return false;
        var origin=workers[who].transform.position+Vector3.up*1.45f;var delta=cargo.position-origin;
        return delta.magnitude<=2.4f&&Vector3.Angle(look*Vector3.forward,delta)<=65&&Physics.Raycast(origin,delta.normalized,out var hit,delta.magnitude+.1f,~0,QueryTriggerInteraction.Ignore)&&hit.collider==cargoCollider;
    }
    bool CanOpenShip()=>AboardLocal&&missionPhase>=0&&!RescueNearby()&&(cargo==null||!ParcelTarget(local,Quaternion.Euler(pitch,yaw,0)))&&!equipment.Target(workers[local].transform.position+Vector3.up*1.57f,Quaternion.Euler(pitch,yaw,0));
    CarryInput ReadControls(){
        if(holder!=local){carryYaw=0;carryPitch=0;carryDistance=.8f;}
        var cmd=new CarryInput{seq=seq,yaw=yaw,pitch=pitch,distance=carryDistance,turnYaw=carryYaw,turnPitch=carryPitch};
        if(UiOpen)return cmd;
        var m=Mouse.current;
        if(m!=null){var delta=m.delta.ReadValue()*controls.Sensitivity;
            if(holder==local&&controls.Held(7)){carryYaw=Mathf.Repeat(carryYaw+delta.x+180,360)-180;carryPitch=Mathf.Clamp(carryPitch-delta.y,-80,80);}
            else{yaw+=delta.x;pitch=Mathf.Clamp(pitch-delta.y,-70,70);}
            if(holder==local)carryDistance=Mathf.Clamp(carryDistance+m.scroll.ReadValue().y*.002f,.75f,1.6f);
        }
        cmd.yaw=yaw;cmd.pitch=pitch;cmd.distance=carryDistance;cmd.turnYaw=carryYaw;cmd.turnPitch=carryPitch;
        cmd.x=(controls.Held(3)?1:0)-(controls.Held(2)?1:0);cmd.z=(controls.Held(0)?1:0)-(controls.Held(1)?1:0);
        cmd.interact=controls.Pressed(4);cmd.rescue=controls.Held(4);cmd.drop=controls.Pressed(5);cmd.jump=controls.Pressed(8);cmd.quiet=controls.Held(9);cmd.call=controls.Pressed(10);cmd.reset=controls.Pressed(12)&&hosting;
        if(controls.Pressed(6)){if(holder==local||equipment.Carrier==local)cmd.place=true;else cmd.shove=true;}
        if(cmd.interact&&CanOpenShip()){
            OpenShip();cmd.x=cmd.z=0;cmd.interact=cmd.rescue=cmd.jump=cmd.shove=cmd.place=cmd.drop=cmd.call=false;
        }
        return cmd;
    }
    void RenderMenus(CarryState s){
        if(shipMenu&&(!AboardLocal||(s.danger!=null&&s.danger.IsDown(local))))ClosePanels();
        if(!UiOpen){playHud.HideMenu();return;}
        string kind=settingsMenu?"settings":!active?"home":shipMenu?"ship":journalOpen?"journal":"pause";
        bool fresh=playHud.BeginMenu(kind+(s.visit!=null&&s.visit.active?"visit":"idle")+(CarryLanguage.Korean?"ko":"en"),kind=="ship"?"FLATBED / SHIP TERMINAL":kind=="settings"?"CONTROLS & SETTINGS":kind=="journal"?"CINDER DEPOT / SHARED FIELD LOG":"NO RETURNS",kind=="ship"?"CINDER DEPOT / BAY 04 · shared crew wallet":kind=="settings"?"Click a binding, then press a key. Esc cancels. Esc always closes menus.":kind=="journal"?"The world keeps moving. Read aboard. Records reset on next arrival; not saved after exit.":"Direct LAN / choose host or join. Menus do not pause the world.");
        if(fresh){
            if(kind=="home"){
                playHud.MenuLabel("Address label","HOST LAN ADDRESS",28,194,650,30);playHud.MenuAddress(address,v=>address=v);
                playHud.MenuButton("host","HOST",28,150,310,StartHost);playHud.MenuButton("join","JOIN",700,230,370,StartJoin);
                playHud.MenuLabel("Port",cinderReview?"Direct LAN / TCP 27842\nSame PC: 127.0.0.1\nBAY 04 receipt / return aboard to settle.":"Direct LAN / TCP 27841\nSame PC: 127.0.0.1\nAllow host through Windows Firewall if prompted.",28,310,1000,100);
                playHud.MenuButton("settings","CONTROLS & SETTINGS",28,480,510,()=>settingsMenu=true);playHud.MenuButton("quit","QUIT",560,480,510,()=>Application.Quit());
            }else if(kind=="pause"){
                playHud.MenuButton("resume","RESUME",28,150,510,ClosePanels);playHud.MenuButton("settings","CONTROLS & SETTINGS",560,150,510,()=>settingsMenu=true);
                playHud.MenuButton("leave","LEAVE SESSION",28,215,510,Disconnect);playHud.MenuButton("quit","QUIT",560,215,510,()=>Application.Quit());
                playHud.MenuLabel("Help","Use opens the ship terminal aboard. Buy supplies there.\nPlace with left click, rotate with right click, adjust reach with wheel.",28,320,1000,90);
            }else if(kind=="settings"){
                for(int i=0;i<controls.Names.Length;i++){int binding=i;playHud.MenuButton("bind"+i,"",28+(i/7)*542,145+(i%7)*46,510,()=>controls.Rebind(binding));}
                playHud.MenuLabel("Sensitivity","",28,480,490,30);playHud.MenuSlider("Sensitivity slider",28,515,controls.Sensitivity,.04f,.3f,v=>{controls.Sensitivity=v;controls.Save();});
                playHud.MenuLabel("Fov","",570,480,490,30);playHud.MenuSlider("Fov slider",570,515,controls.Fov,65,100,v=>{controls.Fov=v;eye.fieldOfView=v;controls.Save();});
                playHud.MenuButton("defaults","RESTORE DEFAULTS",28,558,510,()=>{controls.Defaults();eye.fieldOfView=controls.Fov;});playHud.MenuButton("back","BACK",570,558,490,()=>{controls.Cancel();settingsMenu=false;});
            }else if(kind=="ship"){
                playHud.MenuLabel("Route","",28,150,630,90,24);playHud.MenuLabel("Crew","",28,250,630,35);playHud.MenuLabel("Reason","",28,296,630,90);
                playHud.MenuButton("shipAction","",28,402,630,()=>{if((missionPhase==2||missionPhase==3)&&!receiptCollected&&!(hosting?mission?.Visit?.Departing==true:target?.visit?.departing==true)&&!confirmReturn){confirmReturn=true;return;}uiCommands.action=true;uiCommands.confirmReturn=confirmReturn;confirmReturn=false;});
                if(cinderReview&&s.visit!=null&&s.visit.active){
                    playHud.MenuLabel("Equipment","REVIVAL / 100 UNBANKED CR",710,150,360,60,22);
                    for(int i=0;i<4;i++){int targetSlot=i;playHud.MenuButton("revive"+i,"",710,220+i*50,360,()=>uiCommands.revive=targetSlot);}
                    playHud.MenuLabel("Revival","",710,430,360,100,18);
                }else{
                    playHud.MenuLabel("Equipment","DECOY BEACON / 120 CR",710,150,360,60,22);playHud.MenuLabel("Equipment details","One shared device. Two 8-second signals per shift. Carry from the ship; place on field ground.",710,230,360,130);
                    playHud.MenuButton("buy","Buy beacon license",710,402,360,()=>uiCommands.buy=true);
                    playHud.MenuLabel("Stock","",710,458,360,80);
                }
                if(!cinderReview)playHud.MenuButton("contract","Change selected contract",28,470,630,()=>uiCommands.contract=true);
            }else{
                for(int i=0;i<2;i++){bool found=clues!=null&&(clues.Mask&(1<<i))!=0;playHud.MenuLabel("Record"+i,found?(i==0?CarryClues.FirstTitle:CarryClues.SecondTitle):"UNRECORDED / inspect the site",28,145+i*170,1044,32,22);playHud.MenuLabel("Body"+i,found?(i==0?CarryClues.FirstBody:CarryClues.SecondBody):"No entry yet. A teammate can share it by inspecting a terminal.",28,187+i*170,1044,120);}
            }
            if(kind!="settings"){playHud.MenuButton("language","한국어 / English",710,558,360,CarryLanguage.Toggle);if(active)playHud.MenuButton("close","CLOSE / Esc",28,558,630,ClosePanels);}
            if(kind=="home"||kind=="pause")playHud.MenuLabel("Status","",28,420,1044,50);
        }
        if(kind=="settings"){
            for(int i=0;i<controls.Names.Length;i++)playHud.SetMenuButton("bind"+i,T(controls.Names[i])+"   /   "+controls.Key(i),!controls.Rebinding);
            playHud.SetMenuText("Subtitle",controls.Notice!=""?controls.Notice:"Click a binding, then press a key. Esc cancels. Esc always closes menus.");
            playHud.SetSlider("Sensitivity slider",controls.Sensitivity);playHud.SetSlider("Fov slider",controls.Fov);
            playHud.SetMenuText("Sensitivity",T("Mouse sensitivity")+"   "+controls.Sensitivity.ToString("0.00"));playHud.SetMenuText("Fov","FOV   "+Mathf.RoundToInt(controls.Fov));
        }else if(kind=="ship"){
            int aboard=0;for(int i=0;i<4;i++)if((s.occupiedMask&(1<<i))!=0&&CarryMission.Aboard(s.positions[i],cinderReview)&&(s.danger==null||!s.danger.IsDown(i)))aboard++;
            string action=s.phase==0?"SELECT CINDER ROUTE":s.phase==1?"DEPART / CINDER DEPOT":s.phase==4?"PREPARE NEXT SHIFT":!s.receiptCollected?confirmReturn?"CONFIRM RETURN / 0 CR":"RETURN WITHOUT RECEIPT":"RETURN & SETTLE";
            bool all=aboard==s.players,host=local==0;
            playHud.SetMenuText("Route",s.phase==4?string.Format(T("RECEIPT {0} + RETURN {1} CR"),s.receipt,s.returnPay):CarryMission.Objective(s.phase));
            playHud.SetMenuText("Crew",string.Format(T("ABOARD {0}/{1} / WALLET {2} CR"),aboard,s.players,s.credits));
            playHud.SetMenuText("Reason",!host?"Host controls departure, return and shared purchases":s.phase==1||s.phase==2||s.phase==3?!all?"Wait for every crew member to board":!s.receiptCollected&&s.phase!=1?"Returning now pays 0 CR. Click again to confirm.":"Crew ready / confirm using the button":"Select the route or prepare the next shift");
            playHud.SetMenuButton("shipAction",action,host&&((s.phase!=1&&s.phase!=2&&s.phase!=3)||all));
            playHud.SetMenuButton("buy",s.unlocked?"OWNED":s.credits<120?"INSUFFICIENT FUNDS / 120 CR":"Buy beacon license",host&&!s.unlocked&&s.credits>=120&&(s.phase==0||s.phase==4));
            playHud.SetMenuText("Stock",s.unlocked?string.Format(T("USES {0}/2 / SHARED DEVICE"),s.charges):"Buy during preparation or shift report");
            playHud.SetMenuButton("contract",s.hard?"RISK / receipt 450 + return 180":"STANDARD / receipt 300 + return 120",host&&s.phase==1&&s.deliveries>0&&s.hazard&&!cinderReview);
            if(cinderReview){
                var v=s.visit;bool field=v!=null&&v.active;
                playHud.SetMenuButton("shipAction",field&&v.departing?(v.automatic?"AUTOMATIC DEPARTURE / CANNOT CANCEL":"CANCEL DEPARTURE"):action,s.phase!=1||all);
                if(field&&v.automatic)playHud.SetMenuButton("shipAction","AUTOMATIC DEPARTURE / CANNOT CANCEL",false);
                playHud.SetMenuText("Crew",string.Format(T("ABOARD {0}/{1} / WALLET {2} CR"),aboard,s.players,s.credits));
                playHud.SetMenuText("Reason",s.phase<2||v==null?"Select the route or prepare the next shift":s.phase==4?string.Format(T("GROSS {0} / REVIVAL -{1} / RETURN -{2} / PAID {3} CR"),v.gross,v.revivalFees,v.returnFees,v.paid):string.Format(T("UNBANKED {0} CR / REVIVAL 100 CR\nOutside crew cost 100 CR each at departure."),v.available));
                if(s.phase==4)playHud.SetMenuText("Route",v!=null&&v.failed?"VISIT FAILED / NO PAY":"VISIT SETTLED");
                if(field){
                    playHud.SetMenuText("Route",v.departing?v.automatic?"AUTOMATIC DEPARTURE / RETURN NOW":"DEPARTURE STARTED / RETURN NOW":CarryMission.Objective(s.phase));
                    for(int i=0;i<4;i++)playHud.SetMenuButton("revive"+i,string.Format(T("REVIVE EMPLOYEE {0} / 100 CR"),i+1),v.eliminated[i]&&(s.occupiedMask&(1<<i))!=0&&v.reviving<0&&v.available>=CarryVisit.RevivalCost);
                    playHud.SetMenuText("Revival",v.reviving>=0?string.Format(T("REVIVING {0} / {1}s"),v.reviving+1,Mathf.CeilToInt(v.revivalLeft)):v.available<CarryVisit.RevivalCost?"Not enough unbanked earnings. Deliver more or depart.":"Select one eliminated employee. One revival at a time.");
                }
                playHud.SetMenuButton("buy",s.unlocked?"OWNED":s.credits<120?"INSUFFICIENT FUNDS / 120 CR":"Buy beacon license",host&&!s.unlocked&&s.credits>=120&&(s.phase==0||s.phase==4));
            }

        }else if(kind=="home"||kind=="pause")playHud.SetMenuText("Status",status+(hazard&&!cinderReview&&active?"\n"+(hosting?T(saveNotice):T("Using host progress / personal save unchanged")):""));
    }
    bool Placement(int who,out Vector3 position,out Quaternion rotation){
        position=cargo.position;rotation=Quaternion.Euler(0,inputs[who].yaw+inputs[who].turnYaw,0);
        Vector3 origin=workers[who].transform.position+Vector3.up*1.57f,dir=Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0)*Vector3.forward;
        RaycastHit? nearest=null;foreach(var h in Physics.RaycastAll(origin,dir,2.8f,~0,QueryTriggerInteraction.Ignore))if(h.collider!=cargoCollider&&h.collider!=workers[who]&&(!nearest.HasValue||h.distance<nearest.Value.distance))nearest=h;
        if(!nearest.HasValue||nearest.Value.normal.y<.8f)return false;position=nearest.Value.point+Vector3.up*.35f;
        foreach(var c in Physics.OverlapBox(position,new Vector3(.4f,.325f,.325f)*.99f,rotation,~0,QueryTriggerInteraction.Ignore))if(c!=cargoCollider&&c!=workers[who])return false;
        var delta=position-cargo.position;
        foreach(var h in Physics.BoxCastAll(cargo.position,new Vector3(.4f,.325f,.325f)*.98f,delta.normalized,cargo.rotation,delta.magnitude,~0,QueryTriggerInteraction.Ignore))if(h.collider!=cargoCollider&&h.collider!=workers[who]&&h.distance<delta.magnitude-.04f)return false;
        return true;
    }
    void PlaceParcel(int who){if(holder!=who)return;if(!Placement(who,out var p,out var r)){status="Placement blocked / aim at clear nearby ground";return;}Release();cargo.position=p;cargo.rotation=r;status="Parcel placed";}
    void PreviewPlacement(){
        bool show=active&&!UiOpen&&holder==local;
        if(!show){if(placementPreview)placementPreview.enabled=false;return;}
        bool clear=Placement(local,out var p,out var r);
        if(!placementPreview){var g=new GameObject("Parcel placement preview");placementPreview=g.AddComponent<LineRenderer>();placementPreview.sharedMaterial=new Material(Shader.Find("Universal Render Pipeline/Unlit"));placementPreview.widthMultiplier=.018f;placementPreview.positionCount=17;placementPreview.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;}
        placementPreview.enabled=true;placementPreview.sharedMaterial.color=clear?new Color(.25f,.9f,.65f):new Color(1,.35f,.15f);
        var corners=new Vector3[8];for(int i=0;i<8;i++)corners[i]=p+r*new Vector3((i&1)==0?-.4f:.4f,(i&2)==0?-.325f:.325f,(i&4)==0?-.325f:.325f);
        int[] path={0,1,3,2,0,4,5,1,3,7,6,2,0,4,6,7,5};for(int i=0;i<path.Length;i++)placementPreview.SetPosition(i,corners[path[i]]);
    }
    string ContextPrompt(){
        var visit=hosting?mission?.Visit?.Snapshot():target?.visit;
        if(visit!=null&&visit.active&&visit.eliminated[local])return T("ELIMINATED / wait for ship revival or departure");
        if(visit!=null&&visit.active&&visit.down[local])return (CrewCount==1?controls.Key(4)+T(" hold to self-revive / two chances per landing"):T("DOWN / teammate rescue needed"))+" / "+Mathf.CeilToInt(visit.bleedout[local])+"s";
        if(danger!=null&&danger.IsDown(local))return T("DOWN / wait for teammate rescue. All down: emergency recovery.");
        if(holder==local)return controls.Key(6)+T(" place · ")+controls.Key(7)+T(" hold to rotate · Wheel reach · ")+controls.Key(5)+T(" release");
        if(equipment.Carrier==local)return controls.Key(6)+T(" place beacon · ")+controls.Key(5)+T(" release");

        if(hazard&&danger!=null&&danger.RescueAt(local)>0)return string.Format(T("RESCUING {0}% / keep holding "),Mathf.RoundToInt(danger.RescueAt(local)/2.5f*100))+controls.Key(4);
        if(RescueNearby())return controls.Key(4)+T(" hold to rescue");
        var origin=eye.transform.position;var look=eye.transform.rotation;
        if(missionPhase==3&&!receiptCollected&&ReceiptFeedback.CanReach(origin,look))return controls.Key(4)+T(" COLLECT RECEIPT");
        if(clues!=null&&clues.Target(origin,look)>=0)return controls.Key(4)+T(" INSPECT TERMINAL");
        if(equipment.Target(origin,look))return controls.Key(4)+T(" PICK UP BEACON");
        if(ParcelTarget(local,look))return controls.Key(4)+T(" PICK UP PARCEL");
        if(AboardLocal&&missionPhase>=0)return controls.Key(4)+T(" OPEN SHIP TERMINAL");
        if(hazard&&(missionPhase==2||missionPhase==3)){float cooldown=danger==null?0:danger.CooldownAt(local);return controls.Key(9)+T(" quiet · ")+controls.Key(10)+T(" call · ")+controls.Key(6)+(cooldown>0?string.Format(T(" baton ready in {0}s"),Mathf.CeilToInt(cooldown)):T(" baton"));}
        return T("Aim at a nearby object to use it / Esc settings");
    }
}
}
