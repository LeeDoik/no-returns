using UnityEngine;

namespace NoReturns.CarryLab {
public sealed partial class EmployeeVisual : MonoBehaviour {
    Animator animator;Vector3 previous;bool sampled;float speed;
    Transform hand,index,middle,little;
    readonly Transform[] fingers=new Transform[15];readonly Quaternion[] gripRotations=new Quaternion[15],openRotations=new Quaternion[15];
    public bool Ready=>animator&&animator.avatar&&animator.avatar.isValid&&animator.isHuman;
    public float Speed=>speed;
    public Vector3 RightHandPosition=>hand?hand.position:transform.position;
    public bool Walking=>animator&&animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    [System.Serializable] public class State {public bool ready,walking,visible,rootMotion,beaconCarrying,carryClamped,batonSwing,batonClamped;public float downAmount,downRoll,downWristBend;public string downPhase;public float rescueWeight,rescueWristBend,rescueFootError,beaconContactError,beaconWristBend,speed,carryWeight,leftContactError,rightContactError,batonWeight,batonElapsed,batonGripError,airWeight,landingWeight,verticalSpeed;public string locomotion,direction;public Vector3 localVelocity,leftFootPosition,rightFootPosition;public float directionWeight;public Quaternion leftFoot,rightUpperArm,chest,leftKnee;public Vector3 rightHand;}
    public State Capture()=>new State{ready=Ready,walking=Walking,visible=GetComponentInChildren<SkinnedMeshRenderer>().enabled,
        downAmount=downAmount,downRoll=downRoll,downWristBend=downWristBend,downPhase=downPhase,rootMotion=animator.applyRootMotion,speed=speed,leftFoot=animator.GetBoneTransform(HumanBodyBones.LeftFoot).localRotation,
        rescueWeight=rescueWeight,rescueWristBend=rescueWristBend,rescueFootError=rescueFootError,beaconCarrying=beaconCarrying,beaconContactError=beaconContactError,beaconWristBend=beaconWristBend,carryWeight=carryWeight,carryClamped=carryClamped,leftContactError=leftContactError,rightContactError=rightContactError,
        rightHand=hand.position,batonWeight=batonWeight,batonElapsed=batonElapsed,batonSwing=batonSwing,batonClamped=batonClamped,batonGripError=batonGripError,
        airWeight=airWeight,landingWeight=landingWeight,verticalSpeed=verticalSpeed,locomotion=locomotion,direction=direction,localVelocity=localVelocity,directionWeight=directionWeight,
        leftFootPosition=transform.InverseTransformPoint(animator.GetBoneTransform(HumanBodyBones.LeftFoot).position),rightFootPosition=transform.InverseTransformPoint(animator.GetBoneTransform(HumanBodyBones.RightFoot).position),
        leftKnee=animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).localRotation,
        rightUpperArm=rightArm.upper.localRotation,chest=animator.GetBoneTransform(HumanBodyBones.Chest).localRotation};
    void Awake(){animator=GetComponent<Animator>();animator.applyRootMotion=false;
        hand=animator.GetBoneTransform(HumanBodyBones.RightHand);index=animator.GetBoneTransform(HumanBodyBones.RightIndexProximal);
        middle=animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);little=animator.GetBoneTransform(HumanBodyBones.RightLittleProximal);
        var curlAxis=(little.position-index.position).normalized;
        for(int i=0;i<fingers.Length;i++){var bone=animator.GetBoneTransform((HumanBodyBones)((int)HumanBodyBones.RightThumbProximal+i));fingers[i]=bone;
            if(bone)openRotations[i]=bone.localRotation;
            if(bone)gripRotations[i]=bone.localRotation*Quaternion.AngleAxis(i<3?30:55,bone.InverseTransformDirection(curlAxis));}
        InitializeCarry();InitializeDirectionalFeet();}
    public bool BatonGrip(out Vector3 position,out Quaternion rotation) {
        position=Vector3.zero;rotation=Quaternion.identity;if(!hand||!index||!middle||!little)return false;
        var across=(index.position-little.position).normalized;
        var palm=Vector3.Cross((middle.position-hand.position).normalized,across).normalized;
        // World-space metres avoid the imported FBX's 100x bone scale.
        for(int i=0;i<fingers.Length;i++)if(fingers[i])fingers[i].localRotation=gripRotations[i];
        position=Vector3.Lerp(hand.position,middle.position,.95f)+palm*.025f;
        rotation=Quaternion.LookRotation(-palm,across);return true;
    }
    void OnEnable(){downAmount=0;downPhase="Standing";sampled=false;speed=0;ResetLocomotion();}
    public void Team(Color color) {
        var block=new MaterialPropertyBlock();block.SetColor("_BaseColor",Color.Lerp(Color.white,color,.32f));
        foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())renderer.SetPropertyBlock(block);
    }
    public void Animate(Vector3 position,bool down,float delta) {
        Vector3 change=position-previous;
        verticalSpeed=sampled&&delta>0&&change.magnitude<2?change.y/delta:0;
        if(change.magnitude>=2)ResetLocomotion();
        change.y=0;
        float measured=sampled&&delta>0&&change.magnitude<2?change.magnitude/delta:0;
        previous=position;sampled=true;
        speed=down?0:Mathf.Lerp(speed,Mathf.Min(measured,8),1-Mathf.Exp(-12*delta));
        animator.SetFloat("Speed",airWeight>.5f?0:speed);
        UpdateDirection(measured>0?change/delta:Vector3.zero,down,delta);
        animator.SetFloat("StrideRate",Mathf.Clamp(speed/2.2f,.35f,2.2f));
        // Evaluate a fresh base pose even while down; procedural bone rotations must not accumulate.
        animator.speed=1;
    }
}
}
