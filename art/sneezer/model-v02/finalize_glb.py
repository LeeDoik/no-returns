import json,struct
from pathlib import Path
root=Path(__file__).resolve().parent
p=root/'sneezer.glb'
data=p.read_bytes()
size,typ=struct.unpack_from('<II',data,12)
doc=json.loads(data[20:20+size])
merged={'name':'SneezeCycle','samplers':[],'channels':[]}
for anim in doc.get('animations',[]):
    offset=len(merged['samplers'])
    merged['samplers'].extend(anim['samplers'])
    for channel in anim['channels']:
        channel['sampler']+=offset
        node=doc['nodes'][channel['target']['node']]
        if node.get('name','').startswith(('Face_','Idle_','Warning_','Sneeze_')):
            merged['samplers'][channel['sampler']]['interpolation']='STEP'
        merged['channels'].append(channel)
doc['animations']=[merged]
blob=json.dumps(doc,separators=(',',':')).encode()
blob+=b' '*((-len(blob))%4)
rest=data[20+size:]
p.write_bytes(struct.pack('<III',0x46546c67,2,20+len(blob)+len(rest))+struct.pack('<II',len(blob),0x4e4f534a)+blob+rest)
assert len(doc['animations'])==1 and merged['channels']
assert all('bufferView' in im for im in doc.get('images',[]))
report={'animation':'SneezeCycle','channels':len(merged['channels']),'embedded_images':len(doc.get('images',[])),'meshes':len(doc.get('meshes',[])),'file_bytes':p.stat().st_size,'rig':'Rigid object transforms; no skeleton required.'}
(root/'glb-validation.json').write_text(json.dumps(report,indent=2),encoding='utf-8')
print(json.dumps(report))
