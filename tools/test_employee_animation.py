"""Four native players: real movement drives the locally prepared Humanoid visuals."""
import json
import math
import shutil
import time
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True, delivery=False)
    checks, seq = [], 1
    report = dict(status="FAIL", run=str(run), checks=checks)

    def send(slot, **values):
        nonlocal seq
        seq += 1
        lab.command(folders[slot], seq, **values)

    def employees(slot):
        return lab.state(folders[slot], "animation.json").get("employees") or []

    def require(name, predicate):
        lab.wait(name, predicate)
        checks.append(name)

    try:
        require("all sixteen Humanoid instances ready", lambda: all(
            len(employees(i)) == 4 and all(e and e["ready"] for e in employees(i)) for i in range(4)))
        require("local body hidden, three teammates visible, root motion disabled", lambda: all(
            all(e["visible"] == (i != j) and not e["rootMotion"] for j, e in enumerate(employees(i))) for i in range(4)))
        require("four stationary employees idle on every peer", lambda: all(
            all(not e["walking"] and e["speed"] < .08 for e in employees(i)) for i in range(4)))
        lab.walk(send, folders, 2, -24, -18.65)
        send(2, z=1)
        require("remote actual movement enters Walk on every peer", lambda: all(employees(i)[2]["walking"] for i in range(4)))
        before = [employees(i)[2]["leftFoot"] for i in range(4)]
        time.sleep(.2)
        require("walking animates the foot on every peer", lambda: all(
            math.dist(list(before[i].values()), list(employees(i)[2]["leftFoot"].values())) > .015 for i in range(4)))
        send(2)
        require("stopping returns to Idle on every peer", lambda: all(
            not employees(i)[2]["walking"] and employees(i)[2]["speed"] < .08 for i in range(4)))
        # A host-controlled employee must animate on clients too.
        send(0, z=-1)
        require("host actual movement enters Walk on every peer", lambda: all(employees(i)[0]["walking"] for i in range(4)))
        time.sleep(.25)
        send(0)
        require("host stopping returns to Idle on every peer", lambda: all(not employees(i)[0]["walking"] for i in range(4)))
        # Face the tested employee and capture the actual native game camera.
        p, target = lab.state(folders[0])["positions"][0], lab.state(folders[0])["positions"][2]
        dx, dz = target["x"]-p["x"], target["z"]-p["z"]
        send(2, yaw=math.degrees(math.atan2(-dx, -dz)))
        send(0, yaw=math.degrees(math.atan2(dx, dz)), pitch=10, capture=True)
        require("native teammate capture", lambda: (folders[0]/"screen.png").exists())
        shutil.copy2(folders[0]/"screen.png", run/"employee-in-game.png")
        for folder in folders.values():
            log = (folder/"player.log").read_text(errors="replace")
            assert not any(t in log for t in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error", "prototype visuals")), log[-2000:]
        checks.append("four logs have no animation errors or placeholder fallback")
        report.update(status="PASS", final=[employees(i) for i in range(4)])
    except BaseException as error:
        report.update(error=str(error))
        raise
    finally:
        (run/"animation-check.json").write_text(json.dumps(report, indent=2)+"\n")
        (lab.OUT/"animation-latest.json").write_text(json.dumps(report, indent=2)+"\n")
        lab.stop()
        for process in processes.values():
            try: process.wait(timeout=5)
            except Exception: process.kill()
    print(json.dumps(dict(status=report["status"], checks=len(checks), run=str(run))))


if __name__ == "__main__":
    main()
