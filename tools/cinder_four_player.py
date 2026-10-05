"""Build, launch, stop and check four native Cinder players using the existing LAN runtime."""
import argparse
import json
import math
import os
from pathlib import Path
import plistlib
import shutil
import signal
import socket
import subprocess
import sys
import time
import uuid

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / "artifacts/cinder-four-player"
SESSION = OUT / "session.json"
PORT = 27842


def player():
    folder = ROOT / "builds/CinderFourPlayer"
    if sys.platform == "darwin":
        app = folder / "NoReturns.app"
        info = app / "Contents/Info.plist"
        if not info.is_file():
            raise RuntimeError("Build the player first: python3 tools/cinder_four_player.py build")
        with info.open("rb") as stream:
            return app / "Contents/MacOS" / plistlib.load(stream)["CFBundleExecutable"]
    if sys.platform == "win32":
        return folder / "NoReturns.exe"
    raise RuntimeError("This test player supports macOS and Windows.")


def port_busy():
    with socket.socket() as probe:
        try:
            probe.bind(("127.0.0.1", PORT))
            return False
        except OSError:
            return True


def wait(name, condition, seconds=25):
    end = time.monotonic() + seconds
    while time.monotonic() < end:
        if condition():
            print("PASS", name, flush=True)
            return
        time.sleep(.08)
    raise AssertionError(name)


def command_line(pid):
    if sys.platform == "win32":
        return subprocess.check_output(["powershell", "-NoProfile", "-Command",
            f"(Get-CimInstance Win32_Process -Filter 'ProcessId={int(pid)}').CommandLine"], text=True).strip()
    result = subprocess.run(["ps", "-p", str(pid), "-o", "command="], capture_output=True, text=True)
    return result.stdout.strip()


def stop():
    if not SESSION.exists():
        return
    session = json.loads(SESSION.read_text())
    for pid in reversed(session["pids"]):
        text = command_line(pid)
        # A stale PID must never terminate another application.
        if session["player"] in text and "--cinder-session " + session["id"] in text:
            try:
                os.kill(pid, signal.SIGTERM)
            except ProcessLookupError:
                pass
    SESSION.unlink()


def command(folder, seq, **values):
    value = dict(seq=seq, x=0, z=0, yaw=0, pitch=0)
    value.update(values)
    temporary = folder / "input.tmp"
    temporary.write_text(json.dumps(value))
    os.replace(temporary, folder / "input.json")


def state(folder, name="state.json"):
    try:
        return json.loads((folder / name).read_text())
    except (OSError, ValueError):
        return {}


def launch(automated=False, delivery=True, hazard=True, companion=False, headless=False, quick=False):
    count = 2 if companion or quick else 4
    if quick and (not automated or not headless or companion or not hazard or not delivery):
        raise ValueError("Quick fixtures require automated, headless hazard clients without a companion bot")
    binary = player()
    if not binary.is_file():
        raise RuntimeError("Build the player first: python3 tools/cinder_four_player.py build")
    if SESSION.exists() or port_busy():
        raise RuntimeError("A session or TCP 27842 is already in use. Stop this test first.")
    token = uuid.uuid4().hex
    run = OUT / (time.strftime("run-%Y%m%d-%H%M%S-") + token[:6])
    run.mkdir(parents=True)
    processes, folders = {}, {}
    session = dict(id=token, player=str(binary), run=str(run), pids=[])

    def start(slot):
        folder = run / str(slot)
        folder.mkdir(exist_ok=True)
        folders[slot] = folder
        args = [str(binary), "--host"] if slot == 0 else [str(binary), "--join", "127.0.0.1"]
        if companion:
            args += ["--companion-practice"]
            if slot == 1:
                args += ["--companion-bot"]
        if quick:
            args += ["--quick-test"]
        if headless:
            args += ["-batchmode", "-nographics"]
        if not delivery:
            args += ["--map-only"]
        elif not hazard:
            args += ["--delivery-only"]
        args += ["--cinder-session", token, "-screen-width", "800", "-screen-height", "500",
                 "-screen-fullscreen", "0", "-logFile", str(folder / "player.log")]
        if not headless:
            args += ["--cinder-window-count", str(count), "--cinder-window-slot", str(slot),
                     "--cinder-window-dir", str(folder)]
        if automated or (companion and slot == 1):
            if not (companion and slot == 1):
                command(folder, 1)
            args += ["--test-dir", str(folder)]
        with (folder / "stdout.log").open("ab") as log:
            processes[slot] = subprocess.Popen(args, cwd=binary.parent, stdout=log, stderr=log,
                                               start_new_session=sys.platform != "win32")
        session["pids"].append(processes[slot].pid)
        SESSION.write_text(json.dumps(session, indent=2) + "\n")
        return processes[slot]

    try:
        start(0)
        wait("host listening", lambda: port_busy() and processes[0].poll() is None)
        for slot in range(1, count):
            start(slot)
            if automated or companion:
                wait(f"assigned slot {slot}", lambda: state(folders[slot]).get("recipient") == slot)
        if companion:
            wait("companion bot ready (rebuild if this times out)", lambda:
                 state(folders[1], "animation.json").get("companionBot") and
                 state(folders[1]).get("occupiedMask") == 3 and
                 state(folders[1]).get("phase") == -1 and not state(folders[1]).get("hazard"))
        if automated:
            wait(f"{count} assigned slots", lambda: all(state(folders[i]).get("recipient") == i and
                state(folders[i]).get("occupiedMask") == (1 << count)-1 for i in range(count)))
        print("Session:", run, flush=True)
        return run, processes, folders, start
    except BaseException as error:
        stop()
        for process in processes.values():
            if process.poll() is None:
                process.terminate()
        if automated:
            report = dict(status="FAIL", error=str(error), checks=[], run=str(run))
            for path in (run / "check.json", OUT / "latest.json"):
                path.write_text(json.dumps(report, indent=2) + "\n")
        raise


def walk(send, folders, slot, x, z, pitch=0):
    end = time.monotonic() + 20
    while time.monotonic() < end:
        positions = state(folders[0]).get("positions")
        if not positions:
            time.sleep(.08)
            continue
        p = positions[slot]
        dx, dz = x - p["x"], z - p["z"]
        distance = math.hypot(dx, dz)
        if distance < .15:
            send(slot, pitch=pitch)
            return
        send(slot, yaw=math.degrees(math.atan2(dx, dz)), z=min(1, distance * 2), pitch=pitch)
        time.sleep(.08)
    raise AssertionError(f"Blocked movement: slot {slot} to {(x, z)}; {state(folders[0])}")


def check():
    run, processes, folders, start = launch(True, delivery=False)
    checks, seq = [], 1

    def send(slot, **values):
        nonlocal seq
        seq += 1
        command(folders[slot], seq, **values)

    def all_state(predicate):
        return all(predicate(state(folders[i])) for i in range(4))

    def require(name, predicate, seconds=25):
        wait(name, predicate, seconds)
        checks.append(name)

    def go(slot, x, z):
        walk(send, folders, slot, x, z)

    report = dict(status="FAIL", run=str(run), checks=checks)
    try:
        require("Cinder mode and four peers replicated", lambda: all_state(lambda s:
            s.get("cinderReview") and s.get("protocol") == 15 and s.get("players") == 4
            and s.get("phase") == -1 and not s.get("hazard")))
        send(3, yaw=-81.5, pitch=45, interact=True)
        require("slot 3 picks up shared parcel", lambda: all_state(lambda s: s.get("holder") == 3))
        send(2, drop=True)
        time.sleep(.5)
        assert all_state(lambda s: s.get("holder") == 3), "Another peer dropped owned cargo"
        checks.append("non-owner cannot release cargo")
        go(2, -24, -18.65)
        go(2, -24, -15.5)
        go(3, -22.6, -18.65)
        go(3, -22.6, -16.65)
        require("slot 3 movement replicated", lambda: all_state(lambda s:
            abs(s["positions"][3]["z"] + 16.65) < .25))
        send(3, drop=True)
        require("owner releases cargo on all peers", lambda: all_state(lambda s: s.get("holder") == -1))
        send(0, reset=True)
        require("host reset returns all four to landing", lambda: all_state(lambda s:
            s.get("holder") == -1 and abs(s["positions"][3]["z"] + 18.65) < .2))
        # Four actual controllers traverse the retained 3m lane together, not four virtual width probes.
        for i, x in enumerate((-24, -23.3, -22.6, -21.9)):
            if i == 3:
                go(i, -19.7, -19.7)  # Walk around the grounded shared parcel.
            go(i, x, -20.05 if i < 2 else -19.7 if i == 3 else -18.65)
            go(i, x, -15.5)
        before = [p["z"] for p in state(folders[0])["positions"]]
        for i in range(4):
            send(i, z=1)
        time.sleep(.8)
        for i in range(4):
            send(i)
        require("four simultaneous controllers pass west lane", lambda: all_state(lambda s:
            all(p["z"] - before[i] > 1 for i, p in enumerate(s["positions"]))))
        start(4)
        require("fifth peer rejected", lambda: "Room full" in state(folders[4]).get("message", ""))
        processes[4].terminate()
        processes[4].wait(timeout=10)
        processes[2].terminate()
        processes[2].wait(timeout=10)
        require("disconnect preserves remaining slot identities", lambda: all(
            state(folders[i]).get("occupiedMask") == 11 and state(folders[i]).get("recipient") == i
            for i in (0, 1, 3)))
        start(2)
        require("freed slot 2 rejoins", lambda: all_state(lambda s: s.get("occupiedMask") == 15)
                and state(folders[2]).get("recipient") == 2)
        go(2, -22.6, -18.65)
        go(2, -22.6, -13.3)
        go(0, -24, -17)
        for i in (0, 3):
            send(i, yaw=25 if i == 0 else -125, pitch=10, capture=True)
        require("two native player captures", lambda: all(
            (folders[i] / "capture.png").is_file() and (folders[i] / "screen.png").is_file()
            for i in (0, 3)))
        require("Cinder HUD evidence and Korean font", lambda: all(
            state(folders[i], "ui.json").get("objective") in
            ("CINDER / FOUR-PLAYER MAP TEST", "CINDER / 4인 맵 테스트") and
            state(folders[i], "ui.json").get("koreanGlyph") for i in range(4)))
        logs = [p for p in run.rglob("player.log") if any(v in p.read_text(errors="replace")
                for v in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error"))]
        assert not logs, logs
        checks.append("no runtime exceptions or invalid bounds in player logs")
        report.update(status="PASS", scope="Four native local processes, scripted input; human fun, LAN/WAN and performance remain unverified.",
                      final_states=[state(folders[i]) for i in range(4)])
    except BaseException as error:
        report.update(error=str(error), last_states=[state(folders[i]) for i in range(4)])
        raise
    finally:
        stop()
        for process in processes.values():
            if process.poll() is None:
                process.terminate()
            process.wait(timeout=10)
        (run / "check.json").write_text(json.dumps(report, indent=2) + "\n")
        (OUT / "latest.json").write_text(json.dumps(report, indent=2) + "\n")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("action", choices=("build", "start", "stop", "check"))
    parser.add_argument("--map-only", action="store_true", help="Start movement/carrying without the delivery loop")
    parser.add_argument("--delivery-only", action="store_true", help="Start delivery without the Listener")
    parser.add_argument("--companion", action="store_true", help="Manual host plus one nearby demonstration bot, safe practice only")
    args = parser.parse_args()
    if args.action == "build":
        binary = shutil.which("unity") or str(Path.home() / ".unity/bin/unity")
        receipt = OUT / "build.json"
        receipt.unlink(missing_ok=True)
        # Schedule on the next Editor tick; a synchronous build outlasts the pipeline's 5s main-thread limit.
        result = subprocess.run([binary, "command", "eval", "--code",
            f"if(UnityEditor.BuildPipeline.isBuildingPlayer)throw new System.Exception(\"Build already running\");NoReturns.Editor.CinderFourPlayerBuild.Queue({str(sys.platform == 'darwin').lower()});return true;",
            "--project-path", str(ROOT / "NoReturns"), "--caller", "plugin", "--skill", "unity-cli", "--format", "json"],
            capture_output=True, text=True)
        response = json.loads(result.stdout)
        if result.returncode or not response.get("success") or not response.get("data", {}).get("result", {}).get("success"):
            raise RuntimeError(result.stdout + result.stderr)
        wait("native build receipt", receipt.is_file, 900)
        report = json.loads(receipt.read_text())
        if report["status"] != "PASS" or report["errors"]:
            raise RuntimeError(report)
        from cinder_build_stamp import record
        record(player())
        print("Built:", player())
    elif args.action == "start":
        launch(delivery=not args.map_only, hazard=not args.delivery_only, companion=args.companion)
    elif args.action == "stop":
        stop()
    else:
        check()


if __name__ == "__main__":
    main()
