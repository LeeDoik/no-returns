"""Windowless native carry-pose replication and unchanged parcel ownership rules."""
import json
import math
import time
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True, delivery=False, headless=True)
    report = dict(status="FAIL", run=str(run), checks=[])
    sequence = 1

    def send(slot, **values):
        nonlocal sequence
        sequence += 1
        lab.command(folders[slot], sequence, **values)

    def require(name, condition):
        lab.wait(name, condition)
        report["checks"].append(name)

    def poses():
        return [lab.state(f, "animation.json").get("employees", [None]*4)[3] for f in folders.values()]

    try:
        send(3, yaw=-81.5, pitch=45, interact=True)
        require("four peers agree on parcel owner", lambda: all(lab.state(f).get("holder") == 3 for f in folders.values()))
        # Move away from landing props before testing free parcel rotation.
        lab.walk(send, folders, 2, -24, -18.65)
        lab.walk(send, folders, 2, -24, -15.5)
        lab.walk(send, folders, 3, -22.6, -18.65)
        lab.walk(send, folders, 3, -22.6, -16.65)
        send(3, yaw=0, pitch=0)
        require("default reach holds both hands near parcel on four peers", lambda: all(
            p and p["carryWeight"] > .99 and max(p["leftContactError"], p["rightContactError"]) < .07 for p in poses()))
        report["default_contact_errors"] = [[p["leftContactError"], p["rightContactError"]] for p in poses()]
        send(2, drop=True)
        time.sleep(.3)
        assert all(lab.state(f)["holder"] == 3 for f in folders.values())
        report["checks"].append("another employee cannot drop the parcel")
        for yaw in (90, -90, 180):
            send(3, turnYaw=yaw)
            def rotated():
                for folder in folders.values():
                    q = lab.state(folder).get("rotation", {})
                    dot = q.get("y", 0)*math.sin(math.radians(yaw/2)) + q.get("w", 0)*math.cos(math.radians(yaw/2))
                    if abs(dot) < .999:
                        return False
                return True
            lab.wait("parcel rotation replicated", rotated)
            require(f"rotated parcel {yaw} degrees retains finite bounded hands", lambda: all(
                p and p["carryWeight"] > .99 and 0 <= p["leftContactError"] < .2 and 0 <= p["rightContactError"] < .2 for p in poses()))
            time.sleep(.3)
        send(3, distance=1.6)
        require("maximum reach clamps arms on four peers", lambda: all(
            p and p["carryClamped"] and p["leftContactError"] > .35 and p["rightContactError"] > .35 for p in poses()))
        send(3, distance=.75)
        require("bringing parcel back restores hand contact", lambda: all(
            p and max(p["leftContactError"], p["rightContactError"]) < .07 for p in poses()))
        send(3, drop=True)
        require("drop clears pose on every peer", lambda: all(lab.state(f)["holder"] == -1 for f in folders.values())
                and all(p and p["carryWeight"] == 0 for p in poses()))
        for f in folders.values():
            log = (f / "player.log").read_text(errors="replace")
            assert not any(term in log for term in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error", "prototype visuals"))
        report["checks"].append("four native logs have no runtime errors or art fallback")
        report["status"] = "PASS"
    except BaseException as error:
        report["error"] = str(error)
        raise
    finally:
        (run / "carry-check.json").write_text(json.dumps(report, indent=2) + "\n")
        (lab.OUT / "carry-latest.json").write_text(json.dumps(report, indent=2) + "\n")
        lab.stop()
        for process in processes.values():
            try:
                process.wait(timeout=5)
            except Exception:
                process.kill()
    print(json.dumps(report, indent=2))


if __name__ == "__main__":
    main()
