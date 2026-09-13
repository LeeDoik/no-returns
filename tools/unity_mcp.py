"""Invoke the official Unity stdio MCP server (also usable before client reload)."""
import argparse
import json
import os
from pathlib import Path
import queue
import subprocess
import threading

ROOT = Path(__file__).resolve().parents[1]
ALIAS = ROOT.parents[1] / 'NoReturnsUnity'
PROJECT = ALIAS if ALIAS.exists() and ALIAS.resolve() == (ROOT / 'NoReturns').resolve() else ROOT / 'NoReturns'


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('tool', nargs='?')
    parser.add_argument('--arguments', default='{}')
    parser.add_argument('--arguments-file')
    parser.add_argument('--project-path', default=str(PROJECT))
    args = parser.parse_args()
    binary = Path(os.environ['LOCALAPPDATA']) / 'Unity/bin/unity.exe'
    proc = subprocess.Popen([str(binary), 'mcp', '--project-path', args.project_path],
                            stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.DEVNULL)
    messages = queue.Queue()

    def receive():
        for line in proc.stdout:
            try:
                messages.put(json.loads(line))
            except (ValueError, UnicodeError):
                pass
        messages.put({'closed': True})

    threading.Thread(target=receive, daemon=True).start()

    def send(message):
        proc.stdin.write((json.dumps({'jsonrpc': '2.0', **message}) + '\n').encode('utf-8'))
        proc.stdin.flush()

    def request(number, method, params):
        send({'id': number, 'method': method, 'params': params})
        while True:
            response = messages.get(timeout=120)
            if response.get('closed'):
                raise RuntimeError('Unity MCP server closed before responding')
            if response.get('id') == number:
                if 'error' in response:
                    raise RuntimeError(response['error'])
                return response['result']

    try:
        request(1, 'initialize', {'protocolVersion': '2024-11-05', 'capabilities': {},
                                'clientInfo': {'name': 'no-returns-mcp-check', 'version': '1.0'}})
        send({'method': 'notifications/initialized'})
        if args.tool:
            arguments = json.loads(Path(args.arguments_file).read_text(encoding='utf-8') if args.arguments_file else args.arguments)
            result = request(2, 'tools/call', {'name': args.tool, 'arguments': arguments})
        else:
            result = request(2, 'tools/list', {})
        print(json.dumps(result, ensure_ascii=True, indent=2))
        if result.get('isError'):
            raise SystemExit(1)
    finally:
        proc.terminate()
        try:
            proc.wait(timeout=5)
        except subprocess.TimeoutExpired:
            proc.kill()


if __name__ == '__main__':
    main()
