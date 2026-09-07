import tempfile
import unittest
from pathlib import Path
from tools.prepare_steampipe import create


class SteamPipeTests(unittest.TestCase):
    def test_configuration_is_explicit_and_preview_only(self):
        with tempfile.TemporaryDirectory() as temp:
            root = Path(temp)
            content = root / 'content'
            content.mkdir()
            for name in ['NO_RETURNS.exe', 'NO_RETURNS.pck']:
                (content / name).write_bytes(b'test fixture')
            for app, depot in [(0, 123), (480, 123), (123, -1)]:
                with self.assertRaises(ValueError): create(app, depot, content, root / 'out')
            config = create(123456789, 123456790, content, root / 'out')
            text = config.read_text()
            self.assertIn('"Preview" "1"', text)
            self.assertNotIn('SetLive', text)
            self.assertIn('"123456790" "depot_build.vdf"', text)
            (content / 'steam_appid.txt').write_text('480')
            with self.assertRaises(ValueError): create(123456789, 123456790, content, root / 'out')
            (content / 'steam_appid.txt').unlink()
            (content / 'secret.txt').write_text('test fixture')
            with self.assertRaises(ValueError): create(123456789, 123456790, content, root / 'out')


if __name__ == '__main__':
    unittest.main()
