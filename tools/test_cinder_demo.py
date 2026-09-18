"""Real Cinder Demo players; ordinary input only, no teleport or seeded wallet.
Run after building: python tools/test_cinder_demo.py --crew 2
The outer west/north route isolates delivery integration from enemy encounter QA.
"""
import argparse
import json
import math
import os
from pathlib import Path
import shutil
import subprocess
import time

ROOT = Path(__file__).resolve().parents[1]

def map_point(x, y):
    return ((x - 450) * .12, (360 - y) * .12)

# Cross the actual ship ramp, skirt the west buildings, enter BAY 04 north door.
OUTBOUND = [(-38.2, -30), (-32.8, -30), map_point(185, 610),
            map_point(185, 475), map_point(185, 100), map_point(722.5, 100),
            map_point(722.5, 150), (32.7, 17.2), (32.7, 15.8)]
RETURN = [(32.7, 17.2), map_point(722.5, 150), map_point(722.5, 100),
          map_point(185, 100), map_point(185, 475), map_point(185, 610),
          (-32.8, -30), (-38.2, -30)]

class DemoTest:
    def __init__(self, args):
        self.args = args
        self.output = ROOT / 'artifacts/cinder-demo-loop'
        self.run = self.output / time.strftime('run-%Y%m%d-%H%M%S')
        self.run.mkdir(parents=True)
        self.folders = [self.run / ('host' if i == 0 else 'client-' + str(i)) for i in range(args.crew)]
        for folder in self.folders:
            folder.mkdir()
        self.processes, self.checks, self.seq = [], [], 0

    def read(self, i=0, name='state.json'):
        for _ in range(20):
            try:
                return json.loads((self.folders[i] / name).read_text(encoding='utf-8'))
            except (OSError, ValueError):
                time.sleep(.005)
        return {}

    def command(self, i=0, **values):
        self.seq += 1
        data = dict(seq=self.seq, x=0, z=0, yaw=0, pitch=0, quiet=True)
        data.update(values)
        temporary = self.folders[i] / 'input.tmp'
        temporary.write_text(json.dumps(data), encoding='utf-8')
        for _ in range(100):
            try:
                os.replace(temporary, self.folders[i] / 'input.json')
                return
            except PermissionError:
                time.sleep(.01)
        raise RuntimeError('Locked input file: ' + str(temporary))

    def every(self, predicate):
        return all(predicate(self.read(i)) for i in range(self.args.crew))

    def phase(self, number):
        return self.every(lambda s: s.get('phase') == number)

    def wait(self, name, predicate, seconds=30):
        deadline = time.monotonic() + seconds
        while time.monotonic() < deadline:
            if any(p.poll() is not None for p in self.processes):
                raise AssertionError('Player exited during ' + name)
            if predicate():
                self.checks.append(name)
                print('PASS', name, flush=True)
                return
            time.sleep(.04)
        raise AssertionError(name + ' timed out; ' + json.dumps([self.read(i) for i in range(self.args.crew)]))

    def start(self, i):
        self.command(i)
        argv = [str(self.args.exe)] + (['--host'] if i == 0 else ['--join', '127.0.0.1'])
        argv += ['--test-dir', str(self.folders[i]), '-logFile', str(self.folders[i] / 'player.log'),
                 '-screen-width', '960', '-screen-height', '600', '-screen-fullscreen', '0']
        startup = subprocess.STARTUPINFO()
        startup.dwFlags |= subprocess.STARTF_USESHOWWINDOW
        startup.wShowWindow = 0
        self.processes.append(subprocess.Popen(argv, cwd=self.args.exe.parent, startupinfo=startup))

    def go(self, x, z, i=0, pitch=0, yaw=None):
        deadline = time.monotonic() + 100
        while time.monotonic() < deadline:
            s = self.read()
            positions = s.get('positions', [])
            if len(positions) <= i:
                time.sleep(.05)
                continue
            if s.get('phase') == 4:
                raise AssertionError('Unexpected abort during walk: ' + json.dumps(s))
            p = positions[i]
            dx, dz = x - p['x'], z - p['z']
            distance = math.hypot(dx, dz)
            if distance < .15:
                self.command(i, pitch=pitch, yaw=0 if yaw is None else yaw)
                time.sleep(.18)
                return
            self.command(i, pitch=pitch, yaw=math.degrees(math.atan2(dx, dz)), z=min(1, distance * 2))
            time.sleep(.07)
        raise AssertionError('Walk failed: ' + str((i, x, z)) + ' ' + json.dumps(self.read()))

    def capture(self, name, i=0, **view):
        self.command(i, **view)
        time.sleep(.3)
        source = self.folders[i] / 'capture.png'
        stamp = source.stat().st_mtime_ns if source.exists() else 0
        self.command(i, capture=True, **view)
        self.wait(name + ' capture', lambda: source.exists() and source.stat().st_mtime_ns > stamp)
        shutil.copy2(source, self.run / (name + '.png'))

    def stable(self, name, predicate, seconds=.8):
        end = time.monotonic() + seconds
        while time.monotonic() < end:
            assert predicate(), name
            time.sleep(.04)
        self.checks.append(name)

    def exercise(self):
        assert self.args.exe.exists(), 'Missing Cinder Demo build: ' + str(self.args.exe)
        self.start(0)
        self.wait('fresh ship preparation with zero wallet', lambda: self.read().get('phase') == 0 and self.read().get('credits') == 0)
        for i in range(1, self.args.crew):
            self.start(i)
            self.wait('client assigned slot ' + str(i), lambda i=i: self.read(i).get('recipient') == i)
        self.wait('crew joined before departure', lambda: self.every(lambda s: s.get('players') == self.args.crew and s.get('occupiedMask') == (1 << self.args.crew) - 1))
        assert self.every(lambda s: s.get('hazard')), 'Demo must include hazards'
        self.capture('01-ship', yaw=-90, pitch=15)
        self.command(action=True)
        self.wait('route selection replicated', lambda: self.phase(1))
        self.command(action=True)
        self.wait('auto arrival replicated', lambda: self.phase(2))
        self.go(-38.6, -30)
        cargo = self.read()['cargo']
        p = self.read()['positions'][0]
        dx, dz = cargo['x'] - p['x'], cargo['z'] - p['z']
        self.command(yaw=math.degrees(math.atan2(dx, dz)), pitch=math.degrees(math.atan2(p['y'] + 1.57 - cargo['y'], math.hypot(dx, dz))), interact=True)
        self.wait('physical cargo pickup replicated', lambda: self.every(lambda s: s.get('holder') == 0))
        for x, z in OUTBOUND:
            self.go(x, z)
        self.command(yaw=180, pitch=45)
        self.stable('held cargo cannot be received', lambda: self.read().get('phase') == 2 and self.read().get('credits') == 0)
        self.capture('02-receiver-held', yaw=180, pitch=45)
        self.command(yaw=180, pitch=45, drop=True)
        self.wait('receiver scan progress replicated', lambda: self.every(lambda s: 0 < s.get('receiptProgress', 0) < 1), seconds=12)
        self.wait('receipt issued on all peers', lambda: self.phase(3), seconds=12)
        self.wait('printed receipt ready on all peers', lambda: self.every(lambda s: s.get('receiptReady')), seconds=5)
        self.stable('receipt alone does not pay', lambda: self.every(lambda s: s.get('credits') == 0))
        self.go(29.8, 16.3)
        self.go(29.8, 13.7)
        self.go(30.6, 13.7)
        self.command(yaw=0, pitch=31, interact=True)
        self.wait('physical terminal receipt collection replicated', lambda: self.every(lambda s: s.get('receiptCollected')))
        self.capture('03-receipt-terminal', yaw=0, pitch=20)
        for language in ('en', 'ko'):
            if self.read(name='ui.json').get('language') != language:
                self.command(toggleLanguage=True)
            self.wait(language + ' interface', lambda language=language: self.read(name='ui.json').get('language') == language)
            self.capture('04-language-' + language, yaw=0, pitch=20)
        self.command(0 if self.args.crew == 1 else 1, action=True)
        self.stable('return blocked until all crew aboard', lambda: self.phase(3) and self.every(lambda s: s.get('credits') == 0))
        for x, z in RETURN:
            self.go(x, z)
        self.command(action=True)
        self.wait('420 settlement replicated', lambda: self.phase(4) and self.every(lambda s: s.get('credits') == 420 and s.get('receipt') == 300 and s.get('returnPay') == 120))
        self.stable('settlement paid only once', lambda: self.every(lambda s: s.get('credits') == 420))
        self.capture('05-settlement', yaw=-90)
        self.command(buy=True)
        self.wait('120 beacon purchase shared', lambda: self.every(lambda s: s.get('credits') == 300 and s.get('unlocked') and s.get('beaconExists')))
        self.command(action=True)
        self.wait('new shift preserves wallet and resets receiver', lambda: self.phase(0) and self.every(lambda s: s.get('credits') == 300 and s.get('charges') == 2 and s.get('beaconExists') and not s.get('receiptCollected') and s.get('receiptProgress') == 0))
        position = self.read()['beaconPosition']
        assert abs(position['z'] + 30) < 2.6 and -44.3 < position['x'] < -36.9 and .65 < position['y'] < 3, 'Physical beacon must spawn aboard: ' + str(position)
        self.checks.append('physical beacon aboard after new shift')
        self.capture('06-next-shift', yaw=-90, pitch=20)
        for i in range(self.args.crew):
            log = (self.folders[i] / 'player.log').read_text(errors='replace')
            assert not any(error in log for error in ('NullReferenceException', 'IndexOutOfRangeException', 'MissingReferenceException')), 'Player exception: ' + str(i)
        self.checks.append('no null/index/missing-reference player exceptions')

    def execute(self):
        report = dict(status='FAIL', crew=self.args.crew, run=str(self.run), checks=self.checks,
                      scope='Real local processes and ordinary scripted input; no human play, 10-15 minute experience, WAN or Steam validation.',
                      outbound=OUTBOUND, returnRoute=RETURN)
        try:
            self.exercise()
            report.update(status='PASS', final=[self.read(i) for i in range(self.args.crew)])
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
            (self.output / 'latest.json').write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding='utf-8')
            (self.run / 'report.json').write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding='utf-8')
            print(json.dumps(report, ensure_ascii=False, indent=2), flush=True)

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--crew', type=int, choices=(1, 2, 4), default=2)
    parser.add_argument('--exe', type=Path, default=ROOT / 'builds/CinderDemo/NoReturns-CinderDemo.exe')
    DemoTest(parser.parse_args()).execute()
