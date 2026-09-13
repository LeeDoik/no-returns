"""Validate the current bilingual documentation entry points and local links."""
from pathlib import Path
import re
ROOT=Path(__file__).resolve().parents[1]
files=[ROOT/'README.md',ROOT/'README.en.md']
files+=list((ROOT/'docs').rglob('*.md'))
errors=[]
for p in files:
 text=p.read_text(encoding='utf-8')
 for label,target in re.findall(r'\[([^\]]+)\]\(([^)]+)\)',text):
  target=target.strip('<>').split('#')[0]
  if not target or '://' in target or target.startswith('mailto:'):continue
  if not (p.parent/target).exists():errors.append(f'{p.relative_to(ROOT)}: missing {target}')
 if p.name.endswith('.ko.md'):
  other=p.with_name(p.name.replace('.ko.md','.en.md'))
  if not other.exists():other=p.with_name(p.name.replace('.ko.md','.md'))
  if not other.exists():errors.append(f'{p}: missing English counterpart')
  elif re.findall(r'- \[([ x])\]',text)!=re.findall(r'- \[([ x])\]',other.read_text(encoding='utf-8')):errors.append(f'{p}: checkbox mismatch')
for error in errors: print(error)
print(f'DOCS {"FAIL" if errors else "PASS"}: {len(files)} maintained/archived entry documents')
raise SystemExit(bool(errors))
