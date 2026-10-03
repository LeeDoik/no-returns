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
        if(quickTest&&hosting&&(name=="beacon"||name=="rescue"||name=="baton"))pendingQuickFixture=name;
#endif
    }
    void ApplyQuickFixture(){
#if CARRY_TEST_AUTOMATION || UNITY_EDITOR
        if(!quickTest||pendingQuickFixture==null||(occupiedMask&3)!=3)return;
        var name=pendingQuickFixture;pendingQuickFixture=null;ClosePanels();ResetRoom();
        mission=new CarryMission(new CarryProgress{beacon=true},cinder:true);mission.Act(true,true);mission.Act(true,true);missionPhase=mission.Phase;
        suppression.Begin();outer.Reset();
        for(int i=0;i<4;i++){
            workers[i].enabled=false;workers[i].transform.position=new Vector3(-22.5f+i*1.5f,.035f,-19);workers[i].enabled=true;fall[i]=0;
            inputs[i]=new CarryInput{distance=.8f};
        }
        threat.PrepareQuickFixture(new Vector3(-22.5f,.035f,name=="baton"?-17:-8),name=="baton",name=="rescue");
        equipment.Apply(new CarryState{beaconExists=name=="beacon",beaconCarrier=-1,beaconPosition=new Vector3(-22.5f,.23f,-18),charges=2});
        danger=threat.Snapshot();quickFixture=name;Physics.SyncTransforms();
#endif
    }
}
}
