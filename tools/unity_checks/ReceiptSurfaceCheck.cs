using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using NoReturns.CarryLab;
public static class ReceiptSurfaceCheck {
    public static object Run(){
        if(EditorApplication.isPlaying)throw new Exception("Stop Play first");
        var old=SceneManager.GetActiveScene();
        var data=UnityEngine.Object.FindObjectsByType<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(FindObjectsInactive.Include);
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        var target=new RenderTexture(480,320,24);var pixels=new Texture2D(480,320,TextureFormat.RGB24,false);
        var material=new Material(Resources.Load<Material>("ReceiptUI/Screen"));var active=RenderTexture.active;Camera camera=null;
        var rows=new System.Collections.Generic.List<object>();
        try{
            SceneManager.SetActiveScene(scene);
            camera=new GameObject("Receipt test camera").AddComponent<Camera>();camera.cullingMask=1<<31;camera.fieldOfView=35;camera.farClipPlane=20;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=Color.black;camera.targetTexture=target;
            var light=new GameObject("Receipt test light").AddComponent<Light>();light.type=LightType.Directional;light.cullingMask=1<<31;light.transform.rotation=Quaternion.Euler(40,-25,0);
            var terminal=FacilityArt.Place("Receipt",new Vector3(-8.1f,.8f,9),new Vector3(.75f,1.6f,.65f),180);
            var terminalPosition=terminal.transform.position;
            foreach(var r in terminal.GetComponentsInChildren<Renderer>()){r.gameObject.layer=31;material.SetTexture("_BaseMap",r.sharedMaterial.GetTexture("_BaseMap"));r.sharedMaterial=material;}
            var wall=GameObject.CreatePrimitive(PrimitiveType.Cube);wall.layer=31;wall.transform.localScale=new Vector3(2,2,.1f);wall.GetComponent<Renderer>().sharedMaterial=Resources.Load<Material>("CarrySurface");
            foreach(bool cinder in new[]{false,true}){
                var offset=cinder?CarryMission.CinderReceiptOffset:Vector3.zero;
                terminal.transform.position=terminalPosition+offset;material.SetVector("_ScreenOffset",offset);
                foreach(string view in new[]{"front","rear","wall"}){
                    camera.transform.position=new Vector3(-8.1f,1.25f,view=="rear"?10.5f:7.5f)+offset;
                    camera.transform.LookAt(new Vector3(-8.1f,1.25f,9)+offset);
                    wall.transform.position=new Vector3(-8.1f,1.25f,8.15f)+offset;wall.SetActive(view=="wall");
                    Color32[] before=null;int changed=0;
                    foreach(string state in new[]{"place-ko","take-ko"}){
                        material.SetTexture("_ScreenMap",Resources.Load<Texture2D>("ReceiptUI/"+state));camera.Render();RenderTexture.active=target;
                        pixels.ReadPixels(new Rect(0,0,480,320),0,0);pixels.Apply();var current=pixels.GetPixels32();
                        if(before==null)before=current;else changed=current.Where((p,i)=>!p.Equals(before[i])).Count();
                        var output=Path.GetFullPath("../artifacts/cinder-four-player/crt-"+cinder+"-"+view+"-"+state+".png");Directory.CreateDirectory(Path.GetDirectoryName(output));File.WriteAllBytes(output,pixels.EncodeToPNG());
                    }
                    if(view=="front"?changed<50:changed!=0)throw new Exception("CRT mask/occlusion failed: "+cinder+" "+view+" "+changed);
                    rows.Add(new {cinder,view,changed});
                }
            }
            var result=new {status="PASS",width=480,height=320,rows};
            var path=Path.GetFullPath("../artifacts/cinder-four-player/receipt-surface.json");Directory.CreateDirectory(Path.GetDirectoryName(path));File.WriteAllText(path,Newtonsoft.Json.JsonConvert.SerializeObject(result,Newtonsoft.Json.Formatting.Indented)+"\n");return result;
        }finally{
            if(camera)camera.targetTexture=null;RenderTexture.active=active;target.Release();UnityEngine.Object.DestroyImmediate(target);UnityEngine.Object.DestroyImmediate(pixels);UnityEngine.Object.DestroyImmediate(material);
            EditorSceneManager.CloseScene(scene,true);SceneManager.SetActiveScene(old);
            foreach(var d in UnityEngine.Object.FindObjectsByType<UnityEngine.Rendering.Universal.UniversalAdditionalLightData>(FindObjectsInactive.Include).Except(data))UnityEngine.Object.DestroyImmediate(d);
        }
    }
}
