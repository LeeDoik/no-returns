"""Extract non-destructive editorial crops from the approved concept boards."""
import os
from pathlib import Path
import runpy
import shutil
import subprocess

ROOT = Path(__file__).resolve().parent
SOURCE = ROOT.parents[1] / 'docs/art/space-concepts'
# Reuse the local FFmpeg discovery; preparation also refreshes the temp soundtrack.
import sys
sys.argv = ['finish.py', '--prepare']
try:
    runpy.run_path(str(ROOT / 'finish.py'))
except SystemExit:
    pass
import glob
ffmpeg = shutil.which('ffmpeg') or glob.glob(os.path.join(os.environ['LOCALAPPDATA'], 'Microsoft/WinGet/Packages/Gyan.FFmpeg*/ffmpeg*/bin/ffmpeg.exe'))[-1]
crops = [
    ('concept-02.png','exterior.png','1536:510:0:90'),
    ('first-delivery-01.png','arrival.png','744:214:14:126'),
    ('first-delivery-01.png','carry.png','744:214:777:126'),
    ('first-delivery-01.png','wait.png','744:214:14:434'),
    ('first-delivery-01.png','receipt.png','744:214:777:434'),
    ('first-delivery-01.png','clue.png','744:214:14:744'),
    ('suppression-stages-01.png','stable.png','736:306:20:156'),
    ('rescue-extraction-01.png','assist.png','732:326:780:140'),
    ('rescue-extraction-01.png','extract.png','730:346:22:584'),
    ('rescue-extraction-01.png','aboard.png','730:350:780:586'),
]
for source, output, crop in crops:
    subprocess.run([ffmpeg,'-hide_banner','-loglevel','error','-y','-i',str(SOURCE/source),'-vf','crop='+crop,str(ROOT/output)], check=True)
subprocess.run([ffmpeg,'-hide_banner','-loglevel','error','-y','-i',str(ROOT/'threat-motion.mp4'),'-vf','crop=iw:ih*0.9:0:0','-an','-c:v','libx264','-crf','18',str(ROOT/'threat-clean.mp4')],check=True)
for name, duration in [('exterior',5),('arrival',5),('carry',4),('stable',5),('wait',6),('assist',3),('extract',3),('receipt',7),('clue',4),('aboard',5)]:
    vf = f"scale=1280:720:force_original_aspect_ratio=increase,crop=1280:720,zoompan=z='1+0.0002*on':x='iw/2-iw/zoom/2':y='ih/2-ih/zoom/2':d={duration*24}:s=1280x720:fps=24"
    subprocess.run([ffmpeg,'-hide_banner','-loglevel','error','-y','-i',str(ROOT/(name+'.png')),'-vf',vf,'-frames:v',str(duration*24),'-an','-c:v','libx264','-preset','fast','-crf','19','-pix_fmt','yuv420p',str(ROOT/(name+'-pan.mp4'))],check=True)
