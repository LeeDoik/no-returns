using UnityEngine;
namespace NoReturns.CarryLab {
// Host-owned ledger; restore progression only, never an unfinished shift.
public sealed class CarryMission {
    public static readonly Vector3 CinderReceiptOffset=new Vector3(23,0,3.4f);
    readonly bool cinder,multiple;
    public CarryDeliveries Deliveries {get;private set;}
    public void SetCrew(int crew){if(multiple&&(Deliveries==null||Phase<2&&Deliveries.Entries.Length!=Mathf.Clamp(crew+2,3,6))&&Phase!=4)Deliveries=new CarryDeliveries(crew);}
    public bool CollectDelivery(int receiver){if(Deliveries==null||Visit==null||!Visit.Active)return false;int pay=Deliveries.Collect(receiver);Visit.AddDeliveryReward(pay);if(pay>0)ReceiptCollected=true;return pay>0;}
    public CarryVisit Visit {get;private set;}
    public void BeginVisit(int mask){if(cinder){Visit??=new CarryVisit();Visit.Begin(mask);}}
    public void CompleteVisit(){if(Visit==null||Visit.Active||Phase==4)return;Receipt=Visit.Gross;ReturnPay=0;Credits+=Visit.Paid;if(!Visit.Failed&&ReceiptCollected)SuccessfulDeliveries+=Deliveries==null?1:Deliveries.Completed;Phase=4;}
    public CarryMission(CarryProgress progress=null,bool cinder=false,bool multiple=false){
        this.cinder=cinder;this.multiple=multiple;if(multiple)Deliveries=new CarryDeliveries(1);
        if(progress==null)return;
        Credits=progress.credits;SuccessfulDeliveries=progress.deliveries;BeaconUnlocked=progress.beacon;
    }
    public int Phase {get;private set;} // 0 ship, 1 route selected, 2 field, 3 received, 4 report
    public int Credits {get;private set;}
    public int Receipt {get;private set;}
    public int ReturnPay {get;private set;}
    public bool BeaconUnlocked {get;private set;}
    public bool HardContract {get;private set;}
    public int SuccessfulDeliveries {get;private set;}
    public bool BuyBeacon(bool host,bool aboard){
        if(!host||!aboard||(Phase!=0&&Phase!=4)||BeaconUnlocked||Credits<120)return false;
        Credits-=120;BeaconUnlocked=true;return true;
    }
    public bool ToggleContract(bool host,bool aboard){
        if(!host||!aboard||Phase!=1||SuccessfulDeliveries==0)return false;
        HardContract=!HardContract;return true;
    }
    float stable,printTime;
    public bool ReceiptCollected {get;private set;}
    public bool ReceiptReady=>Phase==3&&printTime>=.75f;
    public bool CollectReceipt(){if(!ReceiptReady||ReceiptCollected)return false;ReceiptCollected=true;Visit?.CreditReceipt(HardContract?630:420);return true;}
    public float ReceiptProgress=>Phase==3?1:Phase==2?Mathf.Clamp01(stable/.75f):0;
    public static bool Aboard(Vector3 p,bool cinder=false)=>cinder
        ? Mathf.Abs(p.x+20.7f)<1.5f && p.z< -24.8f && p.z> -30.9f && p.y>.8f && p.y<3
        : Mathf.Abs(p.x)<3 && p.z< -3.8f && p.z> -10 && p.y<2;
    public bool Act(bool aboard,bool allAboard){
        if(!aboard)return false;
        if(Phase==0){Phase=1;return false;}
        if(Phase==1){if(allAboard)Phase=2;return false;}
        if(Phase==2||Phase==3){if(cinder)return false;if(allAboard){bool paid=Phase==3&&ReceiptCollected;Receipt=paid?(HardContract?450:300):0;ReturnPay=paid?(HardContract?180:120):0;Credits+=Receipt+ReturnPay;if(paid)SuccessfulDeliveries++;Phase=4;}return false;}
        Phase=0;HardContract=false;Receipt=0;ReturnPay=0;stable=0;printTime=0;ReceiptCollected=false;if(multiple)Deliveries=new CarryDeliveries(1);return true;
    }
    public void Tick(Vector3 p,Vector3 velocity,int holder,float dt){
        if(Deliveries!=null)return;
        if(Phase==3){printTime+=dt;return;}
        if(Phase!=2)return;
        if(cinder)p-=CinderReceiptOffset;
        bool onBench=holder<0 && p.x> -7.05f && p.x< -4.95f && p.z>8.4f && p.z<9.6f && p.y>.2f && p.y<.65f && velocity.sqrMagnitude<.04f;
        stable=onBench?stable+dt:0;
        if(stable>=.75f){Phase=3;printTime=0;}
    }
    public void Abort(){if(Phase>0&&Phase<4){Visit?.Fail();Phase=4;Receipt=0;ReturnPay=0;stable=0;}}
    public static string Objective(int phase)=>phase switch {
        0=>"SHIP / Open terminal to select CINDER DEPOT",
        1=>"CINDER DEPOT / Board together, then depart at terminal",
        2=>"Deliver parcel onto BAY 04 floor. Return through ship terminal.",
        3=>"Collect the receipt at BAY 04. Return aboard and settle at ship terminal.",
        _=>"SHIFT REPORT / Prepare next shift at ship terminal"
    };
}
}
