"""Windowless native host + companion: normal network inputs, no player windows."""
import json
import math
import time
import cinder_four_player as lab


def main():
    run, processes, folders, _ = lab.launch(True, companion=True, headless=True)
    report = dict(status="FAIL", run=str(run), checks=[])
    actions, positions, heights, speeds = set(), [], [], {}
    held = released = hidden = restored = struck = False
    try:
        start = time.monotonic()
        while time.monotonic() - start < 37:
            state = lab.state(folders[0])
            bot = lab.state(folders[1], "animation.json")
            host = lab.state(folders[0], "animation.json")
            assert state.get("occupiedMask") == 3 and not state["hazard"] and state["phase"] == -1
            assert not host.get("companionBot") and host.get("companionPractice") and bot.get("companionBot")
            assert all(p.poll() is None for p in processes.values())
            action = bot.get("companionAction", "")
            actions.add(action)
            p = state["positions"][1]
            positions.append((p["x"], p["z"]))
            heights.append(p["y"])
            if bot.get("employees"):
                speeds.setdefault(action, []).append(bot["employees"][1]["speed"])
            if state["holder"] == 1:
                held = True
                hidden |= all(not lab.state(folders[i], "animation.json")["batons"][1]["visible"] for i in range(2))
            elif held:
                released = True
                restored |= all(lab.state(folders[i], "animation.json")["batons"][1]["visible"] for i in range(2))
            struck |= state["danger"]["cooldown"][1] > 5
            time.sleep(.08)
        assert {"Idle", "Walking / sidestep", "Slow walk", "Jump", "Baton", "Carry parcel", "Put down"} <= actions, actions
        report["checks"].append("two peers only; human controls remain separate; safe practice and seven actions")
        assert max(math.dist(a, positions[0]) for a in positions) > 1.5
        assert max(heights) - min(heights) > .35
        report["checks"].append("actual replicated walking and jump height")
        assert max(speeds["Slow walk"]) < max(speeds["Walking / sidestep"])
        assert struck and held and released and hidden and restored, (struck, held, released, hidden, restored)
        report["checks"].append("slower movement, real baton cooldown, parcel pickup/drop and baton visibility on both peers")
        # Move the human with the existing ordinary input channel; the bot must follow, without teleporting.
        lab.command(folders[0], 2, yaw=270, z=1)
        time.sleep(3)
        lab.command(folders[0], 3)
        lab.wait("companion follows relocated human", lambda: lab.state(folders[1], "animation.json").get("companionAction") == "Following", 8)
        lab.wait("companion catches up", lambda: math.dist(
            list(lab.state(folders[0])["positions"][0].values()), list(lab.state(folders[0])["positions"][1].values())) < 5, 20)
        report["checks"].append("follows relocated human through replicated movement")
        for folder in folders.values():
            log = (folder / "player.log").read_text(errors="replace")
            assert not any(t in log for t in ("Exception:", "Invalid AABB", "Assertion failed", "Fatal Error", "prototype visuals")), log[-1800:]
        report["checks"].append("both headless native logs free of runtime errors and art fallback")
        report.update(status="PASS", actions=sorted(actions), jump_height_range=max(heights)-min(heights))
    except BaseException as error:
        report["error"] = str(error)
        raise
    finally:
        (run / "companion-check.json").write_text(json.dumps(report, indent=2) + "\n")
        (lab.OUT / "companion-latest.json").write_text(json.dumps(report, indent=2) + "\n")
        lab.stop()
        for process in processes.values():
            try:
                process.wait(timeout=5)
            except Exception:
                process.kill()
    print(json.dumps(report, indent=2))


if __name__ == "__main__":
    main()
