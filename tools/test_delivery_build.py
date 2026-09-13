"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/space-play-02'
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
 args+=['--delivery','--test-dir',str(folders[role]),'-logFile',str(folders[role]/'player.log'),'-screen-width','960','-screen-height','600','-screen-fullscreen','0']
 si=subprocess.STARTUPINFO();si.dwFlags|=subprocess.STARTF_USESHOWWINDOW;si.wShowWindow=0
 p=subprocess.Popen(args,cwd=EXE.parent,startupinfo=si);procs.append(p);return p
def go(x,z,pitch=0):
 until=time.monotonic()+18
 while time.monotonic()<until:
  p=state('host').get('p0')
  if not p:time.sleep(.03);continue
  dx=x-p['x'];dz=z-p['z']
  if math.hypot(dx,dz)<.15:command('host',pitch=pitch);time.sleep(.2);return
  command('host',pitch=pitch,yaw=math.degrees(math.atan2(dx,dz)),z=min(1,math.hypot(dx,dz)*2));time.sleep(.08)
 raise AssertionError('Could not walk to '+str((x,z))+' '+str(state('host')))
def phase(n):return state('host').get('phase')==n and state('client').get('phase')==n
def action(role='host'):command(role,interact=True);time.sleep(.35)
try:
 command('host');command('client');hp=start('host');wait('ship preparation',lambda:state('host').get('phase')==0)
 cp=start('client');wait('crew joins before departure',lambda:state('host').get('players')==2)
 if json.loads((folders['host']/'ui.json').read_text(encoding='utf-8'))['language']!='ko':
  command('host',toggleLanguage=True);wait('Korean initial language',lambda:json.loads((folders['host']/'ui.json').read_text(encoding='utf-8'))['language']=='ko')
 action();wait('route selected on both peers',lambda:phase(1))
 action();wait('arrival replicated',lambda:phase(2))
 action();wait('empty return settlement',lambda:phase(4))
 assert state('host')['credits']==0;checks.append('no payment for empty return')
 action();wait('next shift preparation',lambda:phase(0))
 action();action();wait('second arrival',lambda:phase(2))
 go(-1,-4.2);command('host',yaw=27,pitch=20,interact=True);wait('mission cargo pickup',lambda:state('host').get('holder')==0)
 for x,z in [(1,-2),(1,5),(-6,5)]:go(x,z)
 go(-6,8.2,pitch=45)
 command('host',pitch=45);time.sleep(.4)
 assert state('host')['phase']==2 and state('host')['credits']==0;checks.append('held parcel not accepted')
 command('host',pitch=45,interact=True);time.sleep(.3)
 assert state('host')['holder']==0;checks.append('E does not drop held cargo')
 command('host',pitch=45,drop=True)
 wait('scan progress replicated',lambda:0<state('host').get('receiptProgress',0)<1 and 0<state('client').get('receiptProgress',0)<1,seconds=8)
 wait('receipt replicated',lambda:phase(3),seconds=8)
 command('host',yaw=-35,pitch=28,capture=True);time.sleep(.6)
 import shutil
 shutil.copy2(folders['host']/'capture.png',run/'receipt-complete.png')
 command('host',toggleLanguage=True);wait('English language applied',lambda:json.loads((folders['host']/'ui.json').read_text(encoding='utf-8'))['language']=='en')
 stamp=(folders['host']/'capture.png').stat().st_mtime_ns
 command('host',yaw=-35,pitch=28,capture=True)
 wait('fresh English terminal capture',lambda:(folders['host']/'capture.png').stat().st_mtime_ns>stamp)
 time.sleep(.2)
 assert json.loads((folders['host']/'ui.json').read_text(encoding='utf-8'))['language']=='en';checks.append('English toggle')
 shutil.copy2(folders['host']/'capture.png',run/'receipt-english.png')
 command('host',toggleLanguage=True);time.sleep(.2)
 wait('delivery alone does not pay',lambda:state('host').get('credits')==0 and state('client').get('credits')==0)
 command('host',interact=True);time.sleep(.3)
 assert not state('host').get('receiptCollected');checks.append('remote receipt pickup rejected')
 go(-8.1,7.5);command('host',yaw=0,pitch=12,interact=True)
 wait('receipt pickup replicated',lambda:state('host').get('receiptCollected') and state('client').get('receiptCollected'))
 assert state('host')['credits']==0;checks.append('pickup alone does not pay')
 action();assert state('host')['phase']==3;checks.append('remote return action rejected')
 action('client');assert state('host')['phase']==3;checks.append('return blocked while partner remains outside')
 command('host',interact=True);time.sleep(.3);assert state('host')['holder']==-1;checks.append('accepted parcel cannot be reclaimed')
 for x,z in [(-8.1,5),(1,5),(1,-3),(-1,-5)]:go(x,z)
 action();wait('return settlement replicated',lambda:phase(4))
 assert state('host')['credits']==420 and state('client')['credits']==420;checks.append('return bonus paid once')
 time.sleep(.5);assert state('host')['credits']==420;checks.append('receipt does not repay during settlement')
 command('host',capture=True);time.sleep(.6)
 action();wait('next shift retains wallet',lambda:phase(0) and state('host').get('credits')==420)
 wait('scanner reset on both peers',lambda:state('host').get('receiptProgress')==0 and state('client').get('receiptProgress')==0)
 action();action();wait('third arrival',lambda:phase(2))
 cp.terminate();cp.wait(timeout=5)
 wait('disconnect aborts without return pay',lambda:state('host').get('phase')==4 and state('host').get('credits')==420 and state('host').get('returnPay')==0)
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two real processes, scripted movement; not human fun, production map or persistence validation','final':state('host')}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()

