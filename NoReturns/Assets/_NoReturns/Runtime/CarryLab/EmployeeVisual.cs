using UnityEngine;

namespace NoReturns.CarryLab {
public sealed class EmployeeVisual : MonoBehaviour {
    Animator animator;Vector3 previous;bool sampled;float speed;
    public bool Ready=>animator&&animator.avatar&&animator.avatar.isValid&&animator.isHuman;
    public float Speed=>speed;
    public bool Walking=>animator&&animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    [System.Serializable] public class State {public bool ready,walking,visible,rootMotion;public float speed;public Quaternion leftFoot;}
    public State Capture()=>new State{ready=Ready,walking=Walking,visible=GetComponentInChildren<SkinnedMeshRenderer>().enabled,
        rootMotion=animator.applyRootMotion,speed=speed,leftFoot=animator.GetBoneTransform(HumanBodyBones.LeftFoot).localRotation};
    void Awake(){animator=GetComponent<Animator>();animator.applyRootMotion=false;}
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
