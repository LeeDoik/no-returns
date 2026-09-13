using System;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace NoReturns.Editor {
public static class FacilityArtImport {
 public static void Prepare(){
  const string src="Assets/_NoReturns/Art/PSXKit01/Selected/";const string dst="Assets/_NoReturns/Resources/PSXKit01/";
  Directory.CreateDirectory(dst);AssetDatabase.Refresh();
  foreach(string name in new[]{"Wall","Corner","Door","Floor","Lamp","Rack"}){
   string texPath=src+"NR_"+name+"_BaseColor_512.png";
   var ti=(TextureImporter)AssetImporter.GetAtPath(texPath);ti.filterMode=FilterMode.Point;ti.mipmapEnabled=true;ti.maxTextureSize=512;ti.textureCompression=TextureImporterCompression.Uncompressed;ti.SaveAndReimport();
   var material=AssetDatabase.LoadAssetAtPath<Material>(dst+name+".mat");
   if(!material){material=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,dst+name+".mat");}
   material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(texPath));material.SetColor("_BaseColor",Color.white);material.SetFloat("_Smoothness",0);material.SetFloat("_Metallic",0);
   var source=AssetDatabase.LoadAssetAtPath<GameObject>(src+"NR_"+name+"_Selected.fbx");if(!source)throw new Exception("Missing model "+name);
   var g=new GameObject(name);var model=UnityEngine.Object.Instantiate(source);model.transform.SetParent(g.transform,false);
   foreach(var r in g.GetComponentsInChildren<Renderer>())r.sharedMaterial=material;
   if(g.GetComponentsInChildren<Collider>().Length!=0)throw new Exception("Visual source has collider "+name);
   PrefabUtility.SaveAsPrefabAsset(g,dst+name+".prefab");UnityEngine.Object.DestroyImmediate(g);
  }
  AssetDatabase.SaveAssets();Debug.Log("FACILITY_IMPORT_PASS 6");
 }
}
}
