using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class CarryRoom {
    bool companionPractice,companionBot;
    string companionAction="Idle";
    float companionClock,companionPulse;
    static readonly float[] companionAngles={0,40,-40,80,-80,120,-120};

    void TickCompanionBatons(){
        for(int i=0;i<4;i++){
            danger.cooldown[i]=Mathf.Max(0,danger.cooldown[i]-Time.fixedDeltaTime);
            if(Present(i)&&inputs[i].shove&&holder!=i&&equipment.Carrier!=i&&danger.cooldown[i]<=0)danger.cooldown[i]=6;
            inputs[i].shove=false;inputs[i].call=false;
        }
    }

    CarryInput ReadCompanion(){
        var cmd=new CarryInput{seq=seq,distance=1.1f};
        if(local==0||target==null)return cmd;
        companionClock+=Time.deltaTime;companionPulse-=Time.deltaTime;
        float t=companionClock%32;
        Vector3 here=workers[local].transform.position,host=workers[0].transform.position;
        Vector3 delta=host-here;delta.y=0;
        bool following=delta.magnitude>6;
        Vector3 goal=host+Quaternion.Euler(0,target.yaws[0]+Mathf.Sin(companionClock*.6f)*65,0)*Vector3.forward*2.7f;
        Vector3 look=host+Vector3.up*1.3f;
        companionAction=following?"Following":t<3?"Idle":t<11?"Walking / sidestep":t<16?"Slow walk":t<19?"Jump":t<22?"Baton":t<30?"Carry parcel":"Put down";
        bool moving=following||(t>=3&&t<16);
        cmd.quiet=t>=11&&t<16;
        if(!following&&t>=22&&t<30){
            if(holder==local){moving=true;companionAction="Carry parcel";look=goal+Vector3.up*1.57f;}
            else if(holder<0&&Vector3.Distance(cargo.position,host)<7){
                goal=cargo.position;goal.y=here.y;look=cargo.position;moving=Vector3.Distance(here,goal)>1.1f;
                if(!moving&&companionPulse<=0){cmd.interact=true;companionPulse=.8f;}
            }
        }
        if(holder==local&&(t>=30||t<3)){cmd.drop=true;moving=false;}
        if(!following&&companionPulse<=0){
            if(t>=16&&t<19){cmd.jump=true;companionPulse=1.2f;}
            if(t>=19&&t<22){cmd.shove=true;companionPulse=1;}
        }
        var aim=look-(here+Vector3.up*1.57f);
        yaw=Mathf.Atan2(aim.x,aim.z)*Mathf.Rad2Deg;
        pitch=-Mathf.Atan2(aim.y,new Vector2(aim.x,aim.z).magnitude)*Mathf.Rad2Deg;
        cmd.yaw=yaw;cmd.pitch=pitch;
        if(moving){
            var direction=goal-here;direction.y=0;
            if(direction.magnitude>.5f){
                // ponytail: local steering for nearby demonstrations, not a full maze/vertical navigator.
                Vector3 best=Vector3.zero;float score=float.MaxValue;
                foreach(float angle in companionAngles){
                    var step=Quaternion.Euler(0,angle,0)*direction.normalized;
                    if(!CompanionStep(here,step))continue;
                    float cost=(here+step-goal).sqrMagnitude+Mathf.Abs(angle)*.003f;
                    if(cost<score){score=cost;best=step;}
                }
                var relative=Quaternion.Euler(0,-yaw,0)*best;
                float speed=following?1:holder==local?.6f:cmd.quiet?.28f:.55f;
                cmd.x=relative.x*speed;cmd.z=relative.z*speed;
            }
        }
        return cmd;
    }
    bool CompanionStep(Vector3 here,Vector3 direction){
        foreach(var hit in Physics.CapsuleCastAll(here+Vector3.up*.43f,here+Vector3.up*1.38f,.36f,direction,.65f,~0,QueryTriggerInteraction.Ignore))
            if(hit.collider!=workers[local]&&hit.collider!=cargoCollider)return false;
        foreach(var hit in Physics.RaycastAll(here+direction*.65f+Vector3.up*.4f,Vector3.down,.8f,~0,QueryTriggerInteraction.Ignore))
            if(hit.collider!=cargoCollider&&hit.collider.GetComponentInParent<CharacterController>()==null&&hit.normal.y>.65f)return true;
        return false;
    }
}
}
