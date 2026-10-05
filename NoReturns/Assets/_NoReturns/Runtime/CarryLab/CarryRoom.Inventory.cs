using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    CarryInventory inventory=new CarryInventory();bool inventoryMenu;
    readonly Dictionary<int,GameObject> medicineVisuals=new Dictionary<int,GameObject>();
    int InventorySlots {get{int mask=mission?.Visit?.Active==true?mission.Visit.Mask:occupiedMask;return (mask&(mask-1))==0?2:1;}}
    InventoryState Personal=>missionPhase<0?null:hosting?inventory.Snapshot(InventorySlots):target?.inventory;
    bool MedicineFree(int who)=>Present(who)&&!Carrying(who)&&equipment.Carrier!=who&&(mission?.Visit==null||!mission.Visit.Active||mission.Visit.CanAct(who));
    int MedicineTarget(int who,Quaternion look){
        var origin=workers[who].transform.position+Vector3.up*1.45f;
        if(Physics.Raycast(origin,look*Vector3.forward,out var hit,2.2f,~0,QueryTriggerInteraction.Ignore))for(int i=0;i<4;i++)if(i!=who&&Present(i)&&hit.collider==workers[i])return i;
        return who;
    }
    int GroundMedicineTarget(int who,Quaternion look){
        if(!Physics.Raycast(workers[who].transform.position+Vector3.up*1.45f,look*Vector3.forward,out var hit,2.4f,~0,QueryTriggerInteraction.Ignore))return -1;
        foreach(var pair in medicineVisuals)if(pair.Value&&hit.collider.gameObject==pair.Value)return pair.Key;return -1;
    }
    void InventoryInput(int who,CarryInput input){
        if(!cinderReview||mission==null)return;
        if(inventory.Selected[who]>=InventorySlots)inventory.Selected[who]=0;
        bool free=MedicineFree(who);var look=Quaternion.Euler(input.pitch,input.yaw,0);
        if(input.specialSlot>=0&&free)inventory.Select(who,input.specialSlot,InventorySlots);
        if(input.buyMedicine&&free)status=inventory.Buy(who,InventorySlots,mission,CarryMission.Aboard(workers[who].transform.position,true))?"Medicine equipped":"Purchase blocked / empty slot, ship preparation and 40 CR required";
        if(input.useMedicine&&free){int targetSlot=MedicineTarget(who,look);status=inventory.Use(who,targetSlot,InventorySlots,mission.Visit,targetSlot==who||CarryThreat.Sight(workers[who].transform.position,workers[targetSlot].transform.position))?"Medicine used / health restored":"Medicine unavailable / injured living target and empty hands required";}
        if(input.dropSpecial&&free&&(mission.Phase==2||mission.Phase==3)){
            var ahead=workers[who].transform.position+Quaternion.Euler(0,input.yaw,0)*Vector3.forward*.85f;
            if(Physics.Raycast(ahead+Vector3.up*1.2f,Vector3.down,out var floor,2.5f,~0,QueryTriggerInteraction.Ignore)&&floor.normal.y>.65f){var p=floor.point+Vector3.up*.14f;if(Physics.OverlapBox(p,new Vector3(.15f,.12f,.1f),Quaternion.identity,~0,QueryTriggerInteraction.Ignore).Length==0)inventory.Drop(who,InventorySlots,p);}
        }
        input.specialSlot=-1;input.useMedicine=input.buyMedicine=input.dropSpecial=false;
    }
    void DisplayInventory(){
        if(!active){foreach(var g in medicineVisuals.Values)if(g)g.SetActive(false);return;}
        var s=Personal;if(!cinderReview||s==null||!s.Ready)return;
        var alive=new HashSet<int>();foreach(var item in s.ground){
            alive.Add(item.id);if(!medicineVisuals.TryGetValue(item.id,out var g)||!g){g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name="Medicine "+item.id;g.transform.localScale=new Vector3(.3f,.25f,.2f);g.GetComponent<Renderer>().sharedMaterial=CarryWorld.Mat(new Color(.85f,.79f,.65f));var label=new GameObject("Medicine label");label.transform.SetParent(g.transform,false);label.transform.localPosition=new Vector3(0,.51f,0);label.transform.localRotation=Quaternion.Euler(90,0,0);var text=label.AddComponent<TextMesh>();text.text="MED";text.anchor=TextAnchor.MiddleCenter;text.characterSize=.18f;text.fontSize=32;text.color=new Color(.5f,.08f,.06f);medicineVisuals[item.id]=g;}
            g.transform.position=item.position;g.SetActive(active);
        }
        var removed=new List<int>();foreach(var pair in medicineVisuals)if(!alive.Contains(pair.Key)){Destroy(pair.Value);removed.Add(pair.Key);}foreach(int id in removed)medicineVisuals.Remove(id);
    }
    string InventoryPrompt(){var s=Personal;if(s==null||!s.Ready)return "";int slot=Mathf.Min(s.selected[local],s.slots-1);return string.Format(T("SPECIAL {0}/{1}: {2}"),slot+1,s.slots,T(s.medicine[local*2+slot]>0?"MEDICINE":"EMPTY"))+" / "+controls.Key(13)+T(" heal self / aimed teammate · ")+controls.Key(15)+T(" drop special")+(s.slots==2?" / "+controls.Key(14)+T(" switch slot"):"");}
}
}
