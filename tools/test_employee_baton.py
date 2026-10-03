"""Windowless native checks of the restored original baton and directional footsteps."""
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

    def observe(slot):
        axes = [list(a["batons"][slot]["axis"].values()) for a in animations()]
        seen, angle = [False, False], [0, 0]
        send(slot, shove=True)
        start = time.monotonic()
        while time.monotonic()-start < .7:
            for peer, a in enumerate(animations()):
                seen[peer] |= lab.state(folders[peer])["danger"]["cooldown"][slot] > 5.5
                pose, weapon = a["employees"][slot], a["batons"][slot]
                assert pose["batonWeight"] == 0 and not pose["batonSwing"]
                axis = list(weapon["axis"].values())
                dot = sum(x*y for x,y in zip(axes[peer],axis))/math.sqrt(sum(x*x for x in axes[peer])*sum(x*x for x in axis))
                angle[peer] = max(angle[peer], math.degrees(math.acos(max(-1,min(1,dot)))))
                if peer != slot:
                    assert weapon["handAttached"] and weapon["handDistance"] < .15
            time.sleep(.02)
        assert all(seen) and min(angle)>15, (seen,angle)
        report["checks"].append(f"slot {slot}: original weapon swing replicated with hand attachment and no attack arm/chest layer")
        report.setdefault("weapon_rotation_degrees", []).append(angle)
        before=lab.state(folders[0])["danger"]["cooldown"][slot]
        send(slot, shove=True)
        time.sleep(.2)
        assert 0 < lab.state(folders[0])["danger"]["cooldown"][slot] < before
        report["checks"].append(f"slot {slot}: cooldown prevents a second attack")
        send(slot)

    try:
        send(1)
        require("both peers retain the original unmodified arm layer", lambda: all(
            all(p["batonWeight"] == 0 and not p["batonSwing"] for p in a["employees"][:2]) for a in animations()))
        observe(0)
        observe(1)
        cargo=lab.state(folders[0])["cargo"];p=lab.state(folders[0])["positions"][0]
        dx,dy,dz=cargo["x"]-p["x"],cargo["y"]-(p["y"]+1.57),cargo["z"]-p["z"]
        send(0,yaw=math.degrees(math.atan2(dx,dz)),pitch=-math.degrees(math.atan2(dy,math.hypot(dx,dz))),interact=True)
        require("human picks up parcel",lambda:all(lab.state(f)["holder"]==0 for f in folders.values()))
        require("carry hides weapon on both peers",lambda:all(a["employees"][0]["carryWeight"]==1 and not a["batons"][0]["visible"] for a in animations()))
        require("human cooldown expires while carrying",lambda:lab.state(folders[0])["danger"]["cooldown"][0]==0)
        send(0,shove=True);time.sleep(.2)
        assert all(lab.state(f)["danger"]["cooldown"][0]==0 for f in folders.values())
        report["checks"].append("carrying rejects an attack")
        send(0,drop=True)
        require("release restores hand-attached weapon without arm attack layer",lambda:all(a["batons"][0]["visible"] and a["employees"][0]["batonWeight"]==0 for a in animations()))
        # Use the existing clear access lane, away from the dropped cargo and spawn props.
        send(0,yaw=270,z=.5);time.sleep(1.5);send(0);time.sleep(.4)
        # Actual controller movement, viewed from both peers at two different body yaws.
        for yaw in (0,90):
            for x,z,name in ((0,-.22,"Backward"),(-.22,0,"StrafeLeft"),(.22,0,"StrafeRight")):
                origin=lab.state(folders[0])["positions"][0]
                started=time.monotonic()
                send(0,x=x,z=z,yaw=yaw)
                require(f"{name} at yaw {yaw} drives both actual employee poses",lambda:all(
                    a["employees"][0]["walking"] and a["employees"][0]["direction"]==name and a["employees"][0]["directionWeight"]>.9
                    and a["employees"][0]["leftFootPosition"]["x"]<-.03 and a["employees"][0]["rightFootPosition"]["x"]>.03 for a in animations()))
                moved=lab.state(folders[0])["positions"][0]
                assert math.dist(list(origin.values()),list(moved.values()))>.05
                send(0,x=-x,z=-z,yaw=yaw);time.sleep(time.monotonic()-started);send(0,yaw=yaw);time.sleep(.55)
        send(0,jump=True)
        require("jump suspends direction correction on both peers",lambda:all(a["employees"][0]["airWeight"]>.8 and a["employees"][0]["directionWeight"]==0 for a in animations()))
        send(0)
        require("landing returns to idle directional pose",lambda:all(a["employees"][0]["locomotion"]=="Grounded" and a["employees"][0]["direction"]=="Idle" and a["employees"][0]["directionWeight"]==0 for a in animations()))
        for folder in folders.values():
            log=(folder/"player.log").read_text(errors="replace")
            assert not any(term in log for term in ("Exception:","Invalid AABB","Assertion failed","Fatal Error","prototype visuals"))
        report["checks"].append("native logs free of errors and placeholder fallback")
        report["status"]="PASS"
    except BaseException as error:
        report["error"]=str(error);raise
    finally:
        (run/"baton-check.json").write_text(json.dumps(report,indent=2)+"\n")
        (lab.OUT/"baton-latest.json").write_text(json.dumps(report,indent=2)+"\n")
        lab.stop()
        for process in processes.values():
            try:process.wait(timeout=5)
            except Exception:process.kill()
    print(json.dumps(report,indent=2))


if __name__=="__main__":main()
