"""Drive two real Windows players through ordinary input; no simulation teleport API."""
from pathlib import Path
import json, os, subprocess, time, math, sys
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
SAVE_TEST='--save-restart' in sys.argv
OUT=ROOT/('artifacts/space-play-05' if SAVE_TEST else 'artifacts/space-play-04')
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
 command('host',buy=True);time.sleep(.4)
 assert not state('host')['unlocked'] and state('host')['credits']==0;checks.append('insufficient funds rejected')
 command('host',action=True);time.sleep(.4);command('host',action=True);wait('hazard delivery begins',lambda:state('host').get('phase')==2)
 go('host',-1,-4.2);command('host',quiet=True,yaw=27,pitch=20,interact=True);wait('cargo acquired',lambda:state('host').get('holder')==0)
 for x,z in [(2.8,-2),(2.8,11),(-6,11)]:go('host',x,z)
 go('host',-6,10.35,pitch=-20);command('host',quiet=True,yaw=180,pitch=-20);time.sleep(.35)
 command('host',quiet=True,yaw=180,pitch=-20,interact=True)
 wait('quiet route delivers with live listener',lambda:state('host').get('phase')==3 and state('client').get('phase')==3)
 assert state('host')['credits']==300 and state('client')['credits']==300 and danger()['hits']==0;checks.append('both peers secure pay without damage')
 for x,z in [(-6,12.5),(2.8,12.5),(2.8,-2),(-1,-5)]:go('host',x,z,quiet=False)
 command('host',action=True);wait('return pays both peers 420',lambda:state('host').get('credits')==420 and state('client').get('credits')==420)
 command('client',buy=True);time.sleep(.4)
 assert not state('host')['unlocked'] and state('host')['credits']==420;checks.append('client purchase denied')
 command('host',buy=True);wait('license and 300 balance replicated',lambda:state('host').get('unlocked') and state('client').get('unlocked') and state('client').get('credits')==300)
 command('host',buy=True);time.sleep(.4);assert state('host')['credits']==300;checks.append('duplicate purchase does not charge')
 command('host',action=True);wait('next shift prepared',lambda:state('host').get('phase')==0)
 command('host',action=True);wait('route selected',lambda:state('host').get('phase')==1)
 command('client',contract=True);time.sleep(.4);assert not state('host')['hard'];checks.append('client contract change rejected')
 command('host',contract=True);wait('risk selection replicated',lambda:state('host').get('hard') and state('client').get('hard'))
 command('host',action=True);wait('arrival refills two shared charges',lambda:state('host').get('phase')==2 and state('client').get('charges')==2)
 command('host',contract=True);time.sleep(.4);assert state('host')['hard'];checks.append('field contract changes rejected')
 command('client',deploy=True,pitch=-60);time.sleep(.4);assert state('host')['charges']==2;checks.append('sky deployment does not consume')
 for x,z in [(2.8,-2),(2.8,3.35)]:go('client',x,z)
 command('client',quiet=True,yaw=-45,pitch=30,deploy=True)
 wait('client beacon shared on both peers',lambda:state('host').get('charges')==1 and state('client').get('charges')==1 and state('client').get('beaconTime',0)>0)
 command('client',quiet=True,yaw=-45,pitch=30,deploy=True);time.sleep(.4)
 assert state('host')['charges']==1;checks.append('active beacon blocks duplicate deployment')
 wait('listener investigates remote beacon',lambda: danger().get('state')==1 and abs(danger()['noiseTarget']['z']-state('host')['beaconPosition']['z'])<.1)
 command('client',quiet=True,capture=True)
 for x,z in [(2.8,-2),(1,-5)]:go('client',x,z)
 wait('signal expires on both peers',lambda:state('host').get('beaconTime')==0 and state('client').get('beaconTime')==0)
 assert state('host')['charges']==1 and state('host')['credits']==300;checks.append('expiry neither refills stock nor changes wallet')
 command('host',action=True);wait('undelivered return preserves wallet',lambda:state('client').get('phase')==4 and state('client').get('credits')==300)
 if SAVE_TEST:
  save=folders['host']/'progression-v1.json'
  wait('purchase written to host disk',lambda:save.exists() and json.loads(save.read_text())['beacon'])
  assert not (folders['client']/'progression-v1.json').exists();checks.append('client never imports host progress to disk')
  def stop_players():
   for proc in procs:
    if proc.poll() is None:proc.terminate();proc.wait(timeout=5)
   for folder in folders.values():
    (folder/'state.json').unlink(missing_ok=True)
   command('host',quiet=True);command('client',quiet=True)
  stop_players();hp=start('host')
  wait('restart restores wallet license and delivery unlock',lambda:state('host').get('credits')==300 and state('host').get('unlocked') and state('host').get('deliveries')==1)
  assert state('host')['phase']==0 and state('host')['receipt']==0 and state('host')['returnPay']==0 and not state('host')['hard'];checks.append('restart begins fresh without duplicate receipt or return pay')
  cp=start('client');wait('new client receives restored host progression',lambda:state('client').get('credits')==300 and state('client').get('unlocked'))
  command('host',action=True);wait('restored route selectable',lambda:state('host').get('phase')==1)
  command('host',contract=True);wait('persisted delivery unlock allows risk',lambda:state('client').get('hard'))
  command('host',action=True);wait('restored license refills charges',lambda:state('client').get('phase')==2 and state('client').get('charges')==2)
  stop_players();hp=start('host');wait('mid-shift exit gives no extra bonus',lambda:state('host').get('phase')==0 and state('host').get('credits')==300)
  stop_players()
  backup=json.loads(Path(str(save)+'.bak').read_text())
  (run/'before-corruption.json').write_bytes(save.read_bytes());save.write_text('interrupted/corrupt data')
  hp=start('host');wait('corrupt primary recovers previous backup in player',lambda:state('host').get('phase')==0 and state('host').get('credits')==backup['credits'] and state('host').get('unlocked')==backup['beacon'])
  wait('recovered primary repaired on disk',lambda:save.read_text().startswith('{') and json.loads(save.read_text())['credits']==backup['credits'])
 report={'status':'PASS','checks':checks,'run':str(run),'scope':'Two actual Windows processes: earned pay, return, host purchase, contract selection, client beacon deployment and replicated attraction/expiry. Human feel and economy balance not covered'}
 if SAVE_TEST:report['scope']='Two real Windows processes: earned progression, process termination/restart, host-owned save, license/unlock restoration, no duplicate pay, backup recovery. UI readability, human feel, OS power loss and Cloud not covered.'
 (OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
except Exception as e:
 (OUT/'latest.json').write_text(json.dumps({'status':'FAIL','checks':checks,'error':str(e),'run':str(run)},indent=2));raise
finally:
 for p in procs:
  if p.poll() is None:p.terminate()
 for p in procs:
  try:p.wait(timeout=5)
  except subprocess.TimeoutExpired:p.kill()
