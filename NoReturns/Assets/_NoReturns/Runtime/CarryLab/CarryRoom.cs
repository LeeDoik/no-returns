using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom : MonoBehaviour {
    public bool cinderReview;
    int Port=>cinderReview?27842:27841;
    Vector3 CargoSpawn=>cinderReview?new Vector3(-20.7f,.55f,-18.5f):new Vector3(0,.55f,-3);
    CharacterController[] workers=new CharacterController[4];
    Renderer[][] bodies=new Renderer[4][]; Transform[] rigs=new Transform[4];
    EmployeeVisual[] employeeVisuals=new EmployeeVisual[4];
    CarryInput[] inputs={new CarryInput(),new CarryInput(),new CarryInput(),new CarryInput()};
    float[] fall=new float[4];
    Rigidbody cargo; Collider cargoCollider; Camera eye;
    TcpListener listener; CarryWire wire; TcpClient joining; Task connectTask;
    bool hosting, active, peer, menu=true, capturePending; int local,holder=-1,tick,seq;
    float yaw,pitch,sendAt,lastPacket,connectAt; string address="127.0.0.1",status="Choose HOST or enter the host's LAN address.";
    CarryThreat threat,outer; CarrySuppression suppression; ThreatState danger,outerDanger; bool hazard; CarryClues clues; bool journalOpen;
    CrewHud playHud;
    CarryEquipment equipment=new CarryEquipment(); BatonVisual baton=new BatonVisual(); bool unlocked,hard; int deliveries;
    bool receiptCollected,receiptReady; float receiptProgress; ReceiptFeedback receiptFeedback;
    CarryMission mission; int missionPhase=-1,credits,receipt,returnPay;
    CarrySave save; string saveNotice=""; float retrySaveAt;
    [NonSerialized] string testFolder; int testSeq=-1;
    [NonSerialized] CarryState target;
    [Serializable] class TestCommand {public int revive=-1;public int seq;public float x,z,yaw,pitch,distance=.8f,turnYaw,turnPitch;public string ui,click,fixture;public bool place,confirmReturn,jump,interact,drop,reset,action; public bool capture,toggleLanguage,quiet,call,shove,rescue,buy,contract,deploy,inspect,journal;}
    void Awake(){
        Application.runInBackground=true;Application.targetFrameRate=cinderReview?30:60;Time.fixedDeltaTime=.02f;
        if(!cinderReview)CarryWorld.Build();receiptFeedback=FindFirstObjectByType<ReceiptFeedback>();
        var employee=Resources.Load<GameObject>("EmployeeLocal/Employee");
        if(!employee)Debug.LogWarning("Local employee art is not prepared; using prototype visuals.");
        for(int i=0;i<4;i++) {
            var g=new GameObject("Employee "+i);workers[i]=g.AddComponent<CharacterController>();workers[i].height=1.8f;workers[i].radius=.34f;workers[i].center=new Vector3(0,.9f,0);workers[i].stepOffset=.32f;workers[i].skinWidth=.035f;
            rigs[i]=new GameObject("Employee visuals").transform;rigs[i].SetParent(g.transform,false);
            var cream=CarryWorld.Mat(new Color(.72f,.68f,.56f));var team=CarryWorld.Mat(new[]{new Color(.85f,.3f,.06f),new Color(.05f,.65f,.65f),new Color(.7f,.25f,.75f),new Color(.85f,.8f,.15f)}[i]);
            if(employee){var model=Instantiate(employee,rigs[i],false);employeeVisuals[i]=model.GetComponent<EmployeeVisual>();employeeVisuals[i].Team(team.color);}
            else {
                Visual("Suit",rigs[i],new Vector3(0,.85f,0),new Vector3(.52f,1.0f,.35f),cream);
                Visual("Helmet",rigs[i],new Vector3(0,1.55f,0),new Vector3(.5f,.45f,.45f),team);
                Visual("Visor",rigs[i],new Vector3(0,1.56f,.24f),new Vector3(.39f,.26f,.02f),CarryWorld.Mat(Color.black));
            }
            bodies[i]=g.GetComponentsInChildren<Renderer>();
        }
        var c=CarryWorld.Box("Sealed parcel",CargoSpawn,new Vector3(.8f,.65f,.65f),CarryWorld.Mat(new Color(.78f,.69f,.52f)));
        cargo=c.AddComponent<Rigidbody>();cargo.mass=8;cargo.linearDamping=.8f;cargo.angularDamping=2;cargo.interpolation=RigidbodyInterpolation.Interpolate;cargo.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;cargoCollider=c.GetComponent<Collider>();
        Visual("Seal",c.transform,Vector3.zero,new Vector3(.14f,1.015f,1.015f),CarryWorld.Mat(new Color(.35f,.06f,.06f)));
        var parcelArt=FacilityArt.Place("Parcel",c.transform.position,new Vector3(.8f,.65f,.65f));
        if(parcelArt){c.GetComponent<Renderer>().enabled=false;var seal=c.transform.Find("Seal");if(seal)seal.gameObject.SetActive(false);parcelArt.transform.SetParent(c.transform,true);}
        var cam=new GameObject("First person camera");eye=cam.AddComponent<Camera>();cam.AddComponent<AudioListener>();eye.fieldOfView=80;eye.nearClipPlane=.06f;eye.farClipPlane=cinderReview?250:60;eye.backgroundColor=new Color(.07f,.08f,.1f);
        ResetRoom();
        var args=Environment.GetCommandLineArgs();
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
        companionPractice=cinderReview&&Array.IndexOf(args,"--companion-practice")>=0;
        companionBot=companionPractice&&Array.IndexOf(args,"--companion-bot")>=0&&Array.IndexOf(args,"--join")>=0;
#endif
        hazard=cinderReview?Array.IndexOf(args,"--map-only")<0&&Array.IndexOf(args,"--delivery-only")<0:Array.IndexOf(args,"--hazard")>=0;
        if(companionPractice){hazard=false;threat=new CarryThreat(cinder:true);danger=threat.Snapshot();danger.state=1;}
        if(hazard){threat=new CarryThreat(cinder:cinderReview);danger=threat.Snapshot();outer=new CarryThreat(threat,cinderReview);suppression=new CarrySuppression(cinderReview);outerDanger=outer.Snapshot();if(!cinderReview)clues=new CarryClues();}
        if(!companionPractice&&(hazard||Array.IndexOf(args,"--delivery")>=0||(cinderReview&&Array.IndexOf(args,"--map-only")<0))){mission=new CarryMission(cinder:cinderReview);missionPhase=0;}
        for(int i=0;i<args.Length;i++) {
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
            if(args[i]=="--test-dir"&&i+1<args.Length)testFolder=args[++i];
#endif
            if(args[i]=="--join"&&i+1<args.Length){address=args[++i];local=1;}
        }
        ConfigureQuickTest(args);
        if(testFolder!=null)Directory.CreateDirectory(testFolder);
        CarryLanguage.Initialize(testFolder);controls=new CarryControls(testFolder==null);eye.fieldOfView=controls.Fov;
        if(Array.IndexOf(args,"--host")>=0)StartHost();else if(Array.IndexOf(args,"--join")>=0)StartJoin();
    }
    static void Visual(string name,Transform parent,Vector3 pos,Vector3 size,Material mat){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;Destroy(g.GetComponent<Collider>());g.transform.SetParent(parent,false);g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;}
    void ResetRoom(bool recovered=false){
        Release();threat?.Reset();outer?.Reset();if(outer!=null)outerDanger=outer.Snapshot();if(threat!=null)danger=threat.Snapshot();for(int i=0;i<4;i++){workers[i].enabled=false;workers[i].transform.position=recovered&&cinderReview?new Vector3(-20.7f+(i%2==0?-.95f:.95f),1.035f,-25.5f-(i/2)*1.4f):Spawn(i);workers[i].transform.rotation=Quaternion.identity;workers[i].enabled=true;fall[i]=0;}
        cargo.position=CargoSpawn;cargo.rotation=Quaternion.identity;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;
    }
    void StartHost(){try{
        listener=new TcpListener(IPAddress.Any,Port);listener.Start(4);
        if(hazard&&!cinderReview){
            save=new CarrySave(Path.Combine(testFolder??Application.persistentDataPath,"progression-v1.json"));
            if(!save.Load(out var progress,out saveNotice)){listener.Stop();listener=null;status=saveNotice;return;}
            mission=new CarryMission(progress);missionPhase=0;credits=mission.Credits;unlocked=mission.BeaconUnlocked;deliveries=mission.SuccessfulDeliveries;hard=false;receipt=0;returnPay=0;retrySaveAt=0;equipment.Begin(unlocked,cinderReview);clues.Clear();suppression.Begin();ResetRoom();
        }
        hosting=true;local=0;occupiedMask=1;peer=false;StartSession();status="HOST / waiting for partner";
    }catch(Exception e){listener?.Stop();listener=null;status="Host failed: "+e.Message;}}
    void StartJoin(){try{joining=new TcpClient();connectTask=joining.ConnectAsync(address,Port);connectAt=Time.realtimeSinceStartup;status="Connecting...";}catch(Exception e){status="Join failed: "+e.Message;}}
    void StartSession(){active=true;menu=false;shipMenu=false;settingsMenu=false;journalOpen=false;lastPacket=Time.realtimeSinceStartup;SetCursor(true);}
    void SetCursor(bool locked){if(testFolder!=null||companionBot)return;Cursor.lockState=locked?CursorLockMode.Locked:CursorLockMode.None;Cursor.visible=!locked;}
    void Disconnect(){for(int i=1;i<4;i++){connections[i]?.Dispose();connections[i]=null;}occupiedMask=1;local=0;target=null;Release();wire?.Dispose();wire=null;listener?.Stop();listener=null;joining?.Close();joining=null;connectTask=null;active=false;hosting=false;peer=false;menu=true;shipMenu=false;settingsMenu=false;confirmReturn=false;journalOpen=false;SetCursor(false);status="Disconnected. Host or join again.";}
    void Update(){
        PollConnections();
        if(!companionBot&&Keyboard.current!=null&&Keyboard.current.escapeKey.wasPressedThisFrame)TogglePause();
        if(!active){WriteEvidence(Snapshot());return;}
        if(!hosting&&Time.realtimeSinceStartup-lastPacket>5){Disconnect();status="Host timed out.";return;}
        if(clues!=null&&!menu&&!shipMenu&&!controls.Rebinding&&controls.Pressed(11)){journalOpen=!journalOpen;SetCursor(!journalOpen);}
        ++seq;var cmd=companionBot?ReadCompanion():ReadControls();
        if(testFolder!=null){try{var path=Path.Combine(testFolder,"input.json");if(File.Exists(path)){
            var t=JsonUtility.FromJson<TestCommand>(File.ReadAllText(path));yaw=t.yaw;pitch=t.pitch;cmd.x=t.x;cmd.z=t.z;cmd.yaw=yaw;cmd.pitch=pitch;cmd.distance=t.distance;cmd.turnYaw=t.turnYaw;cmd.turnPitch=t.turnPitch;cmd.quiet=t.quiet;cmd.rescue=t.rescue;
            if(t.seq!=testSeq){testSeq=t.seq;RequestQuickFixture(t.fixture);cmd.revive=t.revive;cmd.inspect=t.inspect;if(t.journal&&clues!=null)journalOpen=!journalOpen;cmd.buy=t.buy;cmd.contract=t.contract;cmd.deploy=t.deploy;cmd.interact=t.interact;cmd.drop=t.drop;cmd.place=t.place;cmd.confirmReturn=t.confirmReturn;cmd.jump=t.jump;cmd.reset=t.reset;cmd.action=t.action;cmd.call=t.call;cmd.shove=t.shove;
                if(t.ui=="ship")OpenShip();else if(t.ui=="pause"){menu=true;SetCursor(false);}else if(t.ui=="settings"){menu=true;settingsMenu=true;SetCursor(false);}else if(t.ui=="close")ClosePanels();
                if(t.toggleLanguage)CarryLanguage.Toggle();if(t.click!=null&&playHud!=null)playHud.Click(t.click);if(t.capture)capturePending=true;
            }
        }}catch(IOException){} }
        // Preserve one-shot inputs until a simulation tick / outgoing packet consumes them.
        if(inputs[local].revive>=0)cmd.revive=inputs[local].revive;cmd.place|=inputs[local].place;cmd.confirmReturn|=inputs[local].confirmReturn;cmd.drop|=inputs[local].drop;cmd.inspect|=inputs[local].inspect;cmd.buy|=inputs[local].buy;cmd.contract|=inputs[local].contract;cmd.deploy|=inputs[local].deploy;cmd.call|=inputs[local].call;cmd.shove|=inputs[local].shove;cmd.action|=inputs[local].action;cmd.interact|=inputs[local].interact;cmd.jump|=inputs[local].jump;cmd.reset|=inputs[local].reset;if(UiOpen){cmd.revive=-1;cmd.place=false;cmd.confirmReturn=false;cmd.drop=false;cmd.x=0;cmd.z=0;cmd.quiet=true;cmd.jump=false;cmd.interact=false;cmd.action=false;cmd.call=false;cmd.shove=false;cmd.rescue=false;cmd.buy=false;cmd.contract=false;cmd.deploy=false;cmd.inspect=false;cmd.reset=false;}if(uiCommands.revive>=0)cmd.revive=uiCommands.revive;cmd.buy|=uiCommands.buy;cmd.contract|=uiCommands.contract;cmd.action|=uiCommands.action;cmd.confirmReturn|=uiCommands.confirmReturn;inputs[local]=cmd;
        if(!hosting&&wire!=null&&Time.realtimeSinceStartup>=sendAt){wire.Send(JsonUtility.ToJson(cmd));uiCommands=new CarryInput();inputs[local].revive=-1;inputs[local].place=false;inputs[local].confirmReturn=false;inputs[local].drop=false;inputs[local].inspect=false;inputs[local].buy=false;inputs[local].contract=false;inputs[local].deploy=false;inputs[local].call=false;inputs[local].shove=false;inputs[local].action=false;inputs[local].interact=false;inputs[local].jump=false;inputs[local].reset=false;sendAt=Time.realtimeSinceStartup+.033f;}
    }
    static bool Finite(float n)=>!float.IsNaN(n)&&!float.IsInfinity(n);
    void FixedUpdate(){
        if(!active){cargo.isKinematic=true;return;}
        if(hosting){
            ApplyQuickFixture();
            int phaseAtTick=mission==null?-1:mission.Phase;
            bool soloRescue=inputs[0].rescue;bool departureHandled=false;
            tick++;if(inputs[0].reset){if(companionPractice)RestartCompanionRescue();else if(mission==null)ResetRoom();inputs[0].reset=false;}
            for(int i=0;i<4;i++){
                if(!Present(i))continue;var input=inputs[i];if(i>0&&Time.realtimeSinceStartup-lastInputs[i]>.3f){input.x=0;input.z=0;input.rescue=false;input.call=false;input.shove=false;}
                if((hazard||companionPractice)&&threat.Down[i]){input.revive=-1;input.place=false;input.confirmReturn=false;input.drop=false;input.x=0;input.z=0;input.jump=false;input.interact=false;input.action=false;input.call=false;input.shove=false;input.rescue=false;input.buy=false;input.contract=false;input.deploy=false;input.inspect=false;}
                workers[i].transform.rotation=Quaternion.Euler(0,input.yaw,0);
                Vector3 move=Quaternion.Euler(0,input.yaw,0)*Vector3.ClampMagnitude(new Vector3(input.x,0,input.z),1)*(hazard&&input.quiet?(holder==i?1f:1.5f):(holder==i?2.5f:4f))*Time.fixedDeltaTime;
                if(holder==i&&move.sqrMagnitude>0){foreach(var hit in Physics.BoxCastAll(cargo.position,new Vector3(.4f,.325f,.325f)*.98f,move.normalized,cargo.rotation,move.magnitude+.025f,~0,QueryTriggerInteraction.Ignore)){if(hit.collider!=workers[i]&&hit.collider!=cargoCollider){move=Vector3.zero;break;}}}
                if(workers[i].isGrounded&&fall[i]<0)fall[i]=-2;
                if(input.jump&&workers[i].isGrounded&&holder!=i&&equipment.Carrier!=i)fall[i]=5;
                fall[i]-=18*Time.fixedDeltaTime;move.y=fall[i]*Time.fixedDeltaTime;workers[i].Move(move);
                if(workers[i].transform.position.y < -5){workers[i].enabled=false;workers[i].transform.position=Spawn(i);workers[i].enabled=true;fall[i]=0;}
                if((input.drop||input.place)&&equipment.Carrier==i)equipment.PutDown(i,workers[i].transform.position,Quaternion.Euler(input.pitch,input.yaw,0),mission!=null&&(mission.Phase==2||mission.Phase==3));
                if(input.drop&&holder==i){Release();if(hazard)threat.Hear(workers[i].transform.position,10);status="Parcel set down";}input.drop=false;
                if(input.place&&holder==i)PlaceParcel(i);input.place=false;
                ResolveUse(i,input);
                if(equipment.Carrier==i){input.rescue=false;input.shove=false;}
                if(hazard){
                    bool aboard=CarryMission.Aboard(workers[i].transform.position,cinderReview);
                    if(input.inspect&&clues!=null)status=clues.Inspect(workers[i].transform.position+Vector3.up*1.57f,Quaternion.Euler(input.pitch,input.yaw,0),mission.Phase);
                    if(input.deploy)status=equipment.Deploy(workers[i].transform.position,Quaternion.Euler(input.pitch,input.yaw,0),holder!=i&&!threat.Down[i],mission)?"Beacon deployed":"Aim at yard floor within 8m, empty hands; beacon must be ready";
                }
                if(mission!=null&&input.buy){bool bought=mission.BuyBeacon(i==0,CarryMission.Aboard(workers[i].transform.position,cinderReview));if(bought)equipment.Begin(true,cinderReview);status=bought?"Beacon license purchased":"Purchase unavailable: host, ship/report, 120 CR required";}
                if(hazard&&!cinderReview&&input.contract)status=mission.ToggleContract(i==0,CarryMission.Aboard(workers[i].transform.position,cinderReview))?"Contract changed":"Select route aboard after first delivery; host only";
                input.buy=false;input.contract=false;input.deploy=false;input.inspect=false;
                if(cinderReview&&mission?.Visit!=null&&input.revive>=0){
                    status=mission.Visit.StartRevival(i,input.revive,AboardMask())?"Revival started / fee deducted":"Revival unavailable / eliminated crew, free chamber and 100 unbanked CR required";
                }
                input.revive=-1;
                if(input.action&&mission!=null&&mission.Phase==phaseAtTick&&(cinderReview||i==0)){
                    int before=mission.Phase;
                    if(cinderReview&&(before==2||before==3)){
                        if(!departureHandled&&mission.Visit!=null&&(mission.Visit.Departing||mission.ReceiptCollected||input.confirmReturn)){
                            departureHandled=mission.Visit.ToggleDeparture(i,AboardMask());
                            if(departureHandled)status=mission.Visit.Departing?"Departure started / crew outside may be left behind":"Departure canceled";
                        }
                    }else{
                        if(mission.Act(CarryMission.Aboard(workers[i].transform.position,cinderReview),AllAboard())){equipment.Begin(mission.BeaconUnlocked,cinderReview);ResetRoom();}
                        if(before==1&&mission.Phase==2){
                            if(hazard){threat.Reset();clues?.Clear();suppression?.Begin();outer?.Reset();}
                            mission.BeginVisit(occupiedMask);threat?.AttachVisit(mission.Visit);outer?.AttachVisit(mission.Visit);
                        }
                    }
                }
                input.action=false;input.confirmReturn=false;
                if(input.interact)Interact(i);input.interact=false;input.jump=false;if(i==local)uiCommands=new CarryInput();
            }
            if(companionPractice)TickCompanionBatons();
            if(hazard){
                bool field=mission.Phase==2||mission.Phase==3;
                clues?.Display(field,mission.Phase==3);
                suppression?.Tick(field,Time.fixedDeltaTime);threat.Hard=mission.HardContract;
                bool evacuate=threat.Tick(Positions(),occupiedMask,inputs,holder,mission.Phase==2||mission.Phase==3,Time.fixedDeltaTime);
                outer?.Tick(Positions(),occupiedMask,inputs,holder,field&&suppression.Intrusion,Time.fixedDeltaTime);
                if(holder>=0&&threat.Down[holder]){Release();status="Employee down / parcel released";}
                if(evacuate&&mission.Visit==null){mission.Abort();ResetRoom(true);status="Emergency recovery / secured pay retained";}
                danger=threat.Snapshot();outerDanger=outer?.Snapshot();for(int i=0;i<4;i++){inputs[i].call=false;inputs[i].shove=false;}
            }
            if(cinderReview&&mission?.Visit!=null&&mission.Visit.Active){
                int revived=mission.Visit.Tick(Time.fixedDeltaTime,AboardMask(),soloRescue);
                if(revived>=0)MoveAboard(revived);
                if(!mission.Visit.Active){mission.CompleteVisit();Release();for(int i=0;i<4;i++)if(Present(i))MoveAboard(i);status=mission.Visit.Failed?"Visit failed / unbanked earnings lost":"Departure complete / payout secured";}
                if(threat!=null)danger=threat.Snapshot();
            }
            equipment.Tick(mission!=null&&(mission.Phase==2||mission.Phase==3),Time.fixedDeltaTime,threat,suppression!=null&&suppression.Intrusion?outer:null);
            if(equipment.Carrier>=0){int carrier=equipment.Carrier;if(hazard&&threat.Down[carrier])equipment.RecoverCarrier(workers[carrier].transform.position);else equipment.Follow(workers[carrier].transform.position,Quaternion.Euler(inputs[carrier].pitch,inputs[carrier].yaw,0));}
            if(holder>=0){cargo.isKinematic=true;var p=workers[holder].transform;var look=Quaternion.Euler(inputs[holder].pitch,inputs[holder].yaw,0);var rotation=look*Quaternion.Euler(inputs[holder].turnPitch,inputs[holder].turnYaw,0);var desired=p.position+Vector3.up*1.57f+look*new Vector3(0,-.54f,Mathf.Clamp(inputs[holder].distance,.75f,1.6f));
                var delta=desired-cargo.position;var next=desired;
                if(delta.magnitude>.001f){foreach(var h in Physics.BoxCastAll(cargo.position,new Vector3(.4f,.325f,.325f)*.98f,delta.normalized,cargo.rotation,delta.magnitude,~0,QueryTriggerInteraction.Ignore)){if(h.collider==cargoCollider||h.collider==workers[holder])continue;next=cargo.position+delta.normalized*Mathf.Min(Vector3.Distance(cargo.position,next),Mathf.Max(0,h.distance-.03f));}}
                cargo.MovePosition(next);
                // Reject rotation into a wall/floor even when translation has already stopped.
                bool clear=true;foreach(var overlap in Physics.OverlapBox(next,new Vector3(.4f,.325f,.325f)*.99f,rotation,~0,QueryTriggerInteraction.Ignore)){if(overlap!=cargoCollider&&overlap!=workers[holder]){clear=false;break;}}
                if(clear)cargo.MoveRotation(rotation);
                if(Vector3.Distance(next,p.position+Vector3.up) > 2.4f)Release();
            }else{cargo.isKinematic=mission!=null&&mission.Phase>=3;if(cargo.position.y < -3){cargo.position=cinderReview?CargoSpawn:new Vector3(0,.6f,-3);cargo.linearVelocity=Vector3.zero;} }
            if(mission!=null){mission.Tick(cargo.position,cargo.linearVelocity,holder,Time.fixedDeltaTime);unlocked=mission.BeaconUnlocked;hard=mission.HardContract;deliveries=mission.SuccessfulDeliveries;receiptCollected=mission.ReceiptCollected;receiptReady=mission.ReceiptReady;receiptProgress=mission.ReceiptProgress;missionPhase=mission.Phase;credits=mission.Credits;receipt=mission.Receipt;returnPay=mission.ReturnPay;}
            if(hazard&&save!=null&&Time.realtimeSinceStartup>=retrySaveAt){
                bool written=save.Write(mission,out var notice);
                if(notice!=null&&(!written||!saveNotice.StartsWith("Recovered previous backup",StringComparison.Ordinal)))saveNotice=notice;
                retrySaveAt=written?0:Time.realtimeSinceStartup+1;
            }
            if(tick%3==0){Broadcast();WriteEvidence(Snapshot());}
        }else if(target!=null){
            cargo.isKinematic=true;holder=target.holder;tick=target.tick;hazard=target.hazard;if(hazard&&threat==null)threat=new CarryThreat(cinder:cinderReview);if(hazard&&outer==null){outer=new CarryThreat(threat,cinderReview);suppression=new CarrySuppression(cinderReview);}suppression?.Apply(target.shiftElapsed);outerDanger=target.outerDanger;if(hazard&&!cinderReview&&clues==null)clues=new CarryClues();clues?.Apply(target.clueMask);if(hazard&&mission==null)mission=new CarryMission(cinder:cinderReview);unlocked=target.unlocked;hard=target.hard;deliveries=target.deliveries;equipment.Apply(target);danger=target.danger;receiptCollected=target.receiptCollected;receiptReady=target.receiptReady;receiptProgress=target.receiptProgress;missionPhase=target.phase;credits=target.credits;receipt=target.receipt;returnPay=target.returnPay;
            for(int i=0;i<4;i++){workers[i].enabled=false;workers[i].transform.position=Vector3.Lerp(workers[i].transform.position,target.positions[i],.55f);workers[i].transform.rotation=Quaternion.Euler(0,target.yaws[i],0);}
            cargo.position=Vector3.Lerp(cargo.position,target.cargo,.55f);cargo.rotation=Quaternion.Slerp(cargo.rotation,target.rotation,.55f);WriteEvidence(target);
        }
    }
    void ResolveUse(int who,CarryInput input){
        if(!input.interact)return;
        input.interact=false;
        if(holder==who||equipment.Carrier==who)return;
        var pos=workers[who].transform.position;var origin=pos+Vector3.up*1.57f;var look=Quaternion.Euler(input.pitch,input.yaw,0);
        if((hazard||companionPractice)&&threat.RescueTarget(who,Positions(),occupiedMask)>=0)return;
        if(mission!=null&&mission.Phase==3&&!mission.ReceiptCollected&&ReceiptFeedback.CanReach(origin,look)){input.interact=true;return;}
        if(clues!=null&&clues.Target(origin,look)>=0){input.inspect=true;return;}
        if(equipment.PickUp(who,pos,look,holder!=who))return;
        if(ParcelTarget(who,look)){input.interact=true;return;}
        
    }
    void Interact(int who){
        if((hazard||companionPractice)&&threat.Down[who])return;
        if(mission!=null&&mission.Phase==3){if(ReceiptFeedback.CanReach(workers[who].transform.position+Vector3.up*1.57f,Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0))&&mission.CollectReceipt())status="Receipt collected / return aboard to get paid";return;}
        if(mission!=null&&mission.Phase!=2)return;
        if(!ParcelTarget(who,Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0)))return;
        holder=who;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;cargo.isKinematic=true;Physics.IgnoreCollision(cargoCollider,workers[who],true);status="Parcel held";
    }
    void Release(){if(cargo==null)return;if(holder>=0)Physics.IgnoreCollision(cargoCollider,workers[holder],false);holder=-1;cargo.isKinematic=false;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;}
    CarryState Snapshot()=>new CarryState{visit=mission?.Visit?.Snapshot(),cinderReview=cinderReview,positions=Positions(),yaws=Yaws(),occupiedMask=occupiedMask,recipient=local,beaconCarrier=equipment.Carrier,beaconExists=equipment.Exists,receiptCollected=receiptCollected,receiptReady=receiptReady,receiptProgress=receiptProgress,shiftElapsed=suppression==null?0:suppression.Elapsed,suppressionStage=suppression==null?0:suppression.Stage,outerDanger=outerDanger,clueMask=clues==null?0:clues.Mask,unlocked=unlocked,hard=hard,deliveries=deliveries,charges=equipment.Charges,beaconTime=equipment.Remaining,beaconPosition=equipment.Position,hazard=hazard,danger=danger,phase=missionPhase,credits=credits,receipt=receipt,returnPay=returnPay,tick=tick,holder=holder,p0=workers[0].transform.position,p1=workers[1].transform.position,yaw0=inputs[0].yaw,yaw1=inputs[1].yaw,cargo=cargo.position,rotation=cargo.rotation,players=CrewCount,connected=peer,ack=inputs[local].seq,message=status};
    [Serializable] class AnimationEvidence {public EmployeeVisual.State[] employees;public BatonVisual.State[] batons;public FirstPersonArms.State hands;public bool companionPractice,companionBot;public string companionAction,quickFixture;}
    [Serializable] class UiEvidence {public string menu,context,shipAction,buyButton,ledger,revival,language,host,objective,status,firstRecord,secondRecord,suppressionCue;public bool koreanGlyph,journalOpen;}
    static void WriteAtomic(string path,string value){var temp=path+".tmp";File.WriteAllText(temp,value);if(File.Exists(path))File.Replace(temp,path,null);else File.Move(temp,path);}
    void WriteEvidence(CarryState s){if(testFolder==null)return;try{WriteAtomic(Path.Combine(testFolder,"ui.json"),JsonUtility.ToJson(new UiEvidence{ledger=playHud==null?"":playHud.MenuText("Crew"),revival=playHud==null?"":playHud.MenuText("Revival"),menu=settingsMenu?"settings":shipMenu?"ship":journalOpen?"journal":menu?"pause":"",context=playHud==null?"":playHud.ContextText,shipAction=playHud==null?"":playHud.MenuText("shipAction"),buyButton=playHud==null?"":playHud.MenuText("buy"),suppressionCue=suppression==null?null:T(CarrySuppression.Cue(suppression.Stage,cinderReview)),journalOpen=journalOpen,firstRecord=T(clues!=null&&(clues.Mask&1)!=0?CarryClues.FirstBody:"Not discovered"),secondRecord=T(clues!=null&&(clues.Mask&2)!=0?CarryClues.SecondBody:"Not discovered"),language=CarryLanguage.Korean?"ko":"en",host=T("HOST"),objective=T(cinderReview&&missionPhase<0?"CINDER / FOUR-PLAYER MAP TEST":CarryMission.Objective(missionPhase)),status=T(status),koreanGlyph=CarryLanguage.Font.HasCharacter('한')}));WriteAtomic(Path.Combine(testFolder,"state.json"),JsonUtility.ToJson(s));var animation=new AnimationEvidence{companionPractice=companionPractice,companionBot=companionBot,companionAction=companionAction,quickFixture=quickFixture,hands=baton.Hands,employees=new EmployeeVisual.State[4],batons=new BatonVisual.State[4]};for(int i=0;i<4;i++)if(employeeVisuals[i]&&employeeVisuals[i].Ready)animation.employees[i]=employeeVisuals[i].Capture();for(int i=0;i<4;i++)animation.batons[i]=baton.Capture(i);WriteAtomic(Path.Combine(testFolder,"animation.json"),JsonUtility.ToJson(animation));}catch(IOException){} }
    void LateUpdate(){if(eye==null)return;eye.transform.position=workers[local].transform.position+Vector3.up*((hazard||companionPractice)&&danger!=null&&danger.IsDown(local)?.55f:1.57f);eye.transform.rotation=Quaternion.Euler(pitch,yaw,0);
        // Apply before every render, including the inactive startup menu.
        for(int i=0;i<4;i++){bool visible=i!=local&&Present(i);foreach(var r in bodies[i])r.enabled=visible;}
        for(int i=0;i<4;i++)workers[i].gameObject.SetActive(Present(i));
        for(int i=0;i<4;i++)if(Present(i)&&employeeVisuals[i]){
            bool down=(hazard||companionPractice)&&danger!=null&&danger.IsDown(i);
            employeeVisuals[i].Animate(workers[i].transform.position,down,Time.deltaTime);
            employeeVisuals[i].GroundLocomotion(down,Time.deltaTime);
            employeeVisuals[i].DownPose(down,true,Time.deltaTime);
            employeeVisuals[i].CarryPose(cargoCollider as BoxCollider,active&&holder==i,down,Time.deltaTime);
        }
        receiptFeedback?.Display(active?missionPhase:-1,receiptProgress,receiptCollected,receiptReady);
        suppression?.Display(active&&(missionPhase==2||missionPhase==3));
        if(outer!=null&&outerDanger!=null)outer.Display(outerDanger,active&&(missionPhase==2||missionPhase==3)&&suppression.Stage>=2);
        clues?.Display(active&&(missionPhase==2||missionPhase==3),missionPhase==3);
        equipment.Display(active&&missionPhase>=0,eye,local,equipment.Carrier>=0?employeeVisuals[equipment.Carrier]?.transform:null);
        for(int i=0;i<4;i++)if(employeeVisuals[i])employeeVisuals[i].RescuePose(danger==null?0:danger.RescueAt(i),!active||!Present(i)||i==local||holder==i||equipment.Carrier==i||(danger!=null&&danger.IsDown(i)),Time.deltaTime);
        baton.Display(eye,workers,local,occupiedMask,active&&(hazard||companionPractice),holder,equipment.Carrier,danger,Yaws(),employeeVisuals,cargoCollider as BoxCollider,equipment.LocalHeldVisual);
        for(int i=0;i<4;i++)if(employeeVisuals[i])employeeVisuals[i].BeaconPose(active&&Present(i)&&i!=local&&equipment.Carrier==i&&!(danger!=null&&(danger.IsDown(i)||danger.RescueAt(i)>0))?equipment.HeldVisual:null);
        if(threat!=null&&danger!=null){threat.Display(danger,active&&(missionPhase==2||missionPhase==3));
            for(int i=0;i<4;i++){bool down=danger.IsDown(i)&&!employeeVisuals[i];rigs[i].localRotation=Quaternion.Euler(0,0,down?90:0);rigs[i].localPosition=down?new Vector3(.5f,.3f,0):Vector3.zero;}
        }
        if(playHud==null)playHud=gameObject.AddComponent<CrewHud>();
        playHud.Apply(hosting?Snapshot():target??Snapshot(),local,active&&!UiOpen);
        if(active&&!UiOpen)playHud.SetContext(ContextPrompt());
        if(active&&!UiOpen&&!(hazard&&(missionPhase==2||missionPhase==3)))playHud.SetMovement(controls.Key(0)+" / "+controls.Key(1)+" / "+controls.Key(2)+" / "+controls.Key(3)+T(" move · ")+controls.Key(8)+T(" jump · Esc settings"));
        if(companionPractice&&active&&!UiOpen)playHud.SetMovement(T(companionBot?"BOT / ":"YOU + BOT / safe practice · ")+T(companionBot?companionAction:danger!=null&&danger.state==2?"Hold use near the downed bot to rescue":danger!=null&&danger.state==1?"Bot approaching for rescue practice":"Move freely; the bot follows and demonstrates nearby")+" / "+controls.Key(12)+T(" restart rescue demo"));
        RenderMenus(hosting?Snapshot():target??Snapshot());PreviewPlacement();
        if(capturePending && testFolder!=null){capturePending=false;var rt=new RenderTexture(960,600,24);rt.Create();
            var req=new UnityEngine.Rendering.Universal.UniversalRenderPipeline.SingleCameraRequest{destination=rt};
            UnityEngine.Rendering.RenderPipeline.SubmitRenderRequest(eye,req);var old=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(960,600,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,960,600),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(testFolder,"capture.png"),tex.EncodeToPNG());ScreenCapture.CaptureScreenshot(Path.Combine(testFolder,"screen.png"));RenderTexture.active=old;Destroy(tex);rt.Release();Destroy(rt);
        }
    }
    static string T(string text)=>CarryLanguage.Text(text);
    void OnDestroy(){if(eye)baton.Dispose(eye);for(int i=1;i<4;i++)connections[i]?.Dispose();wire?.Dispose();listener?.Stop();joining?.Close();controls?.Dispose();suppression?.Dispose();if(placementPreview){Destroy(placementPreview.sharedMaterial);Destroy(placementPreview.gameObject);}SetCursor(false);}
}
}
