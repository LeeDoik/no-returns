"""Actual Unity physics fixture; deliberately stepped time, separate from player tests."""
from pathlib import Path
import json,subprocess,sys,time
R=Path(__file__).resolve().parents[1];out=R/'artifacts/review-fixes';out.mkdir(parents=True,exist_ok=True)
code=r"""

var before=new System.Collections.Generic.HashSet<UnityEngine.GameObject>(UnityEngine.Object.FindObjectsByType<UnityEngine.GameObject>(UnityEngine.FindObjectsInactive.Include));
var passes=new System.Collections.Generic.List<string>();var failures=new System.Collections.Generic.List<string>();
System.Action<bool,string> check=(ok,label)=>{if(ok)passes.Add(label);else failures.Add(label);};
var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
try{
 var host=new UnityEngine.GameObject("Review room fixture").AddComponent<NoReturns.CarryLab.CarryRoom>();var type=host.GetType();
 System.Action<string,object> set=(name,value)=>type.GetField(name,flags).SetValue(host,value);
 System.Func<string,object> get=(name)=>type.GetField(name,flags).GetValue(host);
 if(((UnityEngine.CharacterController[])get("workers"))[0]==null)type.GetMethod("Awake",flags).Invoke(host,null);
 set("active",true);set("hosting",true);set("peer",true);
 var inputs=(NoReturns.CarryLab.CarryInput[])get("inputs");
 System.Action tick=()=>type.GetMethod("FixedUpdate",flags).Invoke(host,null);
 for(int initial=0;initial<4;initial++){
  if(initial==2)continue;
  var m=new NoReturns.CarryLab.CarryMission();if(initial>=1)m.Act(true,true);if(initial==3){m.Act(true,true);m.Tick(new UnityEngine.Vector3(-6,1.3f,9),UnityEngine.Vector3.zero,-1,1);}
  set("mission",m);inputs[0].action=true;inputs[1].action=true;tick();
  check(m.Phase==(initial==0?1:initial==1?2:4),"simultaneous F advances once from phase "+initial);
  if(initial==3)check(m.Credits==420,"same-tick return pays once");
 }
 var normal=new NoReturns.CarryLab.CarryThreat();var outer=new NoReturns.CarryLab.CarryThreat(normal);
 var clues=new NoReturns.CarryLab.CarryClues();var clock=new NoReturns.CarryLab.CarrySuppression();
 set("hazard",true);set("threat",normal);set("outer",outer);set("clues",clues);set("suppression",clock);
 var arrival=new NoReturns.CarryLab.CarryMission();arrival.Act(true,true);set("mission",arrival);
 inputs[0].action=true;inputs[1].action=false;tick();type.GetMethod("LateUpdate",flags).Invoke(host,null);
 var grid=(System.Collections.Generic.Dictionary<UnityEngine.Vector2Int,int>)normal.GetType().GetField("grid",flags).GetValue(normal);
 check(!grid.ContainsKey(new UnityEngine.Vector2Int(-8,10))&&!grid.ContainsKey(new UnityEngine.Vector2Int(-8,11)),"first-arrival grid includes recorder collision");
 var players=new[]{new UnityEngine.Vector3(0,.335f,8),new UnityEngine.Vector3(1,0,-5)};
 var quiet=new[]{new NoReturns.CarryLab.CarryInput{quiet=true},new NoReturns.CarryLab.CarryInput{quiet=true}};
 for(int kind=0;kind<2;kind++){
  normal.Reset();outer.Reset();var enemy=kind==0?normal:outer;
  for(int n=0;n<1400&&!normal.Down[0];n++){if(kind==0&&n%40==0)enemy.Hear(players[0],12);enemy.Tick(players,true,quiet,-1,true,.02f);}
  check(normal.Down[0],(kind==0?"listener":"outer")+" reaches employee on low-step center");
  if(normal.Down[0])check(enemy.Snapshot().position.y>.25f,"creature stands above step top "+kind+" at "+enemy.Snapshot().position);
 }
 normal.Reset();players[0]=new UnityEngine.Vector3(-1,0,-5);
 normal.GetType().GetField("position",flags).SetValue(normal,new UnityEngine.Vector3(-1,0,-4.2f));
 for(int n=0;n<150;n++)normal.Tick(players,false,quiet,-1,true,.02f);
 check(!normal.Down[0],"listener respects ship safety even when near ship");
 var g=new UnityEngine.GameObject("Gap character fixture");var cc=g.AddComponent<UnityEngine.CharacterController>();cc.height=1.8f;cc.radius=.34f;cc.center=UnityEngine.Vector3.up*.9f;cc.skinWidth=.035f;cc.stepOffset=.32f;g.transform.position=new UnityEngine.Vector3(-15.36f,.04f,17);UnityEngine.Physics.SyncTransforms();
 for(int n=0;n<130;n++)cc.Move(new UnityEngine.Vector3(0,-.04f,-.04f));
 check(g.transform.position.z>14,"west wall/rack gap cannot be entered");
 var output="PASS "+passes.Count+" FAIL "+failures.Count+"\n"+string.Join("\n",passes)+"\nFAILURES\n"+string.Join("\n",failures);
 var root=System.IO.File.ReadAllText("CarryWorkspace.txt").Trim();System.IO.File.WriteAllText(System.IO.Path.Combine(root,"artifacts/review-fixes/rules-report.txt"),output);
 if(failures.Count>0)throw new System.Exception(output);UnityEngine.Debug.Log(output);
}finally{
 foreach(var g in UnityEngine.Object.FindObjectsByType<UnityEngine.GameObject>(UnityEngine.FindObjectsInactive.Include))if(g!=null&&!before.Contains(g)&&g.transform.parent==null)UnityEngine.Object.DestroyImmediate(g);
}

"""
request=out/'rules-eval.json';request.write_text(json.dumps({'code':code,'timeout':60000}),encoding='utf-8')
r=subprocess.run(['python',str(R/'tools/unity_mcp.py'),'eval','--arguments-file',str(request)],capture_output=True,text=True,encoding='utf-8')
(out/('rules-'+time.strftime('%Y%m%d-%H%M%S')+'.json')).write_text(r.stdout,encoding='utf-8');(out/'rules-latest.json').write_text(r.stdout,encoding='utf-8');print((out/'rules-report.txt').read_text(encoding='utf-8') if (out/'rules-report.txt').exists() else r.stdout)
data=json.loads(r.stdout)
if r.returncode!=0 or data.get('isError'):sys.exit(1)
inner=json.loads(data['content'][0]['text']);sys.exit(0 if inner.get('success') else 1)
