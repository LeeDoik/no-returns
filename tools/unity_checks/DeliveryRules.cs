using System;
using UnityEngine;
using NoReturns.CarryLab;
public static class DeliveryRules {
    public static object Run(){
        int checks=0;
        Action<bool,string> require=(ok,label)=>{if(!ok)throw new Exception(label);checks++;};
        foreach(bool cinder in new[]{false,true}){
            var m=new CarryMission(cinder:cinder);
            var pad=new Vector3(-6,.325f,9)+(cinder?CarryMission.CinderReceiptOffset:Vector3.zero);
            m.Act(false,true);require(m.Phase==0,"outside cannot select");
            m.Act(true,false);m.Act(true,false);require(m.Phase==1,"missing crew blocks departure");
            m.Act(true,true);require(m.Phase==2,"all aboard depart");
            m.Tick(pad,Vector3.zero,0,1);require(m.Phase==2,"held cargo rejected");
            m.Tick(pad+Vector3.right*2,Vector3.zero,-1,1);require(m.Phase==2,"off-pad rejected");
            m.Tick(pad,Vector3.right,-1,1);require(m.Phase==2,"moving cargo rejected");
            m.Tick(pad,Vector3.zero,-1,.74f);require(m.Phase==2,"no early acceptance");
            m.Tick(pad,Vector3.zero,-1,.02f);require(m.Phase==3&&m.Credits==0,"stable acceptance without pay");
            require(!m.CollectReceipt(),"printing blocks collection");
            m.Tick(pad,Vector3.zero,-1,.74f);require(!m.ReceiptReady,"no early printed receipt");
            m.Tick(pad,Vector3.zero,-1,.02f);require(m.CollectReceipt()&&!m.CollectReceipt()&&m.Credits==0,"one shared receipt without pay");
            m.Act(false,true);m.Act(true,false);require(m.Phase==3,"return requires aboard and entire crew");
            m.Act(true,true);require(m.Phase==4&&m.Credits==420&&m.Receipt==300&&m.ReturnPay==120&&m.SuccessfulDeliveries==1,"pay once");
            m.Act(true,true);require(m.Phase==0&&m.Credits==420&&!m.ReceiptCollected,"next shift resets receipt, keeps pay");
            m.Act(true,true);m.Act(true,true);m.Act(true,true);require(m.Phase==4&&m.Credits==420&&m.ReturnPay==0,"empty return pays zero");
            m.Act(true,true);m.Act(true,true);m.Act(true,true);m.Abort();require(m.Phase==4&&m.Credits==420&&m.ReturnPay==0,"abort preserves secured balance");
        }
        require(CarryMission.Aboard(new Vector3(-20.7f,1.035f,-26),true),"Cinder ship deck aboard");
        require(!CarryMission.Aboard(new Vector3(-20.7f,.4f,-22.5f),true),"Cinder ramp outside");
        require(CarryMission.Aboard(new Vector3(0,0,-5))&&!CarryMission.Aboard(new Vector3(0,0,-5),true),"legacy boarding retained");
        return new {status="PASS",checks};
    }
}
