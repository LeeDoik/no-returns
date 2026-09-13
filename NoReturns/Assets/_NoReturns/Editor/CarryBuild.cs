using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using NoReturns.CarryLab;
namespace NoReturns.Editor {
public static class CarryBuild {
 public const string Scene="Assets/_NoReturns/Scenes/CarryRoom.unity";
 [MenuItem("NO RETURNS/Build Carry Test")]
 public static void Build(){
  FacilityArtImport.Prepare();
  PropArtImport.Prepare();
  if(!File.Exists(Scene)){var s=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("Carry test runtime").AddComponent<CarryRoom>();EditorSceneManager.SaveScene(s,Scene);}
  Directory.CreateDirectory("Assets/_NoReturns/Resources");
  if(!File.Exists("Assets/_NoReturns/Resources/CarrySurface.mat")) AssetDatabase.CreateAsset(new Material(Shader.Find("Universal Render Pipeline/Lit")),"Assets/_NoReturns/Resources/CarrySurface.mat");
  AssetDatabase.SaveAssets();
  PlayerSettings.runInBackground=true;PlayerSettings.defaultScreenWidth=1280;PlayerSettings.defaultScreenHeight=720;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;
  PlayerSettings.bundleVersion="0.9.1";
  var workspace=File.Exists("CarryWorkspace.txt")?File.ReadAllText("CarryWorkspace.txt").Trim():Path.GetFullPath("..");
  var output=Path.Combine(workspace,"builds/CarryTest/NoReturns.exe");Directory.CreateDirectory(Path.GetDirectoryName(output));
  var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{Scene},locationPathName=output,target=BuildTarget.StandaloneWindows64,options=BuildOptions.None,extraScriptingDefines=new[]{"CARRY_TEST_AUTOMATION"}});
  if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new System.Exception("Carry build failed: "+report.summary.result);
  Debug.Log("SPACE-PLAY-01 BUILD PASS "+output);
 }
}
}
