"""Create an Editor rule check; no changes to a running player's wallet or location."""
from pathlib import Path
import json,subprocess
R=Path(__file__).resolve().parents[1]
out=R/'artifacts/space-play-04';out.mkdir(exist_ok=True,parents=True)
code='''
var m=new NoReturns.CarryLab.CarryMission();
System.Action<bool,string> check=(ok,label)=>{if(!ok)throw new System.Exception(label);};
check(!m.BuyBeacon(true,true),"empty wallet");
m.Act(true,true);check(!m.ToggleContract(true,true),"locked until delivery");m.Act(true,true);
m.Tick(new UnityEngine.Vector3(-6,1.225f,9),UnityEngine.Vector3.zero,-1,1);
check(m.Credits==300 && m.SuccessfulDeliveries==1,"standard receipt");
check(!m.BuyBeacon(true,true),"no purchase during delivery");
m.Act(true,true);check(m.Credits==420,"standard return");
check(!m.BuyBeacon(false,true)&&!m.BuyBeacon(true,false),"purchase authority/location");
check(m.BuyBeacon(true,true)&&m.Credits==300,"purchase");
check(!m.BuyBeacon(true,true)&&m.Credits==300,"duplicate purchase");
m.Act(true,true);m.Act(true,true);
check(!m.ToggleContract(false,true)&&!m.ToggleContract(true,false),"contract authority/location");
check(m.ToggleContract(true,true)&&m.HardContract,"risk unlocked");m.Act(true,true);
check(!m.ToggleContract(true,true),"field locked");
m.Tick(new UnityEngine.Vector3(-6,1.225f,9),UnityEngine.Vector3.zero,-1,1);
check(m.Credits==750&&m.Receipt==450,"risk receipt");
m.Tick(new UnityEngine.Vector3(-6,1.225f,9),UnityEngine.Vector3.zero,-1,1);
check(m.Credits==750&&m.SuccessfulDeliveries==2,"single receipt");
m.Act(true,true);check(m.Credits==930&&m.ReturnPay==180,"risk return");
m.Abort();check(m.Credits==930,"settled abort unchanged");m.Act(true,true);
check(!m.HardContract&&m.BeaconUnlocked&&m.Credits==930,"new shift retains license and resets route");
var equipment=new NoReturns.CarryLab.CarryEquipment();equipment.Begin(true);
check(!equipment.Deploy(UnityEngine.Vector3.zero,UnityEngine.Quaternion.identity,false,m)&&equipment.Charges==2,"blocked deploy preserves stock");
UnityEngine.Debug.Log("SPACE-PLAY-04 RULES PASS: 17 ledger, authority, reset and rejection assertions");
'''
p=out/'rules-eval.json';p.write_text(json.dumps({'code':code,'timeout':5000}),encoding='utf-8')
r=subprocess.run(['python',str(R/'tools/unity_mcp.py'),'eval','--arguments-file',str(p)],capture_output=True,text=True,encoding='utf-8')
(out/'rules-result.json').write_text(r.stdout,encoding='utf-8');print(r.stdout);raise SystemExit(r.returncode)
