using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    float rescueWeight,rescueWristBend,rescueFootError;
    public void RescuePose(float progress,bool blocked,float delta){
        rescueWeight=blocked?0:Mathf.MoveTowards(rescueWeight,progress>0?1:0,Mathf.Max(0,delta)*6);
        rescueWristBend=rescueFootError=0;if(rescueWeight<=0)return;
        var hips=animator.GetBoneTransform(HumanBodyBones.Hips);
        var chest=animator.GetBoneTransform(HumanBodyBones.Chest);
        var left=animator.GetBoneTransform(HumanBodyBones.LeftFoot);var right=animator.GetBoneTransform(HumanBodyBones.RightFoot);
        var lp=left.position;var rp=right.position;var lr=left.rotation;var rr=right.rotation;
        hips.position-=transform.up*(.28f*rescueWeight);
        chest.rotation=Quaternion.AngleAxis(24*rescueWeight,transform.right)*chest.rotation;
        PlantLeg(true,lp,lr);PlantLeg(false,rp,rr);
        rescueFootError=Mathf.Max(Vector3.Distance(lp,left.position),Vector3.Distance(rp,right.position));
        float press=.018f*Mathf.Sin(progress*Mathf.PI*4);
        var leftGoal=transform.position+transform.rotation*new Vector3(-.20f,.72f+press,.43f);
        var rightGoal=transform.position+transform.rotation*new Vector3(.20f,.72f-press,.43f);
        // Blend the goals from the animated palms; wrist support remains capped during transition.
        leftGoal=Vector3.Lerp(Vector3.Lerp(leftArm.wrist.position,leftArm.middle.position,.85f),leftGoal,rescueWeight);
        rightGoal=Vector3.Lerp(Vector3.Lerp(rightArm.wrist.position,rightArm.middle.position,.85f),rightGoal,rescueWeight);
        RescueHands(leftGoal,rightGoal,transform.up,transform.forward);rescueWristBend=RescueWristBend;
    }
}
}
