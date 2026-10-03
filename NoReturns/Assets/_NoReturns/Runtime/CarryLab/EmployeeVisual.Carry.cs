using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual {
    // Presentation only. The host-owned parcel collider remains the source of contact targets.
    sealed class Arm {
        public Transform upper,lower,wrist,middle,index,little;
        public float side;
    }
    Arm leftArm,rightArm;
    float carryWeight,leftContactError,rightContactError;
    bool carryClamped;
    void InitializeCarry(){leftArm=ArmBones(true);rightArm=ArmBones(false);}
    Arm ArmBones(bool left)=>new Arm{
        upper=animator.GetBoneTransform(left?HumanBodyBones.LeftUpperArm:HumanBodyBones.RightUpperArm),
        lower=animator.GetBoneTransform(left?HumanBodyBones.LeftLowerArm:HumanBodyBones.RightLowerArm),
        wrist=animator.GetBoneTransform(left?HumanBodyBones.LeftHand:HumanBodyBones.RightHand),
        middle=animator.GetBoneTransform(left?HumanBodyBones.LeftMiddleProximal:HumanBodyBones.RightMiddleProximal),
        index=animator.GetBoneTransform(left?HumanBodyBones.LeftIndexProximal:HumanBodyBones.RightIndexProximal),
        little=animator.GetBoneTransform(left?HumanBodyBones.LeftLittleProximal:HumanBodyBones.RightLittleProximal),side=left?-1:1};

    public void CarryPose(BoxCollider parcel,bool carrying,bool down,float delta){
        if(leftArm==null)InitializeCarry();
        carryWeight=down?0:Mathf.MoveTowards(carryWeight,carrying?1:0,Mathf.Max(0,delta)*6);
        leftContactError=rightContactError=0;carryClamped=false;
        if(!parcel||carryWeight<=0)return;
        // Choose the box side facing the employee, even while the parcel is rotated.
        var local=parcel.transform.InverseTransformPoint(transform.position+Vector3.up*1.3f)-parcel.center;
        var half=parcel.size*.5f;
        bool xFace=Mathf.Abs(local.x)/half.x>Mathf.Abs(local.z)/half.z;
        Vector3 normal=xFace?Vector3.right*Mathf.Sign(local.x):Vector3.forward*Mathf.Sign(local.z);
        Vector3 tangent=Vector3.Cross(Vector3.up,normal);
        if(Vector3.Dot(parcel.transform.TransformDirection(tangent),transform.right)<0)tangent=-tangent;
        float extent=xFace?half.x:half.z,span=Mathf.Min(.23f,(xFace?half.z:half.x)*.75f);
        Vector3 center=parcel.center+normal*(extent+.012f)+Vector3.up*(half.y*.68f);
        Vector3 worldNormal=parcel.transform.TransformDirection(normal),along=-parcel.transform.up;
        leftContactError=Reach(leftArm,parcel.transform.TransformPoint(center-tangent*span),worldNormal,along);
        rightContactError=Reach(rightArm,parcel.transform.TransformPoint(center+tangent*span),worldNormal,along);
    }

    float Reach(Arm arm,Vector3 contact,Vector3 normal,Vector3 along){
        if(!arm.upper||!arm.lower||!arm.wrist||!arm.middle||!arm.index||!arm.little)return 0;
        Vector3 fingers=(arm.middle.position-arm.wrist.position).normalized;
        Vector3 palm=Vector3.Cross(fingers,(arm.index.position-arm.little.position).normalized)*arm.side;
        Quaternion turn=Quaternion.LookRotation(along,-normal)*Quaternion.Inverse(Quaternion.LookRotation(fingers,palm));
        Quaternion handRotation=turn*arm.wrist.rotation;
        Vector3 palmOffset=(arm.middle.position-arm.wrist.position)*.85f;
        Vector3 goal=contact-turn*palmOffset;
        carryClamped|=PoseArm(arm,goal,handRotation,carryWeight);
        return Vector3.Distance(Vector3.Lerp(arm.wrist.position,arm.middle.position,.85f),contact);
    }

    bool PoseArm(Arm arm,Vector3 goal,Quaternion handRotation,float weight){
        Quaternion upperBase=arm.upper.localRotation,lowerBase=arm.lower.localRotation,handBase=arm.wrist.localRotation;
        Vector3 origin=arm.upper.position,direction=goal-origin;
        float upperLength=Vector3.Distance(origin,arm.lower.position),lowerLength=Vector3.Distance(arm.lower.position,arm.wrist.position);
        float reach=Mathf.Clamp(direction.magnitude,Mathf.Abs(upperLength-lowerLength)+.001f,upperLength+lowerLength-.005f);
        bool clamped=direction.magnitude>upperLength+lowerLength-.005f;
        direction=direction.sqrMagnitude>.000001f?direction.normalized:transform.forward;
        // Bend elbows down and slightly away from the torso; never stretch bone positions/scales.
        Vector3 bend=Vector3.ProjectOnPlane(-transform.up+transform.right*(arm.side*.35f),direction).normalized;
        if(bend.sqrMagnitude<.001f)bend=Vector3.ProjectOnPlane(transform.forward,direction).normalized;
        float alongArm=(upperLength*upperLength-lowerLength*lowerLength+reach*reach)/(2*reach);
        Vector3 elbow=origin+direction*alongArm+bend*Mathf.Sqrt(Mathf.Max(0,upperLength*upperLength-alongArm*alongArm));
        arm.upper.rotation=Quaternion.FromToRotation(arm.lower.position-origin,elbow-origin)*arm.upper.rotation;
        arm.lower.rotation=Quaternion.FromToRotation(arm.wrist.position-arm.lower.position,origin+direction*reach-arm.lower.position)*arm.lower.rotation;
        arm.wrist.rotation=handRotation;
        arm.upper.localRotation=Quaternion.Slerp(upperBase,arm.upper.localRotation,weight);
        arm.lower.localRotation=Quaternion.Slerp(lowerBase,arm.lower.localRotation,weight);
        arm.wrist.localRotation=Quaternion.Slerp(handBase,arm.wrist.localRotation,weight);
        return clamped;
    }
}
}
