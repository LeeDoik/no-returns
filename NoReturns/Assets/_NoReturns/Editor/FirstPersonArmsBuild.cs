using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace NoReturns.Editor {
public static class FirstPersonArmsBuild {
    public const string PrefabPath=EmployeeAnimationBuild.Folder+"/FirstPersonArms.prefab";
    [Serializable] class Receipt {public string status="FAIL",error;public int sourceTriangles,leftTriangles,rightTriangles,leftVertices,rightVertices;}
    [MenuItem("NO RETURNS/Art/Prepare Local First Person Arms")]
    public static void Prepare(){
        var receipt=new Receipt();GameObject model=null;
        try{
            EmployeeAnimationBuild.ValidateAssets();
            model=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EmployeeAnimationBuild.PrefabPath));model.name="FirstPersonArms";
            var animator=model.GetComponent<Animator>();var source=model.GetComponentInChildren<SkinnedMeshRenderer>();
            if(source.gameObject==model||source.GetComponentsInChildren<Transform>().Length>1)throw new Exception("Arm extraction requires a separate mesh leaf");
            receipt.sourceTriangles=source.sharedMesh.triangles.Length/3;
            foreach(bool left in new[]{true,false}){
                var upper=animator.GetBoneTransform(left?HumanBodyBones.LeftUpperArm:HumanBodyBones.RightUpperArm);
                var allowed=new HashSet<Transform>(upper.GetComponentsInChildren<Transform>());
                var mesh=Extract(source.sharedMesh,source.bones,allowed);
                string path=EmployeeAnimationBuild.Folder+(left?"/FirstPersonLeft.asset":"/FirstPersonRight.asset");
                var existing=AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if(existing){EditorUtility.CopySerialized(mesh,existing);UnityEngine.Object.DestroyImmediate(mesh);mesh=existing;EditorUtility.SetDirty(existing);}else AssetDatabase.CreateAsset(mesh,path);
                var go=new GameObject(left?"First person left arm":"First person right arm");go.transform.SetParent(source.transform.parent,false);
                go.transform.localPosition=source.transform.localPosition;go.transform.localRotation=source.transform.localRotation;go.transform.localScale=source.transform.localScale;
                var arm=go.AddComponent<SkinnedMeshRenderer>();arm.sharedMesh=mesh;arm.bones=source.bones;arm.rootBone=source.rootBone;arm.sharedMaterials=source.sharedMaterials;arm.localBounds=source.localBounds;arm.updateWhenOffscreen=true;
                if(left){receipt.leftTriangles=mesh.triangles.Length/3;receipt.leftVertices=mesh.vertexCount;}else{receipt.rightTriangles=mesh.triangles.Length/3;receipt.rightVertices=mesh.vertexCount;}
            }
            UnityEngine.Object.DestroyImmediate(source.gameObject);
            PrefabUtility.SaveAsPrefabAsset(model,PrefabPath);AssetDatabase.SaveAssets();receipt.status="PASS";
        }catch(Exception e){receipt.error=e.ToString();throw;}finally{
            if(model)UnityEngine.Object.DestroyImmediate(model);var folder=Path.GetFullPath("../artifacts/first-person-arms");Directory.CreateDirectory(folder);File.WriteAllText(Path.Combine(folder,"prepare.json"),JsonUtility.ToJson(receipt,true));
        }
    }
    static Mesh Extract(Mesh source,Transform[] bones,HashSet<Transform> allowed){
        var weights=source.boneWeights;var triangles=source.triangles;
        var permitted=new bool[source.vertexCount];
        for(int i=0;i<weights.Length;i++){
            var w=weights[i];float sum=0;
            if(allowed.Contains(bones[w.boneIndex0]))sum+=w.weight0;if(allowed.Contains(bones[w.boneIndex1]))sum+=w.weight1;
            if(allowed.Contains(bones[w.boneIndex2]))sum+=w.weight2;if(allowed.Contains(bones[w.boneIndex3]))sum+=w.weight3;
            permitted[i]=sum>=.6f;
        }
        var indices=new List<int>();for(int i=0;i<triangles.Length;i+=3)if(permitted[triangles[i]]&&permitted[triangles[i+1]]&&permitted[triangles[i+2]]){indices.Add(triangles[i]);indices.Add(triangles[i+1]);indices.Add(triangles[i+2]);}
        if(indices.Count<300)throw new Exception("Missing hand/sleeve source triangles");
        var used=indices.Distinct().ToArray();var map=used.Select((v,i)=>(v,i)).ToDictionary(x=>x.v,x=>x.i);
        var vertices=source.vertices;var normals=source.normals;var uv=source.uv;var tangents=source.tangents;
        var mesh=new Mesh{name="Employee first-person arm"};mesh.vertices=used.Select(i=>vertices[i]).ToArray();mesh.normals=used.Select(i=>normals[i]).ToArray();mesh.uv=used.Select(i=>uv[i]).ToArray();
        if(tangents.Length==source.vertexCount)mesh.tangents=used.Select(i=>tangents[i]).ToArray();
        mesh.boneWeights=used.Select(i=>weights[i]).ToArray();mesh.bindposes=source.bindposes;mesh.triangles=indices.Select(i=>map[i]).ToArray();mesh.RecalculateBounds();return mesh;
    }
    public static void Validate(){
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
        if(!prefab||prefab.GetComponentsInChildren<SkinnedMeshRenderer>().Length!=2||prefab.GetComponentsInChildren<Collider>().Length!=0)throw new Exception("Prepare Local First Person Arms before building.");
    }
}
}
