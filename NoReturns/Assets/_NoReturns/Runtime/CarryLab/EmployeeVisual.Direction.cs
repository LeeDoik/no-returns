using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    Vector3 localVelocity;
    float directionWeight;
    string direction="Idle";
    readonly Vector3[] neutralFeet=new Vector3[2];
    readonly Quaternion[] neutralFootRotations=new Quaternion[2];
    void InitializeDirectionalFeet(){
        for(int i=0;i<2;i++){
            var foot=animator.GetBoneTransform(i==0?HumanBodyBones.LeftFoot:HumanBodyBones.RightFoot);
            neutralFeet[i]=transform.InverseTransformPoint(foot.position);
            neutralFootRotations[i]=Quaternion.Inverse(transform.rotation)*foot.rotation;
        }
    }
    void UpdateDirection(Vector3 worldVelocity,bool down,float delta){
        var measured=down?Vector3.zero:transform.InverseTransformDirection(worldVelocity);
        localVelocity=Vector3.Lerp(localVelocity,measured,1-Mathf.Exp(-12*Mathf.Max(0,delta)));
        direction=localVelocity.magnitude<.12f?"Idle":Mathf.Abs(localVelocity.x)>Mathf.Abs(localVelocity.z)*1.2f?
            (localVelocity.x<0?"StrafeLeft":"StrafeRight"):localVelocity.z<0?"Backward":"Forward";
    }
    // Redirect the imported Walk's alternating foot trajectory, keeping torso facing and bone lengths.
    public void DirectionPose(bool grounded){
        directionWeight=grounded&&localVelocity.magnitude>.12f?Mathf.Clamp01((.85f-localVelocity.normalized.z)/.35f):0;
        if(directionWeight<=0)return;
        var travel=localVelocity.normalized;
        for(int i=0;i<2;i++){
            var foot=animator.GetBoneTransform(i==0?HumanBodyBones.LeftFoot:HumanBodyBones.RightFoot);
            var animated=transform.InverseTransformPoint(foot.position);
            var rest=neutralFeet[i];
            float stride=animated.z-rest.z;
            var goal=new Vector3(rest.x+stride*travel.x*.4f,animated.y,rest.z+stride*travel.z*.65f);
            // Keep separate lanes during lateral steps, including diagonal direction changes.
            goal.x=i==0?Mathf.Min(goal.x,-.045f):Mathf.Max(goal.x,.045f);
            var rotation=Quaternion.Slerp(foot.rotation,transform.rotation*neutralFootRotations[i],directionWeight*.8f);
            PlantLeg(i==0,transform.TransformPoint(Vector3.Lerp(animated,goal,directionWeight)),rotation);
        }
    }
}
}
