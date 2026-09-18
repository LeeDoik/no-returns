using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using NoReturns.CarryLab;
using NoReturns.Trials;
namespace NoReturns.Editor {
public static class CinderDemoBuild {
 public const string ScenePath="Assets/_NoReturns/Scenes/CinderDeliveryDemo.unity";
 static string Workspace=>File.Exists("CarryWorkspace.txt")?File.ReadAllText("CarryWorkspace.txt").Trim():Path.GetFullPath("..");
 [MenuItem("NO RETURNS/Demo/Create Cinder Delivery Demo")]
 public static void Create(){
  var scene=EditorSceneManager.OpenScene("Assets/_NoReturns/Scenes/CinderDepotBlockout.unity",OpenSceneMode.Single);
  EditorSceneManager.SaveScene(scene,ScenePath);
  foreach(var walk in UnityEngine.Object.FindObjectsByType<CinderBlockoutWalk>(FindObjectsSortMode.None))UnityEngine.Object.DestroyImmediate(walk.gameObject);
  foreach(var name in new[]{"Trial carried parcel","Blockout parcel"}){var item=GameObject.Find(name);if(item)UnityEngine.Object.DestroyImmediate(item);}
  foreach(var renderer in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))if(renderer.name.Contains("STATIC MARKER")||renderer.name.StartsWith("LISTENER "))UnityEngine.Object.DestroyImmediate(renderer.gameObject);
  new GameObject("Cinder demo layout").AddComponent<CinderDemoLayout>();
  new GameObject("Cinder delivery runtime").AddComponent<CarryRoom>();
  EditorSceneManager.SaveScene(scene,ScenePath);AssetDatabase.SaveAssets();
  Validate();
 }
 [MenuItem("NO RETURNS/Demo/Validate Cinder Delivery Demo")]
 public static void Validate(){
  CinderDemoLayout.Initialize();if(!CinderDemoLayout.Active)throw new Exception("Missing Cinder layout");
  for(int i=0;i<4;i++)if(!CarryMission.Aboard(CinderDemoLayout.Spawn(i)))throw new Exception("Crew spawn outside ship "+i);
  var m=new CarryMission();m.Act(true,true);m.Act(true,true);
  m.Tick(CinderDemoLayout.Reception+Vector3.up*.4f,Vector3.zero,0,1);if(m.Phase!=2)throw new Exception("Held delivery accepted");
  m.Tick(CinderDemoLayout.Reception+Vector3.up*.4f,Vector3.zero,-1,1);if(m.Phase!=3)throw new Exception("Cinder reception failed");
  if(m.CollectReceipt())throw new Exception("Receipt not yet printed");
  m.Tick(Vector3.zero,Vector3.zero,-1,1);if(!m.CollectReceipt())throw new Exception("Receipt collection failed");
  m.Act(true,false);if(m.Phase!=3)throw new Exception("Crew left behind");
  m.Act(true,true);if(m.Credits!=420)throw new Exception("Incorrect settlement");
  var dir=Path.Combine(Workspace,"artifacts/cinder-demo");Directory.CreateDirectory(dir);File.WriteAllText(Path.Combine(dir,"rules.txt"),"PASS: four spawns aboard, held rejection, stable acceptance, print delay, receipt, all aboard, 420 settlement");
 }
 [MenuItem("NO RETURNS/Demo/Build Cinder Delivery Demo")]
 public static void Build(){
  // Build the reviewed saved scene; never overwrite manual scene edits here.
  if(EditorSceneManager.GetActiveScene().path!=ScenePath)EditorSceneManager.OpenScene(ScenePath);
  Validate();EditorSceneManager.SaveOpenScenes();
  var output=Path.Combine(Workspace,"builds/CinderDemo/NoReturns-CinderDemo.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None,extraScriptingDefines=new[]{"CARRY_TEST_AUTOMATION"}});
  if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new Exception("Cinder demo build failed");
  File.WriteAllText(Path.Combine(Workspace,"artifacts/cinder-demo/build-success.txt"),DateTime.UtcNow.ToString("O"));
 }
}
}
