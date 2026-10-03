using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    Transform chest;
    float batonWeight,batonElapsed=6,batonGripError;
    bool batonSwing,batonClamped,swingCancelled;
    float previousBatonCooldown;
    public float BatonMotionCooldown=>swingCancelled?0:Mathf.Max(0,6-batonElapsed);
    public const float BatonMotionSeconds=.30f;
    public static readonly Vector3 BatonReady=new Vector3(.30f,1.21f,.20f);
    public static readonly Quaternion BatonReadyRotation=Quaternion.Euler(15,-10,-15);

    // Keep the earlier upright grip; send one short forward pulse without a lateral swing.
    public static void BatonMotion(float cooldown,out Vector3 grip,out Quaternion rotation,out float twist){
        float t=6-Mathf.Clamp(cooldown,0,6);
        grip=BatonReady;rotation=BatonReadyRotation;twist=0;
        if(cooldown<=0||t>=BatonMotionSeconds)return;
        float extension;
        if(t<.06f)extension=1-Mathf.Pow(1-t/.06f,3);
        else if(t<.10f)extension=1;
        else extension=1-Mathf.SmoothStep(0,1,(t-.10f)/.20f);
        grip=BatonReady+Vector3.forward*(.20f*extension);
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
