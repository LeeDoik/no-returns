using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace NoReturns.Editor {
    public static class ProjectBootstrap {
        public const string ScenePath = "Assets/_NoReturns/Scenes/Bootstrap.unity";
        [MenuItem("NO RETURNS/Configure Project Foundation")]
        public static void Configure() {
            foreach(var folder in new[]{"Scenes","Runtime","Editor","Art","Audio","Data","Prefabs","Tests"}) Directory.CreateDirectory("Assets/_NoReturns/"+folder);
            AssetDatabase.Refresh();
            PlayerSettings.productName = "NO RETURNS";
            PlayerSettings.companyName = "NO RETURNS";
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.defaultScreenWidth = 1280; PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            EditorSettings.serializationMode = SerializationMode.ForceText;
            if(!File.Exists(ScenePath)) {
                var scene=EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects,NewSceneMode.Single);
                Camera.main.clearFlags=CameraClearFlags.SolidColor; Camera.main.backgroundColor=new Color(.025f,.03f,.045f);
                EditorSceneManager.SaveScene(scene,ScenePath);
            }
            else EditorSceneManager.OpenScene(ScenePath);
            AssetDatabase.DeleteAsset("Assets/Scenes/SampleScene.unity");
            AssetDatabase.DeleteAsset("Assets/TutorialInfo");
            AssetDatabase.DeleteAsset("Assets/Readme.asset");
            EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};
            AssetDatabase.SaveAssets();Validate();
        }
        public static void Validate() {
            if(PlayerSettings.productName!="NO RETURNS")throw new Exception("Product name mismatch");
            if(!File.Exists(ScenePath))throw new Exception("Missing Bootstrap scene");
            if(EditorBuildSettings.scenes.Length!=1||EditorBuildSettings.scenes[0].path!=ScenePath)throw new Exception("Unexpected build scenes");
            if(Directory.Exists("Assets/_Project")||File.Exists("../project.godot"))throw new Exception("Old game survived in active project");
            if(!File.ReadAllText("Packages/manifest.json").Contains("com.unity.pipeline"))throw new Exception("MCP package missing");
            Debug.Log("SPACE-01 FOUNDATION PASS: new project, scene, product identity, MCP package, no legacy game.");
        }
    }
}
