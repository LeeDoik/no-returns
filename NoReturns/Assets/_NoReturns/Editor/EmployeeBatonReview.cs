using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class EmployeeBatonReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxHandDistance,maxArmChange,maxChestChange;}
    public static void Review(){
        var receipt=new Receipt();var folder=Path.GetFullPath("../artifacts/employee-baton-dynamic");Directory.CreateDirectory(folder);
        GameObject model=null;
        try{
            EmployeeAnimationBuild.ValidateAssets();model=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));
            var visual=model.GetComponent<EmployeeVisual>();typeof(EmployeeVisual).GetMethod("Awake",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(visual,null);
            var animator=model.GetComponent<Animator>();
            foreach(var state in new[]{"Idle","Walk"})foreach(float yaw in new[]{0f,90f,135f})foreach(float phase in new[]{0f,.2f,.4f,.6f,.8f})for(int frame=0;frame<=30;frame++){
                model.transform.rotation=Quaternion.Euler(0,yaw,0);animator.Play(state,0,phase);animator.Update(.001f);
                var before=visual.Capture();visual.BatonGrip(out var grip,out var rotation);
                visual.AttackPose(6-frame/60f,true);visual.BatonGrip(out grip,out rotation);
                var weapon=rotation;
                var after=visual.Capture();receipt.maxArmChange=Mathf.Max(receipt.maxArmChange,Quaternion.Angle(before.rightUpperArm,after.rightUpperArm));
                receipt.maxChestChange=Mathf.Max(receipt.maxChestChange,Quaternion.Angle(before.chest,after.chest));
                receipt.maxHandDistance=Mathf.Max(receipt.maxHandDistance,Vector3.Distance(grip,visual.RightHandPosition));
                if(!float.IsFinite(weapon.x)||receipt.maxHandDistance>.15f||!float.IsFinite(visual.Capture().batonGripError))throw new Exception("Dynamic motion must keep the baton attached and finite");receipt.samples++;
            }
            if(receipt.maxArmChange<10||receipt.maxChestChange<1||BatonVisual.Strike(6)!=0||BatonVisual.Strike(5.5f)!=0||BatonVisual.Strike(0)!=0)throw new Exception("Dynamic arm/chest motion or return failed");
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{if(model)UnityEngine.Object.DestroyImmediate(model);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
}
}
