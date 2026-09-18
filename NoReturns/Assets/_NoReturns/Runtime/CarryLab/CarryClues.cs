using UnityEngine;
namespace NoReturns.CarryLab {
// Host owns Mask. Props and journal text are local presentation of that result.
public sealed class CarryClues {
    public int Mask {get;private set;}
    readonly GameObject[] terminals=new GameObject[2];
    readonly Material[] screens=new Material[2];
    public const string FirstTitle="01 / THE CLOSED SHIFT";
    public const string FirstBody="MAINTENANCE: This depot closed nineteen shifts ago.\nThe terminal is warm. Fresh ink stains the feed roller.\nSomeone has been using it.";
    public const string SecondTitle="02 / ALREADY RECEIVED";
    public const string SecondBody="PARCEL CND-041: RECEIVED.\nThe receipt is dated before your ship landed.\nReceiver signature: EMPLOYEE 00.\nNo one answers behind the shutter.";
    public CarryClues(){
        Vector3[] positions={new Vector3(7.7f,1.35f,0),new Vector3(-7.8f,1.35f,10.6f)};
        if(CinderDemoLayout.Active)positions[0]=new Vector3(-1.2f,1.35f,21);
        for(int i=0;i<1;i++){
            terminals[i]=CarryWorld.Box(i==0?"Maintenance terminal":"Reception recorder",positions[i],new Vector3(.35f,.75f,.65f),CarryWorld.Mat(new Color(.2f,.23f,.24f)));
            screens[i]=CarryWorld.Mat(new Color(.6f,.32f,.05f));
            float side=i==0?-1:1;
            Part("Screen",terminals[i].transform,new Vector3(side*.51f,.1f,0),new Vector3(.04f,.65f,.75f),screens[i]);
            Part("Pedestal",terminals[i].transform,new Vector3(0,-1.1f,0),new Vector3(.65f,1.6f,.65f),CarryWorld.Mat(new Color(.16f,.18f,.19f)));
        }
        Display(false,false);
    }
    static GameObject Part(string name,Transform parent,Vector3 p,Vector3 size,Material mat){
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.GetComponent<Collider>().enabled=false;g.transform.SetParent(parent,false);g.transform.localPosition=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;return g;
    }
    public void Clear(){Mask=0;}
    public void Apply(int mask){Mask=mask&3;}
    public int Target(Vector3 origin,Quaternion look){
        if(!Physics.Raycast(origin,look*Vector3.forward,out var hit,2.4f,~0,QueryTriggerInteraction.Ignore))return -1;
        if(hit.collider.name=="Receipt terminal collision")return 1;
        if(hit.collider.gameObject==terminals[0])return 0;
        return -1;
    }
    public string Inspect(Vector3 origin,Quaternion look,int phase){
        if(phase!=2&&phase!=3)return "Inspect clues during the delivery shift";
        int id=Target(origin,look);
        if(id<0)return "Aim at a nearby terminal and press E";
        if(id==1&&phase!=3)return "Recorder idle / deliver the parcel first";
        int bit=1<<id;if((Mask&bit)!=0)return "Already in the shared field log";
        Mask|=bit;return "Clue shared / Tab to read safely aboard";
    }
    public void Display(bool field,bool received){
        terminals[0].SetActive(field);
        screens[0].color=new Color(.55f+.1f*Mathf.Sin(Time.time*2),.35f,.08f);
    }
}
}
