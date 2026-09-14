"""Snapshot before a browser export, then verify its new local download."""
import argparse
import json
from pathlib import Path
import struct
import time
import zipfile


def files(directory):
    result = {}
    for path in directory.iterdir():
        try:
            if path.is_file():
                stat = path.stat()
                result[path.name] = (stat.st_size, stat.st_mtime_ns)
        except FileNotFoundError:
            pass
    return result


def validate(path):
    if zipfile.is_zipfile(path):
        with zipfile.ZipFile(path) as archive:
            bad = archive.testzip()
            names = archive.namelist()
            if bad or not any(n.lower().endswith(('.fbx', '.glb')) for n in names):
                raise ValueError('ZIP CRC failed or no model found')
            return {'format': 'zip', 'entries': names, 'crc': 'pass'}
    with path.open('rb') as stream:
        header = stream.read(12)
    if header[:4] == b'glTF':
        _, version, length = struct.unpack('<4sII', header)
        if version != 2 or length != path.stat().st_size:
            raise ValueError('Invalid GLB version/length')
        return {'format': 'glb', 'header_length': 'pass'}
    raise ValueError('Expected a ZIP model archive or GLB')


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--directory', required=True, type=Path)
    parser.add_argument('--prefix', required=True)
    parser.add_argument('--state', required=True, type=Path)
    parser.add_argument('--prepare', action='store_true')
    parser.add_argument('--timeout', type=float, default=120)
    parser.add_argument('--interval', type=float, default=2)
    args = parser.parse_args()
    directory = args.directory.resolve(strict=True)
    if args.prepare:
        args.state.write_text(json.dumps({'directory': str(directory),
            'prefix': args.prefix, 'files': files(directory)}), encoding='utf-8')
        print('ARMED', directory, args.prefix, flush=True)
        return 0
    baseline = json.loads(args.state.read_text(encoding='utf-8'))
    if baseline['directory'] != str(directory) or baseline['prefix'] != args.prefix:
        raise ValueError('Snapshot directory/prefix mismatch')
    previous, stable, seen = {}, {}, set()
    start = time.monotonic()
    while time.monotonic() - start < args.timeout:
        current = files(directory)
        new = {name: value for name, value in current.items()
               if name not in baseline['files']}
        partial = [name for name in new if name.lower().endswith(('.crdownload', '.part'))]
        for name, value in new.items():
            if name not in seen:
                print('OBSERVED', name, value[0], flush=True)
                seen.add(name)
            if not name.startswith(args.prefix) or name in partial or value[0] <= 0:
                continue
            stable[name] = stable.get(name, 0) + 1 if previous.get(name) == value else 0
            if stable[name] >= 2 and not partial:
                path = directory / name
                try:
                    verification = validate(path)
                except (OSError, ValueError, zipfile.BadZipFile) as error:
                    print('VALIDATION_PENDING', name, str(error), flush=True)
                    continue
                print(json.dumps({'status': 'DONE', 'path': str(path), 'bytes': value[0],
                    'seconds': round(time.monotonic()-start, 2), 'verification': verification}), flush=True)
                return 0
        previous = new
        time.sleep(min(args.interval, max(0, args.timeout-(time.monotonic()-start))))
    print(json.dumps({'status': 'TIMEOUT', 'seconds': args.timeout,
        'observed_new_files': sorted(seen)}), flush=True)
    return 1


if __name__ == '__main__':
    raise SystemExit(main())
