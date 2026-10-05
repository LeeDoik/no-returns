using System;
using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    bool quickTest;string pendingQuickFixture,quickFixture="";
    void ConfigureQuickTest(string[] args){
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
        quickTest=cinderReview&&hazard&&testFolder!=null&&Array.IndexOf(args,"--quick-test")>=0&&!companionPractice;
#endif
    }
    void RequestQuickFixture(string name){
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
        if(quickTest&&hosting&&(name=="beacon"||name=="rescue"||name=="baton"||name=="visit"||name=="visit-peer"||name=="visit-empty"||name=="inventory"||name=="inventory-field"))pendingQuickFixture=name;
#endif
    }
    void ApplyQuickFixture(){
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
        if(!quickTest||pendingQuickFixture==null||(occupiedMask&3)!=3)return;
        var name=pendingQuickFixture;pendingQuickFixture=null;ClosePanels();ResetRoom();
        mission=new CarryMission(new CarryProgress{beacon=true,credits=name=="inventory-field"?mission.Credits:name=="inventory"?400:250},cinder:true);if(name!="inventory"){mission.Act(true,true);mission.Act(true,true);}missionPhase=mission.Phase;EnsureParcels(1,true);
        suppression.Begin();outer.Reset();
        for(int i=0;i<4;i++){
            workers[i].enabled=false;workers[i].transform.position=new Vector3(-22.5f+i*1.5f,.035f,-19);workers[i].enabled=true;fall[i]=0;
            inputs[i]=new CarryInput{distance=.8f};
        }
        threat.PrepareQuickFixture(new Vector3(-22.5f,.035f,name=="baton"?-17:-8),name=="baton",name=="rescue");
        equipment.Apply(new CarryState{beaconExists=name=="beacon",beaconCarrier=-1,beaconPosition=new Vector3(-22.5f,.23f,-18),charges=2});
        mission.BeginVisit(occupiedMask);threat.AttachVisit(mission.Visit);outer.AttachVisit(mission.Visit);if(name=="rescue"){mission.Visit.Damage(1,100);}
        if(name.StartsWith("visit",StringComparison.Ordinal)){
            int actor=name=="visit-peer"?1:0,dead=1-actor;MoveAboard(actor);mission.Visit.Eliminate(dead);
            if(name!="visit-empty"){
                mission.Tick(new Vector3(-6,.35f,9)+CarryMission.CinderReceiptOffset,Vector3.zero,-1,1);mission.Tick(Vector3.zero,Vector3.zero,-1,1);mission.CollectReceipt();
            }
        }
        if(name=="inventory"||name=="inventory-field"){mission.Visit.Damage(0,60);mission.Visit.Damage(1,60);if(name=="inventory"){inventory=new CarryInventory();MoveAboard(0);MoveAboard(1);}}
        danger=threat.Snapshot();quickFixture=name;Physics.SyncTransforms();
#endif
    }
}
}
