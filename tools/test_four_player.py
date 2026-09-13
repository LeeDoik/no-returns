"""Four real Windows processes: scripted inputs, not human play or WAN validation."""
import base64, hashlib, json, math, os, subprocess, time
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
EXE=ROOT/'builds/CarryTest/NoReturns.exe'
OUT=ROOT/'artifacts/four-player';OUT.mkdir(exist_ok=True)
RUN=OUT/time.strftime('run-%Y%m%d-%H%M%S');RUN.mkdir()
checks=[];processes={};folders={};seq=0

def state(i):
    for _ in range(20):
        try:return json.loads((folders[i]/'state.json').read_text())
        except (OSError,ValueError):time.sleep(.005)
    return {}

def command(i,**kw):
    global seq
    seq+=1
    value=dict(seq=seq,x=0,z=0,yaw=0,pitch=0,quiet=True);value.update(kw)
    p=folders[i]/'input.tmp';p.write_text(json.dumps(value))
    for _ in range(100):
        try:os.replace(p,folders[i]/'input.json');return
        except PermissionError:time.sleep(.01)
    raise RuntimeError('Input file locked')

def wait(name,condition,seconds=20):
    end=time.monotonic()+seconds
    while time.monotonic()<end:
        if condition():checks.append(name);print('PASS',name,flush=True);return
        time.sleep(.06)
    raise AssertionError(name+' '+json.dumps({i:state(i) for i in folders}))

def all_state(predicate):return all(predicate(state(i)) for i in range(4))
def phase(n):return all_state(lambda s:s.get('phase')==n)
def start(i,mode):
    args=[str(EXE),'--host'] if i==0 else [str(EXE),'--join','127.0.0.1']
    args += [mode,'--test-dir',str(folders[i]),'-logFile',str(folders[i]/'player.log'),'-screen-width','640','-screen-height','400','-screen-fullscreen','0']
    si=subprocess.STARTUPINFO();si.dwFlags|=subprocess.STARTF_USESHOWWINDOW;si.wShowWindow=0
    processes[i]=subprocess.Popen(args,cwd=EXE.parent,startupinfo=si)
def stop():
    for p in processes.values():
        if p.poll() is None:p.terminate()
    for p in processes.values():
        try:p.wait(timeout=5)
        except subprocess.TimeoutExpired:p.kill();p.wait()
    processes.clear()
def setup(mode):
    stop();time.sleep(.5)
    folders.clear()
    for i in range(5):
        folders[i]=RUN/(mode[2:]+'-'+str(i));folders[i].mkdir();command(i)
    if mode=='--hazard':
        saved=dict(version=1,credits=120,deliveries=1,beacon=False)
        saved['checksum']=base64.b64encode(hashlib.sha256(b'1|120|1|0').digest()).decode()
        (folders[0]/'progression-v1.json').write_text(json.dumps(saved))
    start(0,mode);wait(mode+' host ready',lambda:state(0).get('phase')==0)
    for i in range(1,4):
        start(i,mode);wait(mode+' slot '+str(i),lambda i=i:state(i).get('recipient')==i)
    wait(mode+' four replicated slots',lambda:all_state(lambda s:s.get('players')==4 and s.get('occupiedMask')==15))
def go(i,x,z,pitch=0):
    end=time.monotonic()+30
    while time.monotonic()<end:
        positions=state(0).get('positions')
        if not positions:time.sleep(.05);continue
        p=positions[i];dx=x-p['x'];dz=z-p['z'];d=math.hypot(dx,dz)
        if d<.18:command(i,pitch=pitch);time.sleep(.2);return
        command(i,yaw=math.degrees(math.atan2(dx,dz)),pitch=pitch,z=min(1,d*2));time.sleep(.08)
    raise AssertionError('Walk failed '+str((i,x,z))+' '+json.dumps(state(0)))
def action(i=0):command(i,action=True);time.sleep(.4)
def check_stable(name,predicate):
    end=time.monotonic()+.65
    while time.monotonic()<end:
        assert predicate(),name+" "+json.dumps({i:state(i) for i in range(4)})
        time.sleep(.05)
    checks.append(name);print('PASS',name,flush=True)

def main():
    try:
        if '--delivery-only' not in __import__('sys').argv:
            setup('--hazard')
            start(4,'--hazard');wait('fifth player rejected',lambda:'Room full' in state(4).get('message',''))
            processes[4].terminate();processes[4].wait()
            command(0,buy=True);wait('beacon purchase shared',lambda:all_state(lambda s:s.get('beaconExists') and s.get('credits')==0))
            # Slot 3 starts in the rear row, and must carry its own equipment.
            go(3,-2,-5.8);command(3,yaw=180,pitch=45,interact=True)
            wait('slot 3 owns beacon',lambda:all_state(lambda s:s.get('beaconCarrier')==3))
            action();action();wait('four player hazard departure',lambda:phase(2))
            for i in range(3):command(i,shove=True)
            wait('independent cooldowns 0 1 2',lambda:all_state(lambda s:len((s.get('danger') or {}).get('cooldown',[]))==4 and all(s['danger']['cooldown'][i]>3 for i in range(3)) and s['danger']['cooldown'][3]==0))
            go(3,-2,-2);command(3,drop=True)
            wait('slot 3 places active beacon',lambda:all_state(lambda s:s.get('beaconCarrier')==-1 and s.get('charges')==1 and s.get('beaconTime',0)>0))
            command(3,shove=True);wait('slot 3 cooldown shared',lambda:all_state(lambda s:s.get('danger',{}).get('cooldown',[0]*4)[3]>3))
            processes[2].terminate();processes[2].wait()
            wait('slot 2 leave preserves other identities',lambda:all(state(i).get('occupiedMask')==11 and state(i).get('phase')==4 for i in (0,1,3)))
            action();wait('remaining crew next preparation',lambda:state(0).get('phase')==0)
            # Reuse the freed slot with a fresh evidence directory and input sequence.
            folders[2]=RUN/'hazard-rejoined-2';folders[2].mkdir();command(2);start(2,'--hazard')
            wait('freed slot 2 reused',lambda:all_state(lambda s:s.get('occupiedMask')==15) and state(2).get('recipient')==2)


        setup('--delivery');action();action();wait('four player delivery departure',lambda:phase(2))
        go(2,0,-6);go(2,0,-4.2);go(2,-1,-4.2);command(2,yaw=27,pitch=20,interact=True)
        wait('slot 2 owns cargo',lambda:all_state(lambda s:s.get('holder')==2))
        command(3,drop=True);check_stable('other crew cannot drop owned cargo',lambda:state(0).get('holder')==2)
        for x,z in [(1,-2),(1,5),(-6,5),(-6,8.2)]:go(2,x,z,pitch=45 if z>8 else 0)
        time.sleep(.4);command(2,pitch=45,drop=True)
        wait('receipt issued on all four peers',lambda:phase(3))
        check_stable('delivery alone does not pay',lambda:state(0).get('credits')==0)
        go(2,-5,7)
        for x,z in [(0,-6),(0,-2),(1,5),(-6,5),(-8.1,7.5)]:go(3,x,z)
        command(3,yaw=0,pitch=12,interact=True)
        wait('slot 3 collects receipt',lambda:all_state(lambda s:s.get('receiptCollected')))
        action(0);check_stable('return blocked while slot 2 and 3 outside',lambda:state(0).get('phase')==3)
        for i in (2,3):
            for x,z in [(-6,5),(1,5),(1,-2),(2,-5.8)]:go(i,x,z)
            if i==2:go(2,-1,-7)
        wait('four crew back aboard',lambda:all(abs(p['x'])<3 and -10<p['z']<-3.8 for p in state(0)['positions']))
        action(0);wait('four peer settlement exactly 420',lambda:phase(4) and all_state(lambda s:s.get('credits')==420))
        check_stable('settlement not paid again',lambda:all_state(lambda s:s.get('credits')==420))
        for i in range(4):command(i,capture=True)
        time.sleep(1)
        logs=[]
        for f in RUN.rglob('player.log'):
            if 'NullReferenceException' in f.read_text(errors='replace') or 'IndexOutOfRangeException' in f.read_text(errors='replace'):logs.append(str(f))
        assert not logs,logs
        checks.append('no null/index exceptions in player logs')
        report=dict(status='PASS',checks=checks,run=str(RUN),scope='Four local real processes; scripted controls, not human fun, WAN or Steam verification.')
    except Exception as e:
        report=dict(status='FAIL',checks=checks,error=str(e),run=str(RUN));raise
    finally:
        stop();(OUT/'latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))

if __name__=="__main__":main()
