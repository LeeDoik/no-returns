using System;
using UnityEngine;
namespace NoReturns.CarryLab {
// Authoritative visit state. No presentation, physics, networking or persistent-wallet writes.
[Serializable] public sealed class VisitState {
    public bool active,departing,automatic,failed;
    public int gross,available,revivalFees,returnFees,paid,reviving=-1,mask;
    public float elapsed,departureLeft,revivalLeft;
    public int[] health,downs;
    public bool[] down,eliminated;
    public float[] bleedout,protection;
    public bool Ready=>health!=null&&health.Length==4&&downs!=null&&downs.Length==4&&down!=null&&down.Length==4&&eliminated!=null&&eliminated.Length==4&&bleedout!=null&&bleedout.Length==4;
}
public sealed class CarryVisit {
    public const int RevivalCost=100,ReturnCost=100;
    public const float RevivalSeconds=10,DepartureSeconds=12,FinalWarning=1020,FinalDeparture=1080;
    public readonly int[] Health={100,100,100,100},Downs=new int[4];
    public readonly bool[] Down=new bool[4],Eliminated=new bool[4];
    public readonly float[] Bleedout=new float[4],Protection=new float[4];
    public bool Active {get;private set;}
    public bool Departing {get;private set;}
    public bool Automatic {get;private set;}
    public bool Failed {get;private set;}
    public int Mask {get;private set;}
    public int Gross {get;private set;}
    public int RevivalFees {get;private set;}
    public int ReturnFees {get;private set;}
    public int Paid {get;private set;}
    public int Available=>Math.Max(0,Gross-RevivalFees);
    public int Reviving {get;private set;}=-1;
    public float RevivalLeft {get;private set;}
    public float Elapsed {get;private set;}
    public float DepartureLeft {get;private set;}
    int soloCharges; float soloHold,downAge;
    public void Begin(int mask){
        Mask=mask;Active=true;Departing=Automatic=Failed=false;Gross=RevivalFees=ReturnFees=Paid=0;Elapsed=DepartureLeft=0;Reviving=-1;RevivalLeft=0;soloCharges=2;soloHold=downAge=0;
        for(int i=0;i<4;i++){Down[i]=Eliminated[i]=false;Downs[i]=0;Bleedout[i]=Protection[i]=0;}
    }
    public bool Present(int i)=>i>=0&&i<4&&(Mask&(1<<i))!=0;
    public bool CanAct(int i)=>Present(i)&&!Down[i]&&!Eliminated[i];
    public void AddDeliveryReward(int amount){if(Active&&amount>0)Gross+=amount;}
    public void CreditReceipt(int amount){if(Active&&Gross==0)Gross=Math.Max(0,amount);}
    public void Damage(int i,int amount){
        if(!Active||!CanAct(i)||Protection[i]>0||amount<=0)return;
        Health[i]=Math.Max(0,Health[i]-amount);if(Health[i]>0)return;
        Downs[i]++;Down[i]=true;Bleedout[i]=45;
        bool solo=(Mask&(Mask-1))==0;
        if((!solo&&Downs[i]>=2)||(solo&&soloCharges==0))Eliminate(i);
    }
    public void Eliminate(int i){if(!Present(i))return;Down[i]=true;Eliminated[i]=true;Health[i]=0;Bleedout[i]=0;}
    public bool Rescue(int i){if(!Present(i)||!Down[i]||Eliminated[i])return false;Down[i]=false;Health[i]=30;Bleedout[i]=0;Protection[i]=4;return true;}
    public bool Heal(int i){if(!CanAct(i)||Health[i]>=100)return false;Health[i]=100;return true;}
    public bool StartRevival(int actor,int target,int aboardMask){
        if(!Active||!CanAct(actor)||(aboardMask&(1<<actor))==0||!Present(target)||!Eliminated[target]||Reviving>=0||Available<RevivalCost)return false;
        RevivalFees+=RevivalCost;Reviving=target;RevivalLeft=RevivalSeconds;return true;
    }
    public bool ToggleDeparture(int actor,int aboardMask){
        if(!Active||Automatic||!CanAct(actor)||(aboardMask&(1<<actor))==0)return false;
        Departing=!Departing;DepartureLeft=Departing?DepartureSeconds:0;return true;
    }
    public int Tick(float dt,int aboardMask,bool soloRescue=false){
        if(!Active)return -1;dt=Math.Max(0,dt);Elapsed+=dt;int revived=-1;
        for(int i=0;i<4;i++)if(Present(i)){
            Protection[i]=Math.Max(0,Protection[i]-dt);
            if(Down[i]&&!Eliminated[i]){Bleedout[i]-=dt;if(Bleedout[i]<=0)Eliminate(i);}
        }
        if((Mask&(Mask-1))==0){
            for(int i=0;i<4;i++)if(Present(i)&&Down[i]&&!Eliminated[i]){
                float beforeAge=downAge;downAge+=dt;soloHold=soloRescue?soloHold+Math.Max(0,downAge-Math.Max(3,beforeAge)):0;
                if(soloHold>=2&&soloCharges>0){soloCharges--;Rescue(i);soloHold=downAge=0;}
            }
        }
        if(Reviving>=0){RevivalLeft-=dt;if(RevivalLeft<=0){revived=Reviving;Restore(revived);Reviving=-1;RevivalLeft=0;}}
        if(Elapsed>=FinalWarning){Automatic=true;Departing=true;DepartureLeft=Math.Max(0,FinalDeparture-Elapsed);}
        else if(Departing)DepartureLeft=Math.Max(0,DepartureLeft-dt);
        // Boarding is evaluated at liftoff, not when a countdown was requested.
        if(Departing&&DepartureLeft<=0){Finish(aboardMask);return revived;}
        bool living=false;for(int i=0;i<4;i++)if(CanAct(i))living=true;
        bool soloChance=(Mask&(Mask-1))==0&&soloCharges>0;
        if(!living&&Reviving<0){bool waiting=false;if(soloChance)for(int i=0;i<4;i++)if(Present(i)&&Down[i]&&!Eliminated[i])waiting=true;if(!waiting)Fail();}
        return revived;
    }
    public void Restore(int i){Health[i]=100;Downs[i]=0;Down[i]=Eliminated[i]=false;Bleedout[i]=0;Protection[i]=4;}
    public void Finish(int aboardMask){
        if(!Active)return;
        int valid=0;for(int i=0;i<4;i++)if(Present(i)&&!Eliminated[i]&&(aboardMask&(1<<i))!=0)valid|=1<<i;
        if(Reviving>=0)valid|=1<<Reviving;
        if(valid==0){Fail();return;}
        for(int i=0;i<4;i++)if(Present(i)){
            if((valid&(1<<i))==0){ReturnFees+=ReturnCost;Restore(i);}
            else if(i==Reviving)Restore(i);
            else if(Down[i])Rescue(i);
        }
        Paid=Math.Max(0,Available-ReturnFees);Active=false;Departing=false;Reviving=-1;RevivalLeft=0;
    }
    public void Fail(){if(!Active)return;Failed=true;Paid=0;Active=false;Departing=false;Reviving=-1;RevivalLeft=0;for(int i=0;i<4;i++)if(Present(i))Restore(i);}
    public VisitState Snapshot()=>new VisitState{active=Active,departing=Departing,automatic=Automatic,failed=Failed,gross=Gross,available=Available,revivalFees=RevivalFees,returnFees=ReturnFees,paid=Paid,reviving=Reviving,mask=Mask,elapsed=Elapsed,departureLeft=DepartureLeft,revivalLeft=RevivalLeft,health=(int[])Health.Clone(),downs=(int[])Downs.Clone(),down=(bool[])Down.Clone(),eliminated=(bool[])Eliminated.Clone(),bleedout=(float[])Bleedout.Clone(),protection=(float[])Protection.Clone()};
}
}
