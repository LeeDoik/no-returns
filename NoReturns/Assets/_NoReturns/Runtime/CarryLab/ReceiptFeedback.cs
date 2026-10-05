using UnityEngine;
namespace NoReturns.CarryLab {
// Presentation only: the host-owned ledger provides phase and stable progress.
public sealed class ReceiptFeedback : MonoBehaviour {
 public GameObject TerminalArt; public Vector3 TerminalOffset;
 Material signal; Transform scan,paper; Material screenMaterial; string screenKey; AudioSource sound; AudioClip scanning,confirmed; int previous=-1; float printed;
 static GameObject Visual(string name,Vector3 p,Vector3 size,Material mat){var g=CarryWorld.Box(name,p,size,mat);Object.Destroy(g.GetComponent<Collider>());return g;}
 void Awake(){
  signal=CarryWorld.Mat(new Color(.9f,.55f,.08f));signal.EnableKeyword("_EMISSION");
  for(int i=0;i<2;i++){
   Visual("Reception floor outline",new Vector3(-6,.014f,8+i*2),new Vector3(3,.008f,.07f),signal).transform.SetParent(transform,false);
   Visual("Reception floor outline",new Vector3(-7.5f+i*3,.014f,9),new Vector3(.07f,.008f,2),signal).transform.SetParent(transform,false);
  }
  scan=Visual("Reception scanner",new Vector3(-6,.02f,9),new Vector3(2.85f,.008f,.045f),signal).transform;scan.SetParent(transform,false);
  paper=Visual("Printed receipt",Slot,new Vector3(.16f,.01f,.01f),CarryWorld.Mat(new Color(.8f,.77f,.63f))).transform;paper.SetParent(transform,false);
  var audioObject=new GameObject("Terminal audio");audioObject.transform.SetParent(transform);sound=audioObject.AddComponent<AudioSource>();sound.spatialBlend=1;sound.minDistance=2;sound.maxDistance=14;sound.rolloffMode=AudioRolloffMode.Linear;sound.volume=.25f;
  sound.transform.localPosition=new Vector3(-8.1f,1,9);
  scanning=Tone("Scanner motor",.75f,false);confirmed=Tone("Receipt confirmation",.5f,true);
 }
 static AudioClip Tone(string name,float duration,bool chime){
  const int rate=22050;var samples=new float[(int)(rate*duration)];
  for(int i=0;i<samples.Length;i++){float t=(float)i/rate;float frequency=chime?(t<.22f?660:880):95;float envelope=Mathf.Min(t*30,1)*Mathf.Clamp01((duration-t)*20);samples[i]=Mathf.Sin(2*Mathf.PI*frequency*t)*envelope*.45f;}
  var clip=AudioClip.Create(name,samples.Length,1,rate,false);clip.SetData(samples,0);return clip;
 }
 public static readonly Vector3 Slot=new Vector3(-7.96f,.84f,8.691f);
 public static int ReachableReceiver(Vector3 origin,Quaternion look){if(!Physics.Raycast(origin,look*Vector3.forward,out var hit,2.4f,~0,QueryTriggerInteraction.Ignore))return -1;string n=hit.collider.name;if(n=="Receipt terminal collision")return 0;return n.StartsWith("Receipt terminal collision ")&&int.TryParse(n.Substring(27),out int id)&&id>=0&&id<3?id:-1;}
 public static bool CanReach(Vector3 origin,Quaternion look)=>ReachableReceiver(origin,look)==0;
 public void Display(int phase,float progress,bool collected,bool ready){
  sound.transform.localPosition=new Vector3(-8.1f,1,9)+TerminalOffset;
  int state=phase==3?2:phase==2?(progress>0?1:0):-1;
  Color color=state==2?new Color(.12f,.85f,.38f):state==1?new Color(.35f,.8f,.9f):state==0?new Color(.9f,.55f,.08f):new Color(.3f,.32f,.3f);
  signal.color=color;signal.SetColor("_EmissionColor",color*.6f);
  if(!screenMaterial){var terminal=TerminalArt?TerminalArt:GameObject.Find("PSX Receipt");var template=Resources.Load<Material>("ReceiptUI/Screen");if(terminal&&template){screenMaterial=new Material(template);foreach(var renderer in terminal.GetComponentsInChildren<Renderer>()){screenMaterial.SetTexture("_BaseMap",renderer.sharedMaterial.GetTexture("_BaseMap"));renderer.sharedMaterial=screenMaterial;}}}
  string key=(state==2?(collected?"collected":ready?"take":"print"):state==1?"scan":state==0?"place":"standby")+(CarryLanguage.Korean?"-ko":"-en");
  if(screenMaterial&&key!=screenKey){screenMaterial.SetTexture("_ScreenMap",Resources.Load<Texture2D>("ReceiptUI/"+key));screenKey=key;}
  if(screenMaterial){screenMaterial.SetFloat("_Progress",state==1?Mathf.Clamp01(progress):-1);screenMaterial.SetVector("_ScreenOffset",transform.position+TerminalOffset);}
  scan.gameObject.SetActive(state==1);scan.localPosition=new Vector3(-6,.024f,Mathf.Lerp(8.08f,9.92f,progress));
  if(state!=previous){sound.Stop();if(state==1){sound.clip=scanning;sound.loop=true;sound.Play();}else sound.loop=false;if(state==2&&previous>=0){sound.PlayOneShot(confirmed);printed=0;}previous=state;}
  paper.gameObject.SetActive(state==2&&!collected);if(state==2){printed=Mathf.Min(1,printed+Time.deltaTime*1.5f);paper.localScale=new Vector3(.16f,.01f,.01f+.28f*printed);paper.localPosition=Slot+TerminalOffset+Vector3.back*(.005f+.14f*printed);}else printed=0;
 }
 void OnDestroy(){if(screenMaterial)Destroy(screenMaterial);if(scanning)Destroy(scanning);if(confirmed)Destroy(confirmed);}
}
}
