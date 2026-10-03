using UnityEngine;

namespace NoReturns.CarryLab {
public sealed class EmployeeVisual : MonoBehaviour {
    Animator animator;Vector3 previous;bool sampled;float speed;
    Transform hand,index,middle,little;
    readonly Transform[] fingers=new Transform[15];readonly Quaternion[] gripRotations=new Quaternion[15];
    public bool Ready=>animator&&animator.avatar&&animator.avatar.isValid&&animator.isHuman;
    public float Speed=>speed;
    public Vector3 RightHandPosition=>hand?hand.position:transform.position;
    public bool Walking=>animator&&animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    [System.Serializable] public class State {public bool ready,walking,visible,rootMotion;public float speed;public Quaternion leftFoot;}
    public State Capture()=>new State{ready=Ready,walking=Walking,visible=GetComponentInChildren<SkinnedMeshRenderer>().enabled,
        rootMotion=animator.applyRootMotion,speed=speed,leftFoot=animator.GetBoneTransform(HumanBodyBones.LeftFoot).localRotation};
    void Awake(){animator=GetComponent<Animator>();animator.applyRootMotion=false;
        hand=animator.GetBoneTransform(HumanBodyBones.RightHand);index=animator.GetBoneTransform(HumanBodyBones.RightIndexProximal);
        middle=animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal);little=animator.GetBoneTransform(HumanBodyBones.RightLittleProximal);
        var curlAxis=(little.position-index.position).normalized;
        for(int i=0;i<fingers.Length;i++){var bone=animator.GetBoneTransform((HumanBodyBones)((int)HumanBodyBones.RightThumbProximal+i));fingers[i]=bone;
            if(bone)gripRotations[i]=bone.localRotation*Quaternion.AngleAxis(i<3?30:55,bone.InverseTransformDirection(curlAxis));}}
    public bool BatonGrip(out Vector3 position,out Quaternion rotation) {
        position=Vector3.zero;rotation=Quaternion.identity;if(!hand||!index||!middle||!little)return false;
        var across=(index.position-little.position).normalized;
        var palm=Vector3.Cross((middle.position-hand.position).normalized,across).normalized;
        // World-space metres avoid the imported FBX's 100x bone scale.
        for(int i=0;i<fingers.Length;i++)if(fingers[i])fingers[i].localRotation=gripRotations[i];
        position=Vector3.Lerp(hand.position,middle.position,.95f)+palm*.025f;
        rotation=Quaternion.LookRotation(-palm,across);return true;
    }
    void OnEnable(){sampled=false;speed=0;}
    public void Team(Color color) {
        var block=new MaterialPropertyBlock();block.SetColor("_BaseColor",Color.Lerp(Color.white,color,.32f));
        foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>())renderer.SetPropertyBlock(block);
    }
    public void Animate(Vector3 position,bool down,float delta) {
        Vector3 change=position-previous;change.y=0;
        float measured=sampled&&delta>0&&change.magnitude<2?change.magnitude/delta:0;
        previous=position;sampled=true;
        speed=down?0:Mathf.Lerp(speed,Mathf.Min(measured,8),1-Mathf.Exp(-12*delta));
        animator.SetFloat("Speed",speed);
        // ponytail: reuse one forward walk; add side/back clips when those motions are supplied.
        animator.SetFloat("StrideRate",Mathf.Clamp(speed/2.2f,.35f,2.2f));
        animator.speed=down?0:1;
    }
}
}
