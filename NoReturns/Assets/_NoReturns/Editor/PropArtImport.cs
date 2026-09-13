using System;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace NoReturns.Editor {
public static class PropArtImport {
 public static void Prepare(){
  const string src="Assets/_NoReturns/Art/PSXProps01/Selected/";const string dst="Assets/_NoReturns/Resources/PSXKit01/";
  Directory.CreateDirectory(dst);AssetDatabase.Refresh();
  foreach(string name in new[]{"Parcel","Receipt","Beacon","Baton"}){
   string texPath=src+"NR_"+name+"_BaseColor_512.png";
   var ti=(TextureImporter)AssetImporter.GetAtPath(texPath);ti.filterMode=FilterMode.Point;ti.mipmapEnabled=true;ti.maxTextureSize=512;ti.textureCompression=TextureImporterCompression.Uncompressed;ti.SaveAndReimport();
   var material=AssetDatabase.LoadAssetAtPath<Material>(dst+name+".mat");
   if(!material){material=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,dst+name+".mat");}
   material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(texPath));material.SetColor("_BaseColor",Color.white);material.SetFloat("_Smoothness",0);material.SetFloat("_Metallic",0);
   var source=AssetDatabase.LoadAssetAtPath<GameObject>(src+"NR_"+name+"_Selected.fbx");if(!source)throw new Exception("Missing model "+name);
   var g=new GameObject(name);var model=UnityEngine.Object.Instantiate(source);model.transform.SetParent(g.transform,false);
   foreach(var r in g.GetComponentsInChildren<Renderer>()){
    var slots=r.sharedMaterials;
    for(int i=0;i<slots.Length;i++){
     if(slots[i]&&slots[i].name.Contains("RepairedRear")){
      string path=dst+"ParcelRear.mat";var rear=AssetDatabase.LoadAssetAtPath<Material>(path);
      if(!rear){rear=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(rear,path);}
      rear.SetColor("_BaseColor",new Color(.43f,.35f,.24f));rear.SetFloat("_Smoothness",0);slots[i]=rear;
     }else slots[i]=material;
    }r.sharedMaterials=slots;
   }
   if(g.GetComponentsInChildren<Collider>().Length!=0)throw new Exception("Visual source has collider "+name);
   PrefabUtility.SaveAsPrefabAsset(g,dst+name+".prefab");UnityEngine.Object.DestroyImmediate(g);
  }
  const string ui="Assets/_NoReturns/Resources/ReceiptUI/";
  foreach(var file in Directory.GetFiles(ui,"*.png")){var importer=(TextureImporter)AssetImporter.GetAtPath(file.Replace('\\','/'));importer.filterMode=FilterMode.Point;importer.mipmapEnabled=true;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();}
  var screen=AssetDatabase.LoadAssetAtPath<Material>(ui+"Screen.mat");if(!screen){screen=new Material(Shader.Find("NoReturns/ReceiptCRT"));AssetDatabase.CreateAsset(screen,ui+"Screen.mat");}
  screen.SetTexture("_ScreenMap",AssetDatabase.LoadAssetAtPath<Texture2D>(ui+"standby-ko.png"));
  AssetDatabase.SaveAssets();Debug.Log("PROP_IMPORT_PASS 4");
 }
}
}
