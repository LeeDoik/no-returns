using UnityEngine;
namespace NoReturns.CarryLab {
public static class CarryWorld {
    public static Material Mat(Color c){var m=new Material(Resources.Load<Material>("CarrySurface"));m.color=c; m.SetFloat("_Smoothness",0);return m;}
    public static GameObject Box(string name,Vector3 p,Vector3 size,Material m,Transform parent=null){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent);g.transform.position=p;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    public static void Label(string value,Vector3 p,Quaternion rotation,float scale=.18f){var g=new GameObject(value);g.transform.position=p;g.transform.rotation=rotation;var t=g.AddComponent<TextMesh>();t.text=value;t.fontSize=48;t.characterSize=scale;t.anchor=TextAnchor.MiddleCenter;t.color=new Color(.95f,.8f,.5f);}
    public static void Build(){
        var rust=Mat(new Color(.27f,.19f,.16f));var cream=Mat(new Color(.49f,.46f,.38f));var dark=Mat(new Color(.12f,.15f,.17f));var yellow=Mat(new Color(.65f,.44f,.13f));
        Box("Floor",new Vector3(0,-.3f,8),new Vector3(32,.6f,44),dark);
        Box("West wall",new Vector3(-16,2,8),new Vector3(.5f,4,44),rust);Box("East wall",new Vector3(16,2,8),new Vector3(.5f,4,44),rust);
        Box("Rear wall",new Vector3(0,2,-14),new Vector3(32,4,.5f),rust);Box("Far wall",new Vector3(0,2,30),new Vector3(32,4,.5f),rust);
        Box("Corner wall",new Vector3(-3,1.5f,2),new Vector3(7,3,.6f),cream);
        Box("Narrow passage",new Vector3(4,1.5f,5),new Vector3(.7f,3,6),cream);
        // Reception outline is visual-only; the continuous floor supports the parcel.
        new GameObject("Receipt feedback").AddComponent<ReceiptFeedback>();
        Box("Ship boarding zone",new Vector3(0,.012f,-7),new Vector3(6,.015f,6),Mat(new Color(.04f,.3f,.4f)));
        Box("Low step",new Vector3(0,.15f,8),new Vector3(3,.3f,2),rust);
        for(int i=0;i<4;i++)Box("Floor marking",new Vector3(1,.006f,i*3-6),new Vector3(.15f,.01f,1.2f),yellow);
        // Connected wings keep the original delivery loop and add escape choices.
        Box("East service divider",new Vector3(8,2,12),new Vector3(.6f,4,16),cream);
        Box("East dogleg",new Vector3(14,2,6),new Vector3(4,4,.6f),rust);
        Box("North sorting divider",new Vector3(-5.5f,2,20),new Vector3(17,4,.6f),cream);
        Box("North gate divider",new Vector3(6,2,24),new Vector3(.6f,4,8),rust);
        Box("West storage divider",new Vector3(-11,2,9),new Vector3(.6f,4,10),cream);
        for(int i=0;i<3;i++){
            Box("West storage rack",new Vector3(-14.75f,.8f,5+i*4),new Vector3(2,1.6f,1.5f),dark);
            Box("North sorting stack",new Vector3(-10+i*5,.8f,26),new Vector3(2,1.6f,2),rust);
        }
        Box("East gate left post",new Vector3(11,2,28),new Vector3(.5f,4,.5f),yellow);
        Box("East gate right post",new Vector3(15,2,28),new Vector3(.5f,4,.5f),yellow);
        Box("East gate lintel",new Vector3(13,4,28),new Vector3(4.5f,.4f,.5f),yellow);
        for(int i=0;i<4;i++)Box("Escape route marker",new Vector3(-9,.006f,3+i*4),new Vector3(.18f,.01f,1.3f),yellow);
        FacilityArt.Dress();
        RenderSettings.skybox=null;
        RenderSettings.ambientLight=new Color(.45f,.43f,.4f);RenderSettings.fog=true;RenderSettings.fogColor=new Color(.08f,.09f,.12f);RenderSettings.fogMode=FogMode.Linear;RenderSettings.fogStartDistance=15;RenderSettings.fogEndDistance=40;
        var light=new GameObject("Work light").AddComponent<Light>();light.type=LightType.Directional;light.intensity=1.5f;light.color=new Color(1,.86f,.67f);light.transform.rotation=Quaternion.Euler(50,-25,0);light.shadows=LightShadows.Soft;
    }
}
}
