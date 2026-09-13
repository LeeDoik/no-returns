"""Actual Unity physics fixture; deliberately stepped time, separate from player tests."""
from pathlib import Path
import json,subprocess,sys,time
R=Path(__file__).resolve().parents[1];out=R/'artifacts/global-hunt';out.mkdir(parents=True,exist_ok=True)
code=r"""
var before=new System.Collections.Generic.HashSet<UnityEngine.GameObject>();foreach(var g in UnityEngine.Object.FindObjectsByType<UnityEngine.GameObject>(UnityEngine.FindObjectsInactive.Include))before.Add(g);
int count=0;System.Action<bool,string> check=(ok,label)=>{if(!ok)throw new System.Exception(label);count++;};
try{
 NoReturns.CarryLab.CarryWorld.Build();
 var normal=new NoReturns.CarryLab.CarryThreat();var outer=new NoReturns.CarryLab.CarryThreat(normal);
 var players=new[]{new UnityEngine.Vector3(-1,0,-5),new UnityEngine.Vector3(-12,0,17)};
 var inputs=new[]{new NoReturns.CarryLab.CarryInput{quiet=true},new NoReturns.CarryLab.CarryInput{quiet=true}};
 UnityEngine.Physics.SyncTransforms();
 check(!NoReturns.CarryLab.CarryThreat.Sight(outer.Snapshot().position,players[1]),"fixture target is behind walls");
 outer.Tick(players,true,inputs,-1,true,.02f);
 check(outer.Snapshot().state==1&&UnityEngine.Vector3.Distance(outer.Snapshot().noiseTarget,players[1])<.1f,"silent distant player behind walls must be acquired");

 check(outer.Snapshot().pursuedPlayer==1,"ship crew excluded from selection");
 System.Action<int> step=(frames)=>{for(int n=0;n<frames;n++){outer.Tick(players,true,inputs,-1,true,.02f);foreach(var hit in UnityEngine.Physics.OverlapBox(outer.Snapshot().position+UnityEngine.Vector3.up*.95f,new UnityEngine.Vector3(.4f,.8f,.4f)))if(hit.enabled&&hit.attachedRigidbody==null)throw new System.Exception("pursuit intersects static geometry: "+hit.name);}};
 step(1800);check(normal.Down[1],"reach and down distant crew around northern walls");
 outer.Reset();players[1]=new UnityEngine.Vector3(-12,0,-12);for(int n=0;n<2500&&!normal.Down[1];n++)outer.Tick(players,true,inputs,-1,true,.02f);
 check(normal.Down[1]&&outer.Snapshot().position.z< -10,"southwest perimeter reachable beyond old navigation limit "+UnityEngine.JsonUtility.ToJson(outer.Snapshot()));
 outer.Reset();players[0]=new UnityEngine.Vector3(12,0,25);players[1]=new UnityEngine.Vector3(-12,0,-12);step(1);
 check(outer.Snapshot().pursuedPlayer==0,"nearest eligible crew selected");
 normal.Down[0]=true;step(42);check(outer.Snapshot().pursuedPlayer==1,"downed crew excluded and other crew acquired");
 players[1]=new UnityEngine.Vector3(1,0,-5);step(42);check(outer.Snapshot().pursuedPlayer==-1,"crew aboard cancels pursuit");
 outer.Reset();players[0]=new UnityEngine.Vector3(-1,0,-5);players[1]=new UnityEngine.Vector3(-12,0,17);step(1);
 var lure=new UnityEngine.Vector3(13,0,20);outer.Distract(lure,12);step(20);
 check(outer.Snapshot().pursuedPlayer==-1&&UnityEngine.Vector3.Distance(outer.Snapshot().noiseTarget,lure)<.1f,"beacon briefly overrides omniscient pursuit");
 step(90);check(outer.Snapshot().pursuedPlayer==1,"crew reacquired after last beacon pulse");
 outer.Reset();players[0]=new UnityEngine.Vector3(-12,0,-12);players[1]=new UnityEngine.Vector3(12,0,25);outer.Tick(players,false,inputs,-1,true,.02f);
 check(outer.Snapshot().pursuedPlayer==0,"disconnected peer excluded in solo");
 outer.Reset();var atGate=outer.Snapshot().position;outer.Tick(players,false,inputs,-1,false,10);
 check(outer.Snapshot().pursuedPlayer==-1&&outer.Snapshot().position==atGate,"suppressed creature stays dormant");
 UnityEngine.Debug.Log("GLOBAL HUNT RULES PASS "+count);
}finally{
 foreach(var g in UnityEngine.Object.FindObjectsByType<UnityEngine.GameObject>(UnityEngine.FindObjectsInactive.Include))if(g!=null&&!before.Contains(g)&&g.transform.parent==null)UnityEngine.Object.DestroyImmediate(g);
}
"""
request=out/'rules-eval.json';request.write_text(json.dumps({'code':code,'timeout':60000}),encoding='utf-8')
r=subprocess.run(['python',str(R/'tools/unity_mcp.py'),'eval','--arguments-file',str(request)],capture_output=True,text=True,encoding='utf-8')
(out/('rules-'+time.strftime('%Y%m%d-%H%M%S')+'.json')).write_text(r.stdout,encoding='utf-8');(out/'rules-latest.json').write_text(r.stdout,encoding='utf-8');print(r.stdout)
data=json.loads(r.stdout)
if r.returncode!=0 or data.get('isError'):sys.exit(1)
inner=json.loads(data['content'][0]['text']);sys.exit(0 if inner.get('success') else 1)
