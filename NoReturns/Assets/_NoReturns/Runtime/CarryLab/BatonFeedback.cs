using UnityEngine;
namespace NoReturns.CarryLab {
// The material draws the gauge from the host-owned cooldown; no generated frame textures.
public sealed class BatonFeedback {
    readonly Material screen;
    readonly LineRenderer arc;
    public BatonFeedback(Transform parent) {
        Renderer panel=null;
        foreach(var r in parent.GetComponentsInChildren<Renderer>())if(r.name.Contains("BatonScreen")){panel=r;break;}
        if(!panel)throw new System.InvalidOperationException("Missing integrated BatonScreen mesh");
        screen=new Material(Shader.Find("NoReturns/BatonDisplay"));screen.SetFloat("_Gauge",1);screen.SetFloat("_Charge",1);panel.sharedMaterial=screen;
        var g=new GameObject("Baton electrode arc");g.transform.SetParent(parent,false);arc=g.AddComponent<LineRenderer>();
        arc.useWorldSpace=false;arc.positionCount=7;arc.widthMultiplier=.003f;arc.numCapVertices=0;arc.numCornerVertices=0;
        var glow=new Material(Shader.Find("NoReturns/BatonDisplay"));glow.color=new Color(.65f,.88f,1);arc.sharedMaterial=glow;
    }
    public void Display(float cooldown) {
        screen.SetFloat("_Charge",Mathf.Clamp01(1-cooldown/6));
        bool discharge=cooldown>5.82f;
        // Keep the held weapon alive even while recharging; modulate the local arc only.
        arc.enabled=true;
        int frame=Mathf.FloorToInt(Time.time*18);
        for(int k=0;k<7;k++) {
            float jitter=k==0||k==6?0:Mathf.Sin(frame*7+k*13)*.013f;
            arc.SetPosition(k,new Vector3(Mathf.Lerp(-.029f,.029f,k/6f),.475f+jitter,.003f));
        }
        float flicker=.5f+.5f*Mathf.Sin(frame*12.9898f);
        arc.widthMultiplier=discharge?.006f:Mathf.Lerp(.001f,.0045f,flicker);
    }
}
}
