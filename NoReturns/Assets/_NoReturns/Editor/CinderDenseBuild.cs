using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
// Authored campus in metres. Editable scene objects, no runtime prop scattering.
public static class CinderDenseBuild {
    static Transform root;
    static Material wall, floor, roof, cargo, trim, lamp, signMaterial;
    static Material Material(string name, Color color) {
        const string folder="Assets/_NoReturns/Art/CinderBlockout";
        var path=folder+"/Dense_"+name+".mat";
        var m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
        m.color=color;return m;
    }
    static GameObject Box(string name,float x,float y,float z,float w,float h,float d,Material m) {
        var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;
        o.transform.SetParent(root,false);o.transform.position=new Vector3(x,y,z);o.transform.localScale=new Vector3(w,h,d);
        o.GetComponent<Renderer>().sharedMaterial=m;return o;
    }
    static void Sign(string text,float x,float z,float yaw=0,float y=2.5f) {
        var o=new GameObject("Wayfinding "+text);o.transform.SetParent(root,false);
        o.transform.SetPositionAndRotation(new Vector3(x,y,z),Quaternion.Euler(0,yaw,0));
        var t=o.AddComponent<TextMesh>();t.text=text;t.fontSize=44;t.characterSize=.055f;t.anchor=TextAnchor.MiddleCenter;t.color=new Color(.84f,.9f,.77f);
        t.GetComponent<MeshRenderer>().sharedMaterial=signMaterial;
    }
    // Axis X or Z wall; door centres measured along that axis, 3.2m wide / 3.3m tall.
    static void Wall(string name,bool alongX,float fixedAt,float from,float to,params float[] doors) {
        Array.Sort(doors);float cursor=from;
        Action<float,float,float,float> part=(a,b,y,h)=>{if(b-a<.02f)return;Box(name,alongX?(a+b)/2:fixedAt,y,alongX?fixedAt:(a+b)/2,alongX?b-a:.3f,h,alongX?.3f:b-a,wall);};
        foreach(float door in doors){float a=door-1.6f,b=door+1.6f; if(a<cursor||b>to)throw new Exception("Invalid doorway "+name);part(cursor,a,2,4);part(a,b,3.65f,.7f);cursor=b;}
        part(cursor,to,2,4);
    }
    static Transform Building(string name,float x0,float x1,float z0,float z1,float south,float north,float west=float.NaN,float east=float.NaN) {
        var previous=root;var group=new GameObject(name);group.transform.SetParent(root,false);root=group.transform;
        Box("Floor",(x0+x1)/2,-.08f,(z0+z1)/2,x1-x0,.16f,z1-z0,floor);
        Wall("South exterior",true,z0,x0,x1,south);Wall("North exterior",true,z1,x0,x1,north);
        Wall("West exterior",false,x0,z0,z1,float.IsNaN(west)?Array.Empty<float>():new[]{west});
        Wall("East exterior",false,x1,z0,z1,float.IsNaN(east)?Array.Empty<float>():new[]{east});
        Box("Roof",(x0+x1)/2,4.2f,(z0+z1)/2,x1-x0+.3f,.4f,z1-z0+.3f,roof);
        // Fixed services and facade rhythm give each walk-in block an industrial use.
        Box("Service conduit",x0+.35f,3.55f,(z0+z1)/2,.18f,.18f,z1-z0-.6f,trim);
        Box("Roof exhaust",x1-2,4.65f,z1-2,1.3f,.7f,1.3f,trim);
        for(float z=z0+1;z<z1;z+=4) {
            Box("Facade column",x0-.18f,2,z,.16f,4,.28f,trim);
            Box("Facade column",x1+.18f,2,z,.16f,4,.28f,trim);
        }
        foreach(float z in new[]{z0+2,z1-2})Box("Ceiling light",(x0+x1)/2,3.85f,z,2,.08f,.25f,lamp).GetComponent<Collider>().enabled=false;
        Sign(name,south,z0-.18f,0,3.45f);Sign(name,north,z1+.18f,180,3.45f);
        root=previous;return group.transform;
    }
    static void Inside(Transform building,Action details){var old=root;root=building;details();root=old;}
    static void Rack(float x,float z,float length=3) {
        Box("Inventory rack",x,1.15f,z,length,2.3f,.85f,cargo);
        Box("Rack frame",x,2.35f,z,length+.12f,.12f,1,trim);
    }
    static void Container(string name,float x,float z,float w,float d) {
        Box(name,x,1.65f,z,w,3.3f,d,cargo);
        for(float offset=-w/2+.4f;offset<w/2;offset+=1.1f)Box("Freight reinforcement",x+offset,1.65f,z-d/2-.045f,.09f,3.3f,.09f,trim);
    }
    static void Canopy(string name,float x,float z,float w,float d) {
        Box(name,x,4.25f,z,w,.3f,d,roof);
        foreach(float dx in new[]{-w/2+.2f,w/2-.2f})foreach(float dz in new[]{-d/2+.2f,d/2-.2f})Box("Canopy column",x+dx,2.1f,z+dz,.25f,4.2f,.25f,trim);
    }
    [MenuItem("NO RETURNS/Demo/Populate Dense Cinder Facility")]
    public static void Populate() {
        if(EditorSceneManager.GetActiveScene().path!=CinderDemoBuild.ScenePath)EditorSceneManager.OpenScene(CinderDemoBuild.ScenePath);
        root=GameObject.Find("Cinder Depot editable primitive blockout").transform;
        // Ground, bounds and ship are immutable in this operation.
        for(int i=root.childCount-1;i>=0;i--){var o=root.GetChild(i).gameObject;if(o.name.StartsWith("Exterior ground")||o.name.EndsWith("boundary"))continue;UnityEngine.Object.DestroyImmediate(o);}
        wall=Material("Walls",new Color(.29f,.34f,.35f));floor=Material("Floors",new Color(.32f,.32f,.28f));roof=Material("Roofs",new Color(.15f,.2f,.21f));cargo=Material("Freight",new Color(.46f,.27f,.15f));trim=Material("Structure",new Color(.18f,.22f,.22f));lamp=Material("Light",new Color(.62f,.87f,.78f));
        lamp.EnableKeyword("_EMISSION");lamp.SetColor("_EmissionColor",new Color(.3f,.6f,.45f));
        const string signPath="Assets/_NoReturns/Art/CinderBlockout/Dense_Signs.mat";
        signMaterial=AssetDatabase.LoadAssetAtPath<Material>(signPath);
        if(!signMaterial){signMaterial=new Material(Shader.Find("NoReturns/DenseWorldSign"));AssetDatabase.CreateAsset(signMaterial,signPath);}
        signMaterial.mainTexture=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf").material.mainTexture;

        var a=Building("A / SORTING WAREHOUSE",-28,-10,-11,14,-20,-20,0,0);
        Inside(a,()=>{
            Wall("Receiving partition",true,-3,-28,-10,-23);Wall("Dispatch partition",true,6,-28,-10,-15);
            Wall("Records office",false,-19,6,14,10);
            Rack(-13,-7,3);Rack(-25,3,3);Rack(-13,10,3);
            Sign("RECEIVING",-24,-8);Sign("SORTING",-16,2);Sign("RECORDS",-24,11);
        });
        var west=Building("SECURITY / STAFF ENTRY",-47,-33,-19,-3,-39,-39,float.NaN,-10);
        Inside(west,()=>{Wall("Locker corridor",true,-11,-47,-33,-39);Wall("Security office",false,-43,-11,-3,-7);Rack(-45,-16,2);Rack(-35,-6,2);});
        var sorting=Building("WEST / RETURNS ANNEX",-47,-33,5,25,-39,-39,float.NaN,15);
        Inside(sorting,()=>{Wall("Processing room",true,14,-47,-33,-42);Wall("Archive",false,-38,14,25,20);Rack(-35,9,2);Rack(-44,21,3);});
        var north=Building("NORTH / WORKSHOP",-29,-15,24,38,-22,-22,float.NaN,31);
        Inside(north,()=>{Wall("Machine bay",true,31,-29,-15,-25);Rack(-18,34,3);Rack(-26,27,2);});
        var office=Building("DISPATCH / ADMIN",-10,10,18,33,0,0,25,25);
        Inside(office,()=>{
            Wall("South offices",true,23,-10,10,-6,6);Wall("North offices",true,28,-10,10,-6,6);
            Wall("South divider",false,0,18,23,20.5f);Wall("North divider",false,0,28,33,30.5f);
            Rack(-7,20,2);Rack(7,31,2);Sign("DISPATCH",5,20);Sign("ARCHIVE",-5,31);
        });
        var bay=Building("BAY 04 / DELIVERY",24,42,10,30,33,33,25,15);
        Inside(bay,()=>{Wall("Receipt lobby",true,20,24,42,33);Wall("Inspection room",false,35,20,30,25);Rack(27,26,3);Rack(39,27,2);Sign("04 / RECEIPT",32.7f,18);});
        var service=Building("C / PLANT ROOM",25,37,-22,1,31,31,-10,-10);
        Inside(service,()=>{Wall("Pump chamber",true,-15,25,37,29);Wall("Power room",true,-6,25,37,29);Rack(34,-18,2);Rack(28,-3,2);Box("Pump base",34,1,-10,2,2,2,trim);});
        var storage=Building("SOUTH / BULK STORAGE",-9,19,-35,-25,5,5,-30,-30);
        Inside(storage,()=>{Wall("Bulk partitions",false,0,-35,-25,-30);Wall("Cage partitions",false,10,-35,-25,-30);Rack(-5,-33,4);Rack(15,-27,4);});
        var east=Building("EAST / COOLING STATION",44,52,-5,23,48,48,8);
        Inside(east,()=>{Wall("Cooling banks",true,5,44,52,48);Wall("Control booth",true,15,44,52,48);Box("Cooling pump",50,1,10,1.3f,2,3,trim);});

        // Working freight courts, with cross routes and two ways around each island.
        Container("B / freight island",3,-8,7,7);
        Container("B / sorting stack",3,6,7,4);
        Container("B / southern inventory",4,-20,9,3);
        Container("B / cross aisle freight",-3,-1.5f,6,3);
        Container("B / loading screen",13,-5,3,8);
        Container("B / southern screen",-3,-15,4,3);
        Container("North transfer stack",16,29,5,13);
        Container("South loading freight",26,-33,6,8);
        Container("West loading freight",-25,-23,5,8);
        Container("Arrival service stack",-17,-36,8,6);
        Container("Northeast sealed freight",37,37,16,6);
        Container("Northwest utility unit",-42,34,12,8);
        Container("Southeast utility unit",44,-32,13,9);
        Canopy("A/B transfer canopy",-10,0,8,6);
        Canopy("B/C transfer canopy",18,-10,10,6);
        Canopy("North dispatch link",17,25,12,4);
        Canopy("Ship unloading shelter",-29,-30,5,5);
        // Perimeter traversals have offset openings; no uninterrupted safe race track.
        Wall("West maintenance baffle",true,1,-54,-29,-36);
        Wall("West north baffle",true,29,-54,-30,-49.5f);
        Wall("North transfer baffle",false,12,33,43.2f,38.5f);
        Wall("East maintenance baffle",true,-25,37,54,40.5f);
        Wall("South supply baffle",false,22,-43.2f,-25,-39.5f);
        Wall("Central approach baffle",true,10,-8,21,15);
        Wall("Western approach baffle",true,18,-32,-12,-30);
        // Partial bays stop long diagonal sightlines without sealing alternative routes.
        Wall("Loading yard windbreak",false,-14,-24,-14,-19);
        Wall("C approach windbreak",true,5,19,43,23,39);
        foreach(var p in new[]{new Vector2(-31,2),new Vector2(15,13),new Vector2(40,-21)}) {
            Box("Signal mast",p.x,2.2f,p.y,.3f,4.4f,.3f,trim);
            Box("Signal cap",p.x,4.1f,p.y,.65f,.2f,.65f,lamp).GetComponent<Collider>().enabled=false;
        }
        Sign("BAY 04 >",-30,-28);Sign("BAY 04 >",-13,16);Sign("BAY 04 >",17,8);Sign("< SHIP",-30,-26,180);Sign("< SHIP / STAFF EXIT",-30,2,180);
        var existing=GameObject.Find("Dense facility revision");if(!existing){existing=new GameObject("Dense facility revision");existing.transform.SetParent(root,false);}
        Physics.SyncTransforms();EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());AssetDatabase.SaveAssets();
        Debug.Log("CINDER DENSE: same ground/ship, 9 walk-in buildings, rooms and linked freight courts");
    }
}
}
