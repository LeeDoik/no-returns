"""Exercise disk failure/recovery in isolated Editor-side fixtures, never player saves."""
from pathlib import Path
import json,subprocess,time
R=Path(__file__).resolve().parents[1]
out=R/'artifacts/space-play-05';out.mkdir(parents=True,exist_ok=True)
fixture=out/('rules-'+time.strftime('%Y%m%d-%H%M%S'));fixture.mkdir()
code='string root='+json.dumps(fixture.as_posix())+';\n'+'''
int count=0;System.Action<bool,string> check=(ok,label)=>{if(!ok)throw new System.Exception(label);count++;};
string path=System.IO.Path.Combine(root,"progression-v1.json");
var store=new NoReturns.CarryLab.CarrySave(path);NoReturns.CarryLab.CarryProgress p;string message;
check(store.Load(out p,out message)&&p==null,"missing starts new");
var m=new NoReturns.CarryLab.CarryMission();check(store.Write(m,out message),"first write");
m.Act(true,true);m.Act(true,true);m.Tick(new UnityEngine.Vector3(-6,1.225f,9),UnityEngine.Vector3.zero,-1,1);
check(store.Write(m,out message),"receipt write");
check(new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&p.credits==300,"receipt round trip");
var resumed=new NoReturns.CarryLab.CarryMission(p);
check(resumed.Phase==0&&resumed.Receipt==0&&resumed.ReturnPay==0&&resumed.SuccessfulDeliveries==1,"no unfinished bonus restored");
m.Act(true,true);check(store.Write(m,out message),"return write");
string good=System.IO.File.ReadAllText(path);
System.IO.File.WriteAllText(path+".tmp","partial interrupted write");
check(new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&p.credits==420,"temporary file ignored");
System.IO.File.WriteAllText(path,"broken JSON");
var recovery=new NoReturns.CarryLab.CarrySave(path);
check(recovery.Load(out p,out message)&&p.credits==300&&message.StartsWith("Recovered"),"fallback to previous receipt");
check(recovery.Write(new NoReturns.CarryLab.CarryMission(p),out message),"repair primary");
check(new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&p.credits==300,"repaired primary readable");
System.IO.File.Delete(path);
check(new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&p.credits==300,"missing primary uses backup");
System.IO.File.WriteAllText(path,good.Replace("420","421"));System.IO.File.WriteAllText(path+".bak","bad");
string broken=System.IO.File.ReadAllText(path);
check(!new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&System.IO.File.ReadAllText(path)==broken,"checksum rejection preserves files");
System.IO.File.WriteAllText(path,good.Replace("\\"version\\": 1","\\"version\\": 2"));System.IO.File.WriteAllText(path+".bak",good);
check(!new NoReturns.CarryLab.CarrySave(path).Load(out p,out message)&&message.StartsWith("Newer"),"future version refuses backup downgrade");
check(!new NoReturns.CarryLab.CarrySave(System.IO.Path.Combine(path,"blocked.json")).Write(m,out message)&&message.StartsWith("SAVE FAILED"),"write failure visible");
UnityEngine.Debug.Log("SPACE-PLAY-05 SAVE RULES PASS "+count);
'''
request=out/'rules-eval.json';request.write_text(json.dumps({'code':code,'timeout':5000}),encoding='utf-8')
r=subprocess.run(['python',str(R/'tools/unity_mcp.py'),'eval','--arguments-file',str(request)],capture_output=True,text=True,encoding='utf-8')
(out/'rules-result.json').write_text(r.stdout,encoding='utf-8');print(r.stdout)
data=json.loads(r.stdout);inner=json.loads(data['content'][0]['text'])
raise SystemExit(0 if r.returncode==0 and inner.get('success') else 1)
