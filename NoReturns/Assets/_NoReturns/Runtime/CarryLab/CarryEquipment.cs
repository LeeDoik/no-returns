using UnityEngine;
namespace NoReturns.CarryLab {
// Shared stock and timing are host-owned; rendering is local to each peer.
public sealed class CarryEquipment {
    public int Charges {get;private set;}
    public float Remaining {get;private set;}
    public Vector3 Position {get;private set;}
    public int Carrier {get;private set;}=-1;
    public bool Exists {get;private set;}
    float pulse;
    GameObject visual;
    AudioSource audio; AudioClip clip; int lastBeat=-1;
    public void Begin(bool unlocked){Charges=unlocked?2:0;Remaining=0;pulse=0;Exists=unlocked;Carrier=-1;Position=CinderDemoLayout.Active?CinderDemoLayout.ShipPoint(new Vector3(-1.6f,1.26f,-2)):new Vector3(-2,.22f,-7);}
    // Remote placement is no longer supported; stock must be carried from the ship.
    public bool Deploy(Vector3 player,Quaternion look,bool free,CarryMission mission)=>false;
    public bool PickUp(int who,Vector3 player,Quaternion look,bool free){
        if(!Exists||!free||Carrier>=0||Remaining>0)return false;
        var origin=player+Vector3.up*1.57f;var delta=Position-origin;
        if(delta.magnitude>2.4f||Vector3.Angle(look*Vector3.forward,delta)>45)return false;
        if(!Physics.Raycast(origin,delta.normalized,out var hit,delta.magnitude+.2f)||hit.collider.gameObject!=visual)return false;
        Carrier=who;return true;
    }
    public bool PutDown(int who,Vector3 player,Quaternion look,bool field){
        if(Carrier!=who)return false;
        var ahead=player+Quaternion.Euler(0,look.eulerAngles.y,0)*Vector3.forward*.85f;
        if(!Physics.Raycast(ahead+Vector3.up*1.2f,Vector3.down,out var hit,2.5f)||hit.normal.y<.65f)return false;
        var p=hit.point+Vector3.up*.23f;
        foreach(var overlap in Physics.OverlapBox(p,new Vector3(.2f,.19f,.2f)))if(overlap.gameObject!=visual)return false;
        Carrier=-1;Position=p;
        if(field&&!CarryMission.Aboard(p)&&Charges>0){Remaining=8;pulse=0;Charges--;}
        return true;
    }
    public void Follow(Vector3 player,Quaternion look){if(Carrier>=0)Position=player+Vector3.up*1.57f+look*new Vector3(0,-.4f,.75f);}
    public void RecoverCarrier(Vector3 player){if(Carrier>=0){Carrier=-1;Position=player+Vector3.up*.23f;}}
    public void Tick(bool field,float dt,CarryThreat threat,CarryThreat extra=null){
        if(!field){Remaining=0;return;}
        if(Remaining<=0)return;
        pulse-=dt;if(pulse<=0){threat.Hear(Position,12);extra?.Distract(Position,12);pulse=1;}
        Remaining=Mathf.Max(0,Remaining-dt);
    }
    public void Apply(CarryState s){Carrier=s.beaconCarrier;Exists=s.beaconExists;Charges=s.charges;Remaining=s.beaconTime;Position=s.beaconPosition;}
    public void Display(bool field){
        if(visual==null){
            visual=GameObject.CreatePrimitive(PrimitiveType.Cylinder);visual.name="Decoy beacon";visual.GetComponent<Renderer>().sharedMaterial=CarryWorld.Mat(new Color(.1f,.9f,.8f));
            visual.transform.localScale=new Vector3(.42f,.22f,.42f);
            var model=FacilityArt.Place("Beacon",Vector3.zero,new Vector3(.42f,.44f,.34f),180);
            if(model){model.transform.SetParent(visual.transform,true);visual.GetComponent<Renderer>().enabled=false;}
            audio=visual.AddComponent<AudioSource>();audio.spatialBlend=1;audio.minDistance=2;audio.maxDistance=12;audio.rolloffMode=AudioRolloffMode.Linear;
            clip=AudioClip.Create("Beacon pulse placeholder",4410,1,22050,false);var data=new float[4410];
            for(int i=0;i<data.Length;i++)data[i]=Mathf.Sin(i*2*Mathf.PI*660/22050f)*.18f*(1-i/(float)data.Length);
            clip.SetData(data,0);
        }
        visual.SetActive(field&&Exists);visual.GetComponent<Collider>().enabled=Carrier<0;
        visual.transform.position=Position;
        visual.transform.localScale=new Vector3(.42f,.22f,.42f);
        int beat=Mathf.CeilToInt(Remaining);
        if(visual.activeSelf&&Remaining>0&&beat!=lastBeat)audio.PlayOneShot(clip);
        lastBeat=visual.activeSelf?beat:-1;
    }
}
}
