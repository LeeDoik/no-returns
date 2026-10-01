using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoReturns.Trials;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
public static class CinderAppearanceBuild {
    public const string ScenePath = "Assets/_NoReturns/Scenes/CinderAppearanceReview.unity";
    const string Art = "Assets/_NoReturns/Art/CinderAppearance01";
    const string Prefabs = "Assets/_NoReturns/Prefabs/CinderAppearance01";
    static string Output => Path.GetFullPath("../art/cinder-kit-01");

    static Material Material(string name, Texture texture) {
        string path = Art + "/" + name + ".mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (!material) {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            AssetDatabase.CreateAsset(material, path);
        }
        material.SetColor("_BaseColor", Color.white);
        material.SetTexture("_BaseMap", texture);
        material.SetFloat("_Metallic", 0); material.SetFloat("_Smoothness", .05f);
        EditorUtility.SetDirty(material);
        return material;
    }

    [MenuItem("NO RETURNS/Trials/Create Cinder Appearance Review")]
    public static void CreateMenu() => Create();

    public static void Create(bool replaceExisting = false) {
        if (EditorApplication.isPlaying || EditorSceneManager.GetActiveScene().isDirty)
            throw new InvalidOperationException("Stop Play and save the current scene first.");
        if (File.Exists(ScenePath) && !replaceExisting)
            throw new InvalidOperationException("Appearance review already exists. Preserve manual edits in a separate scene before Create(true).");
        AssetDatabase.Refresh();
        Directory.CreateDirectory(Prefabs); AssetDatabase.Refresh();
        foreach (var file in Directory.GetFiles(Art, "*.png")) {
            var importer = (TextureImporter)AssetImporter.GetAtPath(file.Replace('\\', '/'));
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true; importer.filterMode = FilterMode.Point;
            importer.mipmapEnabled = true; importer.wrapMode = TextureWrapMode.Clamp;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.maxTextureSize = 512; importer.SaveAndReimport();
        }
        var surface = Material("Cinder_Surface", AssetDatabase.LoadAssetAtPath<Texture2D>(Art + "/Cinder_Surface_Atlas.png"));
        var sign = Material("Cinder_Sign", AssetDatabase.LoadAssetAtPath<Texture2D>(Art + "/Cinder_Warehouse_Sign.png"));
        var emission = Material("Cinder_Lamp_Emission", null);
        emission.SetColor("_BaseColor", new Color(1, .7f, .25f));
        emission.SetColor("_EmissionColor", new Color(2, 1.05f, .24f));
        emission.EnableKeyword("_EMISSION");
        var parts = new Dictionary<string, GameObject>();
        foreach (var file in Directory.GetFiles(Art, "*.fbx")) {
            string path = file.Replace('\\', '/'), name = Path.GetFileNameWithoutExtension(path);
            var importer = (ModelImporter)AssetImporter.GetAtPath(path);
            importer.isReadable = true; importer.importAnimation = false; importer.bakeAxisConversion = true;
            importer.SaveAndReimport();
            var model = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            var root = new GameObject(name); model.transform.SetParent(root.transform, false); model.name = "Model";
            foreach (var renderer in model.GetComponentsInChildren<Renderer>()) {
                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                    materials[i] = materials[i].name.Contains("Emission") ? emission : materials[i].name.Contains("Sign") ? sign : surface;
                renderer.sharedMaterials = materials;
            }
            if (name.Contains("Lamp")) {
                var lamp = new GameObject("Warm work light"); lamp.transform.SetParent(root.transform, false);
                lamp.transform.localPosition = new Vector3(0, 0, .28f);
                var light = lamp.AddComponent<Light>(); light.type = LightType.Point;
                light.color = new Color(1, .67f, .3f); light.intensity = .65f; light.range = 5;
                light.shadows = LightShadows.None;
            }
            if (name.Contains("Rack")) {
                // ponytail: reserve the storage volume; individual shelf collisions when rack interaction is added.
                var collider = root.AddComponent<BoxCollider>(); collider.size = new Vector3(2.4f, 2.4f, .6f);
                collider.center = Vector3.up * 1.2f;
            }
            parts[name] = PrefabUtility.SaveAsPrefabAsset(root, Prefabs + "/" + name + ".prefab");
            UnityEngine.Object.DestroyImmediate(root);
        }
        var scene = EditorSceneManager.OpenScene(CinderStructureBuild.ScenePath);
        EditorSceneManager.SaveScene(scene, ScenePath);
        var assembly = GameObject.Find("Cinder gray warehouse / original colliders retained").transform;
        var oldParts = new List<Transform>(); foreach (Transform child in assembly) oldParts.Add(child);
        foreach (var old in oldParts) {
            var source = PrefabUtility.GetCorrespondingObjectFromSource(old.gameObject);
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(parts[source.name]);
            instance.transform.SetParent(assembly);
            instance.transform.SetPositionAndRotation(old.position, old.rotation);
            UnityEngine.Object.DestroyImmediate(old.gameObject);
        }
        assembly.name = "Cinder appearance warehouse / original colliders retained";
        var props = new GameObject("Cinder entrance presentation").transform;
        Action<string, Vector3, float> place = (name, position, yaw) => {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(parts["NR_Cinder_" + name]);
            instance.transform.SetParent(props); instance.transform.SetPositionAndRotation(position, Quaternion.Euler(0, yaw, 0));
        };
        place("Lamp_A", new Vector3(-18.6f, 3.65f, -8.551f), 180);
        foreach (float z in new[] {-7.0f, -3.4f}) {
            place("Lamp_A", new Vector3(-25.649f, 3.55f, z), 90);
            place("Lamp_A", new Vector3(-11.551f, 3.55f, z), 270);
        }
        place("Sign_A", new Vector3(-21.2f, 2.3f, -8.551f), 180);
        place("Rack_A", new Vector3(-25.2f, 0, -5.4f), 90);
        foreach (var label in UnityEngine.Object.FindObjectsByType<TextMesh>())
            label.GetComponent<Renderer>().enabled = false; // Hide blockout labels that render through finished reference walls.
        var sunlight = GameObject.Find("Blockout daylight").GetComponent<Light>();
        sunlight.intensity = .9f; sunlight.color = new Color(1, .94f, .83f); sunlight.shadows = LightShadows.Hard;
        RenderSettings.ambientLight = new Color(.55f, .53f, .48f);
        DynamicGI.UpdateEnvironment();
        Validate(); EditorSceneManager.SaveScene(scene, ScenePath); AssetDatabase.SaveAssets();
    }

    [MenuItem("NO RETURNS/Trials/Validate Cinder Appearance Review")]
    public static void Validate() {
        if (EditorSceneManager.GetActiveScene().path != ScenePath) throw new Exception("Open the appearance review scene.");
        foreach (string name in new[] {"Cinder_Surface_Atlas", "Cinder_Warehouse_Sign"}) {
            string path = Art + "/" + name + ".png";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            int width = name.Contains("Atlas") ? 512 : 256, height = name.Contains("Atlas") ? 512 : 128;
            if (!texture || texture.width != width || texture.height != height || texture.filterMode != FilterMode.Point
                || !importer.mipmapEnabled || !importer.sRGBTexture || importer.textureCompression != TextureImporterCompression.Uncompressed)
                throw new Exception(name + " texture import mismatch");
        }
        var specs = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(Path.Combine(Output, "production-validation.json")));
        var checks = new List<object>();
        foreach (var spec in specs["parts"]) {
            string name = (string)spec["name"];
            var obj = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs + "/" + name + ".prefab"));
            try {
                var bounds = obj.GetComponentInChildren<Renderer>().bounds;
                var expected = new Vector3((float)spec["size_m"][0], (float)spec["size_m"][1], (float)spec["size_m"][2]);
                var low = new Vector3((float)spec["min_m"][0], (float)spec["min_m"][1], (float)spec["min_m"][2]);
                if (Vector3.Distance(bounds.size, expected) > .0001f || Vector3.Distance(bounds.min, low) > .0001f)
                    throw new Exception(name + " imported bounds/pivot mismatch: " + bounds);
                int triangles = 0;
                foreach (var mesh in obj.GetComponentsInChildren<MeshFilter>()) {
                    if (mesh.sharedMesh.uv.Length != mesh.sharedMesh.vertexCount) throw new Exception(name + " missing UVs");
                    triangles += mesh.sharedMesh.triangles.Length / 3;
                }
                if (triangles != (int)spec["triangles"]) throw new Exception(name + " triangle mismatch");
                foreach (var renderer in obj.GetComponentsInChildren<Renderer>()) {
                    if (renderer.sharedMaterials.Length != spec["materials"].Count()) throw new Exception(name + " material slot mismatch");
                    if (Vector3.Distance(renderer.transform.lossyScale, Vector3.one) > .0001f) throw new Exception(name + " non-unit scale");
                    foreach (var material in renderer.sharedMaterials)
                        if (!material || (material != AssetDatabase.LoadAssetAtPath<Material>(Art + "/Cinder_Lamp_Emission.mat") && !material.GetTexture("_BaseMap")))
                            throw new Exception(name + " missing material/texture");
                }
                checks.Add(new {name, triangles, size_m = new[] {bounds.size.x, bounds.size.y, bounds.size.z}, passed = true});
            } finally { UnityEngine.Object.DestroyImmediate(obj); }
        }
        var warehouse = GameObject.Find("Cinder Depot editable primitive blockout").transform.Find("A WAREHOUSE");
        var assembly = GameObject.Find("Cinder appearance warehouse / original colliders retained").transform;
        var props = GameObject.Find("Cinder entrance presentation").transform;
        if (warehouse.GetComponentsInChildren<Collider>().Length != 10 || assembly.childCount != 530
            || assembly.GetComponentsInChildren<Collider>().Length != 0 || props.GetComponentsInChildren<Collider>().Length != 1
            || props.GetComponentsInChildren<Light>().Length != 5) throw new Exception("Scene ownership/count mismatch");
        foreach (var renderer in warehouse.GetComponentsInChildren<Renderer>()) if (renderer.enabled) throw new Exception("Duplicate warehouse renderer");
        var result = new {scene = ScenePath, imported_parts = checks, structural_instances = assembly.childCount,
            original_colliders = 10, new_rack_colliders = 1, work_lights = 5, presentation_instances = props.childCount,
            structural_triangles = assembly.GetComponentsInChildren<MeshFilter>().Sum(m => m.sharedMesh.triangles.Length / 3),
            presentation_triangles = props.GetComponentsInChildren<MeshFilter>().Sum(m => m.sharedMesh.triangles.Length / 3),
            atlas_px = new[] {512, 512}, sign_px = new[] {256, 128}, filtering = "Point + mipmaps, uncompressed sRGB",
            user_concept_approved = true, user_final_appearance_review = false};
        File.WriteAllText(Path.Combine(Output, "production-unity-validation.json"), Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented) + "\n");
        Debug.Log("CINDER APPEARANCE PASS: 14 imported parts; 530 structures; 7 props; original collision retained");
    }
}
}
