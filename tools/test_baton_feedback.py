"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/baton-feedback'
assert EXE.exists(), 'Missing playable SPACE-PLAY-01 Windows build'
OUT.mkdir(parents=True,exist_ok=True)
run=OUT/('run-'+time.strftime('%Y%m%d-%H%M%S'));run.mkdir()
folders={r:run/r for r in ('host','client')}
for p in folders.values():p.mkdir()
seq=0; procs=[]; checks=[]
def command(role,**kw):
 global seq
 seq+=1
 data=dict(seq=seq,x=0,z=0,yaw=0,pitch=0,interact=False,jump=False,reset=False)
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
def danger(role='host'):return state(role).get('danger') or {}
def go(role,x,z,quiet=True,pitch=0):
 until=time.monotonic()+25
 key='p0' if role=='host' else 'p1'
 while time.monotonic()<until:
  p=state('host').get(key)
  if not p:time.sleep(.03);continue
  dx=x-p['x'];dz=z-p['z']
  if math.hypot(dx,dz)<.18:command(role,quiet=quiet,pitch=pitch);time.sleep(.2);return
  command(role,quiet=quiet,pitch=pitch,yaw=math.degrees(math.atan2(dx,dz)),z=min(1,math.hypot(dx,dz)*2));time.sleep(.08)
 raise AssertionError('walk failed '+role+str((x,z))+str(state('host')))
def capture(role,name,**view):
 import shutil
 f=folders[role]/'capture.png';stamp=f.stat().st_mtime_ns if f.exists() else 0
 command(role,capture=True,quiet=True,**view)
 wait(name+' fresh capture',lambda:f.exists() and f.stat().st_mtime_ns>stamp)
 time.sleep(.05);shutil.copy2(f,run/(name+'.png'))

def face(role):
 p=state('host')['p0' if role=='host' else 'p1'];c=danger()['position'];return math.degrees(math.atan2(c['x']-p['x'],c['z']-p['z']))
try:
 command('host',quiet=True);command('client',quiet=True);start('host')
 wait('host initialized',lambda:state('host').get('hazard') is True)
 start('client');wait('peer initialized',lambda:state('client').get('hazard') is True)
 command('host',action=True);time.sleep(.4);command('host',action=True)
 wait('field active',lambda:state('host').get('phase')==2)
 capture('host','ready',yaw=0,pitch=0)
 command('host',quiet=True,shove=True)
 wait('host use replicated',lambda:danger().get('cooldown0',0)>5 and danger('client').get('cooldown0',0)>5)
 time.sleep(.55);capture('host','charging',yaw=0,pitch=0)
 time.sleep(2);capture('host','half',yaw=0,pitch=0)
 wait('ready restored on both peers',lambda:danger().get('cooldown0')==0 and danger('client').get('cooldown0')==0)
 capture('host','recharged',yaw=0,pitch=0)
 command('client',quiet=True,shove=True)
 wait('client use replicated',lambda:danger().get('cooldown1',0)>5 and danger('client').get('cooldown1',0)>5)
 capture('client','client-charging',yaw=0,pitch=0)
 report=dict(status='PASS',checks=checks,run=str(run),scope='Two-process scripted attack and rendered frames; native mouse hardware and human feel not tested.')
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
finally:
 for p in procs:
  if p.poll() is None:p.terminate();p.wait(timeout=5)
