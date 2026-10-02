"""Rebuild the original 24-second industrial pulse score (stdlib only)."""
from array import array
import math
from pathlib import Path
import random
import wave

RATE = 44100
DURATION = 24
random.seed(41)
samples = array('h')
for i in range(RATE * DURATION):
    t = i / RATE
    envelope = min(1, t / .08, (DURATION - t) / 1.3)
    bed = .075 * math.sin(2 * math.pi * 55 * t) + .035 * math.sin(2 * math.pi * 82.4 * t)
    beat = t % .5
    kick = .27 * math.sin(2 * math.pi * (48 * beat + 7 * (1 - math.exp(-30 * beat)))) * math.exp(-18 * beat)
    tick = .04 * random.uniform(-1, 1) * math.exp(-95 * (t % .25))
    alarm = .07 * math.sin(2 * math.pi * 440 * t) * math.exp(-12 * beat) if 14 <= t < 19 else 0
    hit = sum(.26 * math.sin(2 * math.pi * 38 * (t - at)) * math.exp(-4 * (t - at)) for at in [0, 4, 9, 14, 19] if 0 <= t-at < 1.5)
    # Silence before the title gives the final impact room.
    gain = 0 if 18.6 <= t < 19 else 1
    sample = (bed + kick + tick + alarm + hit) * envelope * gain
    assert abs(sample) < 1, 'Score would clip'
    samples.append(round(sample * 32767))
assert len(samples) == RATE * DURATION
with wave.open(str(Path(__file__).parent / 'assets/industrial-score.wav'), 'wb') as out:
    out.setparams((1, 2, RATE, len(samples), 'NONE', 'not compressed'))
    out.writeframes(samples.tobytes())
print(f'Audio OK: {DURATION}s, peak {max(abs(x) for x in samples)/32767:.3f}')
