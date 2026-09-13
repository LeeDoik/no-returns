"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/physical-beacon'
assert EXE.exists(), 'Missing playable SPACE-PLAY-01 Windows build'
OUT.mkdir(parents=True,exist_ok=True)
run=OUT/('run-'+time.strftime('%Y%m%d-%H%M%S'));run.mkdir()
folders={r:run/r for r in ('host','client')}
for p in folders.values():p.mkdir()
seq=0; procs=[]; checks=[]
def command(role,**kw):
 global seq
 seq+=1
 data=dict(seq=seq,x=0,z=0,yaw=0,pitch=0,interact=False,jump=False,reset=False,toggleLanguage=False,capture=False)
 data.update(kw)
 p=folders[role]/'input.tmp';p.write_text(json.dumps(data))
 for attempt in range(100):
  try:os.replace(p,folders[role]/'input.json');break
  except PermissionError:
   if attempt==99:raise
   time.sleep(.01)
def state(role):
 for attempt in range(3):
  try:return json.loads((folders[role]/'state.json').read_text())
  except (OSError,ValueError):time.sleep(.01)
 return {}
def wait(desc,condition,seconds=12):
 until=time.monotonic()+seconds
 while time.monotonic()<until:
  if condition():checks.append(desc);print('PASS',desc,flush=True);return
  time.sleep(.05)
 raise AssertionError(desc+' timed out; host='+str(state('host'))+' client='+str(state('client')))
def start(role):
 args=[str(EXE),'--host'] if role=='host' else [str(EXE),'--join','127.0.0.1']
 args+=['--hazard','--test-dir',str(folders[role]),'-logFile',str(folders[role]/'player.log'),'-screen-width','960','-screen-height','600','-screen-fullscreen','0']
 si=subprocess.STARTUPINFO();si.dwFlags|=subprocess.STARTF_USESHOWWINDOW;si.wShowWindow=0
 p=subprocess.Popen(args,cwd=EXE.parent,startupinfo=si);procs.append(p);return p
def go(x,z,pitch=0,role="host"):
 until=time.monotonic()+18
 while time.monotonic()<until:
  p=state('host').get('p0' if role=='host' else 'p1')
  if not p:time.sleep(.03);continue
  dx=x-p['x'];dz=z-p['z']
  if math.hypot(dx,dz)<.15:command(role,pitch=pitch);time.sleep(.2);return
  command(role,pitch=pitch,yaw=math.degrees(math.atan2(dx,dz)),z=min(1,math.hypot(dx,dz)*2));time.sleep(.08)
 raise AssertionError('Could not walk to '+str((x,z))+' '+str(state('host')))
def phase(n):return state('host').get('phase')==n and state('client').get('phase')==n
def action(role='host'):command(role,interact=True);time.sleep(.35)
def capture(role,name,**view):
 import shutil
 command(role,**view);time.sleep(.5) # Allow host-authoritative held position to follow the new view.
 f=folders[role]/'capture.png';stamp=f.stat().st_mtime_ns if f.exists() else 0
 command(role,capture=True,**view)
 wait(name+' screenshot',lambda:f.exists() and f.stat().st_mtime_ns>stamp)
 time.sleep(.2);shutil.copy2(f,run/(name+'.png'))

import hashlib,base64
save=dict(version=1,credits=120,deliveries=1,beacon=False)
save['checksum']=base64.b64encode(hashlib.sha256(b'1|120|1|0').digest()).decode()
(folders['host']/'progression-v1.json').write_text(json.dumps(save))
try:
 command('host');command('client');start('host');wait('host seeded test wallet',lambda:state('host').get('credits')==120)
 start('client');wait('partner joins',lambda:state('host').get('players')==2)
 command('host',buy=True);wait('purchase spawns shared beacon aboard',lambda:state('host').get('beaconExists') and state('client').get('beaconExists') and state('host').get('credits')==0)
 assert state('host')['beaconPosition']['z']==-7;checks.append('spawn is aboard')
 capture('host','aboard',yaw=207,pitch=32)
 go(-2,-5.8,role='client');command('client',yaw=180,pitch=45,interact=True)
 wait('client E picks up physical beacon',lambda:state('host').get('beaconCarrier')==1 and state('client').get('beaconCarrier')==1)
 capture('client','carried',yaw=180,pitch=0)
 command('client',yaw=180,drop=True);wait('Q stores aboard without use',lambda:state('host').get('beaconCarrier')==-1 and state('host').get('charges')==2)
 command('client',yaw=180,pitch=50,interact=True);wait('stored beacon can be picked up',lambda:state('host').get('beaconCarrier')==1)
 action();wait('E selects route',lambda:phase(1));action();wait('E departs with carried beacon',lambda:phase(2) and state('client').get('beaconCarrier')==1)
 for x,z in [(-2,-2)]:go(x,z,role='client')
 command('client',yaw=0,drop=True);wait('Q activates delivered beacon on both peers',lambda:state('host').get('beaconCarrier')==-1 and state('host').get('charges')==1 and state('client').get('beaconTime',0)>0)
 capture('client','active',yaw=0,pitch=50)
 command('host',deploy=True,pitch=40);time.sleep(.3);assert state('host')['charges']==1;checks.append('remote deployment cannot create a beacon')
 command('client',yaw=0,pitch=55,interact=True);time.sleep(.3);assert state('host')['beaconCarrier']==-1;checks.append('active beacon cannot be picked up')
 go(-2,-3.5,role='client')
 wait('signal ends without removing item',lambda:state('host').get('beaconTime')==0 and state('host').get('beaconExists'),seconds=12)
 go(-2,-2,role='client')
 command('client',yaw=0,pitch=55,interact=True);wait('spent signal device can be carried again',lambda:state('host').get('beaconCarrier')==1)
 report=dict(status='PASS',checks=checks,run=str(run),scope='Two processes with a seeded test wallet; real movement, purchase, carry, place and activation. Not art or human-feel approval.')
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps(dict(status='FAIL',checks=checks,error=str(e),run=str(run)),indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate();p.wait(timeout=5)
