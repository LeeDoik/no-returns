"""Run behavior regressions plus bounded two- and four-process ENet tests."""
from pathlib import Path
import subprocess
import sys
import time

if __package__:
    from .run_four_test import run as run_four
else:
    from run_four_test import run as run_four

ROOT = Path(__file__).resolve().parents[1]
ENGINE = ROOT / ".tools/godot/Godot_v4.7.2-stable_win64_console.exe"
OUT = ROOT / "artifacts"


def command(script, role=None):
    args = [str(ENGINE), "--headless", "--path", str(ROOT),
            "--log-file", str(OUT / f"{role or 'rules'}-engine.log"),
            "--script", script]
    if role:
        args += ["--", f"--role={role}"]
    return args


def main():
    OUT.mkdir(exist_ok=True)
    if not ENGINE.is_file():
        print(f"Missing runtime: {ENGINE}")
        return 1
    for script, role, success in (
        ("tests/test_cargo_rules.gd", None, b"PASS:"),
        ("tests/test_bevel_mesh.gd", "bevel-mesh", b"BEVEL MESH PASS"),
        ("tests/test_postal_art.gd", "postal-art", b"POSTAL ART PASS"),
        ("tests/test_worker_animation.gd", "worker-animation", b"WORKER ANIMATION PASS"),
        ("tests/test_carry_physics.gd", "carry-physics", b"CARRY PHYSICS PASS"),
        ("tests/test_motion_quality.gd", "motion-quality", b"MOTION QUALITY PASS"),
        ("tests/test_animation_polish.gd", "animation-polish", b"ANIMATION POLISH PASS"),
        ("tests/test_physics_lab.gd", "physics-lab", b"PHYSICS LAB PASS"),
        ("tests/test_authored_presentation.gd", "authored-presentation", b"AUTHORED PRESENTATION PASS"),
        ("tests/test_release_usability.gd", "release-usability", b"RELEASE USABILITY PASS"),
        ("tests/test_release_controls.gd", "release-controls", b"RELEASE CONTROLS PASS"),
        ("tests/test_release_ui.gd", "release-ui", b"RELEASE UI PASS"),
        ("tests/test_physics.gd", "physics", b"PHYSICS PASS"),
        ("tests/test_shift.gd", "shift", b"PASS shift"),
        ("tests/test_sneeze_rules.gd", "sneeze-rules", b"PASS:"),
        ("tests/test_sneeze_scene.gd", "sneeze-scene", b"SNEEZE SCENE PASS"),
        ("tests/test_cargo_wire.gd", "wire", b"WIRE PASS"),
        ("tests/test_cargo_facing.gd", "facing", b"CARGO FACING PASS"),
        ("tests/test_worker_cargo_collision.gd", "collision", b"WORKER CARGO COLLISION PASS"),
        ("tests/test_clinger.gd", "clinger", b"PASS: clinger"),
        ("tests/test_clinger_integration.gd", "clinger-integration", b"CLINGER INTEGRATION PASS"),
        ("tests/test_roster.gd", "roster", b"ROSTER PASS"),
        ("tests/test_round_rules.gd", "round-rules", b"ROUND RULES PASS"),
        ("tests/test_preferences.gd", "preferences", b"PREFERENCES PASS"),
        ("tests/test_hopper.gd", "hopper", b"HOPPER PASS"),
        ("tests/test_expanded.gd", "expanded", b"EXPANDED PASS"),
        ("tests/test_night_depot.gd", "night-depot", b"NIGHT DEPOT PASS"),
        ("tests/test_conveyor.gd", "conveyor", b"CONVEYOR PASS"),
        ("tests/test_sorting_line.gd", "sorting-line", b"SORTING LINE PASS"),
        ("tests/test_reactive_props.gd", "reactive-props", b"REACTIVE PROPS PASS"),
        ("tests/test_spring_visual.gd", "spring-visual", b"SPRING VISUAL PASS"),
        ("tests/test_prop_impact.gd", "prop-impact", b"PROP IMPACT PASS"),
        ("tests/test_paperwork.gd", "paperwork", b"PAPERWORK PASS"),
        ("tests/test_contracts.gd", "contracts", b"CONTRACTS PASS"),
        ("tests/test_campaign.gd", "campaign", b"CAMPAIGN PASS"),
        ("tests/test_packrat.gd", "packrat", b"PACKRAT PASS"),
        ("tests/test_relay_flight.gd", "relay-flight", b"RELAY FLIGHT PASS"),
        ("tests/test_editable_map.gd", "editable-map", b"EDITABLE MAP PASS"),
        ("tests/test_route_challenges.gd", "route-challenges", b"ROUTE CHALLENGES PASS"),
        ("tests/test_winding_routes.gd", "winding-routes", b"WINDING ROUTES PASS"),
        ("tests/test_enclosed_rooms.gd", "enclosed-rooms", b"ENCLOSED ROOMS PASS"),
    ):
        result = subprocess.run(command(script, role), cwd=ROOT,
                                capture_output=True, timeout=30)
        output = result.stdout + result.stderr
        (OUT / f"{role or 'rules'}-test.log").write_bytes(output)
        print(output.decode("utf-8", errors="replace"))
        if result.returncode or success not in output or b"SCRIPT ERROR" in output:
            return 1
    for script, prefix in (("tests/test_network.gd", "network"),
                           ("tests/test_sneeze_network.gd", "sneeze-network"),
                           ("tests/test_clinger_network.gd", "clinger-network"),
                           ("tests/test_expanded_network.gd", "expanded-network"),
                           ("tests/test_campaign_network.gd", "campaign-network"),
                           ("tests/test_protocol_network.gd", "protocol-network"),
                           ("tests/test_map_network.gd", "map-network"),
                           ("tests/test_routes_network.gd", "routes-network"),
                           ("tests/test_reactions_network.gd", "reactions-network"),
                           ("tests/test_worker_animation_network.gd", "worker-animation-network")):
        if run_pair(script, prefix):
            return 1
    return run_four()


def run_pair(script, prefix):
    processes = []
    logs = []
    try:
        for role in ("host", "guest"):
            stream = (OUT / f"{prefix}-{role}.log").open("wb")
            logs.append(stream)
            processes.append(subprocess.Popen(
                command(script, role), cwd=ROOT,
                stdout=stream, stderr=subprocess.STDOUT,
                creationflags=subprocess.CREATE_NO_WINDOW if sys.platform == "win32" else 0))
            if role == "host":
                time.sleep(1)
        deadline = time.monotonic() + 32
        for process in processes:
            process.wait(timeout=max(1, deadline - time.monotonic()))
    except subprocess.TimeoutExpired:
        print("FAIL: network process exceeded its deadline")
        return 1
    finally:
        for process in processes:
            if process.poll() is None:
                process.kill()
                process.wait()
        for stream in logs:
            stream.close()
    failed = False
    for role, process in zip(("host", "guest"), processes):
        output = (OUT / f"{prefix}-{role}.log").read_text(encoding="utf-8", errors="replace")
        print(output)
        if process.returncode or f"PASS network {role}" not in output or "SCRIPT ERROR" in output:
            failed = True
    return int(failed)


if __name__ == "__main__":
    raise SystemExit(main())
