"""Two windowless clients: actual terminal purchases, healing and physical medicine exchange."""
import json
import math
import time
import cinder_four_player as lab
from cinder_build_stamp import require_current


def main():
    require_current(lab.player())
    if lab.SESSION.exists():raise RuntimeError('An existing session is active.')
    deadline=time.monotonic()+40
    while lab.port_busy() and time.monotonic()<deadline:time.sleep(.25)
    run,processes,folders,_=lab.launch(True,headless=True,quick=True)
    report=dict(status='FAIL',run=str(run),checks=[]);seq=100
    def send(i,**kw):
        nonlocal seq
        seq+=1;lab.command(folders[i],seq,**kw)
    def state(i=0):return lab.state(folders[i])
    def both(fn):return all(fn(state(i)) for i in (0,1))
    def check(name,fn,timeout=8):lab.wait(name,fn,timeout);report['checks'].append(name)
    def fixture(name):
        send(1,ui='close');send(0,fixture=name)
        check(name+' prepared',lambda:lab.state(folders[0],'animation.json').get('quickFixture')==name)
        time.sleep(.2)
    def buy(i):
        send(i,ui='ship');check('ship opened '+str(i),lambda:lab.state(folders[i],'ui.json').get('menu')=='ship')
        send(i,click='personal');check('personal equipment opened '+str(i),lambda:bool(lab.state(folders[i],'ui.json').get('personal')))
        send(i,click='buyMedicine')
    try:
        fixture('inventory')
        buy(1);check('peer buys own dose from shared wallet',lambda:both(lambda s:s['credits']==360 and s['inventory']['medicine'][2]==1))
        buy(0);check('host buys separate dose',lambda:both(lambda s:s['credits']==320 and s['inventory']['medicine'][0]==1))
        send(0,click='buyMedicine');time.sleep(.3);check('occupied slot cannot charge again',lambda:both(lambda s:s['credits']==320))
        send(0,ui='close',dropSpecial=True);time.sleep(.3)
        check('pre-departure spare stockpiling rejected',lambda:both(lambda s:s['inventory']['medicine'][0]==1 and len(s['inventory']['ground'])==0))
        fixture('inventory-field')
        send(0,buyMedicine=True,specialSlot=1);time.sleep(.3)
        check('field restock and co-op second slot rejected',lambda:both(lambda s:s['credits']==320 and s['inventory']['slots']==1 and s['inventory']['selected'][0]==0))
        send(0,yaw=90,useMedicine=True)
        check('aimed teammate heals without changing down count',lambda:both(lambda s:s['visit']['health'][1]==100 and s['inventory']['medicine'][0]==0 and s['visit']['downs'][1]==0))
        send(1,yaw=90,useMedicine=True);time.sleep(.3)
        check('healthy self does not consume second dose',lambda:both(lambda s:s['inventory']['medicine'][2]==1))
        send(1,yaw=-90,useMedicine=True)
        check('peer heals host and consumes its own dose',lambda:both(lambda s:s['visit']['health'][0]==100 and s['inventory']['medicine'][2]==0))
        fixture('inventory');buy(0)
        check('exchange dose purchased',lambda:both(lambda s:s['credits']==360 and s['inventory']['medicine'][0]==1))
        fixture('inventory-field');send(0,dropSpecial=True,yaw=0)
        check('drop creates one physical shared medicine',lambda:both(lambda s:s['inventory']['medicine'][0]==0 and len(s['inventory']['ground'])==1))
        med=state()['inventory']['ground'][0]['position'];p=state()['positions'][1];dx=med['x']-p['x'];dz=med['z']-p['z'];dy=med['y']-p['y']-1.45
        send(1,yaw=math.degrees(math.atan2(dx,dz)),pitch=-math.degrees(math.atan2(dy,math.hypot(dx,dz))),interact=True)
        check('teammate raycast pickup fills its slot',lambda:both(lambda s:s['inventory']['medicine'][2]==1 and len(s['inventory']['ground'])==0))
        send(1,interact=True);time.sleep(.3)
        check('pickup cannot duplicate dose',lambda:both(lambda s:sum(s['inventory']['medicine'])==1))
        for p in run.rglob('player.log'):
            assert not any(x in p.read_text(errors='replace') for x in ('Exception:','Invalid AABB','Assertion failed','Fatal Error')),str(p)
        report['checks'].append('clean runtime logs');report['status']='PASS'
    except BaseException as error:report['error']=str(error);raise
    finally:
        report['states']=[state(i) for i in (0,1)]
        (lab.OUT/'inventory-latest.json').write_text(json.dumps(report,indent=2)+'\n')
        lab.stop()
        for p in processes.values():
            try:p.wait(timeout=8)
            except Exception:p.kill()

if __name__=='__main__':main()
