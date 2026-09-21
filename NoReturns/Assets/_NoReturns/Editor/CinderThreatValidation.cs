using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NoReturns.CarryLab;
using UnityEditor;
using UnityEngine;

namespace NoReturns.Editor {
// State-machine regression checks. Geometry, route traversal and network transport need separate tests.
public static class CinderThreatValidation {
    const BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    static readonly Vector3 Safe=new Vector3(1000,0,1000);
    static T Get<T>(CarryThreat target,string name)=>(T)typeof(CarryThreat).GetField(name,Private).GetValue(target);
    static void Set(CarryThreat target,string name,object value)=>typeof(CarryThreat).GetField(name,Private).SetValue(target,value);
    static void Check(bool result,string message){if(!result)throw new Exception("Cinder threat validation: "+message);}
    static void Near(float value,float expected,string message)=>Check(Mathf.Abs(value-expected)<.0001f,message+"; actual="+value+", expected="+expected);
    static CarryInput[] QuietInputs()=>new[]{new CarryInput{quiet=true},new CarryInput{quiet=true},new CarryInput{quiet=true},new CarryInput{quiet=true}};
    static Vector3[] Crew()=>new[]{Safe,Safe+Vector3.right,Safe+Vector3.right*3,Safe+Vector3.right*4};
    static void Park(CarryThreat root){
        // Freeze only locomotion/patrol so assertions exercise real Tick recovery and attack code
        // independently of the current scene's furniture and navigation graph.
        root.Reset();
        var all=new List<CarryThreat>{root};all.AddRange(Get<CarryThreat[]>(root,"additional"));
        foreach(var threat in all){Set(threat,"position",Safe+Vector3.forward*100);Set(threat,"state",3);Set(threat,"timer",100f);}
    }
    static void Dispose(CarryThreat root){
        if(root==null)return;
        foreach(var child in Get<CarryThreat[]>(root,"additional"))Dispose(child);
        UnityEngine.Object.DestroyImmediate(Get<GameObject>(root,"body"));
        UnityEngine.Object.DestroyImmediate(Get<Material>(root,"skin"));
        UnityEngine.Object.DestroyImmediate(Get<AudioClip>(root,"warningClip"));
    }
    [MenuItem("NO RETURNS/Demo/Validate Cinder Listener Patrols")]
    public static void ValidatePatrols(){
        if(EditorApplication.isPlayingOrWillChangePlaymode)throw new Exception("Run patrol validation outside Play Mode");
        Check(GameObject.Find("Cinder Depot editable primitive blockout")!=null,"open Cinder demo scene first");
        CinderDemoLayout.Initialize();Check(CinderDemoLayout.Active,"Cinder demo profile required");
        string workspace=File.Exists("CarryWorkspace.txt")?File.ReadAllText("CarryWorkspace.txt").Trim():Path.GetFullPath("..");
        string directory=Path.Combine(workspace,"artifacts/cinder-dense");Directory.CreateDirectory(directory);
        string evidencePath=Path.Combine(directory,"threat-patrols.txt");
        var report=new StringBuilder("Cinder listener patrol simulation; actual scene colliders, host AI, no occupied crew slots.\n");
        report.AppendLine("3000 ticks x 0.1 seconds = 300 simulated seconds; excludes network, rendered play and human quality.");
        CarryThreat root=null;
        try {
            root=new CarryThreat();root.PrepareNavigation();
            var extra=Get<CarryThreat[]>(root,"additional");Check(extra.Length==2,"three patrol agents required");
            var all=new[]{root,extra[0],extra[1]};
            var graph=Get<List<Vector3>>(root,"nodes");Check(graph.Count>0,"nonempty collider navigation graph");
            foreach(var child in extra){
                Check(ReferenceEquals(graph,Get<List<Vector3>>(child,"nodes")),"shared patrol graph nodes");
                Check(ReferenceEquals(Get<object>(root,"edges"),Get<object>(child,"edges")),"shared patrol graph edges");
            }
            report.AppendLine("Shared navigation nodes: "+graph.Count);
            var visits=new bool[3][];var travelled=new float[3];var previous=new Vector3[3];
            var patrols=new Vector3[3][];
            for(int i=0;i<3;i++){
                patrols[i]=Get<Vector3[]>(all[i],"patrol");visits[i]=new bool[patrols[i].Length];previous[i]=all[i].Snapshot().position;
            }
            var crew=new Vector3[4];for(int i=0;i<4;i++)crew[i]=CinderDemoLayout.Spawn(i);
            var inputs=QuietInputs();
            var walkPoint=typeof(CarryThreat).GetMethod("WalkPoint",BindingFlags.Static|BindingFlags.NonPublic);
            for(int i=0;i<3;i++)for(int p=0;p<patrols[i].Length;p++){
                object[] args={patrols[i][p],Vector3.zero};
                Check((bool)walkPoint.Invoke(null,args),"listener "+(char)('A'+i)+" patrol node "+p+" overlaps an obstacle or has no floor at "+patrols[i][p]);
            }
            // Do not count the spawn as a visit: each agent must actually return to its first node.
            for(int tick=0;tick<3000;tick++){
                Check(!root.Tick(crew,0,inputs,-1,true,.1f),"empty crew mask cannot trigger recovery");
                for(int i=0;i<3;i++){
                    var position=all[i].Snapshot().position;
                    travelled[i]+=Vector3.Distance(previous[i],position);previous[i]=position;
                    for(int p=0;p<patrols[i].Length;p++)if(Vector3.Distance(position,patrols[i][p])<.35f&&(p!=0||travelled[i]>5))visits[i][p]=true;
                }
            }
            bool passed=true;
            for(int i=0;i<3;i++){
                bool complete=true;for(int p=0;p<visits[i].Length;p++)complete&=visits[i][p];
                bool moved=travelled[i]>30;passed&=complete&&moved;
                report.AppendLine("Listener "+(char)('A'+i)+": distance="+travelled[i].ToString("F2",System.Globalization.CultureInfo.InvariantCulture)+"m; all nodes visited="+complete+"; final="+previous[i]);
                for(int p=0;p<visits[i].Length;p++)report.AppendLine("  node "+p+" "+patrols[i][p]+": "+(visits[i][p]?"VISITED":"NOT VISITED"));
            }
            Check(passed,"patrol did not traverse every specified node; see "+evidencePath);
            report.AppendLine("PASS: A/B/C move over real collider navigation and visit every patrol node, sharing one graph.");
            Debug.Log(report.ToString());
        } catch(Exception error){report.AppendLine("FAIL: "+error.Message);throw;}
        finally {Dispose(root);File.WriteAllText(evidencePath,report.ToString());}
    }
    [MenuItem("NO RETURNS/Demo/Validate Cinder Listener State")]
    public static void Validate(){
        if(EditorApplication.isPlayingOrWillChangePlaymode)throw new Exception("Run listener validation outside Play Mode");
        var marker=GameObject.Find("Cinder Depot editable primitive blockout");
        Check(marker!=null,"open Cinder demo scene first");
        CinderDemoLayout.Initialize();
        CarryThreat root=null,outer=null,legacy=null;
        string markerName=marker.name;
        try {
            root=new CarryThreat();outer=new CarryThreat(root);
            var additional=Get<CarryThreat[]>(root,"additional");
            Check(additional.Length==2&&root.AdditionalSnapshots().Length==2,"exactly three listener snapshots");
            Check(root.Snapshot().position==new Vector3(-26,0,10),"A initial position");
            Check(additional[0].Snapshot().position==new Vector3(-6,0,2),"B initial position");
            Check(additional[1].Snapshot().position==new Vector3(21,0,0),"C initial position");
            var patrol=Get<Vector3[]>(root,"patrol");
            Check(patrol[1]==new Vector3(-16,0,10)&&patrol[2]==new Vector3(-16,0,-8),"A east patrol avoids rack");
            foreach(var member in new[]{additional[0],additional[1],outer}){
                Check(ReferenceEquals(root.Down,member.Down),"shared down array");
                Check(ReferenceEquals(root.Rescue,member.Rescue),"shared rescue array");
                Check(ReferenceEquals(root.Cooldown,member.Cooldown),"shared cooldown array");
                Check(ReferenceEquals(Get<float[]>(root,"protection"),Get<float[]>(member,"protection")),"shared protection array");
            }
            foreach(var member in additional)Check(ReferenceEquals(Get<object>(root,"nodes"),Get<object>(member,"nodes")),"listener navigation graph is shared");

            Park(root);var crew=Crew();var inputs=QuietInputs();
            additional[1].Down[1]=true;inputs[0].rescue=true;
            for(int i=0;i<9;i++)Check(!root.Tick(crew,3,inputs,-1,true,.25f),"rescue does not evacuate crew");
            Near(root.Rescue[0],2.25f,"rescue advances once per tick");
            Check(root.Down[1],"rescue cannot complete before 2.5 seconds");
            int rescueCount=root.Snapshot().rescues;
            root.Tick(crew,3,inputs,-1,true,.25f);
            Check(!root.Down[1]&&!additional[0].Down[1]&&!outer.Down[1],"2.5 second rescue clears every threat's down view");
            Check(root.Snapshot().rescues==rescueCount+1,"one rescue event");
            Near(Get<float[]>(root,"protection")[1],4,"rescue gives four seconds protection");
            inputs[0].rescue=false;root.Tick(crew,3,inputs,-1,true,.25f);
            Near(Get<float[]>(root,"protection")[1],3.75f,"protection decreases once with three listeners");
            Set(outer,"position",Safe+Vector3.forward*100);Set(outer,"state",3);Set(outer,"timer",100f);
            outer.Tick(crew,3,inputs,-1,true,.25f);
            Near(Get<float[]>(root,"protection")[1],3.75f,"outer does not decrease shared protection again");

            Park(root);inputs=QuietInputs();
            Set(additional[0],"position",Safe+Vector3.forward*2);
            inputs[0].shove=true;inputs[0].yaw=0;
            root.Tick(crew,3,inputs,-1,true,.25f);
            Check(additional[0].Snapshot().state==4,"baton reaches B although A is far away");
            Near(root.Cooldown[0],6,"one baton use starts shared six second cooldown");
            Check(root.Snapshot().state!=4&&additional[1].Snapshot().state!=4,"baton does not hit distant A/C");
            inputs[0].shove=false;root.Tick(crew,3,inputs,-1,true,.25f);
            Near(root.Cooldown[0],5.75f,"cooldown decreases once per tick");
            Near(additional[1].Cooldown[0],5.75f,"C sees same cooldown");
            outer.Tick(crew,3,inputs,-1,true,.25f);Near(root.Cooldown[0],5.75f,"outer does not decrease cooldown again");

            Park(root);inputs=QuietInputs();root.Down[0]=root.Down[1]=true;
            for(int i=0;i<11;i++)Check(!root.Tick(crew,3,inputs,-1,true,.25f),"all-down recovery waits full three seconds");
            Check(root.Tick(crew,3,inputs,-1,true,.25f),"all-down recovery at three seconds");
            var snapshot=new CarryState{protocol=12,danger=root.Snapshot(),additionalDanger=root.AdditionalSnapshots()};
            var received=JsonUtility.FromJson<CarryState>(JsonUtility.ToJson(snapshot));
            Check(received.protocol==12&&received.additionalDanger.Length==2,"JSON carries all three listeners");
            Check(received.danger.IsDown(0)&&received.additionalDanger[0].IsDown(0)&&received.additionalDanger[1].IsDown(0),"JSON preserves common crew down state");
            root.DisplayAdditional(received.additionalDanger,true);
            for(int i=0;i<2;i++)Check(Get<GameObject>(additional[i],"body").transform.position==received.additionalDanger[i].position,"additional client display uses snapshot position");
            root.DisplayAdditional(null,false);
            foreach(var member in additional)Check(!Get<GameObject>(member,"body").activeSelf,"additional display hides missing snapshot safely");
            root.Reset();
            foreach(var member in new[]{root,additional[0],additional[1]}){
                Check(!member.Down[0]&&!member.Down[1],"reset clears shared down state");
                Near(member.Cooldown[0],0,"reset clears shared cooldown");Near(member.Rescue[0],0,"reset clears rescue progress");
            }

            marker.name="Cinder marker temporarily hidden for listener validation";
            legacy=new CarryThreat();
            Check(legacy.AdditionalSnapshots().Length==0,"legacy scene retains exactly one listener");
            Check(legacy.Snapshot().position==new Vector3(1,0,5),"legacy initial patrol position preserved");
            Debug.Log("PASS Cinder listener state: three spawns, shared arrays, single rescue/protection/cooldown timers, B baton, three-second recovery, JSON/display, reset and legacy one listener. Navigation and transport excluded.");
        } finally {
            marker.name=markerName;
            Dispose(legacy);Dispose(outer);Dispose(root);
        }
    }
}
}
