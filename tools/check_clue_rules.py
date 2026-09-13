"""Editor physics checks for clue visibility and receipt gates; disposable fixtures."""
from pathlib import Path
import json,subprocess
R=Path(__file__).resolve().parents[1];out=R/'artifacts/space-play-06';out.mkdir(parents=True,exist_ok=True)
code='''
var c=new NoReturns.CarryLab.CarryClues();UnityEngine.GameObject wall=null;
var terminals=(UnityEngine.GameObject[])typeof(NoReturns.CarryLab.CarryClues).GetField("terminals",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).GetValue(c);
int count=0;System.Action<bool,string> check=(ok,label)=>{if(!ok)throw new System.Exception(label);count++;};
try{
 c.Display(true,false);UnityEngine.Physics.SyncTransforms();
 var first=new UnityEngine.Vector3(6,1.57f,0);var east=UnityEngine.Quaternion.Euler(7,90,0);
 var second=new UnityEngine.Vector3(-6,1.57f,10.6f);var west=UnityEngine.Quaternion.Euler(7,-90,0);
 check(c.Target(first,east)==0,"first terminal aimed");
 c.Inspect(first,east,0);check(c.Mask==0,"ship phase blocked");
 c.Inspect(first,UnityEngine.Quaternion.Euler(0,-90,0),2);check(c.Mask==0,"facing away blocked");
 c.Inspect(new UnityEngine.Vector3(3,1.57f,0),east,2);check(c.Mask==0,"remote blocked");
 wall=UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Cube);wall.transform.position=new UnityEngine.Vector3(6.7f,1.4f,0);wall.transform.localScale=new UnityEngine.Vector3(.2f,2,2);UnityEngine.Physics.SyncTransforms();
 c.Inspect(first,east,2);check(c.Mask==0,"wall occlusion blocks discovery");UnityEngine.Object.DestroyImmediate(wall);wall=null;UnityEngine.Physics.SyncTransforms();
 c.Inspect(first,east,2);check(c.Mask==1,"first discovery");c.Inspect(first,east,2);check(c.Mask==1,"duplicate stays single");
 check(c.Target(second,west)==1,"recorder actually targeted");
 string idle=c.Inspect(second,west,2);check(c.Mask==1&&idle.StartsWith("Recorder idle"),"receipt gate without cargo occlusion");
 c.Display(true,true);c.Inspect(second,west,3);check(c.Mask==3,"receipt unlocks second clue");
 c.Clear();check(c.Mask==0,"new shift reset");
 UnityEngine.Debug.Log("SPACE-PLAY-06 CLUE RULES PASS "+count);
}finally{if(wall!=null)UnityEngine.Object.DestroyImmediate(wall);foreach(var g in terminals)UnityEngine.Object.DestroyImmediate(g);}
'''
request=out/'rules-eval.json';request.write_text(json.dumps({'code':code,'timeout':5000}),encoding='utf-8')
r=subprocess.run(['python',str(R/'tools/unity_mcp.py'),'eval','--arguments-file',str(request)],capture_output=True,text=True,encoding='utf-8');(out/'rules-result.json').write_text(r.stdout,encoding='utf-8');print(r.stdout)
data=json.loads(r.stdout);inner=json.loads(data['content'][0]['text']);raise SystemExit(0 if r.returncode==0 and inner.get('success') else 1)
