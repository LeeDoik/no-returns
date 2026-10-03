"""Targeted, windowless tests using two real clients and isolated initial fixtures."""
import argparse
import json
import math
import subprocess
import sys
import time
import cinder_four_player as lab
from cinder_build_stamp import require_current


def main(scenario='all', build=False):
    started = time.monotonic()
    try:
        if lab.SESSION.exists():
            raise RuntimeError('An existing test session is active; stop it before quick tests or rebuilding.')
        if build:
            subprocess.run([sys.executable, str(lab.ROOT/'tools/cinder_four_player.py'), 'build'], check=True)
        require_current(lab.player())
        # A recently closed test can leave the listening port in TIME_WAIT.
        deadline=time.monotonic()+30
        while not lab.SESSION.exists() and lab.port_busy() and time.monotonic()<deadline:
            time.sleep(.25)
        run, processes, folders, _ = lab.launch(True, headless=True, quick=True)
    except BaseException as error:
        lab.OUT.mkdir(parents=True,exist_ok=True)
        (lab.OUT/'quick-latest.json').write_text(json.dumps(dict(status='FAIL',stage='startup',scenario=scenario,error=str(error),total_seconds=round(time.monotonic()-started,3)),indent=2)+'\n')
        raise
    report=dict(status='FAIL', scenario=scenario, run=str(run), checks=[], scenario_seconds={})
    seq=1
    def send(slot, **values):
        nonlocal seq
        seq+=1;lab.command(folders[slot],seq,**values)
    def state(slot=0): return lab.state(folders[slot])
    def anim(slot=0): return lab.state(folders[slot],'animation.json')
    def require(name, predicate, timeout=8):
        lab.wait(name,predicate,timeout);report['checks'].append(name)
    def both(predicate): return all(predicate(state(i)) for i in (0,1))
    def fixture(name):
        send(1);send(0,fixture=name)
        require(name+' fixture ready',lambda:anim().get('quickFixture')==name and both(lambda s:s.get('phase')==2))
        time.sleep(.15)
    def aim(slot, p, **values):
        a=state()['positions'][slot];dx=p['x']-a['x'];dy=p['y']-a['y']-1.57;dz=p['z']-a['z']
        send(slot,yaw=math.degrees(math.atan2(dx,dz)),pitch=-math.degrees(math.atan2(dy,math.hypot(dx,dz))),**values)
    try:
        for name in (('beacon','rescue','baton') if scenario=='all' else (scenario,)):
            begin=time.monotonic();fixture(name)
            if name=='beacon':
                require('beacon starts purchased with two charges',lambda:both(lambda s:s.get('beaconExists') and s.get('charges')==2 and s.get('beaconCarrier')==-1))
                send(1,fixture='rescue');time.sleep(.2)
                require('client cannot reset host fixture',lambda:anim().get('quickFixture')=='beacon' and not state()['danger']['down'][1])
                aim(0,state()['beaconPosition'],interact=True)
                require('real pickup replicated',lambda:both(lambda s:s.get('beaconCarrier')==0))
                require('local and remote hands carry beacon',lambda:anim()['hands']['beaconCarrying'] and anim(1)['employees'][0]['beaconCarrying'] and anim(1)['employees'][0]['beaconContactError']<.02)
                send(0,place=True)
                require('real placement consumes one charge',lambda:both(lambda s:s.get('beaconCarrier')==-1 and s.get('charges')==1 and s.get('beaconTime',0)>0))
                require('release restores baton and clears remote pose',lambda:anim()['batons'][0]['visible'] and not anim()['hands']['beaconCarrying'] and not anim(1)['employees'][0]['beaconCarrying'])
            elif name=='rescue':
                require('partner starts down',lambda:both(lambda s:s['danger']['down'][1]))
                require('down pose settles on both clients',lambda:all(anim(i)['employees'][1]['downPhase']=='Down' and anim(i)['employees'][1]['downRoll']>84 for i in (0,1)))
                send(0,rescue=True,yaw=90)
                require('real rescue progress and hands',lambda:both(lambda s:s['danger']['rescue'][0]>.25) and anim()['hands']['rescuing'])
                require('remote rescuer crouches with supported hands',lambda:anim(1)['employees'][0]['rescueWeight']>.9 and anim(1)['employees'][0]['rescueWristBend']<=25.01 and anim(1)['employees'][0]['rescueFootError']<.015)
                send(0,yaw=90)
                require('release cancels rescue',lambda:both(lambda s:s['danger']['rescue'][0]==0) and not anim()['hands']['rescuing'])
                require('remote rescue pose clears on cancel',lambda:anim(1)['employees'][0]['rescueWeight']==0)
                send(0,rescue=True,yaw=90)
                require('real hold revives partner on both clients',lambda:both(lambda s:not s['danger']['down'][1] and s['danger']['rescues']>0))
                send(0)
                require('revived partner visibly gets up',lambda:anim()['employees'][1]['downPhase']=='GettingUp')
                require('get-up returns to standing on both clients',lambda:all(anim(i)['employees'][1]['downPhase']=='Standing' and anim(i)['employees'][1]['downAmount']==0 for i in (0,1)))
                require('revival restores baton',lambda:anim()['batons'][0]['visible'] and not anim()['hands']['rescuing'] and anim(1)['employees'][0]['rescueWeight']==0)
            else:
                send(0,shove=True)
                require('real baton hit stuns target on both clients',lambda:both(lambda s:s['danger']['state']==4 and s['danger']['cooldown'][0]>5))
                require('attack pose returns',lambda:anim()['employees'][0]['batonWeight']==0)
                before=state()['danger']['cooldown'][0];send(0,shove=True);time.sleep(.2)
                require('cooldown rejects repeated attack',lambda:0<state()['danger']['cooldown'][0]<before)
            send(0);send(1);report['scenario_seconds'][name]=round(time.monotonic()-begin,3)
        for folder in folders.values():
            log=(folder/'player.log').read_text(errors='replace')
            assert not any(x in log for x in ('Exception:','Invalid AABB','Assertion failed','Fatal Error','prototype visuals','Prepare FirstPersonArms'))
            assert not (folder/'progression-v1.json').exists()
        report['checks'].append('no runtime errors or progression save');report['status']='PASS'
    except BaseException as error:
        report.update(error=str(error),last_states=[state(i) for i in (0,1)]);raise
    finally:
        lab.stop()
        for p in processes.values():
            try:p.wait(timeout=5)
            except subprocess.TimeoutExpired:p.kill();p.wait()
        report['total_seconds']=round(time.monotonic()-started,3)
        for path in (run/'quick-check.json',lab.OUT/'quick-latest.json'):
            path.write_text(json.dumps(report,indent=2)+'\n')
        print(json.dumps({k:report[k] for k in ('status','scenario','scenario_seconds','total_seconds')},indent=2))


if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('scenario',choices=('all','beacon','rescue','baton'),nargs='?',default='all')
    parser.add_argument('--build',action='store_true',help='Explicitly rebuild once before testing')
    args=parser.parse_args();main(args.scenario,args.build)
