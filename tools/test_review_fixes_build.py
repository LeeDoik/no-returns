"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/review-fixes'
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
def stop(p):
 if p.poll() is None:p.terminate();p.wait(timeout=5)
try:
 command('host',quiet=True);command('client',quiet=True);start('host');wait('hazard host starts',lambda:state('host').get('hazard'))
 cp=start('client');wait('partner connected',lambda:state('host').get('players')==2)
 command('host',action=True);time.sleep(.4);command('host',action=True);wait('arrive',lambda:state('host').get('phase')==2)
 go('host',1,-2);go('host',1,3.4);command('host',quiet=True,call=True)
 wait('host down before disconnect',lambda:danger().get('down0') and danger('client').get('down0'))
 stop(cp)
 wait('disconnect recovers downed host at ship report',lambda:state('host').get('phase')==4 and not danger().get('down0') and state('host')['p0']['z']< -3.8)
 before=state('host')['p0']['z'];command('host',z=1);time.sleep(.15);command('host')
 wait('recovered host can move',lambda:state('host')['p0']['z']>before+.2)
 cp=start('client')
 wait('rejoin refusal states shift reason',lambda:'Shift in progress' in state('client').get('message',''))
 ui=json.loads((folders['client']/'ui.json').read_text(encoding='utf-8'));assert ui['language']=='ko' and '근무' in ui['status'];checks.append('rejoin reason localized in Korean')
 stop(cp);command('host',action=True);wait('host can prepare without recreating session',lambda:state('host')['phase']==0)
 command('client',quiet=True);cp=start('client');wait('rejoin accepted in preparation',lambda:state('host')['players']==2 and state('client').get('connected'))
 command('host',action=True);time.sleep(.4);command('host',action=True);wait('second arrival',lambda:state('host')['phase']==2)
 for x,z in [(2.8,-2),(2.8,10),(0,10),(0,8)]:go('host',x,z)
 assert state('host')['p0']['y']>.28;checks.append('real character can stand on low step')
 next_call=[0]
 def provoke_step():
  if time.monotonic()>next_call[0]:command('host',quiet=True,call=True);next_call[0]=time.monotonic()+.6
  return danger().get('down0') and danger('client').get('down0')
 wait('listener climbs step and downs host on both peers',provoke_step,seconds=25)
 assert danger()['position']['y']>.25;checks.append('listener body elevated on platform')
 stop(cp);wait('second down disconnect also recovers',lambda:state('host')['phase']==4 and not danger()['down0'])
 command('host',action=True);wait('prepare for physical gap test',lambda:state('host')['phase']==0)
 for x,z in [(-12,-3),(-12,17),(-15.36,17)]:go('host',x,z,quiet=False)
 command('host',yaw=180,z=1);time.sleep(1.3);command('host',quiet=True)
 assert state('host')['p0']['z']>14;checks.append('real character blocked from former west-rack safety slit')
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Actual two Windows players and ordinary scripted input. No human feel/fun or WAN claim.'}
 (run/'report.json').write_text(json.dumps(report,indent=2));(OUT/'latest.json').write_text(json.dumps(report,indent=2));print('PASS',len(checks),'checks',run,flush=True)
except Exception as e:
 report={'status':'FAIL','checks':checks,'error':str(e),'run':str(run)};(run/'report.json').write_text(json.dumps(report,indent=2));(OUT/'latest.json').write_text(json.dumps(report,indent=2));raise
finally:
 for p in procs:stop(p)
