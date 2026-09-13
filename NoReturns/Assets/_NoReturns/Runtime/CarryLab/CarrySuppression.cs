using UnityEngine;
namespace NoReturns.CarryLab {
// Host advances this clock. Players read stage cues rather than numerical time.
public sealed class CarrySuppression {
    public float Elapsed {get;private set;}
    public int Stage=>Elapsed<90?0:Elapsed<135?1:Elapsed<180?2:3;
    public bool Intrusion=>Elapsed>=188;
    readonly GameObject device; readonly Material lamp;
    readonly Light workLight;
    readonly AudioSource audio; readonly AudioClip tone;
    float nextSound; int shownStage=-1;
    public CarrySuppression(){
        device=CarryWorld.Box("Suppressor indicator",new Vector3(2.8f,2.8f,-1),new Vector3(.5f,.8f,.5f),CarryWorld.Mat(new Color(.13f,.17f,.18f)));device.GetComponent<Collider>().enabled=false;
        lamp=CarryWorld.Mat(Color.cyan);var bulb=CarryWorld.Box("Suppressor lamp",new Vector3(2.8f,3.35f,-1),new Vector3(.4f,.3f,.4f),lamp,device.transform);bulb.GetComponent<Collider>().enabled=false;
        workLight=GameObject.Find("Work light")?.GetComponent<Light>();
        audio=device.AddComponent<AudioSource>();audio.spatialBlend=0;
        tone=AudioClip.Create("Suppression signal placeholder",6615,1,22050,false);var samples=new float[6615];
        for(int i=0;i<samples.Length;i++)samples[i]=Mathf.Sin(i*2*Mathf.PI*330/22050f)*.12f*(1-i/(float)samples.Length);
        tone.SetData(samples,0);
    }
    public void Begin(){Elapsed=0;}
    public void Tick(bool field,float dt){if(field)Elapsed+=Mathf.Max(0,dt);}
    public void Apply(float elapsed){Elapsed=Mathf.Max(0,elapsed);}
    public static string Cue(int stage)=>stage switch{
        0=>"SUPPRESSOR / steady hum",
        1=>"SUPPRESSOR / irregular signal - plan your return",
        2=>"SUPPRESSOR / failing - movement at the east gate",
        _=>"SUPPRESSOR OFF / outer creature entering - return to ship"
    };
    public void Display(bool field){
        device.SetActive(field);
        int stage=field?Stage:0;
        float intensity=stage==0?1.5f:stage==1?1.15f:stage==2?.8f:.45f;
        if(workLight!=null)workLight.intensity=intensity;
        RenderSettings.ambientLight=new Color(.45f,.43f,.4f)*(stage==3?.65f:1);
        lamp.color=stage==0?Color.cyan:stage==1?new Color(1,.6f,.05f):stage==2?new Color(.9f,.15f,.03f):new Color(.08f,.08f,.09f);
        if(stage==1||stage==2)lamp.color*=.55f+.45f*Mathf.Sin(Elapsed*(stage==1?3:7));
        if(!field){shownStage=-1;return;}
        if(shownStage!=stage||Time.unscaledTime>=nextSound){
            if(stage!=3||shownStage!=3)audio.PlayOneShot(tone,stage==0?.25f:1);
            nextSound=Time.unscaledTime+(stage==0?12:stage==1?5:2);shownStage=stage;
        }
    }
}
}
