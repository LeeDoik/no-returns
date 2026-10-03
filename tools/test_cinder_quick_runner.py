"""Fast guard tests; never launches a game or edits project assets."""
import json
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch
import cinder_build_stamp as stamp
import cinder_four_player as lab
import cinder_quick_test as quick


class QuickGuards(unittest.TestCase):
    def test_changed_source_rejected(self):
        with tempfile.TemporaryDirectory() as folder:
            binary=Path(folder)/'player';binary.write_bytes(b'test')
            record=Path(folder)/'stamp.json'
            record.write_text(json.dumps(dict(source='old',binary=str(binary),binary_mtime=binary.stat().st_mtime_ns)))
            with patch.object(stamp,'STAMP',record),patch.object(stamp,'fingerprint',return_value='changed'):
                with self.assertRaises(RuntimeError):stamp.require_current(binary)

    def test_missing_stamp_rejected(self):
        with tempfile.TemporaryDirectory() as folder,patch.object(stamp,'STAMP',Path(folder)/'missing'):
            with self.assertRaises(RuntimeError):stamp.require_current(Path(folder)/'missing-player')

    def test_stale_startup_reports_failure_without_launch(self):
        with tempfile.TemporaryDirectory() as folder,patch.object(lab,'OUT',Path(folder)),patch.object(quick,'require_current',side_effect=RuntimeError('stale')),patch.object(lab,'launch') as launch:
            with self.assertRaises(RuntimeError):quick.main('beacon')
            launch.assert_not_called()
            report=json.loads((Path(folder)/'quick-latest.json').read_text())
            self.assertEqual((report['status'],report['stage']),('FAIL','startup'))

    def test_quick_mode_cannot_open_manual_windows(self):
        with self.assertRaises(ValueError):lab.launch(quick=True)


if __name__=='__main__':unittest.main()
