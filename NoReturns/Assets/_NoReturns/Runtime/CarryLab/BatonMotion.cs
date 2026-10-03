using UnityEngine;
namespace NoReturns.CarryLab {
// Same host cooldown on every peer; short anticipation, snap, follow-through and return.
public static class BatonMotion {
    public const float Seconds=.42f;
    public static float Amount(float cooldown){
        float t=6-Mathf.Clamp(cooldown,0,6);
        if(cooldown<=0||t>=Seconds)return 0;
        if(t<.025f)return -.18f*Mathf.SmoothStep(0,1,t/.025f);
        if(t<.085f)return Mathf.Lerp(-.18f,1,1-Mathf.Pow(1-(t-.025f)/.06f,3));
        if(t<.14f)return 1;
        return 1-Mathf.SmoothStep(0,1,(t-.14f)/.28f);
    }
    public static void FirstPerson(float cooldown,out Vector3 grip,out Quaternion rotation){
        float amount=Amount(cooldown);
        grip=new Vector3(.26f,-.33f,.46f)+new Vector3(-.065f,.10f,.18f)*amount;
        rotation=Quaternion.Euler(-12-56*amount,180,-18+30*amount);
    }
}
}
