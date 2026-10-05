using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.EditorTools {
public static class VisitRulesReview {
    [Serializable] class Result {public string status="PASS";public List<string> checks=new List<string>();}
    public static object Run(){
        var r=new Result();
        void Check(bool ok,string name){if(!ok)throw new Exception(name);r.checks.Add(name);}
        CarryVisit New(int mask=3,int pay=420){var v=new CarryVisit();v.Begin(mask);v.CreditReceipt(pay);return v;}
        var v=New();v.CreditReceipt(420);Check(v.Gross==420,"receipt idempotence");
        v.Damage(1,60);Check(v.Health[1]==40&&!v.Down[1],"first hit survived");v.Damage(1,60);Check(v.Down[1]&&!v.Eliminated[1],"first down rescuable");
        v.Rescue(1);Check(v.Health[1]==30&&v.Downs[1]==1,"rescue low health retains downs");v.Damage(1,100);Check(!v.Down[1],"rescue protection");v.Tick(4,1);v.Damage(1,60);Check(v.Eliminated[1],"second down eliminates");
        Check(!v.StartRevival(1,1,3)&&!v.StartRevival(0,1,0),"dead and outside actors denied");
        Check(v.StartRevival(0,1,1)&&v.Available==320&&v.RevivalFees==100,"reserve fee once");Check(!v.StartRevival(0,1,1),"single chamber rejects duplicate");
        Check(v.Tick(10,1)==1&&v.Health[1]==100&&v.Downs[1]==0,"full ship revival and reset");
        Check(v.ToggleDeparture(1,2)&&v.Departing,"peer starts departure");Check(v.ToggleDeparture(0,1)&&!v.Departing,"other peer cancels");v.ToggleDeparture(0,1);v.Tick(12,1);Check(!v.Active&&v.Paid==220&&v.ReturnFees==100,"outside teammate fee at liftoff");v.Finish(3);Check(v.Paid==220,"settlement idempotence");
        v=New(3,50);v.Eliminate(1);Check(!v.StartRevival(0,1,1)&&v.Available==50,"insufficient unbanked funds");v.Finish(1);Check(v.Paid==0,"return fee zero floor");
        v=New(15);for(int i=1;i<4;i++)v.Eliminate(i);v.StartRevival(0,1,1);v.Eliminate(0);v.Tick(5,0);Check(v.Active&&v.Reviving==1,"pending revival survives all eliminated");v.Finish(0);Check(!v.Failed&&v.Paid==20&&v.Health[1]==100,"pending revival counts aboard and return fees apply");
        v=New();v.Finish(0);Check(v.Failed&&v.Paid==0,"empty ship loses gross");
        v=New();v.Damage(0,100);v.Damage(1,100);v.Tick(.02f,0);Check(v.Failed,"all down without pending revival fails");
        v=New();v.Damage(1,100);v.Tick(45,1);Check(v.Eliminated[1],"bleedout eliminates");
        v=New();v.Eliminate(1);v.StartRevival(0,1,1);v.Eliminate(0);Check(v.Tick(10,0)==1&&v.Active,"last survivor replaced by completed revival");
        v=New();v.Tick(CarryVisit.FinalWarning,3);Check(v.Automatic&&v.Departing&&!v.ToggleDeparture(0,1),"automatic departure cannot cancel");v.Tick(60,3);Check(v.Paid==420,"automatic deadline settlement");
        v=New();v.Damage(1,100);v.Finish(3);Check(!v.Failed&&v.ReturnFees==0&&v.Health[1]==30,"downed aboard counted and rescued");
        v=New(1);for(int n=0;n<2;n++){v.Damage(0,100);v.Tick(3,0,true);Check(v.Down[0],"solo windup "+n);v.Tick(2,0,true);Check(!v.Down[0]&&v.Health[0]==30,"solo revive "+n);v.Tick(4,0);}v.Damage(0,100);v.Tick(.02f,0);Check(v.Failed,"solo charges exhausted");
        var m=new CarryMission(new CarryProgress{credits=250},true);m.Act(true,true);m.Act(true,true);m.BeginVisit(3);m.Tick(new Vector3(-6,.35f,9)+CarryMission.CinderReceiptOffset,Vector3.zero,-1,1);m.Tick(Vector3.zero,Vector3.zero,-1,1);Check(m.CollectReceipt()&&!m.CollectReceipt()&&m.Credits==250&&m.Visit.Available==420,"actual receipt leaves wallet untouched");m.Visit.Eliminate(1);m.Visit.StartRevival(0,1,1);m.Visit.Finish(1);m.CompleteVisit();m.CompleteVisit();Check(m.Credits==570&&m.Phase==4,"actual wallet settlement exactly once including pending revival");
        string path=Path.GetFullPath("../artifacts/visit-rules.json");Directory.CreateDirectory(Path.GetDirectoryName(path));File.WriteAllText(path,JsonUtility.ToJson(r,true));return r;
    }
}
}
