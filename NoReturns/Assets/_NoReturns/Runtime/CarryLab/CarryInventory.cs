using System;
using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
[Serializable] public sealed class GroundMedicine {public int id;public Vector3 position;}
[Serializable] public sealed class InventoryState {
    public int slots;public int[] medicine,selected;public GroundMedicine[] ground;
    public bool Ready {get{if(slots<1||slots>2||medicine==null||medicine.Length!=8||selected==null||selected.Length!=4||ground==null||ground.Length>32)return false;foreach(int dose in medicine)if(dose<0||dose>1)return false;foreach(int slot in selected)if(slot<0||slot>1)return false;foreach(var item in ground)if(item==null||item.id<0||!float.IsFinite(item.position.x)||!float.IsFinite(item.position.y)||!float.IsFinite(item.position.z))return false;return true;}}
}
// Session-owned doses. Only medicine is available until other tools are implemented.
public sealed class CarryInventory {
    public readonly int[] Medicine=new int[8],Selected=new int[4];
    public readonly List<GroundMedicine> Ground=new List<GroundMedicine>();int nextId;
    int Index(int who,int slots)=>who>=0&&who<4&&slots>=1&&slots<=2&&Selected[who]>=0&&Selected[who]<slots?who*2+Selected[who]:-1;
    public bool Select(int who,int slot,int slots){if(who<0||who>3||slots<1||slots>2||slot<0||slot>=slots)return false;Selected[who]=slot;return true;}
    public bool Buy(int who,int slots,CarryMission mission,bool aboard){int i=Index(who,slots);if(i<0||Medicine[i]!=0||mission==null||!mission.BuyMedicine(aboard))return false;Medicine[i]=1;return true;}
    public bool Use(int who,int target,int slots,CarryVisit visit,bool reachable){int i=Index(who,slots);if(i<0||Medicine[i]!=1||!reachable||visit==null||!visit.CanAct(who)||!visit.Heal(target))return false;Medicine[i]=0;return true;}
    public bool Drop(int who,int slots,Vector3 position){int i=Index(who,slots);if(i<0||Medicine[i]!=1||Ground.Count>=32)return false;Ground.Add(new GroundMedicine{id=nextId++,position=position});Medicine[i]=0;return true;}
    public bool PickUp(int who,int id,int slots){int i=Index(who,slots);if(i<0||Medicine[i]!=0)return false;int g=Ground.FindIndex(x=>x.id==id);if(g<0)return false;Medicine[i]=1;Ground.RemoveAt(g);return true;}
    public InventoryState Snapshot(int slots){var ground=new GroundMedicine[Ground.Count];for(int i=0;i<ground.Length;i++)ground[i]=new GroundMedicine{id=Ground[i].id,position=Ground[i].position};return new InventoryState{slots=slots,medicine=(int[])Medicine.Clone(),selected=(int[])Selected.Clone(),ground=ground};}
}
}
