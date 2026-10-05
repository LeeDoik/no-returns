"""Four actual clients: select two ship parcels, independently carry, verify and bank.

No seeded state or teleports. Hazard AI is disabled to isolate the delivery journey.
"""
import json
import math
import time
import cinder_four_player as lab
from cinder_build_stamp import require_current


def main(headless=False):
    require_current(lab.player())
    if lab.SESSION.exists():
        raise RuntimeError('Stop the existing session before testing.')
    deadline=time.monotonic()+40
    while lab.port_busy() and time.monotonic()<deadline:time.sleep(.25)
    run, processes, folders, _ = lab.launch(True, hazard=False, headless=headless)
    report=dict(status='FAIL',run=str(run),checks=[]);seq=100
    def send(slot,**values):
        nonlocal seq
        seq+=1;lab.command(folders[slot],seq,**values)
    def state(slot=0):return lab.state(folders[slot])
    def both(predicate):return all(predicate(state(i)) for i in range(4))
    def check(name,fn,timeout=25):lab.wait(name,fn,timeout);report['checks'].append(name)
    def go(slot,x,z,pitch=0):
        send(slot,ui='close');lab.wait('controls resumed',lambda:lab.state(folders[slot],'ui.json').get('menu')=='',5)
        lab.walk(send,folders,slot,x,z,pitch)
    def aim(slot,target,**values):
        p=state()['positions'][slot];dx=target[0]-p['x'];dz=target[2]-p['z'];dy=target[1]-p['y']-1.45
        send(slot,yaw=math.degrees(math.atan2(dx,dz)),pitch=-math.degrees(math.atan2(dy,math.hypot(dx,dz))),**values)
    def aboard(slot):
        p=state()['positions'][slot];go(slot,p['x'],-20.05);go(slot,-20.7,-20.05);go(slot,-20.7,-25 if slot<2 else -26.4)
        go(slot,(-21.65,-19.75,-21.65,-19.75)[slot],-25 if slot<2 else -26.4)
    def ship():
        send(0,ui='ship');check('ship terminal opens',lambda:lab.state(folders[0],'ui.json').get('menu')=='ship' and bool(lab.state(folders[0],'ui.json').get('shipAction')));send(0,click='shipAction')
    def pickup(slot,id):
        p=state()['parcels'][id]['position'];aim(slot,(p['x'],p['y'],p['z']),interact=True)
        check('parcel '+str(id)+' picked by '+str(slot),lambda:both(lambda s:s['parcels'][id]['holder']==slot))
    try:
        check('six parcels replicated for four crew',lambda:both(lambda s:len(s.get('parcels',[]))==6 and len(s.get('deliveriesState',[]))==6))
        for i in range(4):aboard(i)
        ship();check('route selected',lambda:both(lambda s:s['phase']==1))
        ship();check('landing starts visit',lambda:both(lambda s:s['phase']==2 and s['visit']['active']))
        # Walk down the centre aisle and take one parcel from each side.
        go(0,-20.7,-27.2);pickup(0,0)
        go(0,-20.7,-25);go(0,-20.7,-20.05);go(0,-22.95,-19.65)
        go(1,-20.7,-27.2);pickup(1,1)
        check('two distinct simultaneous carriers on all peers',lambda:both(lambda s:s['parcels'][0]['holder']==0 and s['parcels'][1]['holder']==1))
        go(0,-22.95,-12.8,45)
        send(0,yaw=0,pitch=62,turnPitch=-62);time.sleep(.4);send(0,yaw=0,pitch=62,turnPitch=-62,place=True)
        check('first verification starts',lambda:both(lambda s:0<s['deliveriesState'][0]['progress']<25),8)
        time.sleep(1);pickup(0,0);paused=state()['deliveriesState'][0]['progress'];time.sleep(.7)
        check('pickup pauses and retains progress',lambda:both(lambda s:not s['deliveriesState'][0]['scanning'] and abs(s['deliveriesState'][0]['progress']-paused)<.1))
        send(0,yaw=0,pitch=62,turnPitch=-62);time.sleep(.4);send(0,yaw=0,pitch=62,turnPitch=-62,place=True)
        check('replacement resumes',lambda:both(lambda s:s['deliveriesState'][0]['progress']>paused),8)
        go(0,-22.95,-15)
        check('verification finishes while player away',lambda:both(lambda s:s['deliveriesState'][0]['verified'] and s['credits']==0),30)
        go(0,-23.8,-13.5);aim(0,(-23.9,1.25,-12),interact=True)
        check('shared first receipt only unbanked',lambda:both(lambda s:s['deliveriesState'][0]['collected'] and s['visit']['gross']==100 and s['credits']==0))
        aim(0,(-23.9,1.25,-12),interact=True);time.sleep(.4)
        check('receipt cannot duplicate',lambda:both(lambda s:s['visit']['gross']==100))
        go(0,-22.95,-19.65);aboard(0)
        send(0,ui='ship');time.sleep(.3);send(0,click='deliveries')
        check('ship manifest reveals remaining WEST bundle',lambda:'55 CR' in lab.state(folders[0],'ui.json').get('manifest',''))
        # Second carrier delivers the other WEST contract without choosing a new contract.
        go(1,-20.7,-25);go(1,-20.7,-20.05);go(1,-22.95,-19.65);go(1,-22.95,-12.8,45)
        send(1,yaw=0,pitch=62,turnPitch=-62);time.sleep(.4);send(1,yaw=0,pitch=62,turnPitch=-62,place=True)
        check('second parcel verifies independently',lambda:both(lambda s:s['deliveriesState'][1]['verified']),32)
        go(1,-23.8,-13.5);aim(1,(-23.9,1.25,-12),interact=True)
        check('second receipt includes single district bonus',lambda:both(lambda s:s['visit']['gross']==275 and s['credits']==0))
        aim(1,(-23.9,1.25,-12),interact=True);time.sleep(.3)
        check('bundle is idempotent',lambda:both(lambda s:s['visit']['gross']==275))
        go(1,-22.95,-19.65);aboard(1)
        ship();check('departure settles selected two and abandons four without penalty',lambda:both(lambda s:s['phase']==4 and s['credits']==275 and s['deliveries']==2 and s['visit']['returnFees']==0),16)
        ship();check('next visit resets all six while keeping wallet',lambda:both(lambda s:s['phase']==0 and s['credits']==275 and len(s['parcels'])==6 and not any(e['collected'] for e in s['deliveriesState'])))
        for p in run.rglob('player.log'):
            assert not any(x in p.read_text(errors='replace') for x in ('Exception:','Invalid AABB','Assertion failed','Fatal Error')),str(p)
        report['checks'].append('clean runtime logs');report['status']='PASS'
    except BaseException as e:
        report['error']=str(e);raise
    finally:
        report['states']=[state(i) for i in range(4)]
        (lab.OUT/'delivery-latest.json').write_text(json.dumps(report,indent=2)+'\n')
        lab.stop()
        for p in processes.values():
            try:p.wait(timeout=10)
            except Exception:p.kill()

if __name__=='__main__':
    import argparse
    parser=argparse.ArgumentParser();parser.add_argument('--headless',action='store_true')
    main(parser.parse_args().headless)
