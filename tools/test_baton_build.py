"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/baton'
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
 command('host',quiet=True);command('client',quiet=True);hp=start('host')
 wait('hazard mode initialized',lambda:state('host').get('hazard') is True)
 cp=start('client');wait('hazard mode replicated',lambda:state('client').get('hazard') is True)
 capture('host','ready',yaw=0,pitch=0)
 command('host',action=True);time.sleep(.4);command('host',action=True)
 wait('arrive with listener active',lambda:state('host').get('phase')==2)
 go('host',-1,-4.2);command('host',quiet=True,yaw=27,pitch=20,interact=True)
 wait('host carries cargo into threat test',lambda:state('host').get('holder')==0)
 go('host',1,-2);go('host',1,3.4)
 assert danger()['noises']==0 and not danger()['down0'];checks.append('quiet approach does not create step noise')
 command('host',quiet=True,call=True)
 wait('call attracts listener',lambda:danger().get('state')==1 and danger().get('noises',0)>0)
 wait('warning occurs before hit',lambda:danger().get('state')==2 and not danger().get('down0'))
 wait('host down replicated',lambda:danger().get('down0') and danger('client').get('down0'))
 capture('host','down',yaw=0,pitch=0)
 wait('down releases cargo',lambda:state('host').get('holder')==-1 and state('client').get('holder')==-1)
 before=state('host')['p0'];command('host',z=1,interact=True);time.sleep(.35)
 after=state('host')['p0'];assert math.dist(list(before.values()),list(after.values()))<.05;checks.append('down blocks movement and pickup')
 command('client',rescue=True);time.sleep(.4);assert danger()['rescue1']==0;checks.append('remote rescue rejected')
 go('client',2.5,-2,quiet=False);go('client',2.5,3.4,quiet=False)
 next_bait=[0]
 def bait_warning():
  if time.monotonic()>next_bait[0]:command('client',quiet=True,call=True);next_bait[0]=time.monotonic()+.5
  return danger().get('state')==2 and not danger().get('down1')
 wait('client baits a close attack warning',bait_warning)
 command('client',quiet=True,yaw=face('client'),shove=True,capture=True)
 wait('client shove stuns on host and client',lambda:danger().get('state')==4 and danger('client').get('state')==4)
 capture('client','strike',yaw=face('client'),pitch=0)
 command('client',quiet=True,rescue=True);wait('rescue progress starts',lambda:danger().get('rescue1',0)>.4)
 command('client',quiet=True,shove=True,yaw=face('client'));time.sleep(.2)
 assert danger()['cooldown1']<5.7 and danger()['down0'];checks.append('shove cooldown prevents immediate reset')
 wait('released rescue input cancels progress',lambda:danger().get('rescue1')==0)
 command('client',quiet=True,rescue=True)
 wait('teammate rescued on both peers',lambda:danger().get('rescues',0)>=1 and not danger().get('down0') and not danger('client').get('down0'))
 command('client',quiet=True);command('host',quiet=True,yaw=0,pitch=45,interact=True)
 wait('dropped cargo can be recovered',lambda:state('host').get('holder')==0)
 next_call=[0]
 def provoke_until_recovered():
  if time.monotonic()>next_call[0]:
   command('host',quiet=True,call=True);command('client',quiet=True,call=True);next_call[0]=time.monotonic()+.5
  return danger().get('evacuations',0)>=1 and state('host').get('phase')==4
 wait('all-down recovery reaches ship report',provoke_until_recovered,seconds=35)
 wait('recovered crew and report replicated',lambda:not danger().get('down0') and not danger().get('down1') and state('client').get('phase')==4)
 assert state('host')['credits']==0 and state('host')['p0']['z']<-3.8;checks.append('undelivered evacuation pays nothing')
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two actual processes and ordinary scripted input; not human fear, sound readability or fun validation','final':state('host')}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
