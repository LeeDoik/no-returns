"""Run real four-process down/rescue checks for the new crew slots."""
import json, math, time
import test_four_player as h
report={}
def down(i):return h.state(0).get('danger',{}).get('down',[False]*4)[i]
def replicated(predicate):return h.all_state(lambda s:predicate(s.get('danger') or {}))
def face(i):
    s=h.state(0);p=s['positions'][i];c=s['danger']['position']
    return math.degrees(math.atan2(c['x']-p['x'],c['z']-p['z']))
try:
    h.setup('--hazard');h.action();h.action();h.wait('hazard active for rescue test',lambda:h.phase(2))
    for x,z in [(0,-6),(2,-6),(2,-2),(1,3.4)]:h.go(2,x,z)
    h.command(2,call=True)
    h.wait('slot 2 warned before hit',lambda:replicated(lambda s:s.get('state')==2 and not s['down'][2]))
    h.wait('slot 2 down on all four',lambda:replicated(lambda s:s['down'][2]))
    h.command(2,z=1,interact=True);p=h.state(0)['positions'][2];time.sleep(.6)
    assert math.dist(list(p.values()),list(h.state(0)['positions'][2].values()))<.08
    h.checks.append('slot 2 down blocks movement')
    time.sleep(3.2);h.check_stable('two safe crew prevent all-down recovery',lambda:h.state(0)['phase']==2)
    h.command(3,rescue=True);time.sleep(.5)
    assert h.state(0)['danger']['rescue'][3]==0;h.checks.append('slot 3 distant rescue rejected')
    for x,z in [(2.5,-7),(2.5,-2),(2.5,3.4)]:h.go(3,x,z)
    pulse=[0]
    def warning():
        if time.monotonic()>pulse[0]:h.command(3,call=True);pulse[0]=time.monotonic()+.5
        return h.state(0)['danger']['state']==2 and not down(3)
    h.wait('slot 3 baits listener',warning)
    h.command(3,shove=True,yaw=face(3))
    h.wait('slot 3 stuns on all four',lambda:replicated(lambda s:s['state']==4))
    h.command(3,rescue=True)
    h.wait('slot 3 rescue progress replicated',lambda:replicated(lambda s:s['rescue'][3]>.3))
    h.wait('slot 3 rescues slot 2 on all four',lambda:replicated(lambda s:not s['down'][2] and s['rescues']>=1))
    h.command(3);h.command(2)
    for i in range(4):h.command(i,capture=True)
    time.sleep(.8)
    report=dict(status='PASS',checks=h.checks,run=str(h.RUN),scope='Four real local processes, scripted down/stun/rescue for slots 2 and 3. Human feel/WAN/Steam untested.')
except Exception as e:
    report=dict(status='FAIL',checks=h.checks,error=str(e),run=str(h.RUN));raise
finally:
    h.stop();(h.OUT/'rescue-latest.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
