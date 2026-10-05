using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.EditorTools {
public static class DeliveryRulesReview {
    [Serializable] public class Result { public string status="PASS"; public List<string> checks=new List<string>(); }
    public static object Run(){
        var result=new Result();
        void Check(bool ok,string name){if(!ok)throw new Exception(name);result.checks.Add(name);}
        for(int crew=1;crew<=4;crew++)Check(new CarryDeliveries(crew).Entries.Length==crew+2,"crew parcel count "+crew);
        var d=new CarryDeliveries(2);var west=CarryDeliveries.Receivers[1]+Vector3.up*.35f;
        d.Tick(0,CarryDeliveries.Receivers[0]+Vector3.up*.35f,Vector3.zero,-1,true,10);Check(d.Entries[0].progress==0,"wrong address rejected");
        d.Tick(0,west,Vector3.zero,0,true,10);Check(d.Entries[0].progress==0,"held parcel rejected");
        d.Tick(0,west,Vector3.one,-1,true,10);Check(d.Entries[0].progress==0,"moving parcel rejected");
        d.Tick(0,west,Vector3.zero,-1,false,10);Check(d.Entries[0].progress==0,"outside visit rejected");
        d.Tick(0,west,Vector3.zero,-1,true,10);Check(d.Entries[0].progress==10&&d.Entries[0].scanning,"verification starts");
        d.Tick(1,west,Vector3.zero,-1,true,10);Check(d.Entries[1].progress==0,"one scanner per receiver");
        d.Tick(0,west,Vector3.zero,0,true,10);Check(d.Entries[0].progress==10&&!d.Entries[0].scanning,"pickup pauses without losing progress");
        d.Tick(1,west,Vector3.zero,-1,true,5);Check(d.Entries[1].progress==5,"paused parcel does not reserve scanner");
        d.Tick(0,west,Vector3.zero,-1,true,15);Check(d.Entries[0].verified&&!d.Entries[0].scanning,"resume completes once");
        Check(d.Collect(0)==0,"wrong terminal cannot collect");Check(d.Collect(1)==100&&d.Completed==1,"first receipt base reward");Check(d.Collect(1)==0,"duplicate receipt rejected");
        d.Tick(1,west,Vector3.zero,-1,true,20);Check(d.Collect(1)==175&&d.Completed==2,"last district receipt includes 55 bonus");Check(d.Collect(1)==0,"bonus cannot repeat");
        d.Tick(2,CarryDeliveries.Receivers[0]+Vector3.up*.35f,Vector3.zero,-1,true,25);d.Tick(3,CarryDeliveries.Receivers[2]+Vector3.up*.35f,Vector3.zero,-1,true,25);Check(d.Entries[2].verified&&d.Entries[3].verified,"independent districts verify concurrently");
        Check(d.Collect(0)==180&&d.Collect(2)==160,"single-contract districts receive no bundle");
        var snapshot=d.Snapshot();snapshot[0].reward=999;Check(d.Entries[0].reward==100,"snapshots cannot mutate ledger");
        var m=new CarryMission(new CarryProgress{credits=250},true,true);m.SetCrew(2);m.Act(true,true);m.Act(true,true);m.BeginVisit(3);m.SetCrew(4);Check(m.Deliveries.Entries.Length==4,"manifest locked after landing");
        for(int i=0;i<2;i++){m.Deliveries.Tick(i,west,Vector3.zero,-1,true,25);Check(m.CollectDelivery(1),"mission receipt "+i);}
        Check(m.Credits==250&&m.Visit.Gross==275,"rewards remain unbanked");m.Visit.Finish(3);m.CompleteVisit();m.CompleteVisit();Check(m.Credits==525&&m.SuccessfulDeliveries==2,"partial departure no abandoned parcel penalty or duplicate payout");
        m.Act(true,true);m.SetCrew(4);Check(m.Deliveries.Entries.Length==6&&m.Deliveries.Completed==0,"next visit resets manifest");
        string path=Path.GetFullPath("../artifacts/delivery-rules.json");Directory.CreateDirectory(Path.GetDirectoryName(path));File.WriteAllText(path,JsonUtility.ToJson(result,true));return result;
    }
}
}
