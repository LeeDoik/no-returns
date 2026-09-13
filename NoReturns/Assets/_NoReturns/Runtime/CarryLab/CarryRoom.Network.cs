using System;
using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    readonly CarryWire[] connections=new CarryWire[4];
    readonly float[] lastInputs=new float[4];
    int occupiedMask=1;
    bool Present(int slot)=>(occupiedMask&(1<<slot))!=0;
    int CrewCount {get {int count=0;for(int i=0;i<4;i++)if(Present(i))count++;return count;}}
    static Vector3 Spawn(int slot)=>new Vector3(slot%2==0?-1:1,0,slot<2?-5:-7);
    Vector3[] Positions(){var p=new Vector3[4];for(int i=0;i<4;i++)p[i]=workers[i].transform.position;return p;}
    float[] Yaws(){var v=new float[4];for(int i=0;i<4;i++)v[i]=hosting?inputs[i].yaw:target?.yaws!=null?target.yaws[i]:0;return v;}
    bool AllAboard(){for(int i=0;i<4;i++)if(Present(i)&&(!CarryMission.Aboard(workers[i].transform.position)||(hazard&&threat.Down[i])))return false;return true;}
    bool AnyOtherDown(int slot){for(int i=0;i<4;i++)if(i!=slot&&Present(i)&&danger!=null&&danger.IsDown(i))return true;return false;}
    void Broadcast(){for(int i=1;i<4;i++)if(connections[i]!=null){var snapshot=Snapshot();snapshot.recipient=i;snapshot.ack=inputs[i].seq;connections[i].Send(JsonUtility.ToJson(snapshot));}}
    void RemovePeer(int slot){
        connections[slot]?.Dispose();connections[slot]=null;occupiedMask&=~(1<<slot);peer=CrewCount>1;
        if(equipment.Carrier==slot)equipment.RecoverCarrier(workers[slot].transform.position);
        if(holder==slot)Release();inputs[slot]=new CarryInput();mission?.Abort();
        // Preserve the prototype's disconnect-aborts-shift rule, recovering down crew as well.
        if(hazard){bool recover=false;for(int i=0;i<4;i++)if(Present(i)&&threat.Down[i])recover=true;if(recover)ResetRoom();}status="HOST / partner left";Broadcast();
    }
    void PollConnections(){
        if(connectTask!=null){
            if(connectTask.IsCompleted){
                if(connectTask.IsFaulted){status="Connection failed. Check address / host / firewall.";joining.Close();}
                else{wire=new CarryWire(joining);hosting=false;lastPacket=Time.realtimeSinceStartup;status="Connecting...";}
                connectTask=null;
            }else if(Time.realtimeSinceStartup-connectAt>8){joining.Close();connectTask=null;status="Connection timed out.";}
        }
        if(hosting&&listener!=null&&listener.Pending()){
            var incoming=listener.AcceptTcpClient();int slot=-1;for(int i=1;i<4;i++)if(connections[i]==null){slot=i;break;}
            if(slot<0||(mission!=null&&mission.Phase!=0)){
                using(var rejected=new CarryWire(incoming))rejected.Send(JsonUtility.ToJson(new CarryState{rejection=slot<0?"Room full / wait for a free crew slot":"Shift in progress / wait for host to prepare next shift"}));
            }else{
                connections[slot]=new CarryWire(incoming);occupiedMask|=1<<slot;peer=true;lastInputs[slot]=Time.realtimeSinceStartup;inputs[slot]=new CarryInput();
                workers[slot].gameObject.SetActive(true);workers[slot].enabled=false;workers[slot].transform.position=Spawn(slot);workers[slot].enabled=true;
                status="HOST / partner connected";Broadcast();
            }
        }
        if(hosting){
            for(int i=1;i<4;i++){
                var connection=connections[i];if(connection==null)continue;
                foreach(var line in connection.Read())try{
                    var v=JsonUtility.FromJson<CarryInput>(line);var previous=inputs[i];
                    if(v==null||v.seq<=previous.seq||!Finite(v.x)||!Finite(v.z)||!Finite(v.yaw)||!Finite(v.pitch))continue;
                    v.x=Mathf.Clamp(v.x,-1,1);v.z=Mathf.Clamp(v.z,-1,1);v.pitch=Mathf.Clamp(v.pitch,-70,70);v.reset=false;
                    v.drop|=previous.drop;v.inspect|=previous.inspect;v.buy|=previous.buy;v.contract|=previous.contract;v.deploy|=previous.deploy;
                    v.call|=previous.call;v.shove|=previous.shove;v.action|=previous.action;v.interact|=previous.interact;v.jump|=previous.jump;
                    inputs[i]=v;lastInputs[i]=Time.realtimeSinceStartup;
                }catch{connection.Dispose();}
                if(connection.Closed||Time.realtimeSinceStartup-lastInputs[i]>5)RemovePeer(i);
            }
        }else if(wire!=null){
            foreach(var line in wire.Read())try{
                var v=JsonUtility.FromJson<CarryState>(line);
                if(v==null||v.protocol!=10){Disconnect();status="Protocol mismatch / use the same game build";break;}
                if(!string.IsNullOrEmpty(v.rejection)){Disconnect();status=v.rejection;break;}
                if(v.recipient<1||v.recipient>3||v.positions==null||v.positions.Length!=4||v.yaws==null||v.yaws.Length!=4)throw new Exception("Invalid crew snapshot");
                target=v;local=v.recipient;occupiedMask=v.occupiedMask;peer=CrewCount>1;lastPacket=Time.realtimeSinceStartup;
                if(!active){StartSession();status="CLIENT / connected";}
            }catch{wire?.Dispose();}
            if(wire!=null&&(wire.Closed||Time.realtimeSinceStartup-lastPacket>5)){Disconnect();status="Host connection lost.";}
        }
    }
}
}
