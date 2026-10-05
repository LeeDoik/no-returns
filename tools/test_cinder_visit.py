"""Two real windowless clients; seed only initial visit state, then use ordinary UI/input."""
import json
import time
import cinder_four_player as lab
from cinder_build_stamp import require_current

def main():
    require_current(lab.player())
    if lab.SESSION.exists():
        raise RuntimeError('Stop the existing session before testing.')
    deadline=time.monotonic()+30
    while lab.port_busy() and time.monotonic()<deadline:time.sleep(.25)
    run, processes, folders, _ = lab.launch(True, headless=True, quick=True)
    report=dict(status='FAIL',checks=[],run=str(run));seq=100
    def send(slot,**values):
        nonlocal seq
        seq+=1;lab.command(folders[slot],seq,**values)
    def state(slot=0):return lab.state(folders[slot])
    def both(predicate):return all(predicate(state(i)) for i in (0,1))
    def check(name,fn,timeout=8):lab.wait(name,fn,timeout);report['checks'].append(name)
    def fixture(name):
        send(1);send(0,fixture=name)
        check(name+' initialized',lambda:lab.state(folders[0],'animation.json').get('quickFixture')==name and both(lambda s:s.get('visit',{}).get('active',False)))
        time.sleep(.2)
    try:
        fixture('visit-empty')
        send(0,revive=1)
        time.sleep(.3)
        check('wallet cannot fund revival',lambda:both(lambda s:s['credits']==250 and s['visit']['reviving']==-1 and s['visit']['revivalFees']==0))
        fixture('visit-peer')
        check('receipt unbanked on both clients',lambda:both(lambda s:s['visit']['available']==420 and s['credits']==250 and s['visit']['eliminated'][0]))
        send(0,revive=0,action=True,confirmReturn=True);time.sleep(.3)
        check('eliminated actor rejected',lambda:both(lambda s:s['visit']['reviving']==-1 and not s['visit']['departing']))
        send(1,ui='ship');check('peer terminal opened',lambda:lab.state(folders[1],'ui.json').get('menu')=='ship')
        send(1,click='revive0')
        check('peer UI starts revival and reserves fee',lambda:both(lambda s:s['visit']['reviving']==0 and s['visit']['available']==320 and s['credits']==250))
        send(1,revive=0);time.sleep(.3)
        check('duplicate revival not charged',lambda:both(lambda s:s['visit']['revivalFees']==100))
        check('revival completes on both clients',lambda:both(lambda s:s['visit']['reviving']==-1 and not s['visit']['eliminated'][0] and s['visit']['health'][0]==100),14)
        check('revived employee placed aboard',lambda:all(-30.9<s['positions'][0]['z']<-24.8 for s in [state(),state(1)]))
        send(1,click='shipAction');check('peer starts departure',lambda:both(lambda s:s['visit']['departing']))
        send(0,ui='ship');time.sleep(.2);send(0,click='shipAction')
        check('other player cancels departure',lambda:both(lambda s:not s['visit']['departing']))
        fixture('visit')
        send(0,ui='ship');time.sleep(.2);send(0,click='shipAction')
        check('countdown leaves wallet unbanked',lambda:both(lambda s:s['visit']['departing'] and s['credits']==250))
        check('left behind fee and single payout replicated',lambda:both(lambda s:s['phase']==4 and s['credits']==570 and s['visit']['paid']==320 and s['visit']['returnFees']==100),16)
        send(0,action=True);time.sleep(.4)
        check('next preparation does not duplicate pay',lambda:both(lambda s:s['credits']==570))
        for folder in folders.values() if isinstance(folders,dict) else folders:
            path=folder/'player.log'
            if path.exists():
                log=path.read_text(errors='replace')
                assert not any(x in log for x in ('Exception:','Invalid AABB','Assertion failed','Fatal Error'))
        report['checks'].append('clean runtime logs')
        report['status']='PASS'
    except BaseException as e:
        report['error']=str(e);raise
    finally:
        report['states']=[state(0),state(1)]
        (lab.OUT/'visit-latest.json').write_text(json.dumps(report,indent=2)+'\n')
        lab.stop()
        for process in processes.values():
            try:process.wait(timeout=8)
            except Exception:process.kill()
if __name__=='__main__':main()
