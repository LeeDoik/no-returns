using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    float batonWeight,batonElapsed=6,batonGripError,previousBatonCooldown;
    bool batonSwing,batonClamped,swingCancelled;
    public float BatonMotionCooldown=>swingCancelled?0:Mathf.Max(0,6-batonElapsed);
    public void AttackPose(float cooldown,bool allowed){
        batonElapsed=6-Mathf.Clamp(cooldown,0,6);
        if(cooldown>previousBatonCooldown+.3f||batonElapsed>=BatonMotion.Seconds)swingCancelled=false;
        previousBatonCooldown=cooldown;
        if(!allowed&&batonElapsed<BatonMotion.Seconds)swingCancelled=true;
        batonWeight=allowed&&!swingCancelled?Mathf.Max(0,BatonMotion.Amount(cooldown)):0;
        batonSwing=allowed&&!swingCancelled&&cooldown>0&&batonElapsed<BatonMotion.Seconds;
        batonClamped=false;batonGripError=0;if(batonWeight<=0)return;
        var chestBone=animator.GetBoneTransform(HumanBodyBones.Chest);
        if(chestBone)chestBone.rotation=Quaternion.AngleAxis(-4*batonWeight,transform.up)*chestBone.rotation;
        PoseGrip(transform.TransformPoint(new Vector3(.24f,1.16f,.34f)),transform.rotation*Quaternion.Euler(65,-10,5),batonWeight);
    }
    public float RightWristBend=>WristBend(rightArm);
    public bool PoseGrip(Vector3 goal,Quaternion rotation,float weight,bool naturalWrist=false){
        if(!BatonGrip(out var grip,out var heldRotation))return false;
        var turn=rotation*Quaternion.Inverse(heldRotation);
        var wristGoal=goal-turn*(grip-rightArm.wrist.position);
        batonClamped=PoseArm(rightArm,wristGoal,turn*rightArm.wrist.rotation,weight,naturalWrist);
        if(naturalWrist){
            // Limit hand/forearm flexion, then solve the palm contact again.
            // Weapon orientation follows the supported hand instead of folding its wrist.
            for(int pass=0;pass<3;pass++){
                SupportWrist(rightArm);
                if(pass<2){BatonGrip(out var contact,out _);batonClamped|=PoseArm(rightArm,rightArm.wrist.position+goal-contact,rightArm.wrist.rotation,1,true);}
            }
        }
        BatonGrip(out var actual,out _);batonGripError=Vector3.Distance(actual,goal);
        return true;
    }
}
}
