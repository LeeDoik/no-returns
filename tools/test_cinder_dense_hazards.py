"""Two real Cinder players: listener warning, down, and partner rescue.
Run separately after the delivery loop; shares the game's local networking port.
No time acceleration, teleport, save seeding, or suppression-completion claim.
"""
import argparse
import math
import json
import subprocess
from pathlib import Path
import time
from test_cinder_demo import DemoTest, ROOT


class HazardTest(DemoTest):
    def __init__(self, args):
        self.args = args
        self.output = ROOT / 'artifacts/cinder-dense-hazards'
        self.run = self.output / time.strftime('run-%Y%m%d-%H%M%S')
        self.run.mkdir(parents=True)
        self.folders = [self.run / name for name in ('host', 'client-1')]
        for folder in self.folders:
            folder.mkdir()
        self.processes, self.checks, self.seq = [], [], 0

    def execute(self):
        report = dict(status='FAIL', crew=2, run=str(self.run), checks=self.checks,
                      scope='Two real local processes; scripted listener warning, down and rescue only. Human feel, delivery, suppression timing, WAN and Steam unverified.')
        try:
            self.exercise()
            report.update(status='PASS', final=[self.read(i) for i in range(2)])
        except Exception as error:
            report['error'] = str(error)
            raise
        finally:
            for process in self.processes:
                if process.poll() is None:
                    process.terminate()
            for process in self.processes:
                try:
                    process.wait(timeout=5)
                except subprocess.TimeoutExpired:
                    process.kill()
                    process.wait()
            payload = json.dumps(report, ensure_ascii=False, indent=2)
            (self.output / 'latest.json').write_text(payload, encoding='utf-8')
            (self.run / 'report.json').write_text(payload, encoding='utf-8')
            print(payload, flush=True)

    def replicated_danger(self, predicate):
        return self.every(lambda state: predicate(state.get('danger') or {}))

    def down(self, slot):
        return self.read().get('danger', {}).get('down', [False] * 4)[slot]

    def exercise(self):
        assert self.args.exe.exists(), 'Missing Cinder Demo build: ' + str(self.args.exe)
        self.start(0)
        self.wait('hazard host prepared', lambda: self.read().get('phase') == 0)
        self.start(1)
        self.wait('two players join before departure', lambda: self.every(lambda s: s.get('players') == 2 and s.get('occupiedMask') == 3))
        self.command(action=True)
        self.wait('route selected on both peers', lambda: self.phase(1))
        self.command(action=True)
        self.wait('hazard field active on both peers', lambda: self.phase(2) and self.every(lambda s: s.get('hazard')))
        # Approach through the west alley and south of warehouse; avoid the loading windbreak.
        # Keep the rescue partner three metres west until the first attack.
        for i, endpoint in ((1, -9), (0, -6)):
            for x, z in [(-38.2, -30), (-32.8, -30), (-32, -24), (-30, -24), (-30, -13), (endpoint, -13), (endpoint, -17)]:
                self.go(x, z, i=i)
        pulse = [0]
        def warned():
            if time.monotonic() > pulse[0]:
                self.command(0, call=True)
                pulse[0] = time.monotonic() + .5
            return self.every(lambda s: len(s.get('additionalDanger', [])) == 2 and s['additionalDanger'][0]['state'] == 2 and not s['danger']['down'][0])
        self.wait('B listener warning precedes host down on both peers', warned, seconds=90)
        self.command(0)
        self.wait('host down replicated', lambda: self.replicated_danger(lambda d: d.get('down', [False] * 4)[0]), seconds=8)
        p = self.read()['positions'][0]
        self.command(0, z=1, interact=True)
        self.stable('downed player cannot move', lambda: math.dist([p[k] for k in ('x', 'y', 'z')], [self.read()['positions'][0][k] for k in ('x', 'y', 'z')]) < .08, seconds=.4)
        self.command(0)
        self.command(1, rescue=True)
        self.stable('distant rescue rejected', lambda: self.read()['danger']['rescue'][1] == 0, seconds=.3)
        self.go(p['x'] - 1.6, p['z'], i=1)
        self.command(1, rescue=True)
        self.wait('partner rescue progress replicated', lambda: self.replicated_danger(lambda d: d.get('rescue', [0] * 4)[1] > .25), seconds=5)
        self.wait('host revived on both peers', lambda: self.replicated_danger(lambda d: not d.get('down', [True] * 4)[0] and d.get('rescues', 0) >= 1), seconds=5)
        self.command(1)
        assert self.phase(2), 'Rescue must preserve active shift'
        assert not self.down(1), 'Rescue partner must remain standing'
        self.capture('rescued-host', i=1, yaw=90, pitch=15)
        for i in range(2):
            log = (self.folders[i] / 'player.log').read_text(errors='replace')
            assert not any(e in log for e in ('NullReferenceException', 'IndexOutOfRangeException', 'MissingReferenceException')), 'Player exception: ' + str(i)
        self.checks.append('no null/index/missing-reference player exceptions')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--exe', type=Path, default=ROOT / 'builds/CinderDemo/NoReturns-CinderDemo.exe')
    args = parser.parse_args()
    args.crew = 2
    HazardTest(args).execute()
