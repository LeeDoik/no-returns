using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace NoReturns.Editor {
public static class CinderSkyBuild {
    public const string MaterialPath = "Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Sky.mat";
    const string CubePath = "Assets/_NoReturns/Art/CinderCompactSite01/Cinder_Dusk_Cube.asset";
    [MenuItem("NO RETURNS/Trials/Apply Cinder Dusk Sky")]
    public static void ApplyMenu() => Apply();
    public static object Apply() {
        var scene = EditorSceneManager.GetActiveScene();
        if (EditorApplication.isPlaying || scene.isDirty || scene.path != NoReturns.Editor.CinderCompactSiteBuild.ScenePath)
            throw new InvalidOperationException("Open the compact site, stop Play and save changes first.");
        var sun = GameObject.Find("Blockout daylight").GetComponent<Light>();
        var shader = Shader.Find("Skybox/Cubemap");
        if (!shader || !shader.isSupported || sun.type != LightType.Directional)
            throw new InvalidOperationException("Supported native sky and existing directional light are required.");
        string collision = Collision();
        string localLights = LocalLights();
        int poses = NoReturns.Editor.CinderSitePropsBuild.PosePoints().Count;
        var sky = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
        if (!sky) { sky = new Material(shader) { name = "Cinder dusk sky" }; AssetDatabase.CreateAsset(sky, MaterialPath); }
        sky.shader = shader; sky.shaderKeywords = new string[0];
        sky.SetTexture("_Tex", Atmosphere());
        sky.SetColor("_Tint", new Color(.5f, .5f, .5f));
        sky.SetFloat("_Exposure", 1); sky.SetFloat("_Rotation", 0);
        EditorUtility.SetDirty(sky);
        RenderSettings.skybox = sky; RenderSettings.sun = sun;
        sun.color = new Color(.84f, .76f, .91f); sun.intensity = .55f;
        sun.transform.rotation = Quaternion.Euler(24, -30, 0);
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.45f, .40f, .45f);
        RenderSettings.fog = true; RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = new Color(.34f, .27f, .38f);
        RenderSettings.fogStartDistance = 35; RenderSettings.fogEndDistance = 115;
        DynamicGI.UpdateEnvironment();
        if (Collision() != collision || LocalLights() != localLights || NoReturns.Editor.CinderSitePropsBuild.PosePoints().Count != poses)
            throw new Exception("Sky setup changed collision, local lights or occupiable prop poses.");
        if (RenderSettings.skybox != sky || !RenderSettings.fog || RenderSettings.fogEndDistance <= RenderSettings.fogStartDistance)
            throw new Exception("Invalid sky or fog settings.");
        var result = new {
            scene = scene.path, material = MaterialPath, shader = shader.name, shader_supported = shader.isSupported,
            cubemap = CubePath, cubemap_face_size = 64, cubemap_faces = 6, exposure = sky.GetFloat("_Exposure"),
            directional_intensity = sun.intensity, directional_color = Rgba(sun.color), directional_rotation = sun.transform.eulerAngles.ToString("F3"),
            ambient_mode = RenderSettings.ambientMode.ToString(), ambient_color = Rgba(RenderSettings.ambientLight),
            fog_mode = RenderSettings.fogMode.ToString(), fog_color = Rgba(RenderSettings.fogColor),
            fog_start_m = RenderSettings.fogStartDistance, fog_end_m = RenderSettings.fogEndDistance,
            collision_preserved = true, local_lights_preserved = true, occupiable_prop_positions = poses,
            geometry_changed = false, runtime_changed = false, dynamic_weather = false, user_sky_quality_review = false
        };
        AssetDatabase.SaveAssets(); EditorSceneManager.SaveScene(scene);
        File.WriteAllText(Path.GetFullPath("../art/cinder-kit-01/sky-settings-validation.json"),
            Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented) + "\n");
        return result;
    }
    static string Collision() => Newtonsoft.Json.JsonConvert.SerializeObject(EditorSceneManager.GetActiveScene().GetRootGameObjects()
        .Select(g => new { g.name, state = NoReturns.Editor.CinderMapAppearanceBuild.ColliderState(g.transform) }));
    static float[] Rgba(Color c) => new[] { c.r, c.g, c.b, c.a };
    static Cubemap Atmosphere() {
        var cube = AssetDatabase.LoadAssetAtPath<Cubemap>(CubePath);
        bool create = !cube;
        if (create) cube = new Cubemap(64, TextureFormat.RGBA32, false) { name = "Cinder dusk atmosphere", filterMode = FilterMode.Bilinear, wrapMode = TextureWrapMode.Clamp };
        for (int face = 0; face < 6; face++) {
            var pixels = new Color[64 * 64];
            for (int y = 0; y < 64; y++) for (int x = 0; x < 64; x++) {
                float u = (x + .5f) / 32 - 1, v = (y + .5f) / 32 - 1;
                var d = (face == 0 ? new Vector3(1, -v, -u) : face == 1 ? new Vector3(-1, -v, u) :
                    face == 2 ? new Vector3(u, 1, v) : face == 3 ? new Vector3(u, -1, -v) :
                    face == 4 ? new Vector3(u, -v, 1) : new Vector3(-u, -v, -1)).normalized;
                var horizon = new Color(.34f, .27f, .38f);
                var color = Color.Lerp(horizon, d.y > 0 ? new Color(.12f, .09f, .18f) : new Color(.24f, .19f, .29f), Mathf.Pow(Mathf.Abs(d.y), .6f));
                // Direction-space noise stays continuous across cube faces; this is a static sky, not weather simulation.
                float cloud = (Mathf.PerlinNoise(d.x * 2.4f + 10, d.z * 2.4f + 20) + Mathf.PerlinNoise(d.y * 4 + 30, d.x * 4 + 40)) * .5f;
                color *= 1 - Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.35f, .8f, cloud)) * .45f * Mathf.Clamp01(d.y * 5);
                pixels[y * 64 + x] = color;
            }
            cube.SetPixels(pixels, (CubemapFace)face);
        }
        cube.Apply(false, false);
        if (create) AssetDatabase.CreateAsset(cube, CubePath); else EditorUtility.SetDirty(cube);
        return cube;
    }
    static string LocalLights() => Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Object.FindObjectsByType<Light>()
        .Where(l => l.type != LightType.Directional).OrderBy(l => l.GetEntityId().ToString())
        .Select(l => new { l.name, l.enabled, l.intensity, color = Rgba(l.color), l.range, l.shadows, l.cullingMask, position = l.transform.position.ToString("F6"), rotation = l.transform.rotation.ToString("F6") }));
}
}
