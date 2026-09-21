using System;
using System.Collections.Generic;
using UnityEngine;
namespace NoReturns.CarryLab {
[Serializable] public sealed class ThreatState {
    public Vector3 position,noiseTarget;
    public int state,hits,rescues,evacuations,noises;
    public int pursuedPlayer=-1;
    public bool[] down; public float[] rescue,cooldown;
    public bool IsDown(int i)=>down!=null&&i<down.Length?down[i]:i==0?down0:i==1&&down1;
    public float RescueAt(int i)=>rescue!=null&&i<rescue.Length?rescue[i]:i==0?rescue0:i==1?rescue1:0;
    public float CooldownAt(int i)=>cooldown!=null&&i<cooldown.Length?cooldown[i]:i==0?cooldown0:i==1?cooldown1:0;
    public bool down0,down1;
    public float rescue0,rescue1,cooldown0,cooldown1,warning;
}
// Host-owned experiment. Clients only display the replicated state.
public sealed class CarryThreat {
    public bool Hard; readonly bool outer,cinder; readonly CarryThreat listenerOwner;
    readonly CarryThreat[] additional=Array.Empty<CarryThreat>();
    readonly bool[] swings=new bool[4];
    float searchClock,distraction; int pursuedPlayer=-1;
    public readonly bool[] Down=new bool[4];
    public readonly float[] Rescue=new float[4],Cooldown=new float[4];
    readonly float[] protection=new float[4],stepClock=new float[4];
    readonly Vector3[] previous=new Vector3[4];
    readonly Vector3[] patrol={new Vector3(1,0,5),new Vector3(1,0,7),new Vector3(-3,0,7),new Vector3(-3,0,5)};
    readonly Dictionary<Vector2Int,int> grid=new Dictionary<Vector2Int,int>();
    readonly List<Vector3> nodes=new List<Vector3>(),path=new List<Vector3>();
    readonly List<List<int>> edges=new List<List<int>>();
    static readonly Vector2Int[] NeighborOffsets={Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};
    readonly GameObject body;
    readonly Material skin;
    readonly AudioSource audio;
    readonly AudioClip warningClip;
    Vector3 position=new Vector3(1,0,5),noiseTarget;
    int state,patrolIndex,victim,hits,rescues,evacuations,noises,displayState=-1;
    float timer,interest,allDown,stunResistance;
    bool gridBuilt;
    public CarryThreat(CarryThreat parent=null):this(parent,-1){}
    // A owns shared crew recovery and the navigation graph; B/C only simulate their own AI.
    CarryThreat(CarryThreat parent,int listenerIndex){
        cinder=GameObject.Find("Cinder Depot editable primitive blockout")!=null;
        outer=parent!=null&&listenerIndex<0;
        if(parent!=null){Down=parent.Down;protection=parent.protection;Rescue=parent.Rescue;Cooldown=parent.Cooldown;swings=parent.swings;}
        if(listenerIndex>=0){listenerOwner=parent;grid=parent.grid;nodes=parent.nodes;edges=parent.edges;}
        if(outer){patrol=new[]{new Vector3(13,0,28),new Vector3(10,0,0),new Vector3(2,0,-2),new Vector3(13,0,16)};position=patrol[0];}
        if(cinder){
            patrol=outer?new[]{CinderPoint(820,320),CinderPoint(820,650),CinderPoint(120,650),CinderPoint(120,80)}
                :listenerIndex==1?new[]{new Vector3(-6,0,2),new Vector3(12,0,2),new Vector3(12,0,-17),new Vector3(-6,0,-17)}
                :listenerIndex==2?new[]{new Vector3(21,0,0),new Vector3(38,0,0),new Vector3(38,0,-20),new Vector3(21,0,-20)}
                :new[]{new Vector3(-26,0,10),new Vector3(-16,0,10),new Vector3(-16,0,-8),new Vector3(-26,0,-8)};
            position=patrol[0];
        }
        body=new GameObject(outer?"OUTER placeholder":cinder?"LISTENER "+(listenerIndex==1?"B":listenerIndex==2?"C":"A")+" placeholder":"LISTENER placeholder");skin=CarryWorld.Mat(new Color(.42f,.3f,.22f));
        Part("Torso",new Vector3(0,1.1f,0),outer?new Vector3(.25f,2.4f,.2f):new Vector3(.5f,1.4f,.35f));
        Part("Listening head",new Vector3(0,outer?2.6f:1.95f,0),outer?new Vector3(.8f,.18f,.2f):new Vector3(.85f,.35f,.45f));
        Part("Left leg",new Vector3(-.22f,.4f,0),new Vector3(.13f,.8f,.15f));
        Part("Right leg",new Vector3(.22f,.4f,0),new Vector3(.13f,.8f,.15f));
        audio=body.AddComponent<AudioSource>();audio.spatialBlend=1;audio.minDistance=2;audio.maxDistance=15;audio.rolloffMode=AudioRolloffMode.Linear;
        warningClip=AudioClip.Create("Listener warning placeholder",11025,1,22050,false);var samples=new float[11025];
        for(int i=0;i<samples.Length;i++)samples[i]=Mathf.Sin(i*2*Mathf.PI*180/22050f)*.15f*(1-i/(float)samples.Length);
        warningClip.SetData(samples,0);
        if(cinder&&parent==null)additional=new[]{new CarryThreat(this,1),new CarryThreat(this,2)};
    }
    void Part(string name,Vector3 p,Vector3 size){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.GetComponent<Collider>().enabled=false;g.transform.SetParent(body.transform,false);g.transform.localPosition=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=skin;}
    static bool Static(Collider c)=>c.enabled && c.GetComponentInParent<CharacterController>()==null && c.attachedRigidbody==null && c.gameObject.name!="Decoy beacon" && c.GetComponentInParent<ReceiptFeedback>()==null;
    static bool Clear(Vector3 p){foreach(var c in Physics.OverlapBox(p+Vector3.up*.95f,new Vector3(.42f,.8f,.42f),Quaternion.identity,~0,QueryTriggerInteraction.Ignore))if(Static(c))return false;return true;}
    public static bool Sight(Vector3 a,Vector3 b){foreach(var h in Physics.RaycastAll(a+Vector3.up*.7f,(b-a).normalized,Vector3.Distance(a,b),~0,QueryTriggerInteraction.Ignore))if(Static(h.collider))return false;return true;}
    const float StepHeight=.32f;
    static Vector3 CinderPoint(float x,float y)=>new Vector3((x-450)*.12f,0,(360-y)*.12f);
    static bool WalkPoint(Vector3 column,out Vector3 foot){
        foot=column;bool supported=false;float highest=float.MinValue;
        foreach(var hit in Physics.BoxCastAll(new Vector3(column.x,StepHeight+.04f,column.z),new Vector3(.42f,.005f,.42f),Vector3.down,Quaternion.identity,StepHeight+.2f,~0,QueryTriggerInteraction.Ignore)){
            if(!Static(hit.collider)||hit.normal.y<.65f||hit.point.y>StepHeight+.001f)continue;
            if(hit.point.y>highest){highest=hit.point.y;supported=true;}
        }
        if(!supported)return false;foot.y=highest;return Clear(foot);
    }
    // Validate the whole body corridor, not just its two endpoints. Cinder geometry is static for the scene lifetime.
    static bool SegmentClear(Vector3 a,Vector3 b){
        var delta=b-a;
        foreach(var hit in Physics.BoxCastAll(a+Vector3.up*.95f,new Vector3(.42f,.8f,.42f),delta.normalized,Quaternion.identity,delta.magnitude,~0,QueryTriggerInteraction.Ignore))
            if(Static(hit.collider))return false;
        return WalkPoint((a+b)*.5f,out var middle)&&Mathf.Abs(middle.y-a.y)<=StepHeight+.001f&&Mathf.Abs(middle.y-b.y)<=StepHeight+.001f;
    }
    void BuildGrid(){
        var elapsed=System.Diagnostics.Stopwatch.StartNew();
        Physics.SyncTransforms();
        int minZ=cinder?-42:outer?-13:3,maxZ=cinder?42:outer?29:13,minX=cinder?-53:outer?-15:-8,maxX=cinder?53:outer?15:3;
        for(int z=minZ;z<=maxZ;z++)for(int x=minX;x<=maxX;x++){
            if(WalkPoint(new Vector3(x,0,z),out var p)&&(!outer||!CarryMission.Aboard(p))){grid[new Vector2Int(x,z)]=nodes.Count;nodes.Add(p);edges.Add(new List<int>(4));}
        }
        for(int n=0;n<nodes.Count;n++){
            var cell=new Vector2Int(Mathf.RoundToInt(nodes[n].x),Mathf.RoundToInt(nodes[n].z));
            foreach(var offset in NeighborOffsets)if(grid.TryGetValue(cell+offset,out int i)&&i>n&&Mathf.Abs(nodes[i].y-nodes[n].y)<=StepHeight+.001f&&(!cinder||SegmentClear(nodes[n],nodes[i]))){edges[n].Add(i);edges[i].Add(n);}
        }
        gridBuilt=true;
        elapsed.Stop();
        if(cinder)Debug.Log("Cinder navigation prepared: "+(outer?"outer":"listener")+", "+nodes.Count+" nodes, "+elapsed.ElapsedMilliseconds+" ms");
    }
    // Called before accepting peers so the first active AI tick cannot stall the connection.
    public void PrepareNavigation(){
        if(!cinder)return;
        if(listenerOwner!=null){if(!listenerOwner.gridBuilt)listenerOwner.BuildGrid();gridBuilt=listenerOwner.gridBuilt;}
        else if(!gridBuilt)BuildGrid();
        foreach(var listener in additional)listener.PrepareNavigation();
    }
    int Nearest(Vector3 p){int result=0;float best=float.MaxValue;for(int i=0;i<nodes.Count;i++){float d=(nodes[i]-p).sqrMagnitude;if(d<best){best=d;result=i;}}return result;}
    void Route(Vector3 goal){
        if(!gridBuilt){if(cinder)PrepareNavigation();else BuildGrid();}path.Clear();if(nodes.Count==0)return;
        int start=Nearest(position),end=Nearest(goal);var parents=new int[nodes.Count];Array.Fill(parents,-1);parents[start]=start;
        var queue=new Queue<int>();queue.Enqueue(start);
        while(queue.Count>0&&parents[end]<0){int n=queue.Dequeue();foreach(int i in edges[n])if(parents[i]<0){parents[i]=n;queue.Enqueue(i);}}
        if(parents[end]<0)return;
        if(cinder&&(position-nodes[start]).sqrMagnitude>.0001f){if(!SegmentClear(position,nodes[start]))return;path.Add(nodes[start]);}
        var reverse=new List<Vector3>();for(int n=end;n!=start;n=parents[n])reverse.Add(nodes[n]);reverse.Reverse();path.AddRange(reverse);
    }
    public void Hear(Vector3 source,float range){HearLocal(source,range);foreach(var listener in additional)listener.HearLocal(source,range);}
    void HearLocal(Vector3 source,float range){if(outer)return;if(state==2||state==3||state==4||CarryMission.Aboard(source)||Vector3.Distance(position,source)>range)return;noiseTarget=source;noises++;state=1;interest=5;Route(source);}
    // The outer creature knows crew positions after entry; geometry still controls movement.
    void SearchCrew(Vector3[] players,int mask){
        int chosen=-1;float best=float.MaxValue;
        for(int i=0;i<players.Length;i++){
            if((mask&(1<<i))==0||Down[i]||CarryMission.Aboard(players[i]))continue;
            float distance=(players[i]-position).sqrMagnitude;
            if(distance<best){chosen=i;best=distance;}
        }
        if(chosen<0){if(pursuedPlayer>=0){state=0;path.Clear();}pursuedPlayer=-1;return;}
        pursuedPlayer=chosen;noiseTarget=players[chosen];state=1;interest=2;Route(noiseTarget);
    }
    // Beacon pulses briefly override global pursuit; ordinary footsteps/calls do not.
    public void Distract(Vector3 source,float range){
        if(!outer){Hear(source,range);return;}
        if(state>=2||CarryMission.Aboard(source)||Vector3.Distance(position,source)>range)return;
        pursuedPlayer=-1;noiseTarget=source;noises++;state=1;interest=5;distraction=1.2f;Route(source);
    }
    public void Reset(){
        if(!outer&&listenerOwner==null){Array.Clear(Down,0,4);Array.Clear(Rescue,0,4);Array.Clear(Cooldown,0,4);Array.Clear(protection,0,4);Array.Clear(swings,0,4);Array.Fill(rescueTargets,-1);}
        if(!cinder){gridBuilt=false;grid.Clear();nodes.Clear();edges.Clear();}
        position=patrol[0];state=0;patrolIndex=0;timer=0;stunResistance=0;allDown=0;searchClock=0;distraction=0;pursuedPlayer=-1;interest=0;displayState=-1;
        Array.Clear(stepClock,0,4);Array.Clear(previous,0,4);path.Clear();foreach(var listener in additional)listener.Reset();
    }
    public bool Tick(Vector3[] players,bool peer,CarryInput[] inputs,int holder,bool field,float dt)=>Tick(players,peer?3:1,inputs,holder,field,dt);
    public bool Tick(Vector3[] players,int mask,CarryInput[] inputs,int holder,bool field,float dt){
        if(!field)return false;
        stunResistance=Mathf.Max(0,stunResistance-dt);
        int count=players.Length;
        if(!outer&&listenerOwner==null)for(int i=0;i<count;i++){
            protection[i]=Mathf.Max(0,protection[i]-dt);Cooldown[i]=Mathf.Max(0,Cooldown[i]-dt);
            swings[i]=(mask&(1<<i))!=0&&!Down[i]&&inputs[i].shove&&holder!=i&&Cooldown[i]<=0;
            if(swings[i])Cooldown[i]=6;
        }
        for(int i=0;i<count;i++){
            if((mask&(1<<i))==0)continue;
            stepClock[i]-=dt;
            if(!Down[i]){
                if(inputs[i].call)HearLocal(players[i],12);
                if(!inputs[i].quiet&&stepClock[i]<=0&&(players[i]-previous[i]).sqrMagnitude>.00001f){HearLocal(players[i],Hard?8:6);stepClock[i]=.45f;}
                if(!outer&&swings[i]){
                    var delta=position-players[i];var facing=Quaternion.Euler(0,inputs[i].yaw,0)*Vector3.forward;
                    if(state!=4&&stunResistance<=0&&delta.magnitude<=2.5f&&Vector3.Angle(delta,facing)<=65&&Sight(players[i],position)){state=4;timer=3;path.Clear();}
                }
            }
            previous[i]=players[i];
        }
        if(outer){
            distraction=Mathf.Max(0,distraction-dt);searchClock-=dt;
            if(state<2&&distraction<=0&&searchClock<=0){searchClock=.8f;SearchCrew(players,mask);}
        }
        if(state==2){timer-=dt;if(timer<=0){if((mask&(1<<victim))!=0&&!Down[victim]&&!CarryMission.Aboard(players[victim])&&protection[victim]<=0&&Vector3.Distance(players[victim],position)<1.5f&&Sight(position,players[victim])){Down[victim]=true;hits++;}state=3;timer=4;}}
        else if(state==3||state==4){timer-=dt;if(timer<=0){if(state==4)stunResistance=2;state=0;path.Clear();}}
        else {
            int close=-1;for(int i=0;i<count;i++)if((mask&(1<<i))!=0&&!Down[i]&&!CarryMission.Aboard(players[i])&&protection[i]<=0&&Vector3.Distance(position,players[i])<1.3f&&Sight(position,players[i])){close=i;break;}
            if(close>=0){state=2;timer=(Hard||outer)?.9f:1.2f;victim=close;path.Clear();}
            else {
                if(state==1){interest-=dt;if(interest<=0){state=0;path.Clear();}}
                if(state==0&&path.Count==0){patrolIndex=(patrolIndex+1)%patrol.Length;Route(patrol[patrolIndex]);}
                if(path.Count>0){var next=Vector3.MoveTowards(position,path[0],dt*(state==1?(outer?3.3f:Hard?3.2f:2.5f):(outer?2.2f:Hard?1.8f:1.4f)));if(WalkPoint(next,out var grounded)&&Mathf.Abs(grounded.y-position.y)<=StepHeight+.001f)position=grounded;if(Vector3.Distance(position,path[0])<.03f)path.RemoveAt(0);}
            }
        }
        if(outer||listenerOwner!=null)return false;
        foreach(var listener in additional){listener.Hard=Hard;listener.Tick(players,mask,inputs,holder,field,dt);}
        for(int i=0;i<count;i++){
            int other=RescueTarget(i,players,mask);
            bool can=(mask&(1<<i))!=0&&!Down[i]&&other>=0&&holder!=i&&inputs[i].rescue;
            if(rescueTargets[i]!=other)Rescue[i]=0;rescueTargets[i]=other;
            Rescue[i]=can?Rescue[i]+dt:0;
            if(Rescue[i]>=2.5f){Down[other]=false;protection[other]=4;Rescue[i]=0;rescues++;}
        }
        bool everyone=mask!=0;for(int i=0;i<count;i++)if((mask&(1<<i))!=0&&!Down[i])everyone=false;
        if(everyone)allDown+=dt;else allDown=0;
        if(allDown>=3){evacuations++;return true;}return false;
    }
    readonly int[] rescueTargets={-1,-1,-1,-1};
    public int RescueTarget(int who,Vector3[] players,int mask){
        int chosen=-1;float nearest=2.0001f;
        for(int i=0;i<players.Length;i++)if(i!=who&&(mask&(1<<i))!=0&&Down[i]){
            float d=Vector3.Distance(players[who],players[i]);if(d<nearest&&Sight(players[who],players[i])){chosen=i;nearest=d;}
        }
        return chosen;
    }
    public ThreatState Snapshot()=>new ThreatState{down=(bool[])Down.Clone(),rescue=(float[])Rescue.Clone(),cooldown=(float[])Cooldown.Clone(),pursuedPlayer=pursuedPlayer,position=position,noiseTarget=noiseTarget,state=state,hits=hits,rescues=rescues,evacuations=evacuations,noises=noises,down0=Down[0],down1=Down[1],rescue0=Rescue[0],rescue1=Rescue[1],cooldown0=Cooldown[0],cooldown1=Cooldown[1],warning=state==2?timer:0};
    public ThreatState[] AdditionalSnapshots(){var result=new ThreatState[additional.Length];for(int i=0;i<result.Length;i++)result[i]=additional[i].Snapshot();return result;}
    public void DisplayAdditional(ThreatState[] states,bool enabled){for(int i=0;i<additional.Length;i++){bool valid=states!=null&&i<states.Length&&states[i]!=null;additional[i].Display(valid?states[i]:null,enabled&&valid);}}
    public void Display(ThreatState s,bool enabled){
        body.SetActive(enabled);if(!enabled)return;
        body.transform.position=s.position;body.transform.localScale=s.state==2?new Vector3(1.15f,.8f,1.15f):Vector3.one;
        skin.color=s.state==2?new Color(1,.16f,.08f):s.state==4?new Color(.1f,.7f,.8f):outer?new Color(.13f,.07f,.18f):new Color(.42f,.3f,.22f);
        if(s.state==2&&displayState!=2)audio.PlayOneShot(warningClip);displayState=s.state;
    }
}
}
