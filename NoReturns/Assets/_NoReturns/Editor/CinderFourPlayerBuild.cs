using System;
using System.IO;
using System.Linq;
using NoReturns.CarryLab;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
public static class CinderFourPlayerBuild {
    public const string ScenePath="Assets/_NoReturns/Scenes/CinderFourPlayerTest.unity";
    [Serializable] class Receipt {public string status="FAIL",unity=Application.unityVersion,output,error;public int errors,warnings;}
    [MenuItem("NO RETURNS/Tests/Build Cinder Four-Player Test (Mac)")]
    public static void Mac()=>Build(BuildTarget.StandaloneOSX);
    [MenuItem("NO RETURNS/Tests/Build Cinder Four-Player Test (Windows)")]
    public static void Windows()=>Build(BuildTarget.StandaloneWindows64);
    static BuildTarget queuedTarget;
    public static void Queue(bool mac) {queuedTarget=mac?BuildTarget.StandaloneOSX:BuildTarget.StandaloneWindows64;EditorApplication.update-=RunQueued;EditorApplication.update+=RunQueued;}
    static void RunQueued() {EditorApplication.update-=RunQueued;Build(queuedTarget);}
    static void Build(BuildTarget target) {
        var scene=EditorSceneManager.GetActiveScene();string prior=scene.path;
        var receipt=new Receipt();bool opened=false;
        try {
            if(EditorApplication.isPlaying||scene.isDirty||string.IsNullOrEmpty(prior))throw new Exception("Stop Play and save the current scene first.");
            if(!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone,target))throw new Exception("Install build support for "+target);
            scene=EditorSceneManager.OpenScene(CinderCompactSiteBuild.ScenePath);
            opened=true;
            EditorSceneManager.SaveScene(scene,ScenePath);
            UnityEngine.Object.DestroyImmediate(GameObject.Find("Blockout employee"));
            UnityEngine.Object.DestroyImmediate(GameObject.Find("Trial carried parcel"));
            foreach(var renderer in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Renderer>()))
                if(renderer.name.StartsWith("Listener zone"))renderer.enabled=false;
            var offset=CarryMission.CinderReceiptOffset;
            FacilityArt.Place("Receipt",new Vector3(-8.1f,.8f,9)+offset,new Vector3(.75f,1.6f,.65f),180);
            var terminal=new GameObject("Receipt terminal collision");terminal.transform.position=new Vector3(-8.1f,.8f,9)+offset;
            terminal.AddComponent<BoxCollider>().size=new Vector3(.75f,1.6f,.65f);
            var feedback=new GameObject("BAY 04 receipt feedback");feedback.transform.position=offset;feedback.AddComponent<ReceiptFeedback>();
            CinderSitePropsBuild.Legend("BAY 04\nRECEPTION",14.9f,2.05f,12.5f,0,feedback.transform);
            CinderSitePropsBuild.Legend("SHIP\nTERMINAL",-20.7f,2.9f,-31.25f,180,feedback.transform);
            new GameObject("Cinder four-player map test").AddComponent<CarryRoom>().cinderReview=true;
            EditorSceneManager.SaveScene(scene);
            var directory=Path.GetFullPath("../builds/CinderFourPlayer");Directory.CreateDirectory(directory);
            string output=Path.Combine(directory,target==BuildTarget.StandaloneOSX?"NoReturns.app":"NoReturns.exe");
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=output,target=target,options=BuildOptions.None,extraScriptingDefines=new[]{"CARRY_TEST_AUTOMATION"}});
            receipt.output=output;receipt.errors=report.summary.totalErrors;receipt.warnings=report.summary.totalWarnings;
            if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new Exception("Cinder four-player build failed: "+report.summary.result);
            receipt.status="PASS";
            Debug.Log("CINDER FOUR-PLAYER BUILD PASS: "+output);
        } catch(Exception e) {receipt.error=e.Message;throw;}
        finally {
            if(opened)EditorSceneManager.OpenScene(prior);
            string receipts=Path.GetFullPath("../artifacts/cinder-four-player");Directory.CreateDirectory(receipts);
            File.WriteAllText(Path.Combine(receipts,"build.json"),JsonUtility.ToJson(receipt,true));
        }
    }
}
}
