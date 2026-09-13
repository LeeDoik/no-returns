using System;
using System.Linq;
using UnityEngine;
using NoReturns.CarryLab;
public static class CrewHudCheck {
 static void Check(bool value,string message){if(!value)throw new Exception(message);Debug.Log("HUD PASS "+message);}
 public static void Run(){
  CarryLanguage.Initialize("hud-check");var go=new GameObject("HUD isolated check");
  try{
   var hud=go.AddComponent<CrewHud>();var s=new CarryState{phase=2,players=4,occupiedMask=15,holder=2,beaconCarrier=1,credits=420,hazard=true,positions=new[]{new Vector3(0,0,-5),Vector3.zero,Vector3.zero,Vector3.zero},danger=new ThreatState{down=new[]{false,false,false,true},rescue=new float[4],cooldown=new float[4]}};
   hud.Apply(s,2,true);var texts=go.GetComponentsInChildren<UnityEngine.UI.Text>();
   Check(texts.Any(t=>t.text.Contains("03 / 화물 운반 / 나")),"local cargo identity");Check(texts.Any(t=>t.text.Contains("04 / 구조 필요")),"down teammate");Check(texts.Any(t=>t.text.Contains("02 / 신호기 운반")),"beacon carrier");
   Check(texts.Any(t=>t.text.Contains("420 CR / 직원 4/4")),"wallet and count");Check(texts.All(t=>!t.raycastTarget),"HUD never blocks input");
   s.occupiedMask=7;s.players=3;hud.Apply(s,2,true);Check(texts.Any(t=>t.text=="04 / 빈자리"),"disconnected slot empty");
   s.holder=-1;s.danger.rescue[2]=1.25f;hud.Apply(s,2,true);Check(texts.Any(t=>t.text.Contains("구조 중 50%")),"rescue progress");
   CarryLanguage.Toggle();hud.Apply(s,2,true);Check(texts.Any(t=>t.text.Contains("RESCUING 50%")),"English toggle");
   hud.Apply(s,2,false);Check(!go.GetComponentInChildren<Canvas>(true).gameObject.activeSelf,"menu hides HUD");
   var scaler=go.GetComponentInChildren<UnityEngine.UI.CanvasScaler>(true);Check(scaler.referenceResolution==new Vector2(1280,720),"reference resolution");
   s.holder=-1;s.beaconCarrier=-1;s.danger.rescue[2]=0;
   for(int language=0;language<2;language++){
    for(int phase=0;phase<5;phase++){s.phase=phase;hud.Apply(s,2,true);Canvas.ForceUpdateCanvases();foreach(var t in texts)Check(t.preferredHeight<=t.rectTransform.rect.height+.1f,"text fits "+language+"/"+phase+"/"+t.name);}
    CarryLanguage.Toggle();
   }
  }finally{UnityEngine.Object.DestroyImmediate(go);if(!CarryLanguage.Korean)CarryLanguage.Toggle();}
 }
}
