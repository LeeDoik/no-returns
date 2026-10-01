using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using NoReturns.Trials;

namespace NoReturns.Editor {
public static class CinderStructureBuild {
    public const string ScenePath = "Assets/_NoReturns/Scenes/CinderStructureReview.unity";
    const string Art = "Assets/_NoReturns/Art/CinderKit01";
    const string Prefabs = "Assets/_NoReturns/Prefabs/CinderKit01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static readonly Dictionary<string, GameObject> parts = new Dictionary<string, GameObject>();
    static Transform assembly;

    static void Place(string name, Vector3 position, float yaw = 0) {
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(parts["NR_Cinder_" + name]);
        instance.transform.SetParent(assembly);
        instance.transform.SetPositionAndRotation(position, Quaternion.Euler(0, yaw, 0));
    }

    [MenuItem("NO RETURNS/Trials/Create Cinder Structure Review")]
    public static void Create() {
        if (EditorApplication.isPlaying || EditorSceneManager.GetActiveScene().isDirty)
            throw new InvalidOperationException("Stop Play and save the current scene before creating the review.");
        Directory.CreateDirectory(Prefabs);
        var materialPath = Art + "/Cinder_Structure_Gray.mat";
        var gray = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        if (!gray) {
            gray = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            gray.color = new Color(.48f, .48f, .48f);
            AssetDatabase.CreateAsset(gray, materialPath);
        }
        parts.Clear();
        foreach (var file in Directory.GetFiles(Art, "*.fbx")) {
            var path = file.Replace('\\', '/');
            var importer = (ModelImporter)AssetImporter.GetAtPath(path);
            importer.isReadable = true;
            importer.importAnimation = false;
            importer.bakeAxisConversion = true;
            importer.SaveAndReimport();
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            var name = Path.GetFileNameWithoutExtension(path);
            var root = new GameObject(name);
            model.transform.SetParent(root.transform, false);
            model.name = "Model";
            foreach (var renderer in model.GetComponentsInChildren<Renderer>()) renderer.sharedMaterial = gray;
            parts[name] = PrefabUtility.SaveAsPrefabAsset(root, Prefabs + "/" + name + ".prefab");
            UnityEngine.Object.DestroyImmediate(root);
        }
        var scene = EditorSceneManager.OpenScene("Assets/_NoReturns/Scenes/CinderDepotBlockout.unity");
        EditorSceneManager.SaveScene(scene, ScenePath);
        var warehouse = GameObject.Find("Cinder Depot editable primitive blockout").transform.Find("A WAREHOUSE");
        foreach (var renderer in warehouse.GetComponentsInChildren<Renderer>()) renderer.enabled = false;
        assembly = new GameObject("Cinder gray warehouse / original colliders retained").transform;
        for (int x = 0; x < 12; x++) for (int z = 0; z < 17; z++) {
            var p = new Vector3(-25.2f + x * 1.2f, 0, -7.8f + z * 1.2f);
            Place("Floor_A", p);
            Place("Ceiling_A", p + Vector3.up * 4);
        }
        foreach (float x in new[] { -25.8f, -11.4f }) {
            for (int z = 0; z < 16; z++) Place("Wall_A", new Vector3(x, 0, -7.65f + z * 1.2f), 90);
            Place("Wall_Fill090", new Vector3(x, 0, 11.4f), 90);
            foreach (float z in new[] { -8.4f, 12f }) Place("Corner_A", new Vector3(x, 0, z));
        }
        foreach (float z in new[] { -8.4f, 12f }) {
            Place("DoorFrame_A", new Vector3(-18.6f, 0, z));
            foreach (int side in new[] { -1, 1 }) {
                for (int i = 0; i < 4; i++) Place("Wall_A", new Vector3(-18.6f + side * (6.45f - i * 1.2f), 0, z));
                Place("Wall_Fill035", new Vector3(-18.6f + side * 2.075f, 0, z));
            }
        }
        foreach (float x in new[] { -25.875f, -11.325f }) {
            for (int z = 0; z < 17; z++) Place("Ceiling_Edge", new Vector3(x, 4, -7.8f + z * 1.2f));
            foreach (float z in new[] { -8.475f, 12.075f }) Place("Ceiling_Corner", new Vector3(x, 4, z));
        }
        foreach (float z in new[] { -8.475f, 12.075f })
            for (int x = 0; x < 12; x++) Place("Ceiling_Edge", new Vector3(-25.2f + x * 1.2f, 4, z), 90);
        var employee = GameObject.Find("Blockout employee");
        employee.transform.SetPositionAndRotation(new Vector3(-18.6f, .06f, -11.4f), Quaternion.identity);
        var parcel = GameObject.Find("Trial carried parcel");
        parcel.transform.SetParent(null, true);
        parcel.transform.SetPositionAndRotation(new Vector3(-18.1f, .35f, -10.6f), Quaternion.identity);
        parcel.transform.localScale = new Vector3(.8f, .65f, .65f);
        Validate();
        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("NO RETURNS/Trials/Validate Cinder Structure Review")]
    public static void Validate() {
        bool map = EditorSceneManager.GetActiveScene().path == CinderMapAppearanceBuild.ScenePath;
        bool appearance = map || EditorSceneManager.GetActiveScene().path == CinderAppearanceBuild.ScenePath;
        if (!appearance && EditorSceneManager.GetActiveScene().path != ScenePath) throw new Exception("Open a Cinder structure/appearance review scene.");
        var results = new List<string>();
        foreach (var path in Directory.GetFiles(Prefabs, "*.prefab")) {
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            var bounds = obj.GetComponentInChildren<Renderer>().bounds;
            var name = Path.GetFileNameWithoutExtension(path);
            var expected = name.Contains("Floor") ? new Vector3(1.2f, .2f, 1.2f)
                : name.Contains("DoorFrame") ? new Vector3(3.8f, 4, .3f)
                : name.Contains("Fill035") ? new Vector3(.35f, 4, .3f)
                : name.Contains("Fill090") ? new Vector3(.9f, 4, .3f)
                : name.Contains("Wall") ? new Vector3(1.2f, 4, .3f)
                : name.Contains("Ceiling_Corner") ? new Vector3(.15f, .3f, .15f)
                : name.Contains("Ceiling_Edge") ? new Vector3(.15f, .3f, 1.2f)
                : name.Contains("Ceiling") ? new Vector3(1.2f, .3f, 1.2f)
                : name.Contains("Beam") ? new Vector3(1.2f, .3f, .3f)
                : name.Contains("Corner") ? new Vector3(.3f, 4, .3f) : new Vector3(.15f, 4, .3f);
            bool valid = Vector3.Distance(bounds.size, expected) < .0001f
                && Mathf.Abs(name.Contains("Floor") ? bounds.max.y : bounds.min.y) < .0001f;
            foreach (var mesh in obj.GetComponentsInChildren<MeshFilter>()) valid &= mesh.sharedMesh.uv.Length == mesh.sharedMesh.vertexCount;
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
                valid &= renderer.sharedMaterials.Length == 1 && renderer.sharedMaterial
                    && Vector3.Distance(renderer.transform.lossyScale, Vector3.one) < .0001f;
            if (!valid) throw new Exception(name + " imported size/pivot/UV mismatch: " + bounds);
            results.Add(name + " size=" + bounds.size.ToString("F3") + " pivot/UV PASS");
            UnityEngine.Object.DestroyImmediate(obj);
        }
        Physics.SyncTransforms();
        var probe = new GameObject("Structure passage probe");
        probe.layer = 2;
        var cc = probe.AddComponent<CharacterController>();
        cc.height = 1.8f; cc.radius = .34f; cc.center = Vector3.up * .9f; cc.skinWidth = .035f; cc.stepOffset = .32f;
        var parcelCollider = GameObject.Find("Trial carried parcel").GetComponent<Collider>();
        bool parcelEnabled = parcelCollider.enabled;
        parcelCollider.enabled = false; // Carrying disables this collider in CinderBlockoutWalk.
        try {
            foreach (float lane in new[] { -1.1f, 0, 1.1f }) foreach (int direction in new[] { -1, 1 }) {
                cc.enabled = false; probe.transform.position = new Vector3(-18.6f + lane, .06f, direction == 1 ? -11.4f : -2.4f); cc.enabled = true;
                for (int i = 0; i < 150; i++) cc.Move(new Vector3(0, -.02f, .06f * direction));
                if (Mathf.Abs(probe.transform.position.z - (direction == 1 ? -2.4f : -11.4f)) > .1f)
                    throw new Exception("Passage blocked: lane=" + lane + " direction=" + direction + " end=" + probe.transform.position);
                results.Add("passage lane=" + lane + " direction=" + direction + " PASS");
            }
            foreach (float z in new[] { -8.4f, -7.8f, -2.4f }) {
                cc.enabled = false; probe.transform.position = new Vector3(-18.6f, .06f, z); cc.enabled = true;
                for (int i = 0; i < 20; i++) cc.Move(Vector3.down * .02f);
                float start = probe.transform.position.y, peak = start, speed = CinderBlockoutWalk.JumpSpeed;
                for (int i = 0; i < 100; i++) {
                    speed -= CinderBlockoutWalk.Gravity * .02f;
                    var flags = cc.Move(Vector3.up * speed * .02f);
                    if ((flags & CollisionFlags.Above) != 0) throw new Exception("Jump hit ceiling");
                    peak = Mathf.Max(peak, probe.transform.position.y);
                    if (i > 5 && cc.isGrounded) break;
                }
                if (peak - start < .6f) throw new Exception("Jump failed");
                results.Add("jump z=" + z + " PASS");
            }
            foreach (float z in new[] { -9f, -8.4f, -7.8f, -2.4f }) foreach (float yaw in new[] { 0f, 90f, 180f, 270f }) foreach (float pitch in new[] { -80f, 0, 80f }) {
                var eye = new Vector3(-18.6f, 1.605f, z);
                var rotation = Quaternion.Euler(pitch, yaw, 0);
                var position = TrialCargoPose.Position(eye, rotation, (BoxCollider)parcelCollider);
                if (Physics.CheckBox(position, new Vector3(.4f, .325f, .325f), rotation, 1 << 0, QueryTriggerInteraction.Ignore)) throw new Exception("Cargo overlap: " + z + "/" + yaw + "/" + pitch);
            }
            results.Add("cargo center lane: 48 yaw/pitch/position samples PASS (shared runtime pose)");
        } finally { parcelCollider.enabled = parcelEnabled; UnityEngine.Object.DestroyImmediate(probe); }
        File.WriteAllLines(Path.Combine(Output, map ? "map-structure-validation.txt" : appearance ? "production-structure-validation.txt" : "unity-validation.txt"), results);
        Debug.Log("CINDER STRUCTURE PASS: " + results.Count + " results");
    }
}
}
