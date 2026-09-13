"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/language'
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
def ui(role):
 try:return json.loads((folders[role]/'ui.json').read_text(encoding='utf-8'))
 except (OSError,ValueError):return {}
try:
 command('host');command('client');hp=start('host');wait('Korean default and glyph',lambda:ui('host').get('language')=='ko' and ui('host').get('host')=='방 만들기' and ui('host').get('koreanGlyph'))
 cp=start('client');wait('client defaults to Korean',lambda:ui('client').get('language')=='ko')
 command('client',toggleLanguage=True);wait('client switches to English',lambda:ui('client').get('language')=='en' and ui('client').get('host')=='HOST')
 wait('host language unchanged',lambda:ui('host').get('language')=='ko')
 cp.terminate();cp.wait(timeout=5);wait('client departure observed',lambda:state('host').get('players')==1)
 command('client');cp=start('client');wait('English preference survives restart',lambda:state('host').get('players')==2 and ui('client').get('language')=='en')
 command('client',toggleLanguage=True);wait('switches back to Korean',lambda:ui('client').get('language')=='ko')
 command('host',toggleLanguage=True);wait('host English independent',lambda:ui('host').get('language')=='en' and ui('client').get('language')=='ko')
 command('host',toggleLanguage=True);wait('both return to Korean',lambda:ui('host').get('language')=='ko' and ui('client').get('language')=='ko')
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two Windows processes; local toggle, glyph availability and PlayerPrefs restart; not rendered layout approval'}
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
