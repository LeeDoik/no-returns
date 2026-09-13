"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/global-hunt'
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
def outer(role='host'):return state(role).get('outerDanger') or {}
def posdist(a,b):return math.sqrt(sum((a[k]-b[k])**2 for k in ('x','y','z')))
try:
 command('host',quiet=True);command('client',quiet=True);start('host')
 wait('host starts',lambda:state('host').get('hazard'))
 start('client');wait('two actual players connected',lambda:state('client').get('hazard') and state('host').get('players')==2)
 command('host',action=True);time.sleep(.4);command('host',action=True)
 wait('arrival starts suppression',lambda:state('host').get('phase')==2)
 for x,z in [(1,-11),(-12,-11),(-12,-12)]:go('client',x,z,quiet=False)
 command('client',quiet=True);command('host',quiet=True)
 for stage in (1,2,3):wait('matching suppression stage '+str(stage),lambda:state('host')['suppressionStage']==stage and state('client')['suppressionStage']==stage,seconds=100)
 wait('global target acquired after entry',lambda:outer().get('pursuedPlayer')==1 and outer('client').get('pursuedPlayer')==1,seconds=12)
 assert posdist(outer()['position'],state('host')['p1'])>35;checks.append('silent target acquired over 35m away behind walls')
 assert outer()['noises']==0;checks.append('acquisition needs no call or nearby footstep')
 # Change destination while the creature is still far away; chase must update.
 go('client',12,-12,quiet=False);command('client',quiet=True)
 wait('moving target destination updates on both peers',lambda:outer()['noiseTarget']['x']>11 and outer('client')['noiseTarget']['x']>11)
 wait('outer reaches southern map beyond old grid',lambda:outer()['position']['z']< -8,seconds=40)
 assert not danger()['down0'];checks.append('aboard host is not attacked')
 wait('client warning agrees before southern attack',lambda:outer()['state']==2 and outer('client')['state']==2,seconds=20)
 wait('southern client down replicated',lambda:danger()['down1'] and danger('client')['down1'])
 downPosition=outer()['position'];assert downPosition['z']< -10;checks.append('attack occurs near actual far-south target')
 wait('down and aboard crew are no longer pursued',lambda:outer()['pursuedPlayer']==-1 and outer('client')['pursuedPlayer']==-1,seconds=7)
 # Move the remaining employee just outside the ship and stop before attack.
 go('host',5,-3,quiet=False);command('host',quiet=True)
 wait('remaining outside employee becomes new target',lambda:outer()['pursuedPlayer']==0 and outer('client')['pursuedPlayer']==0)
 wait('global hunter all-down recovery agrees',lambda:state('host')['phase']==4 and state('client')['phase']==4,seconds=30)
 assert not danger()['down0'] and not danger()['down1'] and state('host')['credits']==0;checks.append('recovery preserves existing no-delivery payment rule')
 for phase in (0,1,2):
  command('host',action=True);wait('new shift phase '+str(phase),lambda:state('host')['phase']==phase)
 wait('next arrival resets hunt and suppression',lambda:state('client')['suppressionStage']==0 and outer()['pursuedPlayer']==-1 and outer('client')['pursuedPlayer']==-1 and outer()['position']['z']==28)
 report={'status':'PASS','checks':checks,'run':str(run),'downPosition':downPosition,'scope':'Two actual Windows players, ordinary input and real elapsed time. Not human difficulty, sound readability, WAN or fun validation.'}
 (run/'report.json').write_text(json.dumps(report,indent=2));(OUT/'latest.json').write_text(json.dumps(report,indent=2));print('PASS',len(checks),'checks',run,flush=True)
except Exception as e:
 report={'status':'FAIL','checks':checks,'error':str(e),'run':str(run)};(run/'report.json').write_text(json.dumps(report,indent=2));(OUT/'latest.json').write_text(json.dumps(report,indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
