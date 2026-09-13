"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/space-play-07'
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
 command('host',quiet=True);command('client',quiet=True);start('host')
 wait('expanded world initialized',lambda:state('host').get('hazard'))
 start('client');wait('protocol 6 connected',lambda:state('client').get('protocol')==6 and state('host').get('players')==2)
 # Physical walking through all three wings while hazards are inactive aboard.
 route=[(10,-2),(10,8),(13,16),(13,27),(13,29),(8,29),(4,29),(-9,29),(-15,29),(-15,22),(-15,17),(-12,17),(-12,2),(-9,2),(-9,-2),(-1,-5)]
 for n,(x,z) in enumerate(route):
  go('host',x,z,quiet=False);checks.append('expanded walking waypoint '+str(n))
 print('PASS expanded loop physically traversed',flush=True)
 assert state('host')['shiftElapsed']==0;checks.append('preparation does not consume suppression')
 command('host',action=True);time.sleep(.4);command('host',action=True)
 wait('arrival starts suppression',lambda:state('host').get('phase')==2 and state('host').get('suppressionStage')==0)
 go('client',7,-2,quiet=False)
 command('client',quiet=True);command('host',quiet=True)
 samples=[]
 for stage in (1,2,3):
  wait('stage '+str(stage)+' agrees across processes',lambda:state('host').get('suppressionStage')==stage and state('client').get('suppressionStage')==stage,seconds=100)
  samples.append(state('host'))
  assert abs(state('host')['shiftElapsed']-state('client')['shiftElapsed'])<1
  assert state('host')['outerDanger']['position']=={'x':13.0,'y':0.0,'z':28.0}
  checks.append('outer remains at gate through stage '+str(stage))
 wait('entry grace lasts at least 7 seconds',lambda:state('host').get('shiftElapsed',0)>187,seconds=10)
 assert state('host')['outerDanger']['position']['z']==28;checks.append('entry does not teleport beside crew')
 wait('outer enters after grace',lambda:state('host')['outerDanger']['position']['z']<27.8)
 wait('outer traverses east dogleg',lambda:state('host')['outerDanger']['position']['z']<5,seconds=35)
 wait('outer attack is warned on both peers',lambda:state('host')['outerDanger']['state']==2 and state('client')['outerDanger']['state']==2,seconds=30)
 p=state('host')['p1'];c=state('host')['outerDanger']['position']
 command('client',quiet=True,shove=True,yaw=math.degrees(math.atan2(c['x']-p['x'],c['z']-p['z'])))
 time.sleep(.15);assert state('host')['outerDanger']['state']!=4;checks.append('shove cannot stun outer')
 wait('outer down shares listener rescue state',lambda:danger().get('down1') and danger('client').get('down1') and state('host')['outerDanger']['hits']>0)
 assert not danger()['down0'];checks.append('aboard host remains safe')
 go('host',5,-3,quiet=False);command('host',quiet=True,call=True)
 wait('outer all-down recovery replicated',lambda:state('host').get('phase')==4 and state('client').get('phase')==4,seconds=30)
 assert not danger()['down0'] and not danger()['down1'] and state('host')['credits']==0;checks.append('emergency recovery restores crew with no false pay')
 before=state('host')['shiftElapsed'];time.sleep(.4);assert state('host')['shiftElapsed']==before;checks.append('report stops suppression clock')
 for phase in (0,1,2):
  command('host',action=True);wait('advance phase '+str(phase),lambda:state('host').get('phase')==phase)
 wait('next arrival resets both clocks and outer',lambda:state('host')['suppressionStage']==0 and state('client')['suppressionStage']==0 and state('host')['shiftElapsed']<3 and state('host')['outerDanger']['position']['z']==28)
 report={'status':'PASS','checks':checks,'samples':samples,'run':str(run),'scope':'Actual two Windows processes; ordinary movement and real elapsed time. Not human readability, fear or fun validation.'}
 (run/'report.json').write_text(json.dumps(report,indent=2));(OUT/'latest.json').write_text(json.dumps(report,indent=2));print('PASS',len(checks),'checks',run,flush=True)
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
