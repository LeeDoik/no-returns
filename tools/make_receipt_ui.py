from pathlib import Path
from PIL import Image,ImageDraw,ImageFont
out=Path('NoReturns/Assets/_NoReturns/Resources/ReceiptUI');out.mkdir(parents=True,exist_ok=True)
labels={'standby':('STANDBY','대기 중'),'place':('PLACE\nPARCEL','화물을\n놓으세요'),'scan':('SCANNING','화물 확인 중'),'print':('PRINTING\nRECEIPT','영수증\n출력 중'),'take':('TAKE RECEIPT\n[E]','영수증 회수\n[E]'),'collected':('RECEIPT\nCOLLECTED','영수증\n회수 완료')}
for state,pair in labels.items():
 for index,lang in enumerate(('en','ko')):
  im=Image.new('RGB',(256,256),(8,17,12));d=ImageDraw.Draw(im);color=(78,216,133) if state in ('take','collected') else (196,179,75)
  for y in range(0,256,3):d.line((0,y,256,y),fill=(10,24,16))
  font=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',24);small=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',12)
  d.text((16,15),'NO RETURNS / CND-041',font=small,fill=(67,125,83));d.line((16,38,240,38),fill=(34,79,50))
  lines=pair[index].split('\n');y=112-(len(lines)-1)*16
  for line in lines:
   width=d.textlength(line,font=font);d.text(((256-width)/2,y),line,font=font,fill=color);y+=34
  d.text((16,224),'RECEPTION // 01',font=small,fill=(67,125,83));im.save(out/f'{state}-{lang}.png')
