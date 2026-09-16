using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using NoReturns.Trials;
namespace NoReturns.Editor {
public static class CinderBlockoutBuild {
 const string ScenePath="Assets/_NoReturns/Scenes/CinderDepotBlockout.unity";
 static string Workspace => File.Exists("CarryWorkspace.txt") ? File.ReadAllText("CarryWorkspace.txt").Trim() : Path.GetFullPath("..");
 static Transform root; static Material ground,wall,roof,path,red,light;
 static Vector3 P(float x,float y,float height=0)=>new Vector3((x-450)*.12f,height,(360-y)*.12f);
 static Material Mat(string name,Color c){string dir="Assets/_NoReturns/Art/CinderBlockout";Directory.CreateDirectory(dir);string p=dir+"/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(p);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,p);}m.color=c;return m;}
 static GameObject Box(string name,Vector3 pos,Vector3 size,Material mat){var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(root);o.transform.position=pos;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=mat;return o;}
 static void Line(string name,float x,float y,float xx,float yy,float width,float height,Material mat,float elevation=0){Vector3 a=P(x,y),b=P(xx,yy);var o=Box(name,(a+b)/2+Vector3.up*(elevation+height/2),new Vector3(width,height,Vector3.Distance(a,b)),mat);o.transform.rotation=Quaternion.LookRotation(b-a);}
 static void Sign(string text,float x,float y){var o=new GameObject(text);o.transform.SetParent(root);o.transform.position=P(x,y,2.6f);var t=o.AddComponent<TextMesh>();t.text=text;t.fontSize=48;t.characterSize=.07f;t.anchor=TextAnchor.MiddleCenter;t.color=Color.white;}
 // Two opposite 3.2m doors; wall height 4m, roof underside 4m.
 static void Building(string name,float x,float y,float w,float d){var c=P(x+w/2,y+d/2);float a=w*.12f,b=d*.12f;var group=new GameObject(name);group.transform.SetParent(root);var prior=root;root=group.transform;
 Box(name+" floor",c+Vector3.down*.1f,new Vector3(a,.2f,b),path);
 Box("West wall",c+Vector3.left*a/2+Vector3.up*2,new Vector3(.3f,4,b),wall);Box("East wall",c+Vector3.right*a/2+Vector3.up*2,new Vector3(.3f,4,b),wall);
 foreach(int side in new[]{-1,1}){float len=(a-3.2f)/2;foreach(int half in new[]{-1,1})Box("Door side",c+new Vector3(half*(a/4+.8f),2,side*b/2),new Vector3(len,4,.3f),wall);Box("Door lintel",c+new Vector3(0,3.65f,side*b/2),new Vector3(3.2f,.7f,.3f),wall);}
 Box("Roof",c+Vector3.up*4.15f,new Vector3(a+.3f,.3f,b+.3f),roof);Sign(name,x+w/2,y+d+2);root=prior;}
 static void Route(string name,float[,] points,float width,Material mat,bool covered=false){for(int i=1;i<points.GetLength(0);i++){Line(name,points[i-1,0],points[i-1,1],points[i,0],points[i,1],width,.025f,mat,.01f);if(covered)Line("Covered walkway roof",points[i-1,0],points[i-1,1],points[i,0],points[i,1],width+.3f,.25f,roof,4.2f);}}
 [MenuItem("NO RETURNS/Trials/Create Cinder Depot Blockout")]
 public static void Create(){
 if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())throw new Exception("Scene switch cancelled");
 var scene=EditorSceneManager.OpenScene("Assets/_NoReturns/Scenes/ShipInteriorTrial.unity",OpenSceneMode.Single);
 // Save under a new path before modifying the loaded trial.
 EditorSceneManager.SaveScene(scene,ScenePath);
 var holder=new GameObject("Reused Ship Interior Trial 05");
 foreach(var o in scene.GetRootGameObjects()){if(o==holder)continue;if(o.name=="Trial landing ground"||o.name=="Trial employee"||o.name=="Trial daylight"){UnityEngine.Object.DestroyImmediate(o);continue;}o.transform.SetParent(holder.transform,true);}
 holder.transform.SetPositionAndRotation(P(110,610),Quaternion.Euler(0,-90,0));
 root=new GameObject("Cinder Depot editable primitive blockout").transform;
 ground=Mat("Outdoor",new Color(.31f,.25f,.19f));wall=Mat("Building",new Color(.34f,.43f,.5f));roof=Mat("Roof",new Color(.21f,.27f,.3f));path=Mat("Indoor",new Color(.45f,.48f,.46f));red=Mat("Listener marker",new Color(.65f,.23f,.3f));light=Mat("Covered route",new Color(.43f,.58f,.54f));
 Box("Exterior ground 108x86.4m",P(450,360,-.17f),new Vector3(108,.3f,86.4f),ground);
 Building("A WAREHOUSE",235,260,120,170);Building("SIDE OFFICE",380,140,120,75);Building("BAY 04",665,150,115,140);Building("C SERVICE",650,360,105,185);Building("STORAGE",370,575,240,60);
 Route("Covered delivery route",new float[,]{{170,592},{245,545},{245,500},{185,475},{185,235},{265,190},{345,190},{400,240},{590,240},{620,215},{645,215}},3.2f,light,true);
 // Bay door lies on its south face; terminate the approach there, not through the wall.
 Route("Bay approach",new float[,]{{645,215},{645,310},{722.5f,310},{722.5f,290}},3.2f,light);
 Route("A lure loop",new float[,]{{210,250},{380,250},{380,455},{210,455},{210,250}},2.4f,path);
 Route("B lure loop",new float[,]{{400,340},{550,340},{550,500},{400,500},{400,340}},2.4f,path);
 Route("C lure loop",new float[,]{{600,320},{790,320},{790,565},{600,565},{600,320}},2.4f,path);
 Route("West link",new float[,]{{185,370},{210,370}},2.4f,light); Route("East link",new float[,]{{380,370},{400,370}},2.4f,light);
 Route("BC link",new float[,]{{550,370},{600,370}},2.4f,light);
 Route("Shortcut",new float[,]{{170,610},{330,565},{400,550},{530,500},{570,410},{570,355},{630,335},{645,310},{722.5f,310}},2.4f,Mat("Shortcut",new Color(.65f,.48f,.23f)));
 Box("B freight island",P(472.5f,432.5f,1.2f),new Vector3(6.6f,2.4f,6.6f),wall);
 foreach(var v in new[]{new Vector2(330,535),new Vector2(100,310),new Vector2(485,370)})Box("Freight obstacle",P(v.x,v.y,1),new Vector3(2,2,2),wall);
 foreach(var v in new[]{new Vector2(60,510),new Vector2(240,85),new Vector2(775,95),new Vector2(820,510)}){Box("Suppressor placeholder",P(v.x,v.y,2.5f),new Vector3(1,5,1),light);Sign("SUPPRESSOR",v.x,v.y);}
 int n=0;foreach(var v in new[]{new Vector2(220,330),new Vector2(535,440),new Vector2(805,450)}){var o=GameObject.CreatePrimitive(PrimitiveType.Capsule);o.name="Listener zone "+(++n)+" STATIC MARKER";o.transform.SetParent(root);o.transform.position=P(v.x,v.y,1.3f);o.transform.localScale=new Vector3(.8f,1.3f,.8f);o.GetComponent<Renderer>().sharedMaterial=red;UnityEngine.Object.DestroyImmediate(o.GetComponent<Collider>());Sign("LISTENER "+n+" / MARKER",v.x,v.y);}
 var employee=new GameObject("Blockout employee");employee.transform.position=P(175,610,.06f);employee.AddComponent<CinderBlockoutWalk>();
 var sun=new GameObject("Blockout daylight").AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=1.3f;sun.transform.rotation=Quaternion.Euler(45,-30,0);RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;RenderSettings.ambientLight=new Color(.5f,.5f,.5f);RenderSettings.fog=false;
 // Ground edge guard; no accidental falling from the review surface.
 Box("North boundary",P(450,0,1),new Vector3(108,2,.4f),roof);Box("South boundary",P(450,720,1),new Vector3(108,2,.4f),roof);Box("West boundary",P(0,360,1),new Vector3(.4f,2,86.4f),roof);Box("East boundary",P(900,360,1),new Vector3(.4f,2,86.4f),roof);
 Validate();EditorSceneManager.SaveScene(scene,ScenePath);AssetDatabase.SaveAssets();
 }
 [MenuItem("NO RETURNS/Trials/Validate Cinder Depot Blockout")]
 public static void Validate(){Physics.SyncTransforms();var results=new List<string>();bool passed=true;var probe=new GameObject("Passage probe");var cc=probe.AddComponent<CharacterController>();cc.height=1.8f;cc.radius=.34f;cc.center=Vector3.up*.9f;cc.stepOffset=.32f;cc.skinWidth=.035f;
 Action<string,Vector3,Vector3> run=(name,a,b)=>{cc.enabled=false;probe.transform.position=a+Vector3.up*.05f;cc.enabled=true;Physics.SyncTransforms();var delta=b-a;delta.y=0;for(int i=0;i<5000&&Vector3.Distance(new Vector3(probe.transform.position.x,0,probe.transform.position.z),new Vector3(b.x,0,b.z))>.12f;i++){var remaining=b-probe.transform.position;remaining.y=0;cc.Move(Vector3.ClampMagnitude(remaining,.06f)+Vector3.down*.02f);}var e=probe.transform.position;bool ok=Vector2.Distance(new Vector2(e.x,e.z),new Vector2(b.x,b.z))<.2f;passed&=ok;results.Add(name+" pass="+ok+" end="+e);};
 foreach(var b in new[]{new float[]{235,260,120,170},new float[]{380,140,120,75},new float[]{665,150,115,140},new float[]{650,360,105,185},new float[]{370,575,240,60}})run("Building both doors",P(b[0]+b[2]/2,b[1]-15),P(b[0]+b[2]/2,b[1]+b[3]+15));
 foreach(var q in new[]{new float[,]{{210,250},{380,250},{380,455},{210,455},{210,250}},new float[,]{{400,340},{550,340},{550,500},{400,500},{400,340}},new float[,]{{600,320},{790,320},{790,565},{600,565},{600,320}}})for(int i=1;i<q.GetLength(0);i++)run("Loop edge",P(q[i-1,0],q[i-1,1]),P(q[i,0],q[i,1]));
 var ship=GameObject.Find("Reused Ship Interior Trial 05").transform;
 run("Ship ramp entry",ship.TransformPoint(new Vector3(0,0,-8)),ship.TransformPoint(new Vector3(0,1.035f,-2.5f)));
 run("Ship ramp exit",ship.TransformPoint(new Vector3(0,1.035f,-2.5f)),ship.TransformPoint(new Vector3(0,0,-8)));
 foreach(var q in new[]{new float[,]{{175,610},{170,592},{245,545},{245,500},{185,475},{185,235},{265,190},{345,190},{400,240},{590,240},{620,215},{645,215},{645,310},{722.5f,310},{722.5f,275}},new float[,]{{175,610},{330,565},{400,550},{530,500},{570,410},{570,355},{630,335},{645,310},{722.5f,310}}})for(int i=1;i<q.GetLength(0);i++)run("Delivery route",P(q[i-1,0],q[i-1,1]),P(q[i,0],q[i,1]));
 UnityEngine.Object.DestroyImmediate(probe);var dir=Path.Combine(Workspace,"artifacts/cinder-blockout");Directory.CreateDirectory(dir);File.WriteAllLines(dir+"/passage.txt",results);if(!passed)throw new Exception("Blockout passage failed; inspect passage.txt");Debug.Log("CINDER BLOCKOUT PASS: "+results.Count);
 }
 [MenuItem("NO RETURNS/Trials/Build Cinder Depot Blockout")]
 public static void Build(){Create();var output=Path.Combine(Workspace,"builds/CinderBlockout/NoReturns-CinderBlockout.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));var result=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});if(result.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new Exception("Blockout build failed");File.WriteAllText(Path.Combine(Workspace,"artifacts/cinder-blockout/build-success.txt"),DateTime.UtcNow.ToString("O"));}
}
}
