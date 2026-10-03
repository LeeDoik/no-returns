"""Actual windowless host/client attack posing, cooldown and carrying exclusion."""
import json
import math
import time
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True, companion=True, headless=True)
    report = dict(status="FAIL", run=str(run), checks=[])
    sequence = 1

    def send(slot, **values):
        nonlocal sequence
        sequence += 1
        lab.command(folders[slot], sequence, **values)

    def require(name, condition):
        lab.wait(name, condition)
        report["checks"].append(name)

    def animations():
        return [lab.state(f, "animation.json") for f in folders.values()]

    def observe(slot, walking=False):
        baseline_poses = [a["employees"][slot] for a in animations()]
        baseline = [p["rightHand"] for p in baseline_poses]
        seen = [False, False]
        displacement = [0, 0]
        arm_angles = [0, 0]
        send(slot, shove=True, z=.5 if walking else 0)
        start = time.monotonic()
        while time.monotonic() - start < .8:
            for peer, a in enumerate(animations()):
                pose = a["employees"][slot]
                seen[peer] |= pose["batonSwing"] and (not walking or pose["walking"])
                qa, qb = list(baseline_poses[peer]["rightUpperArm"].values()), list(pose["rightUpperArm"].values())
                dot = abs(sum(a*b for a, b in zip(qa, qb))) / math.sqrt(sum(a*a for a in qa)*sum(b*b for b in qb))
                arm_angles[peer] = max(arm_angles[peer], math.degrees(2*math.acos(min(1, dot))))
                displacement[peer] = max(displacement[peer], math.dist(list(baseline[peer].values()), list(pose["rightHand"].values())))
                if peer != slot:
                    weapon = a["batons"][slot]
                    assert weapon["handAttached"] and weapon["handDistance"] < .15
            time.sleep(.025)
        assert all(seen) and min(displacement) > .25 and min(arm_angles) > 25, (seen, displacement, arm_angles)
        report["checks"].append(f"slot {slot} arm swing {'while walking' if walking else 'from idle'} replicated; weapon follows hand")
        report.setdefault("hand_travel", []).append(displacement)
        report.setdefault("arm_rotation_degrees", []).append(arm_angles)
        send(slot)
        require(f"slot {slot} returns to ready within cooldown", lambda: all(
            not a["employees"][slot]["batonSwing"] and a["employees"][slot]["batonWeight"] == 1 for a in animations()))

    try:
        # Override bot locomotion with the existing input channel only during this focused test.
        send(1)
        require("both peers use ready arm pose", lambda: all(
            a["employees"][0]["batonWeight"] == 1 and a["employees"][1]["batonWeight"] == 1 for a in animations()))
        observe(0)
        before = lab.state(folders[0])["danger"]["cooldown"][0]
        send(0, shove=True)
        time.sleep(.25)
        assert 0 < lab.state(folders[0])["danger"]["cooldown"][0] < before
        report["checks"].append("cooldown prevents another attack")
        observe(1, walking=True)
        cargo = lab.state(folders[0])["cargo"]
        p = lab.state(folders[0])["positions"][0]
        dx, dy, dz = cargo["x"]-p["x"], cargo["y"]-(p["y"]+1.57), cargo["z"]-p["z"]
        send(0, yaw=math.degrees(math.atan2(dx, dz)), pitch=-math.degrees(math.atan2(dy, math.hypot(dx, dz))), interact=True)
        require("human picks up parcel", lambda: all(lab.state(f)["holder"] == 0 for f in folders.values()))
        require("carry excludes baton arm and weapon on both peers", lambda: all(
            a["employees"][0]["carryWeight"] == 1 and a["employees"][0]["batonWeight"] == 0
            and not a["employees"][0]["batonSwing"] and not a["batons"][0]["visible"] for a in animations()))
        require("human cooldown expires while carrying", lambda: lab.state(folders[0])["danger"]["cooldown"][0] == 0)
        send(0, shove=True)
        time.sleep(.25)
        assert all(lab.state(f)["danger"]["cooldown"][0] == 0 for f in folders.values())
        report["checks"].append("carrying rejects an attack after cooldown expires")
        send(0, drop=True)
        require("release restores ready without replaying attack", lambda: all(
            a["employees"][0]["batonWeight"] == 1 and not a["employees"][0]["batonSwing"]
            and a["batons"][0]["visible"] for a in animations()))
        for folder in folders.values():
            log = (folder / "player.log").read_text(errors="replace")
            assert not any(term in log for term in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error", "prototype visuals"))
        report["checks"].append("native logs free of errors and placeholder fallback")
        report["status"] = "PASS"
    except BaseException as error:
        report["error"] = str(error)
        raise
    finally:
        (run / "baton-check.json").write_text(json.dumps(report, indent=2) + "\n")
        (lab.OUT / "baton-latest.json").write_text(json.dumps(report, indent=2) + "\n")
        lab.stop()
        for process in processes.values():
            try:
                process.wait(timeout=5)
            except Exception:
                process.kill()
    print(json.dumps(report, indent=2))


if __name__ == "__main__":
    main()
