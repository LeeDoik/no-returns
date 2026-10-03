using UnityEngine;
namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    public const float FallSeconds=.65f,GetUpSeconds=.95f;
    float downAmount,downRoll,downWristBend;
    string downPhase="Standing";
    public void DownPose(bool down,bool present,float delta){
        if(!present){downAmount=0;downPhase="Standing";downRoll=downWristBend=0;return;}
        downAmount=Mathf.MoveTowards(downAmount,down?1:0,Mathf.Max(0,delta)/(down?FallSeconds:GetUpSeconds));
        downPhase=down?(downAmount<1?"Falling":"Down"):(downAmount>0?"GettingUp":"Standing");
        downRoll=downWristBend=0;if(downAmount<=0)return;
        float crouch=Mathf.SmoothStep(0,1,Mathf.Clamp01(downAmount/.4f));
        float roll=Mathf.SmoothStep(0,1,Mathf.InverseLerp(.25f,1,downAmount));
        var hips=animator.GetBoneTransform(HumanBodyBones.Hips);
        var left=animator.GetBoneTransform(HumanBodyBones.LeftFoot);var right=animator.GetBoneTransform(HumanBodyBones.RightFoot);
        var lp=left.position;var rp=right.position;var lr=left.rotation;var rr=right.rotation;
        hips.position-=transform.up*(.32f*crouch);
        PlantLeg(true,lp,lr);PlantLeg(false,rp,rr);
        // Fold the legs and brace the arms before rolling the skeleton onto its side.
        TurnDownBone(HumanBodyBones.LeftUpperLeg,-12*roll);TurnDownBone(HumanBodyBones.RightUpperLeg,-12*roll);
        TurnDownBone(HumanBodyBones.LeftLowerLeg,30*roll);TurnDownBone(HumanBodyBones.RightLowerLeg,30*roll);
        TurnDownBone(HumanBodyBones.Chest,18*crouch);
        var leftGoal=transform.TransformPoint(new Vector3(-.21f,.62f,.34f));
        var rightGoal=transform.TransformPoint(new Vector3(.21f,.62f,.34f));
        for(int i=0;i<fingers.Length;i++)if(fingers[i])fingers[i].localRotation=Quaternion.Slerp(fingers[i].localRotation,openRotations[i],crouch);
        Reach(leftArm,leftGoal,transform.up,transform.forward,crouch,true);
        Reach(rightArm,rightGoal,transform.up,transform.forward,crouch,true);
        downWristBend=RescueWristBend;
        downRoll=85*roll;
        hips.rotation=Quaternion.AngleAxis(downRoll,transform.forward)*hips.rotation;
        hips.position-=transform.up*(.29f*roll);
        // Flat-ground visual clearance only; no collider/root/network displacement.
        float clearance=0;
        Clearance(HumanBodyBones.Head,.21f,ref clearance);
        Clearance(HumanBodyBones.Hips,.25f,ref clearance);
        Clearance(HumanBodyBones.Chest,.26f,ref clearance);
        Clearance(HumanBodyBones.LeftUpperLeg,.20f,ref clearance);Clearance(HumanBodyBones.RightUpperLeg,.20f,ref clearance);
        Clearance(HumanBodyBones.LeftToes,.08f,ref clearance);Clearance(HumanBodyBones.RightToes,.08f,ref clearance);
        Clearance(HumanBodyBones.LeftUpperArm,.15f,ref clearance);Clearance(HumanBodyBones.RightUpperArm,.15f,ref clearance);
        Clearance(HumanBodyBones.LeftLowerArm,.09f,ref clearance);Clearance(HumanBodyBones.RightLowerArm,.09f,ref clearance);
        Clearance(HumanBodyBones.LeftHand,.07f,ref clearance);Clearance(HumanBodyBones.RightHand,.07f,ref clearance);
        Clearance(HumanBodyBones.LeftLowerLeg,.11f,ref clearance);Clearance(HumanBodyBones.RightLowerLeg,.11f,ref clearance);
        Clearance(HumanBodyBones.LeftFoot,.08f,ref clearance);Clearance(HumanBodyBones.RightFoot,.08f,ref clearance);
        BootClearance(left,lr,ref clearance);BootClearance(right,rr,ref clearance);
        hips.position+=transform.up*clearance;
    }
    void BootClearance(Transform foot,Quaternion baseRotation,ref float correction){
        var turn=foot.rotation*Quaternion.Inverse(baseRotation);
        for(int x=-1;x<=1;x+=2)for(int y=-1;y<=1;y+=2)for(int z=0;z<2;z++){
            var offset=transform.right*(x*.18f)+transform.up*(y*.14f)+transform.forward*(z==0?-.16f:.32f);
            float height=Vector3.Dot(foot.position+turn*offset-transform.position,transform.up);
            correction=Mathf.Max(correction,-height);
        }
    }
    void TurnDownBone(HumanBodyBones bone,float angle){var t=animator.GetBoneTransform(bone);t.rotation=Quaternion.AngleAxis(angle,transform.right)*t.rotation;}
    void Clearance(HumanBodyBones bone,float radius,ref float correction){
        var t=animator.GetBoneTransform(bone);if(!t)return;
        float height=Vector3.Dot(t.position-transform.position,transform.up);
        correction=Mathf.Max(correction,radius-height);
    }
}
}
