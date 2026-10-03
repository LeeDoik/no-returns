"""Focused safe companion rescue, cancellation and replay through real inputs."""
import json
import math
import time
import cinder_four_player as lab
from cinder_build_stamp import require_current


def main():
    require_current(lab.player())
    started=time.monotonic()
    run,processes,folders,_=lab.launch(True,companion=True,headless=True)
    report=dict(status='FAIL',checks=[])
    def state(i=0):return lab.state(folders[i])
    def anim(i=0):return lab.state(folders[i],'animation.json')
    def check(name,predicate,timeout=10):lab.wait(name,predicate,timeout);report['checks'].append(name)
    def both(predicate):return all(predicate(state(i)) for i in (0,1))
    try:
        check('safe bot approaches and falls nearby',lambda:both(lambda s:not s['hazard'] and s['phase']==-1 and s['danger']['down'][1]) and math.dist(list(state()['positions'][0].values()),list(state()['positions'][1].values()))<1.8,15)
        check('down pose and baton hiding on both peers',lambda:all(anim(i)['employees'][1]['downPhase']=='Down' and not anim(i)['batons'][1]['visible'] for i in (0,1)))
        p=state()['positions'][1];time.sleep(.3)
        check('down bot stays still',lambda:math.dist(list(p.values()),list(state()['positions'][1].values()))<.03)
        lab.command(folders[0],2,rescue=True)
        check('real hold progresses with first-person hands',lambda:both(lambda s:s['danger']['rescue'][0]>.3) and anim()['hands']['rescuing'])
        lab.command(folders[0],3)
        check('release cancels and preserves down',lambda:both(lambda s:s['danger']['down'][1] and s['danger']['rescue'][0]==0))
        lab.command(folders[0],4,rescue=True)
        check('real hold revives on both peers',lambda:both(lambda s:not s['danger']['down'][1] and s['danger']['rescues']==1))
        check('get-up visible',lambda:anim()['employees'][1]['downPhase']=='GettingUp')
        lab.command(folders[0],5)
        check('normal loop resumes standing with baton',lambda:both(lambda s:s['danger']['state']==0) and all(anim(i)['employees'][1]['downPhase']=='Standing' and anim(i)['batons'][1]['visible'] for i in (0,1)))
        before=state()['positions'][0]
        lab.command(folders[0],6,reset=True)
        check('R repeats rescue without room reset',lambda:both(lambda s:s['danger']['down'][1] and s['danger']['rescue'][0]==0))
        check('human position unchanged by replay',lambda:math.dist(list(before.values()),list(state()['positions'][0].values()))<.03)
        lab.command(folders[0],7,rescue=True)
        check('repeat rescue succeeds',lambda:both(lambda s:not s['danger']['down'][1] and s['danger']['rescues']==2))
        lab.command(folders[0],8)
        check('repeat returns to normal',lambda:both(lambda s:s['danger']['state']==0))
        for folder in folders.values():
            log=(folder/'player.log').read_text(errors='replace')
            assert not any(x in log for x in ('Exception:','Invalid AABB','Assertion failed','Fatal Error'))
        report['checks'].append('no runtime errors');report['status']='PASS'
    except BaseException as error:report['error']=str(error);raise
    finally:
        lab.stop()
        for p in processes.values():
            try:p.wait(timeout=5)
            except Exception:p.kill();p.wait()
        report['seconds']=round(time.monotonic()-started,3)
        (lab.OUT/'companion-rescue-latest.json').write_text(json.dumps(report,indent=2)+'\n')
        print(json.dumps(report,indent=2))

if __name__=='__main__':main()
