using System;
using UnityEngine;
namespace NoReturns.CarryLab {
[Serializable] public sealed class DeliveryState {
    public int id,receiver,reward;public float progress;public bool scanning,verified,collected;
}
[Serializable] public sealed class ParcelState {public Vector3 position;public Quaternion rotation;public int holder=-1;}
// One authoritative entry per physical parcel; verified cargo is consumed on receipt collection.
public sealed class CarryDeliveries {
    public static readonly Vector3[] Receivers={new Vector3(17,0,12.4f),new Vector3(-23,0,-12),new Vector3(-2,0,26)};
    public static readonly string[] Addresses={"EAST / BAY 04","WEST / BAY 02","NORTH / BAY 07"};
    public readonly DeliveryState[] Entries;
    readonly bool[] bonusPaid=new bool[3];
    public const float VerifySeconds=25;
    public int Completed {get{int n=0;foreach(var e in Entries)if(e.collected)n++;return n;}}
    public CarryDeliveries(int crew){
        int count=Mathf.Clamp(crew+2,3,6);Entries=new DeliveryState[count];
        int[] routes={1,1,0,2,0,2};int[] pay={100,120,180,160,200,180};
        for(int i=0;i<count;i++)Entries[i]=new DeliveryState{id=i,receiver=routes[i],reward=pay[i]};
    }
    public void Tick(int id,Vector3 p,Vector3 velocity,int holder,bool field,float dt){
        var e=Entries[id];e.scanning=false;if(!field||e.collected||e.verified)return;
        var delta=p-Receivers[e.receiver];
        bool docked=holder<0&&Mathf.Abs(delta.x)<1.05f&&Mathf.Abs(delta.z)<.6f&&delta.y>.2f&&delta.y<.65f&&velocity.sqrMagnitude<.04f;
        // A receiver checks one parcel at a time. A paused earlier parcel does not reserve the machine.
        for(int i=0;i<id&&docked;i++)if(Entries[i].receiver==e.receiver&&Entries[i].scanning)docked=false;
        if(!docked)return;e.scanning=true;e.progress=Mathf.Min(VerifySeconds,e.progress+Mathf.Max(0,dt));
        if(e.progress>=VerifySeconds){e.verified=true;e.scanning=false;}
    }
    public int ReadyAt(int receiver){for(int i=0;i<Entries.Length;i++)if(Entries[i].receiver==receiver&&Entries[i].verified&&!Entries[i].collected)return i;return -1;}
    public int Collect(int receiver){
        int id=ReadyAt(receiver);if(id<0)return 0;var e=Entries[id];e.collected=true;int pay=e.reward;
        if(!bonusPaid[receiver]&&Total(receiver)>=2&&Remaining(receiver)==0){bonusPaid[receiver]=true;pay+=Bonus(receiver);}return pay;
    }
    public int Total(int receiver){int n=0;foreach(var e in Entries)if(e.receiver==receiver)n++;return n;}
    public int Remaining(int receiver){int n=0;foreach(var e in Entries)if(e.receiver==receiver&&!e.collected)n++;return n;}
    public int Bonus(int receiver){int pay=0;foreach(var e in Entries)if(e.receiver==receiver)pay+=e.reward;return pay/4;}
    public DeliveryState[] Snapshot(){var copy=new DeliveryState[Entries.Length];for(int i=0;i<copy.Length;i++){var e=Entries[i];copy[i]=new DeliveryState{id=e.id,receiver=e.receiver,reward=e.reward,progress=e.progress,scanning=e.scanning,verified=e.verified,collected=e.collected};}return copy;}
}
}
