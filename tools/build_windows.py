"""Export and package only the executable, game pack, guides and notices."""
from pathlib import Path
import hashlib
import json
import subprocess
import zipfile

ROOT = Path(__file__).resolve().parents[1]
ENGINE = ROOT / '.tools/godot/Godot_v4.7.2-stable_win64_console.exe'
OUT = ROOT / 'build/NO_RETURNS_0.7'


def main():
    OUT.mkdir(parents=True, exist_ok=True)
    result = subprocess.run([str(ENGINE), '--headless', '--path', str(ROOT),
                             '--log-file', str(ROOT / 'artifacts/export.log'),
                             '--export-release', 'Windows Playtest',
                             str(OUT / 'NO_RETURNS.exe')], capture_output=True)
    log = result.stdout + result.stderr
    (ROOT / 'artifacts/export-output.log').write_bytes(log)
    if result.returncode or b'SCRIPT ERROR' in log or b'Export failed' in log:
        raise RuntimeError(log.decode('utf-8', errors='replace'))
    smoke = subprocess.run([str(OUT / 'NO_RETURNS.exe'), '--headless',
                            '--log-file', str(ROOT / 'artifacts/packed-smoke.log'),
                            '--quit-after', '60', '--', '--role=smoke'],
                           cwd=OUT, capture_output=True, timeout=20)
    smoke_log = smoke.stdout + smoke.stderr
    (ROOT / 'artifacts/packed-smoke-output.log').write_bytes(smoke_log)
    if smoke.returncode or any(marker in smoke_log for marker in
                               [b'SCRIPT ERROR', b'Failed to load', b'Aborting', b'No main scene']):
        raise RuntimeError(smoke_log.decode('utf-8', errors='replace'))
    files = [OUT / 'NO_RETURNS.exe', OUT / 'NO_RETURNS.pck']
    for source, target in [('docs/prototype/07-contracts.ko.md', 'README.ko.md'),
                           ('docs/prototype/07-contracts.en.md', 'README.en.md'),
                           ('docs/prototype/08-reactive-delivery.ko.md', 'REACTIONS.ko.md'),
                           ('docs/prototype/08-reactive-delivery.en.md', 'REACTIONS.en.md'),
                           ('docs/art/02-release.ko.md', 'ART.ko.md'),
                           ('docs/art/02-release.en.md', 'ART.en.md'),
                           ('docs/art/04-animation-integration.ko.md', 'ANIMATION.ko.md'),
                           ('docs/art/04-animation-integration.en.md', 'ANIMATION.en.md'),
                           ('docs/art/05-carry-physics.ko.md', 'PHYSICS.ko.md'),
                           ('docs/art/05-carry-physics.en.md', 'PHYSICS.en.md'),
                           ('THIRD_PARTY_NOTICES.txt', 'THIRD_PARTY_NOTICES.txt')]:
        text = (ROOT / source).read_text(encoding='utf-8')
        text = text.replace('07-contracts.en.md', 'README.en.md').replace('07-contracts.ko.md', 'README.ko.md')
        text = text.replace('08-reactive-delivery.en.md', 'REACTIONS.en.md').replace('08-reactive-delivery.ko.md', 'REACTIONS.ko.md')
        text = text.replace('02-release.en.md', 'ART.en.md').replace('02-release.ko.md', 'ART.ko.md')
        text = text.replace('04-animation-integration.en.md', 'ANIMATION.en.md').replace('04-animation-integration.ko.md', 'ANIMATION.ko.md')
        text = text.replace('../art/05-carry-physics.en.md', 'PHYSICS.en.md').replace('../art/05-carry-physics.ko.md', 'PHYSICS.ko.md')
        text = text.replace('05-carry-physics.en.md', 'PHYSICS.en.md').replace('05-carry-physics.ko.md', 'PHYSICS.ko.md')
        destination = OUT / target
        destination.write_text(text, encoding='utf-8')
        files.append(destination)
    for file in files:
        if not file.is_file() or not file.stat().st_size:
            raise RuntimeError(f'Missing/empty package input: {file}')
    manifest = {p.name: hashlib.sha256(p.read_bytes()).hexdigest() for p in files}
    (OUT / 'SHA256.json').write_text(json.dumps(manifest, indent=2), encoding='ascii')
    files.append(OUT / 'SHA256.json')
    archive = ROOT / 'build/NO_RETURNS_0.7_Windows.zip'
    with zipfile.ZipFile(archive, 'w', zipfile.ZIP_DEFLATED) as zip_file:
        for file in files:
            zip_file.write(file, f'NO_RETURNS_0.7/{file.name}')
    print(f'PACKAGED {archive} ({archive.stat().st_size} bytes)')


if __name__ == '__main__':
    main()
