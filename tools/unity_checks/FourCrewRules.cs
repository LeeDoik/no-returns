using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using NoReturns.CarryLab;
public static class FourCrewRules {
    static void Require(bool value,string label){if(!value)throw new Exception(label);Debug.Log("FOUR CREW PASS: "+label);}
    public static void Run(){
        var old=SceneManager.GetActiveScene();var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        try{
            SceneManager.SetActiveScene(scene);var t=new CarryThreat();var flags=BindingFlags.Instance|BindingFlags.NonPublic;
            typeof(CarryThreat).GetField("state",flags).SetValue(t,4);typeof(CarryThreat).GetField("timer",flags).SetValue(t,100f);
            var p=new[]{new Vector3(10,0,0),new Vector3(8,0,0),new Vector3(1,0,0),Vector3.zero};
            var input=new[]{new CarryInput(),new CarryInput(),new CarryInput(),new CarryInput{rescue=true}};
            t.Down[2]=true;Require(t.RescueTarget(3,p,15)==2,"slot 3 targets down slot 2");
            for(int n=0;n<10;n++)t.Tick(p,15,input,-1,true,.1f);
            Require(t.Rescue[3]>.8f&&t.Down[2],"slot 3 progress accumulates without early rescue");
            p[1]=new Vector3(.5f,0,0);t.Down[1]=true;t.Tick(p,15,input,-1,true,.1f);
            Require(t.Rescue[3]<.2f&&t.Down[1],"switching target resets progress");
            for(int n=0;n<26;n++)t.Tick(p,15,input,-1,true,.1f);
            Require(!t.Down[1]&&t.Down[2],"only nearest target rescued");
            input[3].rescue=false;t.Down[0]=true;t.Down[1]=true;t.Down[2]=true;
            bool recovered=false;for(int n=0;n<40;n++)recovered|=t.Tick(p,15,input,-1,true,.1f);
            Require(!recovered,"alive slot 3 prevents premature all-down recovery");
            for(int n=0;n<40;n++)recovered|=t.Tick(p,7,input,-1,true,.1f);
            Require(recovered,"absent slot 3 does not block all-down recovery");
            var s=t.Snapshot();Require(s.down.Length==4&&s.rescue.Length==4&&s.cooldown.Length==4,"four-slot snapshot sizes");
        }finally{EditorSceneManager.CloseScene(scene,true);SceneManager.SetActiveScene(old);}
    }
}
