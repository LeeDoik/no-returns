"""Arithmetic checks for SPACE-ECO-01 proposal, not runtime game validation."""
from pathlib import Path
from collections import Counter
import json, re
ROOT = Path(__file__).resolve().parents[1]
def pay(b, q, rmax, delivered, condition, crew, returned):
    assert 1 <= crew <= 4 and 0 <= returned <= crew
    return b + int(q * condition) + rmax * returned // crew if delivered else 0
cases = 0
for b, q, rmax, cap in [(240,60,120,420),(360,90,180,630),(480,120,240,840)]:
    assert b+q+rmax == cap
    for crew in range(1,5):
        for condition in (0,0.5,1):
            prior = -1
            for returned in range(crew+1):
                value = pay(b,q,rmax,True,condition,crew,returned)
                assert prior <= value <= cap
                assert pay(b,q,rmax,False,condition,crew,returned) == 0
                prior = value
                cases += 1
for condition,returned,expected in [(1,4,420),(1,2,360),(1,0,300),(0.5,4,390),(0,4,360)]:
    assert pay(240,60,120,True,condition,4,returned)==expected
assert 420 >= 300+120
assert 180+300+60 == 540 and 540-300 == 240
ko=(ROOT/'docs/current/economy.ko.md').read_text(encoding='utf-8')
en=(ROOT/'docs/current/economy.en.md').read_text(encoding='utf-8')
pattern=r'(?<![A-Za-z])\d+(?:\.\d+)?'
assert Counter(re.findall(pattern,ko)) == Counter(re.findall(pattern,en))
assert re.findall(r'- \[([ x])\]',ko) == re.findall(r'- \[([ x])\]',en)
result={'status':'PASS','scope':'Proposal arithmetic and bilingual numeric/checkbox parity only; no Unity, persistence, multiplayer or human balance validation','enumerated_delivery_cases':cases,'undelivered_cases':cases,'table_examples':5,'ui_wallet_flow':[180,360,540,300,240]}
out=ROOT/'artifacts/space-economy/check.json'
out.parent.mkdir(parents=True,exist_ok=True)
out.write_text(json.dumps(result,indent=2),encoding='utf-8')
print(json.dumps(result))
