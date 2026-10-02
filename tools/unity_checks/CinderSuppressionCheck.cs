using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using NoReturns.CarryLab;
public static class CinderSuppressionCheck {
    static readonly BindingFlags Flags=BindingFlags.Instance|BindingFlags.NonPublic;
    static void Check(bool value,string message){if(!value)throw new Exception(message);}
    public static object Run(){
        var scene=UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        Check(!scene.isDirty&&!UnityEditor.EditorApplication.isPlaying&&scene.path==NoReturns.Editor.CinderFourPlayerBuild.ScenePath,"Saved Cinder test scene required");
        var sky=RenderSettings.skybox;var ambient=RenderSettings.ambientLight;var sun=RenderSettings.sun;
        float sunLevel=sun.intensity,fogStart=RenderSettings.fogStartDistance,fogEnd=RenderSettings.fogEndDistance;Color fog=RenderSettings.fogColor;
        var lights=UnityEngine.Object.FindObjectsByType<Light>().Where(l=>l.type!=LightType.Directional).ToArray();
        var levels=Array.ConvertAll(lights,l=>l.intensity);var type=typeof(CarryThreat);var bodies=new List<CarryThreat>();CarrySuppression suppression=null;
        int stages=0,routes=0,steps=0;float buildSeconds=0;int nodes=0;
        try{
            suppression=new CarrySuppression(true);
            Check(lights.Length==40,"Retain 40 local work lights");
            var lamps=(Material[])typeof(CarrySuppression).GetField("lamps",Flags).GetValue(suppression);Check(lamps.Length==5,"Four signals and field boundary");
            foreach(var sample in new[]{(0f,0),(89.99f,0),(90f,1),(134.99f,1),(135f,2),(179.99f,2),(180f,3),(187.99f,3),(188f,3)}){
                suppression.Apply(sample.Item1);Check(suppression.Stage==sample.Item2,"Stage boundary "+sample.Item1);Check(suppression.Intrusion==(sample.Item1>=188),"Entry grace "+sample.Item1);stages++;
            }
            suppression.Apply(0);suppression.Display(true);
            Check(lamps.All(m=>Vector4.Distance(m.color,new Color(.24f,.68f,.5f))<.001f),"Steady green signals");
            suppression.Apply(180);suppression.Display(true);
            for(int i=0;i<lights.Length;i++)Check(Mathf.Abs(lights[i].intensity-levels[i]*.3f)<.001f,"Relative work-light dimming");
            Check(Vector4.Distance(RenderSettings.ambientLight,ambient*.65f)<.001f,"Relative ambient dimming");
            Check(RenderSettings.skybox==sky&&RenderSettings.sun==sun&&sun.intensity==sunLevel&&RenderSettings.fogColor==fog&&RenderSettings.fogStartDistance==fogStart&&RenderSettings.fogEndDistance==fogEnd,"Sky, sun and fog preserved");
            suppression.Tick(false,10);Check(suppression.Elapsed==180,"Report/preparation stops clock");suppression.Display(false);
            for(int i=0;i<lights.Length;i++)Check(Mathf.Abs(lights[i].intensity-levels[i])<.001f,"Report restores work lights");
            Check(RenderSettings.ambientLight==ambient,"Report restores ambient");suppression.Begin();Check(suppression.Elapsed==0&&!suppression.Intrusion,"New shift resets clock");
            var inner=new CarryThreat(cinder:true);bodies.Add(inner);var outer=new CarryThreat(inner,true);bodies.Add(outer);
            Check(ReferenceEquals(inner.Down,outer.Down),"Shared down state");Check(outer.Snapshot().position==new Vector3(29,0,18),"East boundary staging position");
            var start=outer.Snapshot().position;var position=type.GetField("position",Flags);var route=type.GetMethod("Route",Flags);var clock=System.Diagnostics.Stopwatch.StartNew();
            foreach(var goal in new[]{new Vector3(21,0,18),new Vector3(-23,0,-20),new Vector3(-5,0,-10),new Vector3(17,0,14),new Vector3(52,0,41),new Vector3(-52,0,-41),new Vector3(29,0,4)}){
                position.SetValue(outer,start);route.Invoke(outer,new object[]{goal});
                var path=(List<Vector3>)type.GetField("path",Flags).GetValue(outer);
                Check(path.Count>0&&Vector3.Distance(path[path.Count-1],goal)<1.4f,"Outer route to "+goal);routes++;
            }
            buildSeconds=(float)clock.Elapsed.TotalSeconds;nodes=((List<Vector3>)type.GetField("nodes",Flags).GetValue(outer)).Count;
            outer.Reset();var crew=new[]{new Vector3(-21.65f,1.035f,-25.5f),new Vector3(-19.75f,1.035f,-25.5f),new Vector3(-21.65f,1.035f,-26.9f),new Vector3(-19.75f,1.035f,-26.9f)};
            var input=Array.ConvertAll(crew,p=>new CarryInput{quiet=true,call=true,shove=true});
            for(int n=0;n<1600;n++){
                outer.Tick(crew,15,input,-1,true,.05f);var p=outer.Snapshot().position;
                Check((bool)type.GetMethod("Clear",BindingFlags.Static|BindingFlags.NonPublic).Invoke(null,new object[]{p}),"Outer collision at "+p);steps++;
            }
            Check(outer.Snapshot().hits==0&&outer.Snapshot().noises==0&&!inner.Down.Any(d=>d),"Ship is safe; calls and baton do not control outer");
            // Isolated beacon/entry rules use explicit positions; the native-player check uses ordinary controls and real time.
            outer.Reset();position.SetValue(outer,new Vector3(29,0,18));outer.Hear(new Vector3(29,0,16),12);Check(outer.Snapshot().noises==0,"Outer ignores ordinary noise");
            outer.Distract(new Vector3(29,0,16),12);Check(outer.Snapshot().noises==1&&outer.Snapshot().pursuedPlayer==-1,"Beacon overrides pursuit");
            input[0].shove=true;crew[0]=new Vector3(29,0,17.5f);outer.Tick(crew,1,input,-1,true,.02f);Check(outer.Snapshot().state!=4,"Outer resists baton");
            return new {status="PASS",stageBoundarySamples=stages,signals=lamps.Length,localLights=lights.Length,outerRoutes=routes,collisionSteps=steps,outerGridNodes=nodes,routeBuildSeconds=buildSeconds,skyFogSunPreserved=true,sharedDownAndShipSafety=true};
        }finally{
            foreach(var t in bodies){UnityEngine.Object.DestroyImmediate((GameObject)type.GetField("body",Flags).GetValue(t));UnityEngine.Object.DestroyImmediate((Material)type.GetField("skin",Flags).GetValue(t));UnityEngine.Object.DestroyImmediate((AudioClip)type.GetField("warningClip",Flags).GetValue(t));}
            if(suppression!=null){foreach(var m in (Material[])typeof(CarrySuppression).GetField("lamps",Flags).GetValue(suppression))UnityEngine.Object.DestroyImmediate(m);UnityEngine.Object.DestroyImmediate((AudioClip)typeof(CarrySuppression).GetField("tone",Flags).GetValue(suppression));UnityEngine.Object.DestroyImmediate((GameObject)typeof(CarrySuppression).GetField("device",Flags).GetValue(suppression));}
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene.path);
        }
    }
}
