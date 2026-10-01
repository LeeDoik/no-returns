"""Four real Cinder players: walk, deliver, collect receipt and return; no teleport API."""
import math
import shutil
import time
import json
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True)
    checks, seq = [], 1
    report = dict(status="FAIL", run=str(run), checks=checks)

    def send(slot, **values):
        nonlocal seq
        seq += 1
        lab.command(folders[slot], seq, **values)

    def all_state(predicate):
        return all(predicate(lab.state(folders[i])) for i in range(4))

    def require(name, predicate, seconds=25):
        lab.wait(name, predicate, seconds)
        checks.append(name)

    def same_phase(phase):
        return all_state(lambda s: s.get("phase") == phase)

    def go(slot, x, z, pitch=0):
        send(slot, ui="close")
        lab.wait("gameplay controls resumed", lambda: lab.state(folders[slot], "ui.json").get("menu")=="", 5)
        lab.walk(send, folders, slot, x, z, pitch)

    def aboard(slot):
        x = (-21.65, -19.75, -21.65, -19.75)[slot]
        p = lab.state(folders[0])["positions"][slot]
        go(slot, p["x"], -20.05)
        go(slot, -20.7, -20.05)
        go(slot, -20.7, -25 if slot < 2 else -26.4)
        go(slot, x, -25 if slot < 2 else -26.4)

    def ship_click():
        send(0, ui="ship")
        require("host ship terminal opens", lambda: lab.state(folders[0], "ui.json").get("menu") == "ship")
        send(0, click="shipAction")

    def aim(slot, target, **values):
        p = lab.state(folders[0])["positions"][slot]
        dx, dy, dz = target[0]-p["x"], target[1]-(p["y"]+1.57), target[2]-p["z"]
        send(slot, yaw=math.degrees(math.atan2(dx, dz)),
             pitch=-math.degrees(math.atan2(dy, math.hypot(dx, dz))), **values)

    def capture(name, slot, target):
        stamp = (folders[slot] / "capture.png").stat().st_mtime_ns if (folders[slot] / "capture.png").exists() else 0
        aim(slot, target, capture=True)
        require(name, lambda: (folders[slot] / "capture.png").exists() and
                (folders[slot] / "capture.png").stat().st_mtime_ns > stamp)
        time.sleep(.2)
        shutil.copy2(folders[slot] / "capture.png", run / (name + ".png"))
        shutil.copy2(folders[slot] / "screen.png", run / (name + "-hud.png"))

    try:
        require("four Cinder delivery slots", lambda: all_state(lambda s:
                s.get("cinderReview") and s.get("players") == 4 and s.get("phase") == 0 and not s.get("hazard")))
        send(2, ui="settings")
        require("settings menu opens", lambda: lab.state(folders[2], "ui.json").get("menu")=="settings")
        capture("settings-ko",2,(-20.7,2,-31))
        start=lab.state(folders[0])["positions"][2]
        send(2,x=1,z=1,interact=True,drop=True,place=True,action=True,shove=True)
        time.sleep(.5)
        assert lab.state(folders[0])["positions"][2]==start and same_phase(0), "Gameplay leaked through settings"
        checks.append("settings blocks movement and gameplay actions")
        send(2,ui="close")
        require("settings closes",lambda: lab.state(folders[2],"ui.json").get("menu")=="")
        aboard(0)
        ship_click()
        require("ship button selects route", lambda: same_phase(1))
        ship_click()
        time.sleep(.5)
        assert same_phase(1), "Departure allowed while crew outside"
        checks.append("departure requires all four aboard")
        for i in range(1, 4):
            aboard(i)
        ship_click()
        require("all aboard activates field", lambda: same_phase(2))
        send(1, action=True, confirmReturn=True)
        time.sleep(.4)
        assert same_phase(2), "Client changed ship phase"
        checks.append("client cannot command return")
        ship_click()
        time.sleep(.4)
        assert same_phase(2), "Unconfirmed zero-pay return"
        checks.append("zero-pay return needs explicit confirmation")
        send(0, click="shipAction")
        require("return without receipt pays zero", lambda: same_phase(4) and all_state(lambda s: s.get("credits") == 0))
        ship_click()
        require("next shift resets all four and receipt", lambda: same_phase(0) and all_state(lambda s:
                s.get("receiptProgress") == 0 and not s.get("receiptCollected")))
        for i in range(4):
            aboard(i)
        ship_click()
        require("second route selected", lambda: same_phase(1))
        ship_click()
        require("second field arrival", lambda: same_phase(2))
        go(0, -20.7, -25)
        go(0, -20.7, -20.05)
        go(0, -21.7, -19.7)
        cargo = lab.state(folders[0])["cargo"]
        aim(0, (cargo["x"], cargo["y"], cargo["z"]), interact=True)
        require("shared cargo picked up", lambda: all_state(lambda s: s.get("holder") == 0))
        before=lab.state(folders[0])["cargo"]
        send(0,turnYaw=35,distance=1.5)
        require("rotation and reach replicate",lambda: all_state(lambda s: abs(s["rotation"]["y"])>.2 and s["cargo"]["z"]>before["z"]+.2))
        send(0,place=True)
        time.sleep(.4)
        assert all_state(lambda s:s.get("holder")==0), "Flat aim placed through a wall"
        checks.append("blocked placement keeps cargo held")
        # Approved west/north route; actual carrying controls and collision, no teleports.
        route = [(-22.95, -19.65), (-22.95, 25.95), (12.75, 25.95),
                 (12.75, 14.2), (17.0, 14.2), (17.0, 11.6)]
        for x, z in route:
            go(0, x, z, 45 if z == 11.6 else 0)
        time.sleep(.4)
        assert same_phase(2) and all_state(lambda s: s.get("credits") == 0), "Held cargo accepted or paid"
        checks.append("held cargo cannot complete delivery")
        send(0, pitch=62, place=True)
        require("precise BAY 04 placement and scan replicate", lambda: all_state(lambda s: 0 < s.get("receiptProgress", 0) < 1), 8)
        require("receipt prints on all peers without payment", lambda: same_phase(3) and all_state(lambda s: s.get("credits") == 0), 8)
        go(0, 14.9, 10.9)
        require("printing completes on all peers", lambda: all_state(lambda s: s.get("receiptReady")))
        capture("terminal-ko", 0, (14.91, 1.25, 12.17))
        send(0, toggleLanguage=True)
        require("English HUD toggle", lambda: lab.state(folders[0], "ui.json").get("language") == "en")
        capture("terminal-en", 0, (14.91, 1.25, 12.17))
        aim(0, (14.9, 1.25, 12.175), interact=True)
        require("receipt collected by one peer is shared", lambda: all_state(lambda s: s.get("receiptCollected") and s.get("credits") == 0))
        aim(0, (14.9, 1.25, 12.175), interact=True)
        send(1, interact=True)
        time.sleep(.5)
        assert same_phase(3) and all_state(lambda s: s.get("credits") == 0), "Duplicate receipt or early return paid"
        checks.append("duplicate collection and outside crew do not pay")
        capture("terminal-collected", 0, (14.91, 1.25, 12.17))
        for x, z in [(12.75, 10.9), (12.75, 25.95), (-22.95, 25.95), (-22.95, -20.05), (-20.7, -20.05), (-20.7, -25), (-21.65, -25)]:
            go(0, x, z)
        ship_click()
        require("settlement is 300 plus 120 exactly once", lambda: same_phase(4) and all_state(lambda s:
                s.get("credits") == 420 and s.get("receipt") == 300 and s.get("returnPay") == 120 and s.get("deliveries") == 1))
        time.sleep(.5)
        assert all_state(lambda s: s.get("credits") == 420), "Settlement paid twice"
        capture("ship-report", 0, (-20.7, 2.8, -31.2))
        send(1, buy=True)
        time.sleep(.4)
        assert all_state(lambda s: s.get("credits") == 420 and not s.get("unlocked")), "Client purchased shared device"
        checks.append("purchase is host-only")
        send(0, click="buy")
        require("shop purchases once and spawns Cinder beacon", lambda: all_state(lambda s: s.get("credits")==300 and s.get("unlocked") and s.get("charges")==2 and s.get("beaconExists")))
        send(0, click="buy")
        time.sleep(.4)
        assert all_state(lambda s: s.get("credits")==300), "Duplicate purchase charged twice"
        checks.append("owned purchase cannot charge twice")
        capture("ship-shop-owned", 0, (-20.7, 2.8, -31.2))
        send(0,toggleLanguage=True)
        require("Korean ship UI toggle",lambda:lab.state(folders[0],"ui.json").get("language")=="ko")
        capture("ship-shop-owned-ko",0,(-20.7,2.8,-31.2))
        go(1,-20.7,-28)
        device=lab.state(folders[0])["beaconPosition"]
        aim(1,(device["x"],device["y"],device["z"]),interact=True)
        require("client carries purchased physical beacon",lambda:all_state(lambda s:s.get("beaconCarrier")==1))
        send(1,place=True)
        require("aboard beacon placement preserves charges",lambda:all_state(lambda s:s.get("beaconCarrier")==-1 and s.get("charges")==2 and s.get("beaconTime")==0))
        ship_click()
        require("next shift keeps 300 and resets receipt", lambda: same_phase(0) and all_state(lambda s:
                s.get("credits") == 300 and s.get("receiptProgress") == 0 and not s.get("receiptCollected")))
        logs = [p for p in run.rglob("player.log") if any(v in p.read_text(errors="replace")
                for v in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error"))]
        assert not logs, logs
        checks.append("no runtime exceptions or invalid bounds")
        report.update(status="PASS", scope="4 native Mac processes; one employee physically carries to BAY 04 and returns, all four physically board. Human handling/fun, WAN, AI and progression saving remain unverified.",
                      final_states=[lab.state(folders[i]) for i in range(4)])
    except BaseException as error:
        report.update(error=str(error), last_states=[lab.state(folders[i]) for i in range(4)])
        raise
    finally:
        lab.stop()
        for process in processes.values():
            if process.poll() is None:
                process.terminate()
            process.wait(timeout=10)
        for path in (run / "delivery-check.json", lab.OUT / "delivery-latest.json"):
            path.write_text(json.dumps(report, indent=2) + "\n")


if __name__ == "__main__":
    main()
