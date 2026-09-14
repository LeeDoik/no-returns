"""Read-only end-to-end probe of the official Unity MCP stdio bridge."""
import argparse,json,os,queue,subprocess,threading,time
from pathlib import Path
p=argparse.ArgumentParser();p.add_argument('--project',type=Path,default=Path(__file__).resolve().parents[1]/'NoReturns');args=p.parse_args()
exe=Path(os.environ['LOCALAPPDATA'])/'Unity/bin/unity.exe'
proc=subprocess.Popen([str(exe),'mcp','--project-path',str(args.project.resolve())],stdin=subprocess.PIPE,stdout=subprocess.PIPE,stderr=subprocess.DEVNULL,text=True,encoding='utf-8')
messages=queue.Queue()
def read():
 for line in proc.stdout:
  try:messages.put(json.loads(line))
  except json.JSONDecodeError:pass
threading.Thread(target=read,daemon=True).start()
def send(method,params,identifier=None):
 msg={'jsonrpc':'2.0','method':method,'params':params}
 if identifier is not None:msg['id']=identifier
 proc.stdin.write(json.dumps(msg)+'\n');proc.stdin.flush()
 if identifier is None:return
 end=time.monotonic()+30
 while time.monotonic()<end:
  data=messages.get(timeout=max(.01,end-time.monotonic()))
  if data.get('id')==identifier:
   if 'error' in data:raise RuntimeError(data['error'])
   return data['result']
 raise TimeoutError(method)
try:
 init=send('initialize',{'protocolVersion':'2024-11-05','capabilities':{},'clientInfo':{'name':'no-returns-connection-check','version':'1.0'}},1)
 send('notifications/initialized',{})
 catalog=send('tools/list',{},2)
 status=[t for t in catalog['tools'] if t['name']=='editor_status' or t['name'].endswith('_editor_status')]
 if len(status)!=1:
  print(json.dumps({'server':init.get('serverInfo'),'tools':[t['name'] for t in catalog['tools']]}));raise RuntimeError('No unique editor status tool')
 result=send('tools/call',{'name':status[0]['name'],'arguments':{}},3)
 print(json.dumps({'server':init.get('serverInfo'),'tool':status[0]['name'],'result':result},ensure_ascii=False))
 if result.get('isError'):raise RuntimeError('MCP status call failed')
finally:
 proc.terminate()
 try:proc.wait(timeout=5)
 except subprocess.TimeoutExpired:proc.kill();proc.wait()
