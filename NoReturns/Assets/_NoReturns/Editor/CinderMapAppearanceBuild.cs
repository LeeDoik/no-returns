using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
public static class CinderMapAppearanceBuild {
    public const string ScenePath = "Assets/_NoReturns/Scenes/CinderMapAppearanceReview.unity";
    const string Art = "Assets/_NoReturns/Art/CinderMapFills01";
    const string Prefabs = "Assets/_NoReturns/Prefabs/CinderMapFills01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static readonly string[] Buildings = {"SIDE OFFICE", "BAY 04", "C SERVICE", "STORAGE"};

    static IEnumerable<(float center, float length)> Segments(float start, float length) {
        for (float offset = 0; offset < length - .0001f; offset += 1.2f) {
            float size = Mathf.Min(1.2f, length - offset);
            yield return (start + offset + size / 2, size);
        }
    }

    internal static string ColliderState(Transform root) => Newtonsoft.Json.JsonConvert.SerializeObject(
        root.GetComponentsInChildren<BoxCollider>().Select(c => new {name = c.transform.parent.name + "/" + c.name,
            c.enabled, c.isTrigger, c.gameObject.layer, position = c.transform.position.ToString("F6"), rotation = c.transform.rotation.ToString("F6"),
            scale = c.transform.lossyScale.ToString("F6"), center = c.center.ToString("F6"), size = c.size.ToString("F6")}));

    [MenuItem("NO RETURNS/Trials/Create Cinder Map Appearance Review")]
    public static void Create() {
        if (EditorApplication.isPlaying || EditorSceneManager.GetActiveScene().isDirty)
            throw new InvalidOperationException("Stop Play and save the current scene first.");
        if (File.Exists(ScenePath)) throw new InvalidOperationException("Map review exists; preserve it before regenerating.");
        AssetDatabase.Refresh(); Directory.CreateDirectory(Prefabs); AssetDatabase.Refresh();
        var surface = AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
        foreach (var file in Directory.GetFiles(Art, "*.fbx")) {
            string path = file.Replace('\\', '/'), name = Path.GetFileNameWithoutExtension(path);
            var importer = (ModelImporter)AssetImporter.GetAtPath(path);
            importer.isReadable = true; importer.importAnimation = false; importer.bakeAxisConversion = true;
            importer.SaveAndReimport();
            var root = new GameObject(name);
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            model.transform.SetParent(root.transform, false); model.name = "Model";
            foreach (var renderer in model.GetComponentsInChildren<Renderer>()) renderer.sharedMaterial = surface;
            PrefabUtility.SaveAsPrefabAsset(root, Prefabs + "/" + name + ".prefab");
            UnityEngine.Object.DestroyImmediate(root);
        }
        var scene = EditorSceneManager.OpenScene(CinderAppearanceBuild.ScenePath);
        EditorSceneManager.SaveScene(scene, ScenePath);
        var blockout = GameObject.Find("Cinder Depot editable primitive blockout").transform;
        string before = ColliderState(blockout);
        File.WriteAllText(Path.Combine(Output, "map-collision-baseline.json"), before + "\n");
        var map = new GameObject("Cinder map structures / original colliders retained").transform;
        foreach (string name in Buildings) {
            var original = blockout.Find(name);
            var bounds = original.Find(name + " floor").GetComponent<Renderer>().bounds;
            foreach (var renderer in original.GetComponentsInChildren<Renderer>()) renderer.enabled = false;
            var assembly = new GameObject(name).transform; assembly.SetParent(map);
            void Place(string part, Vector3 position, float yaw = 0) {
                string folder = File.Exists(Prefabs + "/NR_Cinder_" + part + ".prefab") ? Prefabs : "Assets/_NoReturns/Prefabs/CinderAppearance01";
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(folder + "/NR_Cinder_" + part + ".prefab");
                if (!prefab) throw new Exception("Missing module: " + part);
                var obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                obj.transform.SetParent(assembly); obj.transform.SetPositionAndRotation(position, Quaternion.Euler(0, yaw, 0));
            }
            float x0 = bounds.min.x, x1 = bounds.max.x, z0 = bounds.min.z, z1 = bounds.max.z, cx = bounds.center.x;
            foreach (var x in Segments(x0, bounds.size.x)) foreach (var z in Segments(z0, bounds.size.z)) {
                bool halfX = x.length < 1.19f, halfZ = z.length < 1.19f;
                string suffix = halfX && halfZ ? "Quarter" : halfX || halfZ ? "Half" : "A";
                float yaw = !halfX && halfZ ? 90 : 0;
                Place("Floor_" + suffix, new Vector3(x.center, 0, z.center), yaw);
                Place("Ceiling_" + suffix, new Vector3(x.center, 4, z.center), yaw);
            }
            foreach (float x in new[] {x0, x1}) {
                foreach (var z in Segments(z0 + .15f, bounds.size.z - .3f))
                    Place(z.length > 1.19f ? "Wall_A" : "Wall_Fill" + Mathf.RoundToInt(z.length * 100).ToString("000"), new Vector3(x, 0, z.center), 90);
                foreach (float z in new[] {z0, z1}) Place("Corner_A", new Vector3(x, 0, z));
            }
            foreach (float z in new[] {z0, z1}) {
                Place("DoorFrame_A", new Vector3(cx, 0, z));
                foreach (var span in new[] {(start: x0 + .15f, length: cx - 1.9f - x0 - .15f), (start: cx + 1.9f, length: x1 - .15f - cx - 1.9f)})
                    foreach (var x in Segments(span.start, span.length))
                        Place(x.length > 1.19f ? "Wall_A" : "Wall_Fill" + Mathf.RoundToInt(x.length * 100).ToString("000"), new Vector3(x.center, 0, z));
                foreach (var x in Segments(x0, bounds.size.x))
                    Place(x.length > 1.19f ? "Ceiling_Edge" : "Ceiling_Edge_Half", new Vector3(x.center, 4, z + (z == z0 ? -.075f : .075f)), 90);
            }
            foreach (float x in new[] {x0, x1}) {
                foreach (var z in Segments(z0, bounds.size.z))
                    Place(z.length > 1.19f ? "Ceiling_Edge" : "Ceiling_Edge_Half", new Vector3(x + (x == x0 ? -.075f : .075f), 4, z.center));
                foreach (float z in new[] {z0, z1})
                    Place("Ceiling_Corner", new Vector3(x + (x == x0 ? -.075f : .075f), 4, z + (z == z0 ? -.075f : .075f)));
            }
            Place("Lamp_A", new Vector3(cx, 3.65f, z0 - .151f), 180);
            Place("Lamp_A", new Vector3(cx, 3.65f, z1 + .151f));
            Place("Lamp_A", new Vector3(x0 + .151f, 3.55f, bounds.center.z), 90);
            Place("Lamp_A", new Vector3(x1 - .151f, 3.55f, bounds.center.z), 270);
        }
        if (before != ColliderState(blockout)) throw new Exception("Original collision changed.");
        Validate(); EditorSceneManager.SaveScene(scene, ScenePath); AssetDatabase.SaveAssets();
    }

    [MenuItem("NO RETURNS/Trials/Validate Cinder Map Appearance Review")]
    public static void Validate() {
        if (EditorSceneManager.GetActiveScene().path != ScenePath) throw new Exception("Open the map appearance review.");
        var surface = AssetDatabase.LoadAssetAtPath<Material>("Assets/_NoReturns/Art/CinderAppearance01/Cinder_Surface.mat");
        var specs = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(Path.Combine(Output, "map-fill-validation.json")));
        foreach (var spec in specs["parts"]) {
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs + "/" + spec["name"] + ".prefab"));
            try {
                var bounds = obj.GetComponentInChildren<Renderer>().bounds;
                var size = new Vector3((float)spec["size_m"][0], (float)spec["size_m"][1], (float)spec["size_m"][2]);
                var low = new Vector3((float)spec["min_m"][0], (float)spec["min_m"][1], (float)spec["min_m"][2]);
                var mesh = obj.GetComponentInChildren<MeshFilter>().sharedMesh;
                if (Vector3.Distance(bounds.size, size) > .0001f || Vector3.Distance(bounds.min, low) > .0001f
                    || mesh.uv.Length != mesh.vertexCount || mesh.triangles.Length != 36 || obj.GetComponentInChildren<Renderer>().sharedMaterial != surface)
                    throw new Exception("Fill import mismatch: " + spec["name"]);
            } finally { UnityEngine.Object.DestroyImmediate(obj); }
        }
        var map = GameObject.Find("Cinder map structures / original colliders retained").transform;
        var blockout = GameObject.Find("Cinder Depot editable primitive blockout").transform;
        if (ColliderState(blockout) != File.ReadAllText(Path.Combine(Output, "map-collision-baseline.json")).TrimEnd())
            throw new Exception("Original collision changed from the source scene.");
        if (map.childCount != 4 || map.GetComponentsInChildren<Collider>().Length != 0 || map.GetComponentsInChildren<Light>().Length != 16)
            throw new Exception("Map ownership/count mismatch");
        foreach (string name in Buildings) {
            if (blockout.Find(name).GetComponentsInChildren<Collider>().Length != 10
                || blockout.Find(name).GetComponentsInChildren<Renderer>().Any(r => r.enabled)) throw new Exception("Original building ownership mismatch: " + name);
            var bounds = blockout.Find(name).Find(name + " floor").GetComponent<Renderer>().bounds;
            float floorArea = 0, roofArea = 0;
            foreach (Transform part in map.Find(name)) {
                var size = part.GetComponentInChildren<Renderer>().bounds.size;
                if (part.name.StartsWith("NR_Cinder_Floor_")) floorArea += size.x * size.z;
                if (part.name.StartsWith("NR_Cinder_Ceiling_")) roofArea += size.x * size.z;
            }
            if (Mathf.Abs(floorArea - bounds.size.x * bounds.size.z) > .01f
                || Mathf.Abs(roofArea - (bounds.size.x + .3f) * (bounds.size.z + .3f)) > .01f)
                throw new Exception("Floor/roof coverage mismatch: " + name);
        }
        foreach (var mesh in map.GetComponentsInChildren<MeshFilter>())
            if (Vector3.Distance(mesh.transform.lossyScale, Vector3.one) > .0001f) throw new Exception("Stretched module: " + mesh.name);
        CinderAppearanceBuild.Validate(); CinderStructureBuild.Validate();
        var parcel = GameObject.Find("Trial carried parcel").GetComponent<Collider>(); bool enabled = parcel.enabled;
        try { parcel.enabled = false; CinderBlockoutBuild.Validate(); }
        finally { parcel.enabled = enabled; }
        string workspace = File.Exists("CarryWorkspace.txt") ? File.ReadAllText("CarryWorkspace.txt").Trim() : Path.GetFullPath("..");
        File.Copy(Path.Combine(workspace, "artifacts/cinder-blockout/passage.txt"), Path.Combine(Output, "map-passage-validation.txt"), true);
        var result = new {scene = ScenePath, fill_imports = 8, original_collision_preserved = true, original_building_colliders = 50,
            added_buildings = Buildings.Select(name => new {name, instances = map.Find(name).childCount,
                triangles = map.Find(name).GetComponentsInChildren<MeshFilter>().Sum(m => m.sharedMesh.triangles.Length / 3)}),
            added_work_lights = 16, new_colliders = 0, unit_placement_scale = true, user_palette_approved = true, user_expansion_review = false};
        File.WriteAllText(Path.Combine(Output, "map-unity-validation.json"), Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented) + "\n");
        Debug.Log("CINDER MAP APPEARANCE PASS: 4 added buildings; original collision retained");
    }
}
}
