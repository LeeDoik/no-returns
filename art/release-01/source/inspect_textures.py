from PIL import Image, ImageOps, ImageDraw
from pathlib import Path
root=Path(__file__).resolve().parents[3]
canvas=Image.new('RGB',(1536,1024),'#222222');draw=ImageDraw.Draw(canvas)
for i,name in enumerate(['cardboard','paint','floor']):
    path=root/'assets/art/release-01/textures'/f'{name}_basecolor.png'
    im=Image.open(path).convert('RGB'); print(name,im.size)
    tile=im.resize((256,256))
    for x in range(2):
        for y in range(2):canvas.paste(tile,(i*512+x*256,y*256))
    canvas.paste(im.crop((0,0,512,512)),(i*512,512))
    draw.text((i*512+12,12),name,fill='red')
canvas.save(root/'artifacts/texture-inspection.png')
for name in ['depot']:
    im=Image.open(root/'art/release-01/concepts'/f'{name}.png');im.thumbnail((1600,1000));im.save(root/'artifacts'/f'{name}-reference.png')
