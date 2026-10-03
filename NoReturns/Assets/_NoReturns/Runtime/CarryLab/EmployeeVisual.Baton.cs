using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    Transform chest;
    float batonWeight,batonElapsed=6,batonGripError;
    bool batonSwing,batonClamped,swingCancelled;
    float previousBatonCooldown;
    public float BatonMotionCooldown=>swingCancelled?0:Mathf.Max(0,6-batonElapsed);
    public const float BatonMotionSeconds=.55f;
    public static readonly Vector3 BatonReady=new Vector3(.30f,1.21f,.20f);
    public static readonly Quaternion BatonReadyRotation=Quaternion.Euler(15,-10,-15);

    // The existing contact is immediate. Ready stance anticipates it; cooldown owns the swing and return.
    public static void BatonMotion(float cooldown,out Vector3 grip,out Quaternion rotation,out float twist){
        float t=6-Mathf.Clamp(cooldown,0,6);
        grip=BatonReady;rotation=BatonReadyRotation;twist=0;
        if(cooldown<=0||t>=BatonMotionSeconds)return;
        var impact=new Vector3(.20f,1.28f,.48f);var impactRotation=Quaternion.Euler(82,-8,-15);
        var follow=new Vector3(-.04f,1.10f,.40f);var followRotation=Quaternion.Euler(120,-25,25);
        var recover=new Vector3(.29f,1.39f,.12f);var recoverRotation=Quaternion.Euler(-15,10,-20);
        if(t<.12f){float v=Mathf.SmoothStep(0,1,t/.12f);grip=Vector3.Lerp(impact,follow,v);rotation=Quaternion.Slerp(impactRotation,followRotation,v);twist=Mathf.Lerp(-12,-15,v);}
        else if(t<.25f){float v=Mathf.SmoothStep(0,1,(t-.12f)/.13f);grip=Vector3.Lerp(follow,recover,v);rotation=Quaternion.Slerp(followRotation,recoverRotation,v);twist=Mathf.Lerp(-15,6,v);}
        else {float v=Mathf.SmoothStep(0,1,(t-.25f)/.30f);grip=Vector3.Lerp(recover,BatonReady,v);rotation=Quaternion.Slerp(recoverRotation,BatonReadyRotation,v);twist=Mathf.Lerp(6,0,v);}
    }

    public void BatonPose(float cooldown,bool allowed,float delta){
        batonElapsed=6-Mathf.Clamp(cooldown,0,6);
        if(cooldown>previousBatonCooldown+.3f||batonElapsed>=BatonMotionSeconds)swingCancelled=false;
        previousBatonCooldown=cooldown;
        if(!allowed&&batonElapsed<BatonMotionSeconds)swingCancelled=true;
        batonSwing=allowed&&!swingCancelled&&cooldown>0&&batonElapsed<BatonMotionSeconds;
        batonGripError=0;batonClamped=false;
        if(!allowed){batonWeight=0;return;}
        if(rightArm==null)InitializeCarry();
        if(!rightArm.upper||!rightArm.lower||!rightArm.wrist)return;
        batonWeight=Mathf.MoveTowards(batonWeight,1,Mathf.Max(0,delta)*10);
        if(!chest)chest=animator.GetBoneTransform(HumanBodyBones.Chest);
        BatonMotion(BatonMotionCooldown,out var target,out var targetRotation,out var twist);
        if(chest)chest.rotation=Quaternion.AngleAxis(twist*batonWeight,transform.up)*chest.rotation;
        if(!BatonGrip(out var grip,out var gripRotation))return;
        Quaternion desiredRotation=transform.rotation*targetRotation;
        Quaternion turn=desiredRotation*Quaternion.Inverse(gripRotation);
        Vector3 wristGoal=transform.position+transform.rotation*target-turn*(grip-rightArm.wrist.position);
        batonClamped=PoseArm(rightArm,wristGoal,turn*rightArm.wrist.rotation,batonWeight);
        if(BatonGrip(out var actual,out _))batonGripError=Vector3.Distance(actual,transform.position+transform.rotation*target);
    }
}
}
