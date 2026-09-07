"""Bounded four-process integration runner; imported by run_tests.py."""
from pathlib import Path
import subprocess
import sys
import time

ROOT = Path(__file__).resolve().parents[1]
ENGINE = ROOT / ".tools/godot/Godot_v4.7.2-stable_win64_console.exe"
OUT = ROOT / "artifacts"
ROLES = ("host", "guest1", "guest2", "guest3")


def run():
    OUT.mkdir(exist_ok=True)
    processes, streams = [], []
    failed = False
    deadline = time.monotonic() + 45
    try:
        for role in ROLES:
            stream = (OUT / f"four-network-{role}.log").open("wb")
            streams.append(stream)
            args = [str(ENGINE), "--headless", "--path", str(ROOT),
                    "--log-file", str(OUT / f"four-{role}-engine.log"),
                    "--script", "tests/test_four_network.gd", "--", f"--role={role}"]
            processes.append(subprocess.Popen(
                args, cwd=ROOT, stdout=stream, stderr=subprocess.STDOUT,
                creationflags=subprocess.CREATE_NO_WINDOW if sys.platform == "win32" else 0))
            if role == "host":
                time.sleep(1)
        for process in processes:
            process.wait(timeout=max(0.1, deadline - time.monotonic()))
    except (subprocess.TimeoutExpired, OSError) as error:
        print(f"FAIL four network: {error}")
        failed = True
    finally:
        for process in processes:
            if process.poll() is None:
                process.kill()
                process.wait()
        for stream in streams:
            stream.close()
    for role, process in zip(ROLES, processes):
        output = (OUT / f"four-network-{role}.log").read_text(encoding="utf-8", errors="replace")
        print(output)
        if (process.returncode or f"PASS four network {role}:" not in output
                or "SCRIPT ERROR" in output or "above the MTU" in output):
            failed = True
    return int(failed or len(processes) != 4)


if __name__ == "__main__":
    raise SystemExit(run())
