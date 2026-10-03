using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    float airWeight,landingWeight,airSeconds,landingElapsed=1,verticalSpeed;
    bool wasAirborne;
    string locomotion="Grounded";
    public const float LandingSeconds=.22f;
    void ResetLocomotion(){airWeight=landingWeight=airSeconds=0;landingElapsed=1;wasAirborne=false;locomotion="Grounded";}

    public void LocomotionPose(bool grounded,float vertical,bool down,float delta) {
        if(down){ResetLocomotion();return;}
        bool airborne=!grounded||vertical>1;
        if(airborne){airSeconds+=delta;landingElapsed=1;}
        else {
            if(wasAirborne&&airSeconds>.12f)landingElapsed=0;
            airSeconds=0;landingElapsed+=delta;
        }
        wasAirborne=airborne;
        airWeight=Mathf.MoveTowards(airWeight,airborne?1:0,Mathf.Max(0,delta)*12);
        landingWeight=landingElapsed<LandingSeconds?Mathf.Sin(Mathf.PI*landingElapsed/LandingSeconds):0;
        locomotion=airborne?(vertical>.3f?"Rising":vertical<-.3f?"Falling":"Apex"):landingWeight>0?"Landing":"Grounded";
        var hips=animator.GetBoneTransform(HumanBodyBones.Hips);
        var left=animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        var right=animator.GetBoneTransform(HumanBodyBones.RightFoot);
        if(landingWeight>0) {
            // Compress the visual hips while keeping the current animated soles planted.
            var leftGoal=left.position;var rightGoal=right.position;
            var leftRotation=left.rotation;var rightRotation=right.rotation;
            hips.position-=transform.up*(.065f*landingWeight);
            PlantLeg(true,leftGoal,leftRotation);PlantLeg(false,rightGoal,rightRotation);
        }
        if(airWeight>0 && airborne) {
            float tuck=vertical<-.3f?1:.7f;
            for(int leg=0;leg<2;leg++) {
                bool side=leg==0;
                var upper=animator.GetBoneTransform(side?HumanBodyBones.LeftUpperLeg:HumanBodyBones.RightUpperLeg);
                var lower=animator.GetBoneTransform(side?HumanBodyBones.LeftLowerLeg:HumanBodyBones.RightLowerLeg);
                var foot=side?left:right;
                upper.rotation=Quaternion.AngleAxis(-18*tuck*airWeight,transform.right)*upper.rotation;
                lower.rotation=Quaternion.AngleAxis(40*tuck*airWeight,transform.right)*lower.rotation;
                foot.rotation=Quaternion.AngleAxis(-15*tuck*airWeight,transform.right)*foot.rotation;
            }
        }
    }
    void PlantLeg(bool side,Vector3 goal,Quaternion rotation) {
        var upper=animator.GetBoneTransform(side?HumanBodyBones.LeftUpperLeg:HumanBodyBones.RightUpperLeg);
        var lower=animator.GetBoneTransform(side?HumanBodyBones.LeftLowerLeg:HumanBodyBones.RightLowerLeg);
        var foot=animator.GetBoneTransform(side?HumanBodyBones.LeftFoot:HumanBodyBones.RightFoot);
        var origin=upper.position;var toward=goal-origin;
        float a=Vector3.Distance(origin,lower.position),b=Vector3.Distance(lower.position,foot.position);
        float reach=Mathf.Clamp(toward.magnitude,Mathf.Abs(a-b)+.001f,a+b-.001f);
        var direction=toward.normalized;var bend=Vector3.ProjectOnPlane(transform.forward,direction).normalized;
        float along=(a*a-b*b+reach*reach)/(2*reach);
        var knee=origin+direction*along+bend*Mathf.Sqrt(Mathf.Max(0,a*a-along*along));
        upper.rotation=Quaternion.FromToRotation(lower.position-origin,knee-origin)*upper.rotation;
        lower.rotation=Quaternion.FromToRotation(foot.position-lower.position,origin+direction*reach-lower.position)*lower.rotation;
        foot.rotation=rotation;
    }
    public void GroundLocomotion(bool down,float delta) {
        bool grounded=false;
        foreach(var hit in Physics.RaycastAll(transform.position+Vector3.up*.2f,Vector3.down,.30f,~0,QueryTriggerInteraction.Ignore)) {
            if(hit.collider.GetComponent<CharacterController>()||hit.collider.attachedRigidbody)continue;
            if(hit.normal.y>.5f){grounded=true;break;}
        }
        LocomotionPose(grounded,verticalSpeed,down,delta);
    }
}
}
