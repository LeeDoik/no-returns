"""Four native Cinder players: hazardous delivery, purchased beacon, real suppression time and outer recovery."""
import json
import math
import shutil
import time
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True)
    checks, seq = [], 1
    report = dict(status="FAIL", run=str(run), checks=checks)

    def send(slot, **values):
        nonlocal seq
        seq += 1
        lab.command(folders[slot], seq, **values)

    def host():
        return lab.state(folders[0])

    def require(name, predicate, seconds=25):
        lab.wait(name, predicate, seconds)
        checks.append(name)

    def all_state(predicate):
        return all(predicate(lab.state(folders[i])) for i in range(4))

    def batons(peer):
        return lab.state(folders[peer], "animation.json").get("batons") or []

    def baton_visible(slot, visible):
        return all(len(batons(peer))==4 and batons(peer)[slot]["visible"]==visible for peer in range(4))

    def go(slot, x, z):
        send(slot, ui="close")
        lab.wait("gameplay controls resumed", lambda: lab.state(folders[slot], "ui.json").get("menu")=="", 5)
        lab.walk(send, folders, slot, x, z)
        assert not host()["danger"]["down"][slot], f"Employee {slot} down on route to {(x,z)}"

    def aim(slot, target, **values):
        p = host()["positions"][slot]
        dx, dy, dz = target[0]-p["x"], target[1]-(p["y"]+1.57), target[2]-p["z"]
        send(slot, yaw=math.degrees(math.atan2(dx, dz)), pitch=-math.degrees(math.atan2(dy, math.hypot(dx, dz))), **values)

    def aboard(slot):
        p = host()["positions"][slot]
        go(slot, p["x"], -20.05)
        go(slot, -20.7, -20.05)
        go(slot, -20.7, -25.5-(slot//2)*1.4)
        go(slot, -21.65 if slot%2==0 else -19.75, -25.5-(slot//2)*1.4)

    def ship_click(confirm=False):
        send(0, ui="ship")
        require("host ship UI", lambda: lab.state(folders[0], "ui.json").get("menu")=="ship")
        send(0, click="shipAction")
        if confirm:
            time.sleep(.3)
            send(0, click="shipAction")

    def depart():
        for i in range(4): aboard(i)
        ship_click()
        require("route selected", lambda: all_state(lambda s:s.get("phase")==1))
        send(0, click="shipAction")
        require("field active on four peers", lambda: all_state(lambda s:s.get("phase")==2))
        send(0, ui="close")
        require("ship UI closed", lambda: lab.state(folders[0], "ui.json").get("menu")=="")

    def capture(name, slot, target):
        image=folders[slot]/"screen.png"
        stamp=image.stat().st_mtime_ns if image.exists() else 0
        aim(slot,target,capture=True)
        require(name,lambda:image.exists() and image.stat().st_mtime_ns>stamp)
        shutil.copy2(image,run/(name+".png"))

    try:
        require("four hazard slots/protocol 14", lambda: all_state(lambda s:s.get("occupiedMask")==15 and s.get("protocol")==14 and s.get("hazard")))
        require("preparation clock stopped",lambda:host().get("shiftElapsed")==0)
        require("remote batons follow right hands, local view remains visible",lambda:all(
            len(batons(peer))==4 and all(b["visible"] and (slot==peer or b["handAttached"] and .05<b["handDistance"]<.19)
                for slot,b in enumerate(batons(peer))) for peer in range(4)))
        p=host()["positions"][3]
        aim(3,(host()["positions"][0]["x"],1,host()["positions"][0]["z"]))
        capture("baton-in-right-hand",0,(p["x"],p["y"]+.8,p["z"]))
        depart()
        go(0,-20.7,-25);go(0,-20.7,-20.05)
        p=host()["cargo"];aim(0,(p["x"],p["y"],p["z"]),interact=True)
        require("cargo pickup with hazards active",lambda:host().get("holder")==0)
        require("parcel hides holder baton on all four peers",lambda:baton_visible(0,False))
        # Use the existing exterior ground/north/east opening to avoid the Listener; no collision changes or teleport.
        for x,z in [(-22.95,-19.65),(-30,-19.65),(-30,5),(-30,32),(0,32),(26,32),(26,18),(21.5,18),(21.5,25.95),(12.75,25.95),(12.75,14.2),(17,14.2),(17,11.6)]:go(0,x,z)
        send(0,pitch=62,place=True)
        require("BAY 04 accepts parcel under hazards",lambda:all_state(lambda s:s.get("phase")==3))
        require("parcel placement restores holder baton on all peers",lambda:baton_visible(0,True))
        go(0,14.9,10.9)
        require("receipt printing complete",lambda:all_state(lambda s:s.get("receiptReady")))
        aim(0,(14.9,1.25,12.175),interact=True)
        require("receipt collected under hazards",lambda:all_state(lambda s:s.get("receiptCollected")))
        capture("suppression-delivery-ko",0,(14.9,1.2,12.4))
        for x,z in [(12.75,10.9),(12.75,25.95),(21.5,25.95),(21.5,18),(26,18),(26,-12),(26,-36),(0,-36),(-30,-36),(-30,-20.05),(-20.7,-20.05)]:go(0,x,z)
        aboard(0);ship_click()
        require("hazardous return pays 420 once",lambda:all_state(lambda s:s.get("phase")==4 and s.get("credits")==420))
        before=host()["shiftElapsed"];time.sleep(.4)
        require("report clock stopped",lambda:host()["shiftElapsed"]==before)
        send(0,click="buy")
        require("purchased shared beacon for 120",lambda:all_state(lambda s:s.get("credits")==300 and s.get("unlocked") and s.get("charges")==2))
        send(0,click="shipAction")
        require("next preparation",lambda:all_state(lambda s:s.get("phase")==0))
        send(0,ui="close");depart()
        go(0,-20.7,-27.8)
        p=host()["beaconPosition"];aim(0,(p["x"],p["y"],p["z"]),interact=True)
        require("host carries purchased beacon",lambda:all_state(lambda s:s.get("beaconCarrier")==0))
        require("beacon hides host baton on all peers",lambda:baton_visible(0,False))
        for x,z in [(-20.7,-25),(-20.7,-20.05),(-23,-20.05),(-23,-17.5)]:go(0,x,z)
        send(0,yaw=0,drop=True)
        require("first signal spends one charge",lambda:all_state(lambda s:s.get("charges")==1 and s.get("beaconTime",0)>0))
        require("beacon release restores host baton on all peers",lambda:baton_visible(0,True))
        beacon=host()["beaconPosition"]
        require("purchased beacon attracts Listener",lambda:host()["danger"]["noises"]>0 and math.dist(list(host()["danger"]["noiseTarget"].values()),list(beacon.values()))<.1)
        go(0,-20.7,-20.05);aboard(0);ship_click(True)
        require("early return preserves 300 wallet",lambda:all_state(lambda s:s.get("phase")==4 and s.get("credits")==300))
        send(0,click="shipAction")
        require("prepare final shift",lambda:all_state(lambda s:s.get("phase")==0))
        send(0,ui="close");depart()
        go(1,-20.7,-27.8)
        p=host()["beaconPosition"];aim(1,(p["x"],p["y"],p["z"]),interact=True)
        require("client carries shared beacon",lambda:all_state(lambda s:s.get("beaconCarrier")==1))
        require("beacon hides client baton on all peers",lambda:baton_visible(1,False))
        for x,z in [(-20.7,-26.9),(-20.7,-20.05),(-23,-20.05),(-30,-20.05),(-30,-36),(0,-36),(29,-36),(29,-12),(29,13)]:go(1,x,z)
        start=host()["outerDanger"]["position"]
        require("outer staged beyond east boundary",lambda:start["x"]==29 and start["z"]==18 and host()["outerDanger"]["hits"]==0)
        require("unstable stage replicated",lambda:all_state(lambda s:s.get("suppressionStage")==1),120)
        require("failing stage replicated",lambda:all_state(lambda s:s.get("suppressionStage")==2),65)
        require("outer stays staged before shutdown",lambda:host()["outerDanger"]["position"]==start and host()["outerDanger"]["pursuedPlayer"]==-1)
        capture("suppression-failing-ko",1,(24.3,4.08,28.95))
        send(1,toggleLanguage=True)
        require("English selected",lambda:lab.state(folders[1],"ui.json").get("language")=="en")
        require("shutdown stage replicated",lambda:all_state(lambda s:s.get("suppressionStage")==3),65)
        capture("suppression-off-en",1,(24.3,4.08,28.95))
        require("entry grace reached",lambda:host().get("shiftElapsed",0)>=184,10)
        require("outer still waits during grace",lambda:host()["outerDanger"]["position"]==start)
        send(1,yaw=0,drop=True)
        require("client deploys active shared signal",lambda:all_state(lambda s:s.get("charges")==1 and s.get("beaconTime",0)>0))
        require("beacon release restores client baton on all peers",lambda:baton_visible(1,True))
        go(1,29,6)
        require("outer enters only after grace",lambda:host().get("shiftElapsed",0)>=188 and host()["outerDanger"]["position"]!=start,12)
        beacon=host()["beaconPosition"]
        require("signal overrides outer pursuit",lambda:all_state(lambda s:(s.get("outerDanger") or {}).get("noises",0)>0 and (s.get("outerDanger") or {}).get("pursuedPlayer")==-1 and math.dist(list(s["outerDanger"]["noiseTarget"].values()),list(beacon.values()))<.1))
        require("signal expiration resumes client pursuit",lambda:host()["beaconTime"]==0 and host()["outerDanger"]["pursuedPlayer"]==1)
        require("outer enters baton range",lambda:math.dist(list(host()["outerDanger"]["position"].values()),list(host()["positions"][1].values()))<=2.4)
        p=host()["outerDanger"]["position"];aim(1,(p["x"],p["y"]+1.2,p["z"]),shove=True)
        time.sleep(.3)
        require("baton cannot stun outer",lambda:host()["outerDanger"]["state"]!=4)
        require("outer downs client on all peers",lambda:all_state(lambda s:s.get("danger",{}).get("down",[False]*4)[1]) and host()["outerDanger"]["hits"]>0,30)
        require("down hides client baton on all peers",lambda:baton_visible(1,False))
        require("three aboard employees stay safe",lambda:not any(host()["danger"]["down"][i] for i in (0,2,3)))
        send(0,ui="close")
        # Expose the remaining crew at the landing; global pursuit must reach them through actual geometry.
        for i,x,z in [(0,-23.5,-20.05),(2,-18,-20.05),(3,-16,-22)]:
            go(i,-20.7,-25.5-(i//2)*1.4);go(i,-20.7,-20.05)
            if i==3:go(i,-20.7,-22)
            go(i,x,z)
        require("outer triggers all-down ship recovery",lambda:all_state(lambda s:s.get("phase")==4 and not any(s.get("danger",{}).get("down",[True]))),100)
        require("recovery retains wallet without new pay",lambda:all_state(lambda s:s.get("credits")==300 and s.get("returnPay")==0))
        before=host()["shiftElapsed"];time.sleep(.4)
        require("recovery report freezes clock",lambda:host()["shiftElapsed"]==before)
        ship_click();require("prepare after recovery",lambda:all_state(lambda s:s.get("phase")==0))
        send(0,ui="close");depart()
        require("new arrival resets suppression and outer",lambda:all_state(lambda s:s.get("suppressionStage")==0 and s.get("shiftElapsed",99)<3 and s.get("charges")==2 and s["outerDanger"]["position"]["x"]==29 and s["outerDanger"]["position"]["z"]==18))
        for i in range(4):
            assert not (folders[i]/"progression-v1.json").exists()
            log=(folders[i]/"player.log").read_text(errors="replace")
            assert "Exception" not in log and "NullReference" not in log, f"Runtime error in slot {i}"
        checks.append("four logs clean/no Cinder progression save")
        report.update(status="PASS",final_state=host(),scope="Four actual Mac players, ordinary controls and real suppression time; excludes human quality and other-PC/Windows/performance testing.")
    except Exception as error:
        report.update(error=str(error),last_state=host())
        raise
    finally:
        (run/"suppression-check.json").write_text(json.dumps(report,indent=2))
        (lab.OUT/"suppression-latest.json").write_text(json.dumps(report,indent=2))
        lab.stop()
        for process in processes.values():
            try: process.wait(timeout=5)
            except Exception: process.kill()
    print("PASS",len(checks),"Cinder suppression checks",run,flush=True)


if __name__=="__main__":
    main()
