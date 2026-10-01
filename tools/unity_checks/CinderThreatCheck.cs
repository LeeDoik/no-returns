using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using NoReturns.CarryLab;
public static class CinderThreatCheck {
    static readonly BindingFlags Flags=BindingFlags.Instance|BindingFlags.NonPublic;
    static void Require(bool ok,string label){if(!ok)throw new Exception(label);}
    public static object Run(){
        Require(!UnityEditor.EditorApplication.isPlaying,"Stop Play first");
        Require(UnityEngine.SceneManagement.SceneManager.GetActiveScene().path==NoReturns.Editor.CinderFourPlayerBuild.ScenePath,"Current Cinder test scene required");
        var t=new CarryThreat(cinder:true);var type=typeof(CarryThreat);int routes=0,steps=0;GameObject barrier=null;
        try{
            var start=t.Snapshot().position;
            var route=type.GetMethod("Route",Flags);var position=type.GetField("position",Flags);
            foreach(var segment in NoReturns.Editor.CinderCompactSiteBuild.Routes().Where(r=>r.name!="Ship ramp"))foreach(var goal in new[]{segment.a,segment.b}){
                position.SetValue(t,start);route.Invoke(t,new object[]{goal});
                var path=(List<Vector3>)type.GetField("path",Flags).GetValue(t);
                Require(path.Count>0&&Vector3.Distance(path[path.Count-1],goal)<1.4f,"No route to "+segment.name+" / "+goal);routes++;
            }
            var crew=new[]{new Vector3(-21.65f,1.035f,-25.5f),new Vector3(-19.75f,1.035f,-25.5f),new Vector3(-21.65f,1.035f,-26.9f),new Vector3(-19.75f,1.035f,-26.9f)};
            var input=Array.ConvertAll(crew,p=>new CarryInput{quiet=true,call=true});t.Reset();
            for(int n=0;n<2400;n++){
                t.Tick(crew,15,input,-1,true,.05f);var p=t.Snapshot().position;
                Require(Vector3.Distance(start,p)<100,"Finite movement");
                Require((bool)type.GetMethod("Clear",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{p}),"Listener penetrates static geometry at "+p);steps++;
            }
            int gridNodes=((List<Vector3>)type.GetField("nodes",Flags).GetValue(t)).Count;
            var s=t.Snapshot();Require(s.noises==0&&s.hits==0&&!s.down.Any(d=>d),"Ship calls and attacks are safe");
            Require(Vector3.Distance(s.position,start)>1,"Patrol moves");
            Require(s.down.Length==4&&s.cooldown.Length==4,"Four crew state");
            // Isolated rule checks use explicit state setup; the four-process test uses only ordinary inputs.
            t.Reset();type.GetField("state",Flags).SetValue(t,4);type.GetField("timer",Flags).SetValue(t,99f);
            crew[0]=new Vector3(-23,0,-13);crew[1]=new Vector3(-23,0,-14);t.Down[1]=true;input[0].rescue=true;
            Require(t.RescueTarget(0,crew,3)==1,"Nearby teammate target");
            barrier=GameObject.CreatePrimitive(PrimitiveType.Cube);barrier.name="Temporary rescue occlusion check";barrier.transform.position=new Vector3(-23,1,-13.5f);barrier.transform.localScale=new Vector3(2,2,.1f);Physics.SyncTransforms();
            Require(t.RescueTarget(0,crew,3)<0,"Wall rejects rescue target");
            for(int n=0;n<30;n++)t.Tick(crew,3,input,-1,true,.1f);
            Require(t.Down[1]&&t.Rescue[0]==0,"Wall blocks rescue completion");
            UnityEngine.Object.DestroyImmediate(barrier);barrier=null;Physics.SyncTransforms();
            for(int n=0;n<30;n++)t.Tick(crew,3,input,0,true,.1f);
            Require(t.Down[1]&&t.Rescue[0]==0,"Occupied hands cannot rescue");
            for(int n=0;n<26;n++)t.Tick(crew,3,input,-1,true,.1f);
            Require(!t.Down[1],"Clear empty-hand rescue completes");
            return new {status="PASS",reachableRouteEndpoints=routes,collisionCheckedPatrolSteps=steps,shipSafety=true,rescueConditions=5,gridNodes=gridNodes};
        }finally{
            if(barrier)UnityEngine.Object.DestroyImmediate(barrier);
            UnityEngine.Object.DestroyImmediate((GameObject)type.GetField("body",Flags).GetValue(t));
            UnityEngine.Object.DestroyImmediate((Material)type.GetField("skin",Flags).GetValue(t));
            UnityEngine.Object.DestroyImmediate((AudioClip)type.GetField("warningClip",Flags).GetValue(t));
        }
    }
    public static object Legacy(){
        var old=UnityEngine.SceneManagement.SceneManager.GetActiveScene();Require(!old.isDirty&&!UnityEditor.EditorApplication.isPlaying,"Stop Play and save first");string prior=old.path;
        try{
            UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene,UnityEditor.SceneManagement.NewSceneMode.Single);CarryWorld.Build();
            var t=new CarryThreat();var type=typeof(CarryThreat);var route=type.GetMethod("Route",Flags);int endpoints=0;
            foreach(var goal in (Vector3[])type.GetField("patrol",Flags).GetValue(t)){
                route.Invoke(t,new object[]{goal});var path=(List<Vector3>)type.GetField("path",Flags).GetValue(t);
                Require(Vector3.Distance(goal,t.Snapshot().position)<.01f||path.Count>0,"Legacy patrol cannot reach "+goal);endpoints++;
            }
            return new {status="PASS",legacyPatrolEndpoints=endpoints,lowStepMetres=.3f};
        }finally{UnityEditor.SceneManagement.EditorSceneManager.OpenScene(prior);}
    }

}
