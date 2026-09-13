"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/space-play-03-delivery'
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
def face(role):
 p=state('host')['p0' if role=='host' else 'p1'];c=danger()['position'];return math.degrees(math.atan2(c['x']-p['x'],c['z']-p['z']))
try:
 command('host',quiet=True);command('client',quiet=True);hp=start('host');wait('hazard host starts',lambda:state('host').get('hazard') is True)
 cp=start('client');wait('hazard partner connected',lambda:state('host').get('players')==2)
 command('host',action=True);time.sleep(.4);command('host',action=True);wait('hazard delivery begins',lambda:state('host').get('phase')==2)
 go('host',-1,-4.2);command('host',quiet=True,yaw=27,pitch=20,interact=True);wait('cargo acquired',lambda:state('host').get('holder')==0)
 for x,z in [(2.8,-2),(2.8,11),(-6,11)]:go('host',x,z)
 go('host',-6,10.35,pitch=-20);command('host',quiet=True,yaw=180,pitch=-20);time.sleep(.35)
 command('host',quiet=True,yaw=180,pitch=-20,interact=True)
 wait('quiet route delivers with live listener',lambda:state('host').get('phase')==3 and state('client').get('phase')==3)
 assert state('host')['credits']==300 and state('client')['credits']==300 and danger()['hits']==0;checks.append('both peers secure pay without damage')
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Quiet delivery under active creature through an alternate route; return under pursuit and human fun not covered'}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
