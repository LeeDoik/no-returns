using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Native framebuffer check: serialized mesh validation alone misses stale render buffers.
public static class CinderBackgroundRenderCheck {
    public static object Main() {
        if(EditorApplication.isPlaying||!GameObject.Find(NoReturns.Editor.CinderBackgroundBuild.RootName))
            throw new InvalidOperationException("Open the background site and stop Play first.");
        var terrain=GameObject.Find("Barren basin").GetComponent<Renderer>();bool enabled=terrain.enabled;
        var lightData=UnityEngine.Object.FindObjectsByType<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(FindObjectsInactive.Include);
        var obj=new GameObject("Temporary background render check");var camera=obj.AddComponent<Camera>();
        camera.transform.SetPositionAndRotation(new Vector3(-31,17,-41),Quaternion.Euler(23,35,0));
        camera.fieldOfView=80;camera.farClipPlane=250;camera.clearFlags=CameraClearFlags.Skybox;
        var target=new RenderTexture(640,360,24);var pixels=new Texture2D(640,360,TextureFormat.RGB24,false);
        var old=RenderTexture.active;int changed=0;
        try {
            Color32[] before=null;
            for(int mode=0;mode<2;mode++) {
                terrain.enabled=mode==0;camera.targetTexture=target;camera.Render();RenderTexture.active=target;
                pixels.ReadPixels(new Rect(0,0,640,360),0,0);pixels.Apply();var current=pixels.GetPixels32();
                if(mode==0)before=current;
                else for(int i=0;i<current.Length;i++)if(!before[i].Equals(current[i]))changed++;
            }
            if(changed<10000)throw new Exception("Terrain is not visible in the native framebuffer: "+changed+" changed pixels.");
            var result=new {scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,width=640,height=360,
                terrain_visibility_changed_pixels=changed,minimum_changed_pixels=10000,renderers_restored=true,
                method="Actual native Camera.Render with terrain enabled/disabled; no image library"};
            File.WriteAllText(Path.GetFullPath("../art/cinder-kit-01/"+NoReturns.Editor.CinderBackgroundBuild.EvidencePrefix+"render-validation.json"),Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");
            return result;
        } finally {
            terrain.enabled=enabled;camera.targetTexture=null;RenderTexture.active=old;target.Release();
            UnityEngine.Object.DestroyImmediate(target);UnityEngine.Object.DestroyImmediate(pixels);UnityEngine.Object.DestroyImmediate(obj);
            foreach(var data in UnityEngine.Object.FindObjectsByType<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(FindObjectsInactive.Include).Except(lightData))
                UnityEngine.Object.DestroyImmediate(data);
        }
    }
}
