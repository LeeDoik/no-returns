"""Rebuild subtitles, original temp score and final trailer from picture.mp4.
Requires Python 3, FFmpeg and the checked-in temporary voice WAVs.
"""
import array
import glob
import json
import math
import os
from pathlib import Path
import random
import shutil
import subprocess
import sys
import wave

ROOT = Path(__file__).resolve().parent
os.chdir(ROOT)
FFMPEG = shutil.which('ffmpeg')
if not FFMPEG:
    matches = glob.glob(os.path.join(os.environ.get('LOCALAPPDATA', ''), 'Microsoft/WinGet/Packages/Gyan.FFmpeg*/ffmpeg*/bin/ffmpeg.exe'))
    FFMPEG = matches[-1] if matches else None
if not FFMPEG:
    raise RuntimeError('FFmpeg is required')
FFPROBE = str(Path(FFMPEG).with_name('ffprobe.exe' if os.name == 'nt' else 'ffprobe'))
LINES = json.loads((ROOT / 'narration.json').read_text(encoding='utf-8-sig'))

def stamp(seconds, ass=False):
    h, remainder = divmod(seconds, 3600)
    m, s = divmod(remainder, 60)
    return f'{h}:{m:02}:{s:05.2f}' if ass else f'{h:02}:{m:02}:{s:02},000'

for lang in ('ko', 'en'):
    (ROOT / f'captions.{lang}.srt').write_text('\n\n'.join(
        f'{i+1}\n{stamp(x["at"])} --> {stamp(x["end"])}\n{x[lang]}'
        for i, x in enumerate(LINES)) + '\n', encoding='utf-8')

ass = '''[Script Info]
ScriptType: v4.00+
PlayResX: 1280
PlayResY: 720
WrapStyle: 0
[V4+ Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
Style: Default,Malgun Gothic,25,&H00E8EAEF,&H000000FF,&H00101010,&H00101010,0,0,0,0,100,100,0,0,1,1,0,2,70,70,24,1
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
'''
for line in LINES:
    text = line['ko']
    if len(text) > 38:
        split = text.rfind(' ', 12, len(text)//2+6)
        text = text[:split] + r'\N' + text[split+1:]
    ass += f'Dialogue: 0,{stamp(line["at"], True)},{stamp(line["end"], True)},Default,,0,0,0,,{text}\n'
(ROOT / 'captions.ko.ass').write_text(ass, encoding='utf-8-sig')

# A deterministic original temporary sound bed; no sampled commercial music.
sr = 24000
rng = random.Random(4104)
samples = array.array('h')
notes = [261.63, 329.63, 392.0, 523.25, 392.0, 329.63]
for i in range(sr * 75):
    t = i / sr
    v = 0.0
    if t < 28:
        phase = t % 1.4
        n = notes[int(t/1.4) % len(notes)]
        v += 0.047 * math.exp(-phase*3) * (math.sin(2*math.pi*n*t)+.25*math.sin(4*math.pi*n*t))
    if 13 < t < 67:
        gain = min((t-13)/12, 1) * 0.036
        pulse = 0.65 + 0.35*math.sin(2*math.pi*0.65*t)
        v += gain * pulse * (math.sin(2*math.pi*55*t)+.5*math.sin(2*math.pi*56.4*t))
        v += 0.003 * (rng.random()*2-1)
    # Familiar delivery chime; its echo is heard after the image has gone black.
    for start in (56.4, 68.1):
        d = t-start
        if 0 <= d < .9:
            n = 784 if d < .22 else 1046.5
            v += .12*math.exp(-d*5)*math.sin(2*math.pi*n*d)
    if 54 < t < 55:
        v += .019 * (rng.random()*2-1) * (0.5+0.5*math.sin(2*math.pi*22*t))
    if 69 < t < 72:
        v += .095*math.exp(-(t-69)*1.8)*math.sin(2*math.pi*42*t)
    samples.append(int(max(-1, min(1, v))*32767))
with wave.open(str(ROOT / 'temp-score.wav'), 'wb') as f:
    f.setnchannels(1); f.setsampwidth(2); f.setframerate(sr); f.writeframes(samples.tobytes())

if '--prepare' in sys.argv:
    print('Prepared subtitles and original temporary score')
    raise SystemExit(0)

args = [FFMPEG, '-hide_banner', '-y', '-i', 'picture.mp4', '-i', 'temp-score.wav']
filters = ['[1:a]aformat=sample_rates=48000:channel_layouts=stereo[bed]']
mixes = ['[bed]']
for i, line in enumerate(LINES):
    if line.get('silent'):
        continue
    args += ['-i', f'voice-{i}.wav']
    delay = line['at']*1000
    filters += [f'[{i+2}:a]loudnorm=I=-18:TP=-3:LRA=7,aformat=sample_rates=48000:channel_layouts=stereo,adelay={delay}|{delay}[v{i}]']
    mixes.append(f'[v{i}]')
for input_index, clip, at in ((9, 'depot-motion.mp4', 18), (10, 'threat-motion.mp4', 33)):
    args += ['-i', clip]
    delay = at * 1000
    filters += [f'[{input_index}:a]volume=0.35,afade=t=in:d=0.25,afade=t=out:st=5.6:d=0.4,aformat=sample_rates=48000:channel_layouts=stereo,adelay={delay}|{delay}[fx{input_index}]']
    mixes.append(f'[fx{input_index}]')
filters += [''.join(mixes)+f'amix=inputs={len(mixes)}:duration=longest:normalize=0,alimiter=limit=0.89,atrim=duration=75[a]']
args += ['-filter_complex', ';'.join(filters), '-map','0:v:0','-map','[a]', '-vf', 'ass=captions.ko.ass',
         '-c:v','libx264','-preset','medium','-crf','19','-pix_fmt','yuv420p',
         '-c:a','aac','-b:a','192k','-t','75','-movflags','+faststart','NO_RETURNS_Trailer_Draft_01_KO.mp4']
subprocess.run(args, check=True)
probe = subprocess.check_output([FFPROBE,'-v','error','-show_streams','-show_format','-of','json','NO_RETURNS_Trailer_Draft_01_KO.mp4'])
(ROOT / 'validation.json').write_bytes(probe)
print('Final trailer exported; validate rendered frames and audio before publication.')
