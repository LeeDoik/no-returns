"""Dense facility integration: ordinary input on geometry-validated routes."""
import argparse
import json
import math
import time
from pathlib import Path
import test_cinder_demo as base

def turns(points):
    result=[]
    for i,p in enumerate(points):
        if i==0 or i==len(points)-1:
            result.append((p['x'],p['z']))
        else:
            a,b=points[i-1],points[i+1]
            if (p['x']-a['x'],p['z']-a['z']) != (b['x']-p['x'],b['z']-p['z']):
                result.append((p['x'],p['z']))
    return result

class DenseTest(base.DemoTest):
    def go(self,x,z,i=0,pitch=25,yaw=0):
        # Follow doorway centres accurately so the parcel clears both jambs.
        held=self.read().get('holder')==i
        if not held:
            return super().go(x,z,i=i,pitch=pitch,yaw=yaw)
        deadline=time.monotonic()+100
        heading=yaw
        following=None
        for n,point in enumerate(base.OUTBOUND[:-1]):
            if point==(x,z):
                following=base.OUTBOUND[n+1]
                break
        while time.monotonic()<deadline:
            state=self.read()
            assert state.get('holder')==i, 'Cargo released while traversing '+str((x,z))
            p=state['positions'][i]
            dx,dz=x-p['x'],z-p['z']
            distance=math.hypot(dx,dz)
            if distance<.3:
                self.command(i,pitch=pitch,yaw=heading)
                time.sleep(.18)
                return
            heading=math.degrees(math.atan2(dx,dz))
            # Human-style cornering: face the next corridor, then sidestep to its centre.
            # Facing into the current wall until the body reaches a node traps the parcel.
            if following is not None and distance<1.2:
                heading=math.degrees(math.atan2(following[0]-x,following[1]-z))
            radians=math.radians(heading)
            speed=min(1,distance*2)
            forward=(dx*math.sin(radians)+dz*math.cos(radians))/distance
            sideways=(dx*math.cos(radians)-dz*math.sin(radians))/distance
            self.command(i,pitch=pitch,yaw=heading,z=forward*speed,x=sideways*speed)
            time.sleep(.07)
        raise AssertionError('Held corner traversal blocked: '+str((x,z))+' '+json.dumps(self.read()))

    def exercise(self):
        super().exercise()
        assert self.every(lambda s:s.get('protocol')==12 and len(s.get('additionalDanger',[]))==2)
        self.checks.append('three listener snapshots on all peers / protocol 12')
        snapshots=[self.read(i) for i in range(self.args.crew)]
        for slot in range(2):
            expected=snapshots[0]['additionalDanger'][slot]
            assert all(s['additionalDanger'][slot]['down']==expected['down'] for s in snapshots)
        self.checks.append('additional listener shared down state consistent')

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--crew',type=int,choices=(1,2,4),default=2)
    parser.add_argument('--exe',type=Path,default=base.ROOT/'builds/CinderDemo/NoReturns-CinderDemo.exe')
    args=parser.parse_args()
    data=json.loads((base.ROOT/'artifacts/cinder-dense/routes.json').read_text())
    route=turns(data['delivery'])
    # Carry through the centre of the west baffle opening, not along its jamb.
    corner=route.index((-32.0,17.0))
    route=[(-32,-30),(-32,-24),(-31,-24),(-31,-2),(-36,-2),(-36,3),(-31,3),(-31,17)]+route[corner+1:]
    route=[{(-30.0,23.0):(-30,22),(-14.0,23.0):(-13,22),(-14.0,25.0):(-13,25),(28.0,33.0):(28,32),(33.0,33.0):(33,32)}.get(point,point) for point in route]
    a=route.index((11.0,25.0)); b=route.index((21.0,38.0))
    route[a:b]=[(11,25),(12.5,25),(12.5,31.5),(11,31.5),(11,38.5)]
    base.OUTBOUND=[(-38.2,-30),(-32.8,-30)]+route+[(32.7,17.2),(32.7,15.8)]
    base.RETURN=[(32.7,17.2)]+list(reversed(route[:-1]))+[(-32.8,-30),(-38.2,-30)]
    DenseTest(args).execute()
