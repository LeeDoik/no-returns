"""Content stamp for the reusable native test build; no Git commit assumption."""
import hashlib
import json
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
STAMP = ROOT / 'artifacts/cinder-four-player/build-source.json'


def fingerprint():
    digest = hashlib.sha256()
    for directory in ('Assets', 'Packages', 'ProjectSettings'):
        for path in sorted((ROOT / 'NoReturns' / directory).rglob('*')):
            if not path.is_file() or 'Editor' in path.parts:
                continue
            # The builder regenerates this scene from CinderCompactSiteReview.
            if path.name in ('CinderFourPlayerTest.unity', 'CinderFourPlayerTest.unity.meta'):
                continue
            digest.update(str(path.relative_to(ROOT)).encode())
            if path.name == 'UniversalRenderPipelineGlobalSettings.asset':
                # Unity rebuilds this list from the authored settings on every build.
                content=re.sub(rb'(    m_RuntimeSettings:\n)      m_List:(?: \[\])?\n(?:      - rid: \d+\n)*',rb'\1      m_List: []\n',path.read_bytes())
                digest.update(content)
                continue
            with path.open('rb') as stream:
                for block in iter(lambda: stream.read(1024 * 1024), b''):
                    digest.update(block)
    return digest.hexdigest()


def record(binary):
    STAMP.write_text(json.dumps(dict(source=fingerprint(), binary=str(binary),
                                    binary_mtime=binary.stat().st_mtime_ns), indent=2)+'\n')


def require_current(binary):
    try:
        stamp = json.loads(STAMP.read_text())
        valid = stamp['source'] == fingerprint() and stamp['binary'] == str(binary) and stamp['binary_mtime'] == binary.stat().st_mtime_ns
    except (OSError, ValueError, KeyError):
        valid = False
    if not valid:
        raise RuntimeError('Test build is missing or stale. Run with --build once; visual-only work can use Editor checks without a native build.')
