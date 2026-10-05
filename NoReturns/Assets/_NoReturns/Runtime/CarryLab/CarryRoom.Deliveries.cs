using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    sealed class ParcelBody {public Rigidbody body;public Collider collider;public int carrier=-1;}
    readonly List<ParcelBody> parcels=new List<ParcelBody>{new ParcelBody()};
    int parcelIndex,parcelCount=1;
    readonly float[] receiverNoise=new float[3];
    Rigidbody cargo {get=>parcels[parcelIndex].body;set=>parcels[parcelIndex].body=value;}
    Collider cargoCollider {get=>parcels[parcelIndex].collider;set=>parcels[parcelIndex].collider=value;}
    int holder {get=>parcels[parcelIndex].carrier;set=>parcels[parcelIndex].carrier=value;}
    DeliveryState[] Manifest=>hosting?mission?.Deliveries?.Entries:target?.deliveriesState;
    bool Multi=>Manifest!=null&&Manifest.Length>0;
    int ParcelMask {get{int mask=0;for(int i=0;i<parcelCount;i++)if(parcels[i].carrier>=0)mask|=1<<parcels[i].carrier;return mask;}}
    int HeldParcel(int who){for(int i=0;i<parcelCount;i++)if(parcels[i].carrier==who)return i;return -1;}
    bool Carrying(int who)=>HeldParcel(who)>=0;
    Vector3 ParcelSpawn(int id)=>new Vector3(-20.7f+(id%2==0?-1.02f:1.02f),1.4f,-28-(id/2)*1.0f);
    void SelectParcelFor(int who){
        int held=HeldParcel(who);if(held>=0){parcelIndex=held;return;}
        parcelIndex=0;if(!Multi)return;
        var look=Quaternion.Euler(inputs[who].pitch,inputs[who].yaw,0);
        if(Physics.Raycast(workers[who].transform.position+Vector3.up*1.45f,look*Vector3.forward,out var hit,2.4f,~0,QueryTriggerInteraction.Ignore))
            for(int i=0;i<parcelCount;i++)if(parcels[i].collider==hit.collider){parcelIndex=i;return;}
    }
    void EnsureParcels(int count,bool reset=false){
        count=Mathf.Clamp(count,1,6);bool changed=count!=parcelCount;
        while(parcels.Count<count){var obj=Instantiate(parcels[0].body.gameObject);obj.name="Delivery parcel "+parcels.Count;parcels.Add(new ParcelBody{body=obj.GetComponent<Rigidbody>(),collider=obj.GetComponent<Collider>()});}
        parcelCount=count;
        for(int i=0;i<parcels.Count;i++){
            bool enabled=i<count&&!(Manifest!=null&&i<Manifest.Length&&Manifest[i].collected);parcels[i].body.gameObject.SetActive(enabled);
            if((reset||changed)&&i<count){parcelIndex=i;Release();cargo.position=Multi?ParcelSpawn(i):CargoSpawn;cargo.rotation=Quaternion.identity;}
            var addressLabel=parcels[i].body.transform.Find("Address label");if(addressLabel)addressLabel.gameObject.SetActive(Multi);
            if(Multi&&i<count){
                var label=parcels[i].body.transform.Find("Address label");
                if(!label){var g=new GameObject("Address label");g.transform.SetParent(parcels[i].body.transform,false);g.transform.localPosition=new Vector3(0,.34f,0);g.transform.localRotation=Quaternion.Euler(90,0,0);var t=g.AddComponent<TextMesh>();t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;t.characterSize=.045f;t.fontSize=40;t.color=Color.black;label=g.transform;}
                var e=Manifest[i];label.GetComponent<TextMesh>().text="NR-"+(i+1).ToString("00")+"\n"+CarryDeliveries.Addresses[e.receiver]+"\n"+e.reward+" CR";
            }
        }
        parcelIndex=0;
    }
    void ReleaseAll(){for(int i=0;i<parcelCount;i++){parcelIndex=i;Release();}parcelIndex=0;}
    ParcelState[] ParcelSnapshots(){if(!Multi)return null;var s=new ParcelState[parcelCount];for(int i=0;i<s.Length;i++)s[i]=new ParcelState{position=parcels[i].body.position,rotation=parcels[i].body.rotation,holder=parcels[i].carrier};return s;}
    void ApplyParcels(CarryState s){
        if(s.parcels==null||s.parcels.Length==0){if(parcelCount!=1)EnsureParcels(1);parcels[0].carrier=s.holder;return;}
        EnsureParcels(s.parcels.Length);
        for(int i=0;i<s.parcels.Length;i++){var p=parcels[i];p.body.isKinematic=true;p.carrier=s.parcels[i].holder;p.body.position=Vector3.Lerp(p.body.position,s.parcels[i].position,.55f);p.body.rotation=Quaternion.Slerp(p.body.rotation,s.parcels[i].rotation,.55f);}
        SelectParcelFor(local);
    }
    readonly List<ReceiptFeedback> receivers=new List<ReceiptFeedback>();
    void EnsureReceivers(){
        if(!Multi||receivers.Count>0)return;
        receivers.Add(receiptFeedback);
        for(int i=1;i<3;i++){
            var center=CarryDeliveries.Receivers[i];var terminalOffset=i==1?new Vector3(1.2f,0,0):Vector3.zero;var root=new GameObject("Delivery receiver "+i);root.transform.position=center-new Vector3(-6,0,9);
            var art=FacilityArt.Place("Receipt",center+terminalOffset+new Vector3(-2.1f,.8f,0),new Vector3(.75f,1.6f,.65f),180);
            var terminal=new GameObject("Receipt terminal collision "+i);terminal.transform.position=center+terminalOffset+new Vector3(-2.1f,.8f,0);terminal.AddComponent<BoxCollider>().size=new Vector3(.75f,1.6f,.65f);
            var feedback=root.AddComponent<ReceiptFeedback>();feedback.TerminalArt=art;feedback.TerminalOffset=terminalOffset;receivers.Add(feedback);art.transform.SetParent(root.transform,true);terminal.transform.SetParent(root.transform,true);
            var sign=new GameObject("Address sign");sign.transform.position=center+terminalOffset+new Vector3(-2.1f,2,0);sign.transform.rotation=Quaternion.Euler(0,180,0);var text=sign.AddComponent<TextMesh>();text.text=CarryDeliveries.Addresses[i];text.anchor=TextAnchor.MiddleCenter;text.characterSize=.12f;text.fontSize=40;sign.transform.SetParent(root.transform,true);
        }
    }
    void DisplayDeliveries(){
        if(!Multi)return;EnsureReceivers();
        for(int r=0;r<receivers.Count;r++){
            if(receivers[r])receivers[r].gameObject.SetActive(true);
            DeliveryState display=null;foreach(var e in Manifest)if(e.receiver==r&&!e.collected&&(display==null||(e.verified&&!display.verified)||(e.scanning&&!display.verified&&!display.scanning)))display=e;
            if(receivers[r])receivers[r].Display(missionPhase==2&&display!=null?display.verified?3:2:-1,display!=null&&display.scanning?display.progress/CarryDeliveries.VerifySeconds:0,false,display!=null&&display.verified);
        }
    }
    string DeliveryList(CarryState s){
        var text="";if(s.deliveriesState==null)return text;
        foreach(var e in s.deliveriesState)text+="NR-"+(e.id+1).ToString("00")+" / "+CarryDeliveries.Addresses[e.receiver]+" / "+e.reward+" CR / "+T(e.collected?"DELIVERED":"AVAILABLE")+"\n";
        for(int r=0;r<3;r++){
            int count=0,remaining=0,pay=0;foreach(var e in s.deliveriesState)if(e.receiver==r){count++;pay+=e.reward;if(!e.collected)remaining++;}
            if(count>=2&&remaining<=1)text+=string.Format(T("{0}: {1} left / bundle +{2} CR"),CarryDeliveries.Addresses[r],remaining,pay/4)+"\n";
        }
        return text;
    }
}
}
