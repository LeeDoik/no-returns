"""Generate preview-only SteamPipe configuration; never logs in or uploads."""
import argparse
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def create(app_id: int, depot_id: int, content: Path, output: Path):
    if app_id <= 0 or depot_id <= 0 or app_id == 480 or depot_id == 480:
        raise ValueError('Supply your actual application and Windows depot IDs; sample IDs are refused.')
    content = content.resolve()
    if not (content / 'NO_RETURNS.exe').is_file() or not (content / 'NO_RETURNS.pck').is_file():
        raise ValueError('Content must contain the exported NO_RETURNS.exe and NO_RETURNS.pck.')
    if (content / 'steam_appid.txt').exists():
        raise ValueError('Remove the development steam_appid.txt override before packaging.')
    for path in (content, output.resolve()):
        if any(char in str(path) for char in ['"', '\n', '\r']):
            raise ValueError('Unsupported path characters.')
    allowed = {'NO_RETURNS.exe', 'NO_RETURNS.pck', 'README.ko.md', 'README.en.md',
               'THIRD_PARTY_NOTICES.txt', 'SHA256.json'}
    unexpected = {p.name for p in content.iterdir()} - allowed
    if unexpected:
        raise ValueError(f'Unexpected content files: {sorted(unexpected)}')
    output.mkdir(parents=True, exist_ok=True)
    mappings = '\n'.join(f'\t"FileMapping" {{ "LocalPath" "{name}" "DepotPath" "." "recursive" "0" }}'
                         for name in sorted(allowed) if (content / name).is_file())
    depot = f'"DepotBuildConfig"\n{{\n\t"DepotID" "{depot_id}"\n{mappings}\n}}\n'
    app = (f'"AppBuild"\n{{\n\t"AppID" "{app_id}"\n'
           f'\t"Desc" "NO RETURNS 0.5 private playtest"\n'
           f'\t"ContentRoot" "{content.as_posix()}"\n'
           f'\t"BuildOutput" "{(output.resolve() / "logs").as_posix()}"\n'
           f'\t"Preview" "1"\n\t"Depots" {{ "{depot_id}" "depot_build.vdf" }}\n}}\n')
    (output / 'depot_build.vdf').write_text(depot, encoding='utf-8')
    (output / 'app_build.vdf').write_text(app, encoding='utf-8')
    return output / 'app_build.vdf'


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--app-id', type=int, required=True)
    parser.add_argument('--depot-id', type=int, required=True)
    parser.add_argument('--content', type=Path, default=ROOT / 'build/NO_RETURNS_0.5')
    parser.add_argument('--output', type=Path, default=ROOT / 'build/steampipe')
    args = parser.parse_args()
    print(create(args.app_id, args.depot_id, args.content, args.output))


if __name__ == '__main__':
    main()
