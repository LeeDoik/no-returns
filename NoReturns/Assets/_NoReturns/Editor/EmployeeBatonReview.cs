using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class EmployeeBatonReview {
    [Serializable] class Receipt {public string status="FAIL",error;public int samples;public float maxHandDistance,maxArmChange,maxChestChange;}
    public static void Review(){
        var receipt=new Receipt();var folder=Path.GetFullPath("../artifacts/employee-baton-original");Directory.CreateDirectory(folder);
        GameObject model=null;
        try{
            EmployeeAnimationBuild.ValidateAssets();model=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));
            var visual=model.GetComponent<EmployeeVisual>();typeof(EmployeeVisual).GetMethod("Awake",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(visual,null);
            var animator=model.GetComponent<Animator>();
            foreach(var state in new[]{"Idle","Walk"})foreach(float yaw in new[]{0f,90f,135f})foreach(float phase in new[]{0f,.2f,.4f,.6f,.8f})for(int frame=0;frame<=30;frame++){
                model.transform.rotation=Quaternion.Euler(0,yaw,0);animator.Play(state,0,phase);animator.Update(.001f);
                var before=visual.Capture();visual.BatonGrip(out var grip,out var rotation);
                var weapon=rotation*Quaternion.Euler(0,0,-65*BatonVisual.Strike(6-frame/60f));
                var after=visual.Capture();receipt.maxArmChange=Mathf.Max(receipt.maxArmChange,Quaternion.Angle(before.rightUpperArm,after.rightUpperArm));
                receipt.maxChestChange=Mathf.Max(receipt.maxChestChange,Quaternion.Angle(before.chest,after.chest));
                receipt.maxHandDistance=Mathf.Max(receipt.maxHandDistance,Vector3.Distance(grip,visual.RightHandPosition));
                if(!float.IsFinite(weapon.x)||receipt.maxHandDistance>.15f||receipt.maxArmChange>.01f||receipt.maxChestChange>.01f)throw new Exception("Original grip must not modify arm/chest or detach baton");receipt.samples++;
            }
            if(BatonVisual.Strike(6)!=1||BatonVisual.Strike(5.5f)!=0||BatonVisual.Strike(0)!=0)throw new Exception("Original half-second return failed");
            receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{if(model)UnityEngine.Object.DestroyImmediate(model);File.WriteAllText(Path.Combine(folder,"review.json"),JsonUtility.ToJson(receipt,true));}
    }
}
}
