"""Four native Cinder players; ordinary movement/use/baton/rescue inputs, no teleport or down injection."""
import json
import math
import shutil
import time
import cinder_four_player as lab


def main(headless=False):
    raise RuntimeError("Legacy single-parcel/full-recovery scenario needs migration to visit and multiple-delivery rules. Use cinder_quick_test.py and test_cinder_delivery.py --headless; these do not replace full hazard regression.")
    run, processes, folders, _ = lab.launch(True, headless=headless)
    checks, seq = [], 1
    report = dict(status="FAIL", run=str(run), checks=checks)

    def send(slot, **values):
        nonlocal seq
        seq += 1
        values.setdefault("quiet",True)
        lab.command(folders[slot], seq, **values)

    def host():
        return lab.state(folders[0])

    def danger(slot=0):
        return lab.state(folders[slot]).get("danger") or {}

    def require(name, predicate, seconds=25):
        lab.wait(name, predicate, seconds)
        checks.append(name)

    def go(slot, x, z, quiet=True, allow_down=False):
        end = time.monotonic() + 45
        while time.monotonic() < end:
            p = host()["positions"][slot]
            dx, dz = x-p["x"], z-p["z"]
            if math.hypot(dx, dz) < .15:
                send(slot)
                return
            seq_value = dict(yaw=math.degrees(math.atan2(dx, dz)), z=min(1, math.hypot(dx, dz)*2))
            send(slot,quiet=quiet,**seq_value)
            if danger().get("down", [False]*4)[slot]:
                if allow_down: return
                raise AssertionError(f"slot {slot} down walking to {(x,z)}")
            time.sleep(.08)
        raise AssertionError(f"slot {slot} blocked walking to {(x,z)}: {host()['positions'][slot]}")

    def aim(slot, target, **values):
        p = host()["positions"][slot]
        dx, dy, dz = target[0]-p["x"], target[1]-(p["y"]+1.57), target[2]-p["z"]
        send(slot, yaw=math.degrees(math.atan2(dx, dz)), pitch=-math.degrees(math.atan2(dy, math.hypot(dx, dz))), **values)

    def face(slot, **values):
        p = danger()["position"]
        aim(slot, (p["x"], p["y"]+1.2, p["z"]), **values)

    def capture(name, slot):
        if headless:
            return
        path = folders[slot] / "screen.png"
        stamp = path.stat().st_mtime_ns if path.exists() else 0
        face(slot, capture=True)
        require(name, lambda: path.exists() and path.stat().st_mtime_ns > stamp)
        shutil.copy2(path, run / (name+".png"))

    try:
        require("four hazard slots/protocol 14", lambda: all(lab.state(folders[i]).get("occupiedMask")==15 and lab.state(folders[i]).get("hazard") and lab.state(folders[i]).get("protocol")==14 for i in range(4)))
        require("Cinder stages start stopped with shared outer down state", lambda: len((host().get("outerDanger") or {}).get("down",[]))==4 and host().get("shiftElapsed")==0)
        for i in range(4):
            go(i, -20.7, -20.05, quiet=False)
            go(i, -20.7, -25.5-(i//2)*1.4, quiet=False)
            go(i, -21.65 if i%2==0 else -19.75, -25.5-(i//2)*1.4, quiet=False)
        send(0, ui="ship")
        require("ship UI opens", lambda: lab.state(folders[0],"ui.json").get("menu")=="ship")
        send(0, click="shipAction")
        require("route selected", lambda: host().get("phase")==1)
        send(0, click="shipAction")
        require("departure enables patrol", lambda: host().get("phase")==2)
        send(0, ui="close")
        require("ship UI closes", lambda: lab.state(folders[0],"ui.json").get("menu")=="")
        for i in range(4): send(i, call=True)
        time.sleep(.5)
        require("ship calls ignored/no attack", lambda: danger().get("noises")==0 and not any(danger().get("down",[True])))
        go(0,-20.7,-25,quiet=False);go(0,-20.7,-20.05,quiet=False)
        cargo=host()["cargo"];aim(0,(cargo["x"],cargo["y"],cargo["z"]),interact=True)
        require("host carries parcel", lambda: host().get("holder")==0)
        go(0,-22.95,-19.65);go(0,-22.95,-13.5)
        require("quiet approach has no footstep noise", lambda: danger().get("noises")==0)
        send(0,call=True)
        require("call causes investigation", lambda: danger().get("state")==1 and danger().get("noises",0)>0)
        require("warning before cargo-carrier attack", lambda: danger().get("state")==2 and not danger()["down"][0])
        require("host down replicated to all four", lambda: all(danger(i).get("down",[False]*4)[0] for i in range(4)))
        require("down releases parcel on all peers", lambda: all(lab.state(folders[i]).get("holder")==-1 for i in range(4)))
        before=host()["positions"][0];send(0,z=1,interact=True,shove=True);time.sleep(.4)
        require("down blocks movement/pickup/baton", lambda: math.dist(list(before.values()),list(host()["positions"][0].values()))<.05 and host().get("holder")==-1 and danger()["cooldown"][0]==0)
        require("down hides the local first-person arms",lambda:not lab.state(folders[0],"animation.json")["hands"]["rightVisible"] and not lab.state(folders[0],"animation.json")["hands"]["leftVisible"])
        send(3,rescue=True);time.sleep(.4)
        require("distant rescue rejected", lambda: danger()["rescue"][3]==0)
        send(3)
        go(3,-20.7,-26.9,quiet=False);go(3,-20.7,-20.05,quiet=False);go(3,-22.95,-19.65,quiet=False);go(3,-22.95,-15.2)
        next_bait=[0]
        def close_warning():
            if time.monotonic()>next_bait[0]:
                send(3,call=True);next_bait[0]=time.monotonic()+.5
            return danger().get("state")==2 and not danger()["down"][3]
        require("slot 3 baits a close warning",close_warning)
        send(3,rescue=True,shove=True)
        require("valid rescue blocks baton contact and posing", lambda: all(
            danger(i).get("rescue",[0]*4)[3]>.06 and danger(i)["cooldown"][3]==0
            and not lab.state(folders[i],"animation.json")["employees"][3]["batonSwing"]
            and lab.state(folders[i],"animation.json")["employees"][3]["batonWeight"]==0 for i in range(4)))
        require("valid rescue shows both supported first-person hands and hides the baton",lambda:lab.state(folders[3],"animation.json")["hands"]["rescuing"] and lab.state(folders[3],"animation.json")["hands"]["leftVisible"] and lab.state(folders[3],"animation.json")["hands"]["rightVisible"] and lab.state(folders[3],"animation.json")["hands"]["wristBend"]<=25.01 and not lab.state(folders[3],"animation.json")["batons"][3]["visible"])
        face(3,shove=True)
        require("slot 3 baton stuns on four peers", lambda: all(danger(i).get("state")==4 for i in range(4)))
        capture("listener-baton-ko",3)
        send(3,rescue=True)
        require("slot 3 rescue progress replicated", lambda: all(danger(i).get("rescue",[0]*4)[3]>.35 for i in range(4)))
        send(3)
        require("release cancels rescue and restores right-hand baton", lambda: danger()["rescue"][3]==0 and not lab.state(folders[3],"animation.json")["hands"]["rescuing"] and not lab.state(folders[3],"animation.json")["hands"]["leftVisible"] and lab.state(folders[3],"animation.json")["batons"][3]["visible"])
        old=danger()["cooldown"][3];face(3,shove=True);time.sleep(.2)
        require("baton cooldown prevents reset", lambda: 0<danger()["cooldown"][3]<old)
        send(3,rescue=True)
        require("slot 3 rescues host on all peers", lambda: all(not danger(i).get("down",[True]*4)[0] and danger(i).get("rescues",0)>0 for i in range(4)))
        send(3);send(0)
        require("revival clears rescue hands on the rescuer",lambda:not lab.state(folders[3],"animation.json")["hands"]["rescuing"] and lab.state(folders[3],"animation.json")["batons"][3]["visible"])
        # Move away from the recovering creature while the revived employee has protection.
        go(0,-22.2,-16.4)
        require("rescuer remains vulnerable after revival",lambda:danger()["down"][3],15)
        send(3,toggleLanguage=True)
        require("English client context/native font", lambda: lab.state(folders[3],"ui.json").get("language")=="en" and lab.state(folders[3],"ui.json").get("koreanGlyph"))
        capture("listener-down-en",3)
        # Bring the other two employees to the same lane, then use calls to test shared emergency recovery.
        for i,x in [(1,-23.7),(2,-22)]:
            go(i,-20.7,-25.5-(i//2)*1.4,quiet=False,allow_down=True);go(i,-20.7,-20.05,quiet=False,allow_down=True);go(i,x,-19.65,quiet=False,allow_down=True);go(i,x,-18.2,allow_down=True)
        next_call=[0]
        def recovered():
            if time.monotonic()>next_call[0]:
                for i in range(4): send(i,call=True)
                next_call[0]=time.monotonic()+.6
            return host().get("phase")==4 and danger().get("evacuations",0)>0
        require("all four down triggers emergency report",recovered,75)
        require("four recovered aboard/zero unpaid reward",lambda: all(lab.state(folders[i]).get("phase")==4 and not any(danger(i).get("down",[True])) for i in range(4)) and host().get("credits")==0 and all(abs(p["x"]+20.7)<1.5 and -30.9<p["z"]<-24.8 and p["y"]>.8 for p in host()["positions"]))
        send(0,ui="ship");require("recovered host can open ship terminal",lambda:lab.state(folders[0],"ui.json").get("menu")=="ship")
        send(0,click="shipAction");require("next shift clears down/cooldown and restores parcel",lambda:host().get("phase")==0 and host().get("holder")==-1 and not any(danger().get("down",[True])) and not any(danger().get("cooldown",[1])))
        require("no Cinder progression file created",lambda: not (folders[0]/"progression-v1.json").exists())
        for folder in folders.values():
            log=(folder/"player.log").read_text(errors="replace")
            assert not any(token in log for token in ("Exception:","Invalid AABB","Fatal error")),log[-2000:]
        checks.append("four player logs have no exceptions")
        report.update(status="PASS",final=host(),scope="Four native Mac processes and ordinary inputs; human fear, audio readability, fun, LAN/Windows and performance remain unverified")
    except BaseException as error:
        report.update(error=str(error),final=host())
        raise
    finally:
        for path in (run/"check.json",lab.OUT/"threat-latest.json"):
            path.write_text(json.dumps(report,indent=2)+"\n")
        lab.stop()
        for process in processes.values():
            try: process.wait(timeout=5)
            except Exception: process.kill()
    print(json.dumps(dict(status=report["status"],checks=len(checks),run=str(run)),indent=2))


if __name__=="__main__":
    import argparse
    parser=argparse.ArgumentParser()
    parser.add_argument("--headless",action="store_true")
    main(parser.parse_args().headless)
