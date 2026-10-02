using UnityEngine;
namespace NoReturns.CarryLab {
// Host advances this clock. Players read stage cues rather than numerical time.
public sealed class CarrySuppression {
    public float Elapsed {get;private set;}
    public int Stage=>Elapsed<90?0:Elapsed<135?1:Elapsed<180?2:3;
    public bool Intrusion=>Elapsed>=188;
    readonly bool cinder;
    readonly GameObject device; readonly Material[] lamps;
    readonly Light[] workLights; readonly float[] lightLevels;
    readonly Color ambient;
    readonly AudioSource audio; readonly AudioClip tone;
    float nextSound; int shownStage=-1;
    public CarrySuppression(bool cinder=false){
        this.cinder=cinder;ambient=RenderSettings.ambientLight;
        if(cinder){
            device=new GameObject("Cinder suppression signal audio");
            var signals=new System.Collections.Generic.List<Material>();
            foreach(var r in Object.FindObjectsByType<Renderer>())if(r.name=="Suppressor signal"||r.name=="Suppression field / visual boundary only")signals.Add(r.material);
            if(signals.Count!=5)throw new System.InvalidOperationException("Expected four Cinder suppressor signals and one field boundary");
            lamps=signals.ToArray();var lights=new System.Collections.Generic.List<Light>();
            foreach(var l in Object.FindObjectsByType<Light>())if(l.type!=LightType.Directional)lights.Add(l);
            workLights=lights.ToArray();
        }else{
            device=CarryWorld.Box("Suppressor indicator",new Vector3(2.8f,2.8f,-1),new Vector3(.5f,.8f,.5f),CarryWorld.Mat(new Color(.13f,.17f,.18f)));device.GetComponent<Collider>().enabled=false;
            lamps=new[]{CarryWorld.Mat(Color.cyan)};var bulb=CarryWorld.Box("Suppressor lamp",new Vector3(2.8f,3.35f,-1),new Vector3(.4f,.3f,.4f),lamps[0],device.transform);bulb.GetComponent<Collider>().enabled=false;
            workLights=new[]{GameObject.Find("Work light")?.GetComponent<Light>()};
        }
        lightLevels=System.Array.ConvertAll(workLights,l=>l?l.intensity:0);
        audio=device.AddComponent<AudioSource>();audio.spatialBlend=0;
        tone=AudioClip.Create("Suppression signal placeholder",6615,1,22050,false);var samples=new float[6615];
        for(int i=0;i<samples.Length;i++)samples[i]=Mathf.Sin(i*2*Mathf.PI*330/22050f)*.12f*(1-i/(float)samples.Length);
        tone.SetData(samples,0);
    }
    public void Begin(){Elapsed=0;}
    public void Tick(bool field,float dt){if(field)Elapsed+=Mathf.Max(0,dt);}
    public void Apply(float elapsed){Elapsed=Mathf.Max(0,elapsed);}
    public static string Cue(int stage,bool cinder=false)=>stage switch{
        0=>"SUPPRESSOR / steady hum",
        1=>"SUPPRESSOR / irregular signal - plan your return",
        2=>cinder?"SUPPRESSOR / failing - movement beyond the east boundary":"SUPPRESSOR / failing - movement at the east gate",
        _=>"SUPPRESSOR OFF / outer creature entering - return to ship"
    };
    public void Display(bool field){
        device.SetActive(field);
        int stage=field?Stage:0;
        float intensity=stage==0?1.5f:stage==1?1.15f:stage==2?.8f:.45f;
        for(int i=0;i<workLights.Length;i++)if(workLights[i])workLights[i].intensity=cinder?lightLevels[i]*intensity/1.5f:intensity;
        RenderSettings.ambientLight=(cinder?ambient:new Color(.45f,.43f,.4f))*(stage==3?.65f:1);
        Color color=stage==0?(cinder?new Color(.24f,.68f,.5f):Color.cyan):stage==1?new Color(1,.6f,.05f):stage==2?new Color(.9f,.15f,.03f):new Color(.08f,.08f,.09f);
        if(stage==1||stage==2)color*=.55f+.45f*Mathf.Sin(Elapsed*(stage==1?3:7));
        foreach(var lamp in lamps)lamp.color=color;
        if(!field){shownStage=-1;return;}
        if(shownStage!=stage||Time.unscaledTime>=nextSound){
            if(stage!=3||shownStage!=3)audio.PlayOneShot(tone,stage==0?.25f:1);
            nextSound=Time.unscaledTime+(stage==0?12:stage==1?5:2);shownStage=stage;
        }
    }
    public void Dispose(){foreach(var lamp in lamps)Object.Destroy(lamp);Object.Destroy(tone);Object.Destroy(device);}
}
}
