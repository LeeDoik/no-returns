"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/space-play-01'
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
 args+=['--test-dir',str(folders[role]),'-logFile',str(folders[role]/'player.log'),'-screen-width','960','-screen-height','600','-screen-fullscreen','0']
 si=subprocess.STARTUPINFO();si.dwFlags|=subprocess.STARTF_USESHOWWINDOW;si.wShowWindow=0
 p=subprocess.Popen(args,cwd=EXE.parent,startupinfo=si);procs.append(p);return p
try:
 command('host');command('client');hp=start('host')
 wait('host simulation starts',lambda:state('host').get('tick',0)>5)
 cp=start('client')
 wait('two processes connected',lambda:state('host').get('players')==2 and state('client').get('players')==2)
 command('host',z=1);command('client',z=1);time.sleep(.2);command('host',yaw=27);command('client',yaw=-27);time.sleep(.3)
 wait('both moved on authority',lambda:state('host').get('p0',{}).get('z',-5)>-4.7 and state('host').get('p1',{}).get('z',-5)>-4.7)
 command('host',yaw=27,interact=True)
 wait('host picks up parcel',lambda:state('host').get('holder')==0)
 wait('held parcel lifts clear of floor',lambda:state('host').get('cargo',{}).get('y',0)>.8)
 command('client',yaw=-27,interact=True);time.sleep(.4)
 wait('ownership contention rejected',lambda:state('host').get('holder')==0 and state('client').get('holder')==0)
 command('host',yaw=90);time.sleep(.7)
 wait('held parcel follows facing',lambda:abs(state('host').get('rotation',{}).get('y',0))>.6)
 command('host',yaw=90,pitch=-45)
 wait('host cargo follows upward view',lambda:state('host').get('cargo',{}).get('y',0)>1.8 and state('host').get('rotation',{}).get('x',0)<-.2)
 wait('vertical pose replicated',lambda:state('client').get('cargo',{}).get('y',0)>1.8)
 command('host',yaw=90,pitch=-45,capture=True);time.sleep(.6)
 if (folders['host']/'capture.png').exists():(folders['host']/'look-up.png').write_bytes((folders['host']/'capture.png').read_bytes())
 command('host',yaw=90,pitch=70);time.sleep(.7)
 down=state('host');q=down['rotation'];x,y,z,w=(q[k] for k in ('x','y','z','w'))
 support=abs(2*(x*y+z*w))*.4+abs(1-2*(x*x+z*z))*.325+abs(2*(y*z-x*w))*.325
 assert down['cargo']['y']<1.4 and down['cargo']['y']-support>-.025 and down['holder']==0,down
 checks.append('downward view stops cargo above floor')
 command('host',yaw=90,pitch=0);time.sleep(.4)
 command('host',yaw=90,interact=True)
 wait('drop replicated',lambda:state('host').get('holder')==-1 and state('client').get('holder')==-1)
 time.sleep(.6)
 command('client',yaw=-90,pitch=40,interact=True)
 wait('client picks up through server',lambda:state('host').get('holder')==1 and state('client').get('holder')==1)
 command('client',yaw=-90,pitch=-40)
 wait('client upward view applied by host',lambda:state('host').get('cargo',{}).get('y',0)>1.8)
 command('client',yaw=0,capture=True);time.sleep(.6)
 cp.terminate();cp.wait(timeout=5)
 wait('disconnect releases cargo',lambda:state('host').get('players')==1 and state('host').get('holder')==-1)
 command('host',reset=True);time.sleep(.3);command('host',z=1);time.sleep(2.5);command('host');time.sleep(.3)
 z=state('host')['p0']['z'];assert .7<z<1.5,('wall collision',z);checks.append('player blocked by wall')
 command('host',reset=True);time.sleep(.3);command('host',z=1);time.sleep(.2);command('host',yaw=27,interact=True)
 wait('pickup after reset',lambda:state('host').get('holder')==0)
 command('host',z=1);time.sleep(2.6);command('host');time.sleep(.3)
 s=state('host');assert .2<s['cargo']['z']<1.5 and s['holder']==0 and s['p0']['z']>-1.5,s;checks.append('held cargo travels then stops before wall')
 command('host',yaw=0,capture=True);time.sleep(1)
 assert (folders['host']/'capture.png').exists(),'render capture missing'
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two real local processes with scripted input, not human feel or WAN validation','final':state('host')}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
