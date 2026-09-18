using UnityEngine;
namespace NoReturns.CarryLab {
// Host-owned ledger; restore progression only, never an unfinished shift.
public sealed class CarryMission {
    public CarryMission(CarryProgress progress=null){
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
    public bool CollectReceipt(){if(!ReceiptReady||ReceiptCollected)return false;ReceiptCollected=true;return true;}
    public float ReceiptProgress=>Phase==3?1:Phase==2?Mathf.Clamp01(stable/.75f):0;
    public static bool Aboard(Vector3 p)=>CinderDemoLayout.Active?CinderDemoLayout.Aboard(p):Mathf.Abs(p.x)<3 && p.z< -3.8f && p.z> -10 && p.y<2;
    public bool Act(bool aboard,bool allAboard){
        if(!aboard)return false;
        if(Phase==0){Phase=1;return false;}
        if(Phase==1){if(allAboard)Phase=2;return false;}
        if(Phase==2||Phase==3){if(allAboard){bool paid=Phase==3&&ReceiptCollected;Receipt=paid?(HardContract?450:300):0;ReturnPay=paid?(HardContract?180:120):0;Credits+=Receipt+ReturnPay;if(paid)SuccessfulDeliveries++;Phase=4;}return false;}
        Phase=0;HardContract=false;Receipt=0;ReturnPay=0;stable=0;printTime=0;ReceiptCollected=false;return true;
    }
    public void Tick(Vector3 p,Vector3 velocity,int holder,float dt){
        if(Phase==3){printTime+=dt;return;}
        if(Phase!=2)return;
        p-=CinderDemoLayout.ReceiptOffset;
        bool onBench=holder<0 && p.x> -7.05f && p.x< -4.95f && p.z>8.4f && p.z<9.6f && p.y>.2f && p.y<.65f && velocity.sqrMagnitude<.04f;
        stable=onBench?stable+dt:0;
        if(stable>=.75f){Phase=3;printTime=0;}
    }
    public void Abort(){if(Phase>0&&Phase<4){Phase=4;ReturnPay=0;stable=0;}}
    public static string Objective(int phase)=>phase switch {
        0=>"SHIP / [E] Select CINDER DEPOT route",
        1=>"CINDER DEPOT selected / Crew aboard, then [E] AUTO ARRIVE",
        2=>"Deliver sealed parcel onto the marked reception floor. [E] aboard: abort and return",
        3=>"COLLECT RECEIPT [E] at terminal / Bring it back aboard, then [E] RETURN TO GET PAID",
        _=>"SHIFT REPORT / [E] inside ship: prepare next shift"
    };
}
}
