using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace NoReturns.CarryLab {
public sealed class CarryRoom : MonoBehaviour {
    const int Port=27841;
    CharacterController[] workers=new CharacterController[2];
    Renderer[][] bodies=new Renderer[2][]; Transform[] rigs=new Transform[2];
    CarryInput[] inputs={new CarryInput(),new CarryInput()};
    float[] fall=new float[2];
    Rigidbody cargo; Collider cargoCollider; Camera eye;
    TcpListener listener; CarryWire wire; TcpClient joining; Task connectTask;
    bool hosting, active, peer, menu=true, capturePending; int local,holder=-1,tick,seq;
    float yaw,pitch,sendAt,lastInput,lastPacket,connectAt; string address="127.0.0.1",status="Choose HOST or enter the host's LAN address.";
    CarryThreat threat,outer; CarrySuppression suppression; ThreatState danger,outerDanger; bool hazard; CarryClues clues; bool journalOpen;
    CarryEquipment equipment=new CarryEquipment(); BatonVisual baton=new BatonVisual(); bool unlocked,hard; int deliveries;
    int supplyChoice; bool receiptCollected,receiptReady; float receiptProgress; ReceiptFeedback receiptFeedback;
    CarryMission mission; int missionPhase=-1,credits,receipt,returnPay;
    CarrySave save; string saveNotice=""; float retrySaveAt;
    string testFolder; int testSeq=-1; CarryState target; 
    [Serializable] class TestCommand {public int seq;public float x,z,yaw,pitch;public bool jump,interact,drop,reset,action; public bool capture,toggleLanguage,quiet,call,shove,rescue,buy,contract,deploy,inspect,journal;}
    void Awake(){
        Application.runInBackground=true;Application.targetFrameRate=60;Time.fixedDeltaTime=.02f;
        CarryWorld.Build();receiptFeedback=FindFirstObjectByType<ReceiptFeedback>();
        for(int i=0;i<2;i++) {
            var g=new GameObject("Employee "+i);workers[i]=g.AddComponent<CharacterController>();workers[i].height=1.8f;workers[i].radius=.34f;workers[i].center=new Vector3(0,.9f,0);workers[i].stepOffset=.32f;workers[i].skinWidth=.035f;
            rigs[i]=new GameObject("Employee visuals").transform;rigs[i].SetParent(g.transform,false);
            var cream=CarryWorld.Mat(new Color(.72f,.68f,.56f));var team=CarryWorld.Mat(i==0?new Color(.85f,.3f,.06f):new Color(.05f,.65f,.65f));
            Visual("Suit",rigs[i],new Vector3(0,.85f,0),new Vector3(.52f,1.0f,.35f),cream);
            Visual("Helmet",rigs[i],new Vector3(0,1.55f,0),new Vector3(.5f,.45f,.45f),team);
            Visual("Visor",rigs[i],new Vector3(0,1.56f,.24f),new Vector3(.39f,.26f,.02f),CarryWorld.Mat(Color.black));
            bodies[i]=g.GetComponentsInChildren<Renderer>();
        }
        var c=CarryWorld.Box("Sealed parcel",new Vector3(0,.55f,-3),new Vector3(.8f,.65f,.65f),CarryWorld.Mat(new Color(.78f,.69f,.52f)));
        cargo=c.AddComponent<Rigidbody>();cargo.mass=8;cargo.linearDamping=.8f;cargo.angularDamping=2;cargo.interpolation=RigidbodyInterpolation.Interpolate;cargo.collisionDetectionMode=CollisionDetectionMode.ContinuousDynamic;cargoCollider=c.GetComponent<Collider>();
        Visual("Seal",c.transform,Vector3.zero,new Vector3(.14f,1.015f,1.015f),CarryWorld.Mat(new Color(.35f,.06f,.06f)));
        var parcelArt=FacilityArt.Place("Parcel",c.transform.position,new Vector3(.8f,.65f,.65f));
        if(parcelArt){c.GetComponent<Renderer>().enabled=false;var seal=c.transform.Find("Seal");if(seal)seal.gameObject.SetActive(false);parcelArt.transform.SetParent(c.transform,true);}
        var cam=new GameObject("First person camera");eye=cam.AddComponent<Camera>();cam.AddComponent<AudioListener>();eye.fieldOfView=80;eye.nearClipPlane=.06f;eye.farClipPlane=60;eye.backgroundColor=new Color(.07f,.08f,.1f);
        ResetRoom();
        var args=Environment.GetCommandLineArgs();
        hazard=Array.IndexOf(args,"--hazard")>=0;if(hazard){threat=new CarryThreat();outer=new CarryThreat(threat);suppression=new CarrySuppression();danger=threat.Snapshot();outerDanger=outer.Snapshot();clues=new CarryClues();}
        if(hazard||Array.IndexOf(args,"--delivery")>=0){mission=new CarryMission();missionPhase=0;}
        for(int i=0;i<args.Length;i++) {
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
            if(args[i]=="--test-dir"&&i+1<args.Length)testFolder=args[++i];
#endif
            if(args[i]=="--join"&&i+1<args.Length){address=args[++i];local=1;}
        }
        if(testFolder!=null)Directory.CreateDirectory(testFolder);
        CarryLanguage.Initialize(testFolder);
        if(Array.IndexOf(args,"--host")>=0)StartHost();else if(Array.IndexOf(args,"--join")>=0)StartJoin();
    }
    static void Visual(string name,Transform parent,Vector3 pos,Vector3 size,Material mat){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;Destroy(g.GetComponent<Collider>());g.transform.SetParent(parent,false);g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;}
    void ResetRoom(){
        Release();threat?.Reset();outer?.Reset();if(outer!=null)outerDanger=outer.Snapshot();if(threat!=null)danger=threat.Snapshot();for(int i=0;i<2;i++){workers[i].enabled=false;workers[i].transform.position=new Vector3(i==0?-1:1,0,-5);workers[i].transform.rotation=Quaternion.identity;workers[i].enabled=true;fall[i]=0;}
        cargo.position=new Vector3(0,.55f,-3);cargo.rotation=Quaternion.identity;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;
    }
    void StartHost(){try{
        listener=new TcpListener(IPAddress.Any,Port);listener.Start(2);
        if(hazard){
            save=new CarrySave(Path.Combine(testFolder??Application.persistentDataPath,"progression-v1.json"));
            if(!save.Load(out var progress,out saveNotice)){listener.Stop();listener=null;status=saveNotice;return;}
            mission=new CarryMission(progress);missionPhase=0;credits=mission.Credits;unlocked=mission.BeaconUnlocked;deliveries=mission.SuccessfulDeliveries;hard=false;receipt=0;returnPay=0;retrySaveAt=0;equipment.Begin(unlocked);clues.Clear();suppression.Begin();ResetRoom();
        }
        hosting=true;local=0;StartSession();status="HOST / waiting for partner";
    }catch(Exception e){listener?.Stop();listener=null;status="Host failed: "+e.Message;}}
    void StartJoin(){try{joining=new TcpClient();connectTask=joining.ConnectAsync(address,Port);connectAt=Time.realtimeSinceStartup;status="Connecting...";}catch(Exception e){status="Join failed: "+e.Message;}}
    void StartSession(){active=true;menu=false;journalOpen=false;lastPacket=Time.realtimeSinceStartup;SetCursor(true);}
    void SetCursor(bool locked){if(testFolder!=null)return;Cursor.lockState=locked?CursorLockMode.Locked:CursorLockMode.None;Cursor.visible=!locked;}
    void Disconnect(){Release();wire?.Dispose();wire=null;listener?.Stop();listener=null;joining?.Close();joining=null;connectTask=null;active=false;hosting=false;peer=false;menu=true;journalOpen=false;SetCursor(false);status="Disconnected. Host or join again.";}
    void Update(){
        if(connectTask!=null){if(connectTask.IsCompleted){if(connectTask.IsFaulted){status="Connection failed. Check address / host / firewall.";joining.Close();}else{wire=new CarryWire(joining);hosting=false;local=1;peer=true;StartSession();status="CLIENT / connected";}connectTask=null;}else if(Time.realtimeSinceStartup-connectAt>8){joining.Close();connectTask=null;status="Connection timed out.";}}
        if(hosting&&listener!=null&&listener.Pending()){var incoming=listener.AcceptTcpClient();if((wire!=null&&!wire.Closed)||(mission!=null&&mission.Phase!=0)){using(var rejected=new CarryWire(incoming)){rejected.Send(JsonUtility.ToJson(new CarryState{rejection=(wire!=null&&!wire.Closed)?"Room full / wait for a free crew slot":"Shift in progress / wait for host to prepare next shift"}));}}else{wire=new CarryWire(incoming);peer=true;lastInput=Time.realtimeSinceStartup;inputs[1]=new CarryInput();workers[1].enabled=false;workers[1].transform.position=new Vector3(1,0,-5);workers[1].enabled=true;status="HOST / partner connected";}}
        if(wire!=null){foreach(var line in wire.Read()){
            try{if(hosting){var v=JsonUtility.FromJson<CarryInput>(line);if(v!=null&&v.seq>inputs[1].seq&&Finite(v.x)&&Finite(v.z)&&Finite(v.yaw)&&Finite(v.pitch)){v.x=Mathf.Clamp(v.x,-1,1);v.z=Mathf.Clamp(v.z,-1,1);v.pitch=Mathf.Clamp(v.pitch,-70,70);v.reset=false;v.inspect|=inputs[1].inspect;v.buy|=inputs[1].buy;v.contract|=inputs[1].contract;v.deploy|=inputs[1].deploy;v.call|=inputs[1].call;v.shove|=inputs[1].shove;v.action|=inputs[1].action;v.interact|=inputs[1].interact;v.jump|=inputs[1].jump;inputs[1]=v;lastInput=Time.realtimeSinceStartup;}}
            else{var v=JsonUtility.FromJson<CarryState>(line);if(v.protocol!=9)throw new Exception("Protocol mismatch");if(!string.IsNullOrEmpty(v.rejection)){Disconnect();status=v.rejection;break;}target=v;lastPacket=Time.realtimeSinceStartup;}}
            catch{wire.Dispose();}
        }if(wire!=null&&wire.Closed){if(hosting){if(equipment.Carrier==1)equipment.RecoverCarrier(workers[1].transform.position);if(holder==1)Release();mission?.Abort();peer=false;wire=null;inputs[1]=new CarryInput();if(hazard&&threat.Down[0]){ResetRoom();status="Partner left / emergency recovery / secured pay retained";}else status="HOST / partner left";}else{Disconnect();status="Host connection lost.";}}}
        if(hosting&&peer&&Time.realtimeSinceStartup-lastInput>5)wire?.Dispose();
        if(!active){WriteEvidence(Snapshot());return;}
        if(!hosting&&Time.realtimeSinceStartup-lastPacket>5){Disconnect();status="Host timed out.";return;}
        if(Keyboard.current!=null&&Keyboard.current.escapeKey.wasPressedThisFrame){if(journalOpen)journalOpen=false;else menu=!menu;SetCursor(!menu);}
        if(hazard&&!menu&&Keyboard.current!=null&&Keyboard.current.tabKey.wasPressedThisFrame){journalOpen=!journalOpen;SetCursor(!journalOpen);}
        if(menu&&hazard&&testFolder==null&&Keyboard.current!=null){var keys=Keyboard.current;if(keys.upArrowKey.wasPressedThisFrame||keys.downArrowKey.wasPressedThisFrame)supplyChoice=1-supplyChoice;if(keys.eKey.wasPressedThisFrame){if(supplyChoice==0)inputs[local].buy=true;else inputs[local].contract=true;}}
        var cmd=new CarryInput{seq=++seq,yaw=yaw,pitch=pitch};
        if(testFolder!=null){try{var path=Path.Combine(testFolder,"input.json");if(File.Exists(path)){var t=JsonUtility.FromJson<TestCommand>(File.ReadAllText(path));yaw=t.yaw;pitch=t.pitch;cmd.x=t.x;cmd.z=t.z;cmd.yaw=yaw;cmd.pitch=pitch;cmd.quiet=t.quiet;cmd.rescue=t.rescue;if(t.seq!=testSeq){testSeq=t.seq;cmd.inspect=t.inspect;if(t.journal&&hazard)journalOpen=!journalOpen;cmd.buy=t.buy;cmd.contract=t.contract;cmd.deploy=t.deploy;cmd.interact=t.interact;cmd.drop=t.drop;cmd.jump=t.jump;cmd.reset=t.reset;cmd.action=t.action;cmd.call=t.call;cmd.shove=t.shove;if(t.toggleLanguage)CarryLanguage.Toggle();if(t.capture)capturePending=true;testSeq=t.seq;}}}catch(IOException){} }
        else if(!menu&&!journalOpen&&Keyboard.current!=null){var k=Keyboard.current;var m=Mouse.current;if(m!=null){yaw+=m.delta.ReadValue().x*.12f;pitch=Mathf.Clamp(pitch-m.delta.ReadValue().y*.12f,-70,70);}cmd.yaw=yaw;cmd.pitch=pitch;cmd.x=(k.dKey.isPressed?1:0)-(k.aKey.isPressed?1:0);cmd.z=(k.wKey.isPressed?1:0)-(k.sKey.isPressed?1:0);cmd.interact=k.eKey.wasPressedThisFrame;cmd.drop=k.qKey.wasPressedThisFrame;cmd.jump=k.spaceKey.wasPressedThisFrame;cmd.quiet=k.leftShiftKey.isPressed;cmd.call=k.cKey.wasPressedThisFrame;cmd.shove=m!=null&&m.leftButton.wasPressedThisFrame;cmd.rescue=k.eKey.isPressed;cmd.reset=k.rKey.wasPressedThisFrame&&hosting;}
        // Preserve one-shot inputs until a simulation tick / outgoing packet consumes them.
        cmd.drop|=inputs[local].drop;cmd.inspect|=inputs[local].inspect;cmd.buy|=inputs[local].buy;cmd.contract|=inputs[local].contract;cmd.deploy|=inputs[local].deploy;cmd.call|=inputs[local].call;cmd.shove|=inputs[local].shove;cmd.action|=inputs[local].action;cmd.interact|=inputs[local].interact;cmd.jump|=inputs[local].jump;cmd.reset|=inputs[local].reset;if(journalOpen){cmd.drop=false;cmd.x=0;cmd.z=0;cmd.quiet=true;cmd.jump=false;cmd.interact=false;cmd.action=false;cmd.call=false;cmd.shove=false;cmd.rescue=false;cmd.buy=false;cmd.contract=false;cmd.deploy=false;cmd.inspect=false;cmd.reset=false;}inputs[local]=cmd;
        if(!hosting&&wire!=null&&Time.realtimeSinceStartup>=sendAt){wire.Send(JsonUtility.ToJson(cmd));inputs[local].drop=false;inputs[local].inspect=false;inputs[local].buy=false;inputs[local].contract=false;inputs[local].deploy=false;inputs[local].call=false;inputs[local].shove=false;inputs[local].action=false;inputs[local].interact=false;inputs[local].jump=false;inputs[local].reset=false;sendAt=Time.realtimeSinceStartup+.033f;}
        for(int i=0;i<2;i++){bool visible=i!=local&&(i==0||peer);foreach(var r in bodies[i])r.enabled=visible;}
        workers[1].gameObject.SetActive(peer||local==1);
    }
    static bool Finite(float n)=>!float.IsNaN(n)&&!float.IsInfinity(n);
    void FixedUpdate(){
        if(!active){cargo.isKinematic=true;return;}
        if(hosting){
            int phaseAtTick=mission==null?-1:mission.Phase;
            tick++;if(inputs[0].reset){if(mission==null)ResetRoom();inputs[0].reset=false;}
            for(int i=0;i<2;i++){
                if(i==1&&!peer)continue;var input=inputs[i];if(i==1&&Time.realtimeSinceStartup-lastInput>.3f){input.x=0;input.z=0;input.rescue=false;input.call=false;input.shove=false;}
                if(hazard&&threat.Down[i]){input.drop=false;input.x=0;input.z=0;input.jump=false;input.interact=false;input.action=false;input.call=false;input.shove=false;input.rescue=false;input.buy=false;input.contract=false;input.deploy=false;input.inspect=false;}
                workers[i].transform.rotation=Quaternion.Euler(0,input.yaw,0);
                Vector3 move=Quaternion.Euler(0,input.yaw,0)*Vector3.ClampMagnitude(new Vector3(input.x,0,input.z),1)*(hazard&&input.quiet?(holder==i?1f:1.5f):(holder==i?2.5f:4f))*Time.fixedDeltaTime;
                if(holder==i&&move.sqrMagnitude>0){foreach(var hit in Physics.BoxCastAll(cargo.position,new Vector3(.4f,.325f,.325f)*.98f,move.normalized,cargo.rotation,move.magnitude+.025f,~0,QueryTriggerInteraction.Ignore)){if(hit.collider!=workers[i]&&hit.collider!=cargoCollider){move=Vector3.zero;break;}}}
                if(workers[i].isGrounded&&fall[i]<0)fall[i]=-2;
                if(input.jump&&workers[i].isGrounded&&holder!=i&&equipment.Carrier!=i)fall[i]=5;
                fall[i]-=18*Time.fixedDeltaTime;move.y=fall[i]*Time.fixedDeltaTime;workers[i].Move(move);
                if(workers[i].transform.position.y < -5){workers[i].enabled=false;workers[i].transform.position=new Vector3(i==0?-1:1,0,-5);workers[i].enabled=true;fall[i]=0;}
                if(input.drop&&equipment.Carrier==i)equipment.PutDown(i,workers[i].transform.position,Quaternion.Euler(input.pitch,input.yaw,0),mission!=null&&(mission.Phase==2||mission.Phase==3));
                if(input.drop&&holder==i){Release();if(hazard)threat.Hear(workers[i].transform.position,10);status="Parcel set down";}input.drop=false;
                ResolveUse(i,input);
                if(equipment.Carrier==i){input.rescue=false;input.shove=false;}
                if(hazard){
                    bool aboard=CarryMission.Aboard(workers[i].transform.position);
                    if(input.inspect)status=clues.Inspect(workers[i].transform.position+Vector3.up*1.57f,Quaternion.Euler(input.pitch,input.yaw,0),mission.Phase);
                    if(input.buy){bool bought=mission.BuyBeacon(i==0,aboard);if(bought)equipment.Begin(true);status=bought?"Beacon license purchased":"Purchase unavailable: host, ship/report, 120 CR required";}
                    if(input.contract)status=mission.ToggleContract(i==0,aboard)?"Contract changed":"Select route aboard after first delivery; host only";
                    if(input.deploy)status=equipment.Deploy(workers[i].transform.position,Quaternion.Euler(input.pitch,input.yaw,0),holder!=i&&!threat.Down[i],mission)?"Beacon deployed":"Aim at yard floor within 8m, empty hands; beacon must be ready";
                }
                input.buy=false;input.contract=false;input.deploy=false;input.inspect=false;
                if(input.action&&mission!=null&&mission.Phase==phaseAtTick){
                    int before=mission.Phase;
                    bool all=CarryMission.Aboard(workers[0].transform.position)&&(!peer||CarryMission.Aboard(workers[1].transform.position));
                    if(hazard)all&=!threat.Down[0]&&(!peer||!threat.Down[1]);
                    if(mission.Act(CarryMission.Aboard(workers[i].transform.position),all)){equipment.Begin(mission.BeaconUnlocked);ResetRoom();}
                    if(hazard&&before==1&&mission.Phase==2){clues.Clear();suppression.Begin();outer.Reset();}
                }
                input.action=false;
                if(input.interact)Interact(i);input.interact=false;input.jump=false;
            }
            if(hazard){
                bool field=mission.Phase==2||mission.Phase==3;
                clues.Display(field,mission.Phase==3);
                suppression.Tick(field,Time.fixedDeltaTime);threat.Hard=mission.HardContract;equipment.Tick(field,Time.fixedDeltaTime,threat,suppression.Intrusion?outer:null);
                bool evacuate=threat.Tick(new[]{workers[0].transform.position,workers[1].transform.position},peer,inputs,holder,mission.Phase==2||mission.Phase==3,Time.fixedDeltaTime);
                outer.Tick(new[]{workers[0].transform.position,workers[1].transform.position},peer,inputs,holder,field&&suppression.Intrusion,Time.fixedDeltaTime);
                if(holder>=0&&threat.Down[holder]){Release();status="Employee down / parcel released";}
                if(evacuate){mission.Abort();ResetRoom();status="Emergency recovery / secured pay retained";}
                danger=threat.Snapshot();outerDanger=outer.Snapshot();for(int i=0;i<2;i++){inputs[i].call=false;inputs[i].shove=false;}
            }
            if(equipment.Carrier>=0){int carrier=equipment.Carrier;if(hazard&&threat.Down[carrier])equipment.RecoverCarrier(workers[carrier].transform.position);else equipment.Follow(workers[carrier].transform.position,Quaternion.Euler(inputs[carrier].pitch,inputs[carrier].yaw,0));}
            if(holder>=0){cargo.isKinematic=true;var p=workers[holder].transform;var look=Quaternion.Euler(inputs[holder].pitch,inputs[holder].yaw,0);var desired=p.position+Vector3.up*1.57f+look*new Vector3(0,-.54f,1.1f);
                var delta=desired-cargo.position;var next=desired;
                if(delta.magnitude>.001f){foreach(var h in Physics.BoxCastAll(cargo.position,new Vector3(.4f,.325f,.325f)*.98f,delta.normalized,cargo.rotation,delta.magnitude,~0,QueryTriggerInteraction.Ignore)){if(h.collider==cargoCollider||h.collider==workers[holder])continue;next=cargo.position+delta.normalized*Mathf.Min(Vector3.Distance(cargo.position,next),Mathf.Max(0,h.distance-.03f));}}
                cargo.MovePosition(next);
                // Reject rotation into a wall/floor even when translation has already stopped.
                bool clear=true;foreach(var overlap in Physics.OverlapBox(next,new Vector3(.4f,.325f,.325f)*.99f,look,~0,QueryTriggerInteraction.Ignore)){if(overlap!=cargoCollider&&overlap!=workers[holder]){clear=false;break;}}
                if(clear)cargo.MoveRotation(look);
                if(Vector3.Distance(next,p.position+Vector3.up) > 2.4f)Release();
            }else{cargo.isKinematic=mission!=null&&mission.Phase>=3;if(cargo.position.y < -3){cargo.position=new Vector3(0,.6f,-3);cargo.linearVelocity=Vector3.zero;} }
            if(mission!=null){mission.Tick(cargo.position,cargo.linearVelocity,holder,Time.fixedDeltaTime);unlocked=mission.BeaconUnlocked;hard=mission.HardContract;deliveries=mission.SuccessfulDeliveries;receiptCollected=mission.ReceiptCollected;receiptReady=mission.ReceiptReady;receiptProgress=mission.ReceiptProgress;missionPhase=mission.Phase;credits=mission.Credits;receipt=mission.Receipt;returnPay=mission.ReturnPay;}
            if(hazard&&save!=null&&Time.realtimeSinceStartup>=retrySaveAt){
                bool written=save.Write(mission,out var notice);
                if(notice!=null&&(!written||!saveNotice.StartsWith("Recovered previous backup",StringComparison.Ordinal)))saveNotice=notice;
                retrySaveAt=written?0:Time.realtimeSinceStartup+1;
            }
            if(tick%3==0){var s=Snapshot();wire?.Send(JsonUtility.ToJson(s));WriteEvidence(s);}
        }else if(target!=null){
            cargo.isKinematic=true;holder=target.holder;tick=target.tick;hazard=target.hazard;if(hazard&&threat==null)threat=new CarryThreat();if(hazard&&outer==null){outer=new CarryThreat(threat);suppression=new CarrySuppression();}suppression?.Apply(target.shiftElapsed);outerDanger=target.outerDanger;if(hazard&&clues==null)clues=new CarryClues();clues?.Apply(target.clueMask);if(hazard&&mission==null)mission=new CarryMission();unlocked=target.unlocked;hard=target.hard;deliveries=target.deliveries;equipment.Apply(target);danger=target.danger;receiptCollected=target.receiptCollected;receiptReady=target.receiptReady;receiptProgress=target.receiptProgress;missionPhase=target.phase;credits=target.credits;receipt=target.receipt;returnPay=target.returnPay;
            for(int i=0;i<2;i++){workers[i].enabled=false;workers[i].transform.position=Vector3.Lerp(workers[i].transform.position,i==0?target.p0:target.p1,.55f);workers[i].transform.rotation=Quaternion.Euler(0,i==0?target.yaw0:target.yaw1,0);}
            cargo.position=Vector3.Lerp(cargo.position,target.cargo,.55f);cargo.rotation=Quaternion.Slerp(cargo.rotation,target.rotation,.55f);WriteEvidence(target);
        }
    }
    void ResolveUse(int who,CarryInput input){
        if(!input.interact)return;
        input.interact=false;
        if(holder==who||equipment.Carrier==who){if(mission!=null&&CarryMission.Aboard(workers[who].transform.position))input.action=true;return;}
        var pos=workers[who].transform.position;var origin=pos+Vector3.up*1.57f;var look=Quaternion.Euler(input.pitch,input.yaw,0);
        if(hazard&&peer&&threat.Down[1-who]&&Vector3.Distance(pos,workers[1-who].transform.position)<=2&&CarryThreat.Sight(pos,workers[1-who].transform.position))return;
        if(mission!=null&&mission.Phase==3&&!mission.ReceiptCollected&&ReceiptFeedback.CanReach(origin,look)){input.interact=true;return;}
        if(hazard&&clues.Target(origin,look)>=0){input.inspect=true;return;}
        if(hazard&&equipment.PickUp(who,pos,look,holder!=who))return;
        var delta=cargo.position-(pos+Vector3.up*1.45f);
        if((mission==null||mission.Phase==2)&&holder<0&&delta.magnitude<=2.4f&&Vector3.Angle(look*Vector3.forward,delta)<65&&Physics.Raycast(pos+Vector3.up*1.45f,delta.normalized,out var hit,delta.magnitude+.1f)&&hit.collider==cargoCollider){input.interact=true;return;}
        if(mission!=null&&CarryMission.Aboard(pos)){input.action=true;return;}
        
    }
    void Interact(int who){
        if(hazard&&threat.Down[who])return;
        if(mission!=null&&mission.Phase==3){if(ReceiptFeedback.CanReach(workers[who].transform.position+Vector3.up*1.57f,Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0))&&mission.CollectReceipt())status="Receipt collected / return aboard to get paid";return;}
        if(mission!=null&&mission.Phase!=2)return;
        if(holder>=0||equipment.Carrier==who)return;
        var origin=workers[who].transform.position+Vector3.up*1.45f;var delta=cargo.position-origin;
        if(delta.magnitude>2.4f)return;
        var look=Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0)*Vector3.forward;
        if(Vector3.Angle(look,delta)>65)return;
        if(Physics.Raycast(origin,delta.normalized,out var hit,delta.magnitude+.1f,~0,QueryTriggerInteraction.Ignore)&&hit.collider!=cargoCollider)return;
        holder=who;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;cargo.isKinematic=true;Physics.IgnoreCollision(cargoCollider,workers[who],true);status="Parcel held";
    }
    void Release(){if(cargo==null)return;if(holder>=0)Physics.IgnoreCollision(cargoCollider,workers[holder],false);holder=-1;cargo.isKinematic=false;cargo.linearVelocity=Vector3.zero;cargo.angularVelocity=Vector3.zero;}
    CarryState Snapshot()=>new CarryState{beaconCarrier=equipment.Carrier,beaconExists=equipment.Exists,receiptCollected=receiptCollected,receiptReady=receiptReady,receiptProgress=receiptProgress,shiftElapsed=suppression==null?0:suppression.Elapsed,suppressionStage=suppression==null?0:suppression.Stage,outerDanger=outerDanger,clueMask=clues==null?0:clues.Mask,unlocked=unlocked,hard=hard,deliveries=deliveries,charges=equipment.Charges,beaconTime=equipment.Remaining,beaconPosition=equipment.Position,hazard=hazard,danger=danger,phase=missionPhase,credits=credits,receipt=receipt,returnPay=returnPay,tick=tick,holder=holder,p0=workers[0].transform.position,p1=workers[1].transform.position,yaw0=inputs[0].yaw,yaw1=inputs[1].yaw,cargo=cargo.position,rotation=cargo.rotation,players=peer?2:1,connected=peer,ack=inputs[1].seq,message=status};
    [Serializable] class UiEvidence {public string language,host,objective,status,firstRecord,secondRecord,suppressionCue;public bool koreanGlyph,journalOpen;}
    void WriteEvidence(CarryState s){if(testFolder==null)return;try{File.WriteAllText(Path.Combine(testFolder,"ui.json"),JsonUtility.ToJson(new UiEvidence{suppressionCue=suppression==null?null:T(CarrySuppression.Cue(suppression.Stage)),journalOpen=journalOpen,firstRecord=T(clues!=null&&(clues.Mask&1)!=0?CarryClues.FirstBody:"Not discovered"),secondRecord=T(clues!=null&&(clues.Mask&2)!=0?CarryClues.SecondBody:"Not discovered"),language=CarryLanguage.Korean?"ko":"en",host=T("HOST"),objective=T(CarryMission.Objective(missionPhase)),status=T(status),koreanGlyph=CarryLanguage.Font.HasCharacter('한')}));File.WriteAllText(Path.Combine(testFolder,"state.tmp"),JsonUtility.ToJson(s));var path=Path.Combine(testFolder,"state.json");if(File.Exists(path))File.Delete(path);File.Move(Path.Combine(testFolder,"state.tmp"),path);}catch(IOException){} }
    void LateUpdate(){if(eye==null)return;eye.transform.position=workers[local].transform.position+Vector3.up*(hazard&&danger!=null&&(local==0?danger.down0:danger.down1)?.55f:1.57f);eye.transform.rotation=Quaternion.Euler(pitch,yaw,0);
        receiptFeedback?.Display(active?missionPhase:-1,receiptProgress,receiptCollected,receiptReady);
        suppression?.Display(active&&(missionPhase==2||missionPhase==3));
        if(outer!=null&&outerDanger!=null)outer.Display(outerDanger,active&&(missionPhase==2||missionPhase==3)&&suppression.Stage>=2);
        clues?.Display(active&&(missionPhase==2||missionPhase==3),missionPhase==3);
        equipment.Display(active&&hazard);
        baton.Display(eye,workers,local,peer,active&&hazard,holder,equipment.Carrier,danger,hosting?inputs[0].yaw:(target==null?0:target.yaw0),hosting?inputs[1].yaw:(target==null?0:target.yaw1));
        if(threat!=null&&danger!=null){threat.Display(danger,active&&(missionPhase==2||missionPhase==3));
            for(int i=0;i<2;i++){bool down=i==0?danger.down0:danger.down1;rigs[i].localRotation=Quaternion.Euler(0,0,down?90:0);rigs[i].localPosition=down?new Vector3(.5f,.3f,0):Vector3.zero;}
        }
        if(capturePending && testFolder!=null){capturePending=false;var rt=new RenderTexture(960,600,24);rt.Create();
            var req=new UnityEngine.Rendering.Universal.UniversalRenderPipeline.SingleCameraRequest{destination=rt};
            UnityEngine.Rendering.RenderPipeline.SubmitRenderRequest(eye,req);var old=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(960,600,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,960,600),0,0);tex.Apply();File.WriteAllBytes(Path.Combine(testFolder,"capture.png"),tex.EncodeToPNG());ScreenCapture.CaptureScreenshot(Path.Combine(testFolder,"screen.png"));RenderTexture.active=old;Destroy(tex);rt.Release();Destroy(rt);
        }
    }
    static string T(string text)=>CarryLanguage.Text(text);
    static void Label(Rect rect,string text)=>GUI.Label(rect,T(text));
    static bool Button(Rect rect,string text)=>GUI.Button(rect,T(text));
    void OnGUI(){
        GUI.skin.font=CarryLanguage.Font;GUI.skin.label.wordWrap=true;
        GUI.skin.label.fontSize=18;GUI.skin.button.fontSize=20;GUI.skin.textField.fontSize=20;
        if(active&&journalOpen&&clues!=null){DrawJournal();return;}
        if(!active||menu){GUI.Box(new Rect(20,20,560,440),"");Label(new Rect(40,38,500,35),hazard?"NO RETURNS / LISTENER TEST":missionPhase<0?"NO RETURNS / CARRY TEST":"NO RETURNS / DELIVERY LOOP");Label(new Rect(40,78,480,55),status);
            if(!active){if(Button(new Rect(40,140,220,45),"HOST"))StartHost();address=GUI.TextField(new Rect(40,202,300,38),address,64);if(Button(new Rect(355,200,170,42),"JOIN"))StartJoin();Label(new Rect(40,252,480,80),"Direct LAN / TCP 27841\nSame PC: 127.0.0.1\nAllow host through Windows Firewall if prompted.");}
            else{if(Button(new Rect(40,140,220,45),"RESUME")){menu=false;SetCursor(true);}if(Button(new Rect(280,140,220,45),"LEAVE SESSION"))Disconnect();Label(new Rect(40,200,480,70),missionPhase<0?"WASD move / Mouse look / E interact / Q set down\nSpace jump (empty hands) / R reset (host)":"WASD move / Mouse look / E interact / Q set down\nSpace jump (empty hands) / E ship action");}
            if(active){Label(new Rect(40,270,100,30),T("FOV")+" "+Mathf.RoundToInt(eye.fieldOfView));eye.fieldOfView=GUI.HorizontalSlider(new Rect(150,285,340,20),eye.fieldOfView,65,100);}
            if(Button(new Rect(40,315,220,35),"QUIT"))Application.Quit();if(Button(new Rect(40,375,480,42),"한국어 / English  ·  "+(CarryLanguage.Korean?"한국어":"English")))CarryLanguage.Toggle();
            if(active&&hazard){GUI.Box(new Rect(600,20,340,440),"");
                Label(new Rect(618,38,304,65),"SHIP SUPPLY / saved host license");
                Label(new Rect(618,110,304,65),string.Format(T("Wallet {0} CR / Beacon {1}"),credits,T(unlocked?"OWNED":"120 CR")));
                if(Button(new Rect(618,180,304,48),(supplyChoice==0?"[E] ":"")+T("Buy beacon license")))inputs[local].buy=true;
                Label(new Rect(618,240,304,90),"Purchase spawns beacon aboard. E carry / Q place. Two uses per shift, 8 seconds each.");
                if(Button(new Rect(618,335,304,48),(supplyChoice==1?"[E] ":"")+T("Change selected contract")))inputs[local].contract=true;
                Label(new Rect(618,390,304,55),hard?"RISK / receipt 450 + return 180":"STANDARD / receipt 300 + return 120");
            }
            if(active&&hazard)Label(new Rect(22,470,Screen.width-44,70),hosting?saveNotice:"Using host progress / personal save unchanged");
            return;}
        Label(new Rect(22,18,600,35),T(hazard?"LISTENER TEST":missionPhase<0?"CARRY TEST":"DELIVERY LOOP")+" / "+T(hosting?"Host":"Client")+" / "+T("CREW")+" "+(peer?"2/2":"1/2"));
        Label(new Rect(22,48,Screen.width-44,55),missionPhase<0?"Move the parcel through the passage and onto the marked floor.":CarryMission.Objective(missionPhase));
        if(missionPhase==3)Label(new Rect(22,165,Screen.width-44,45),receiptCollected?"Receipt collected / return aboard to get paid":"Collect receipt at terminal [E] / No pay until return");
        if(missionPhase>=0)Label(new Rect(22,108,Screen.width-44,85),string.Format(T("SHARED WALLET {0} CR / RECEIPT {1} / RETURN {2}"),credits,receipt,returnPay)+"\n"+T(hazard?"[E] ship action / Blue floor: ship / Gold floor: reception / Host save":"[E] ship action / Blue floor: ship / Gold floor: reception / Session only"));
        Label(new Rect(Screen.width/2-5,Screen.height/2-12,25,30),"+");
        Label(new Rect(22,Screen.height-65,Screen.width-44,50),(holder==local||equipment.Carrier==local)?"[Q] SET DOWN   /   HANDS OCCUPIED":"[E] PICK UP nearby parcel   /   WASD move   /   Esc menu");
        Label(new Rect(22,Screen.height-100,Screen.width-44,45),!hosting&&target!=null?target.message:status);
        if(hazard)Label(new Rect(22,Screen.height-145,Screen.width-44,45),hosting?saveNotice:"Using host progress / personal save unchanged");
        if(hazard&&danger!=null){
            if(missionPhase==2||missionPhase==3)Label(new Rect(22,340,Screen.width-44,42),CarrySuppression.Cue(suppression.Stage));
            Label(new Rect(22,150,Screen.width-44,50),string.Format(T("{0} / Beacon {1}/2 ({2}s) / E carry / Q place"),T(hard?"RISK":"STANDARD"),equipment.Charges,Mathf.CeilToInt(equipment.Remaining)));
            Label(new Rect(22,200,Screen.width-44,65),"Shift quiet walk / C call / LMB baton / Hold E rescue / E inspect / Tab field log");
            if((missionPhase==2||missionPhase==3)&&clues.Target(eye.transform.position,eye.transform.rotation)>=0)Label(new Rect(Screen.width/2-170,Screen.height/2+30,340,60),"[E] Inspect terminal / shared field log");
            Label(new Rect(22,265,Screen.width-44,35),T(danger.state==2?"LISTENER: ATTACK WARNING - MOVE AWAY":danger.state==4?"LISTENER: STUNNED":danger.state==1?"LISTENER: INVESTIGATING SOUND":"LISTENER: PATROLLING / keep quiet"));
            Label(new Rect(22,302,Screen.width-44,35),string.Format(T("Rescue {0}% / Baton cooldown {1}s"),Mathf.RoundToInt((local==0?danger.rescue0:danger.rescue1)/2.5f*100),Mathf.CeilToInt(local==0?danger.cooldown0:danger.cooldown1)));
            if(!(local==0?danger.down0:danger.down1)&&(local==0?danger.down1:danger.down0))Label(new Rect(22,385,Screen.width-44,60),"TEAMMATE DOWN / put cargo down, approach and hold E");
            if(local==0?danger.down0:danger.down1)Label(new Rect(22,385,Screen.width-44,60),"DOWN / wait for teammate rescue. All down: emergency recovery.");
        }
    }
    void DrawJournal(){
        float w=Mathf.Min(860,Screen.width-40),x=(Screen.width-w)/2;
        GUI.Box(new Rect(x,20,w,Screen.height-40),"");
        Label(new Rect(x+22,36,w-44,38),"CINDER DEPOT / SHARED FIELD LOG");
        Label(new Rect(x+22,80,w-44,70),"The world keeps moving. Read aboard. Records reset on next arrival; not saved after exit.");
        if(suppression!=null&&(missionPhase==2||missionPhase==3))Label(new Rect(x+22,128,w-44,28),CarrySuppression.Cue(suppression.Stage));
        for(int i=0;i<2;i++){
            float y=158+i*155;bool found=(clues.Mask&(1<<i))!=0;
            Label(new Rect(x+22,y,w-44,30),found?(i==0?CarryClues.FirstTitle:CarryClues.SecondTitle):"UNRECORDED / inspect the site");
            Label(new Rect(x+22,y+36,w-44,112),found?(i==0?CarryClues.FirstBody:CarryClues.SecondBody):"No entry yet. A teammate can share it by inspecting a terminal.");
        }
        if(Button(new Rect(x+22,Screen.height-85,210,42),"CLOSE / Tab or Esc")){journalOpen=false;SetCursor(true);}
        if(Button(new Rect(x+w-254,Screen.height-85,232,42),"한국어 / English"))CarryLanguage.Toggle();
    }
    void OnDestroy(){wire?.Dispose();listener?.Stop();joining?.Close();SetCursor(false);}
}
}
