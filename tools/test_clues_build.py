"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/space-play-06'
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
 command('host',inspect=True);time.sleep(.4);assert state('host')['clueMask']==0;checks.append('remote inspection rejected')
 for x,z in [(6,-2),(6,0)]:go('client',x,z,quiet=False)
 command('client',quiet=True,yaw=0,pitch=12,inspect=True);time.sleep(.4);assert state('host')['clueMask']==0;checks.append('facing away rejected')
 command('client',quiet=True,yaw=90,pitch=7,inspect=True)
 wait('client discovers first clue on both peers',lambda:state('host').get('clueMask')==1 and state('client').get('clueMask')==1)
 command('client',quiet=True,yaw=90,pitch=7,inspect=True);time.sleep(.4);assert state('host')['clueMask']==1 and state('host')['credits']==0;checks.append('duplicate inspection adds no pay')
 command('client',quiet=True,journal=True,z=1)
 wait('client journal opens',lambda:json.loads((folders['client']/'ui.json').read_text(encoding='utf-8')).get('journalOpen'))
 origin=state('host')['p1'];tick=state('host')['tick'];time.sleep(.6)
 now=state('host')['p1'];assert math.hypot(now['x']-origin['x'],now['z']-origin['z'])<.12 and state('host')['tick']>tick;checks.append('journal blocks local movement while host clock continues')
 command('client',quiet=True,toggleLanguage=True,capture=True)
 wait('English field log on client',lambda:'This depot' in json.loads((folders['client']/'ui.json').read_text(encoding='utf-8')).get('firstRecord',''))
 wait('full field log screenshot created',lambda:(folders['client']/'screen.png').exists())
 assert json.loads((folders['host']/'ui.json').read_text(encoding='utf-8'))['language']=='ko';checks.append('host remains Korean')
 command('client',quiet=True,journal=True)
 wait('client journal closes before movement',lambda:not json.loads((folders['client']/'ui.json').read_text(encoding='utf-8')).get('journalOpen'))
 for x,z in [(6,-2),(2.8,-2),(1,-5)]:go('client',x,z,quiet=False)
 go('host',-1,-4.2);command('host',quiet=True,yaw=27,pitch=20,interact=True);wait('cargo acquired',lambda:state('host').get('holder')==0)
 for x,z in [(2.8,-2),(2.8,11),(-6,11)]:go('host',x,z)
 go('host',-6,9.8,pitch=45);command('host',quiet=True,yaw=180,pitch=45);time.sleep(.35)
 command('host',quiet=True,yaw=0,pitch=12,inspect=True);time.sleep(.3)
 assert state('host')['clueMask']==1;checks.append('reception clue unavailable before delivery')
 command('host',quiet=True,yaw=180,pitch=45);time.sleep(.35)
 command('host',quiet=True,yaw=180,pitch=45,interact=True)
 wait('quiet route delivers with live listener',lambda:state('host').get('phase')==3 and state('client').get('phase')==3)
 assert state('host')['credits']==0 and state('client')['credits']==0 and danger()['hits']==0;checks.append('delivery is unpaid without damage')
 go('host',-6,7.5,quiet=True)
 go('host',-8.1,7.5,quiet=True)
 command('host',quiet=True,yaw=0,pitch=12,inspect=True)
 wait('post-delivery clue shared',lambda:state('host').get('clueMask')==3 and state('client').get('clueMask')==3)
 command('host',quiet=True,yaw=0,pitch=12,capture=True)
 wait('recorder screenshot created',lambda:(folders['host']/'screen.png').exists())
 command('host',quiet=True,yaw=0,pitch=12,interact=True)
 wait('shared receipt collected',lambda:state('host').get('receiptCollected') and state('client').get('receiptCollected'))
 for x,z in [(-8.1,5),(2.8,5),(2.8,-2),(-1,-5)]:go('host',x,z,quiet=False)
 command('host',action=True);wait('return retains notes and normal reward',lambda:state('client').get('phase')==4 and state('client').get('clueMask')==3 and state('client').get('credits')==420)
 command('host',journal=True,capture=True)
 wait('host opens Korean log after return',lambda:json.loads((folders['host']/'ui.json').read_text(encoding='utf-8')).get('journalOpen') and '직원 00' in json.loads((folders['host']/'ui.json').read_text(encoding='utf-8')).get('secondRecord',''))
 time.sleep(.5);command('host',journal=True);time.sleep(.3)
 command('host',action=True);wait('prepare preserves previous log',lambda:state('host').get('phase')==0 and state('host').get('clueMask')==3)
 command('host',action=True);time.sleep(.3);command('host',action=True)
 wait('next arrival clears both field logs',lambda:state('client').get('phase')==2 and state('host').get('clueMask')==0 and state('client').get('clueMask')==0)
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two real processes: view-based clues, post-receipt state, shared records, local journal input, bilingual text, normal rewards and shift reset. Human curiosity and fear are not covered.'}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
