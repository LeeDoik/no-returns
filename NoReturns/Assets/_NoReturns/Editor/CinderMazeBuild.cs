using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NoReturns.Editor {
public static class CinderMazeBuild {
    public const string ScenePath = "Assets/_NoReturns/Scenes/CinderMazeReview.unity";
    public const float Clearance = 3f, Pitch = 3.3f;
    static readonly string[] Names = {"A WAREHOUSE", "SIDE OFFICE", "BAY 04", "C SERVICE", "STORAGE"};
    // Each letter opens a 3m side passage. B opens both sides, making a loop.
    static readonly string[] Gaps = {"RLBRL", "LBR", "RLBR", "LRBLR", "LRLBRLR"};
    static string Output => Path.GetFullPath("../art/cinder-kit-01");
    static Bounds Floor(int i) => GameObject.Find("Cinder Depot editable primitive blockout").transform.Find(Names[i]).Find(Names[i] + " floor").GetComponent<Renderer>().bounds;
    static bool Rotate(int i) => i == 1 || i == 4;
    static Vector3 Point(int i, float across, float along, float y = .035f) => Rotate(i) ? new Vector3(along, y, across) : new Vector3(across, y, along);
    static (float low, float high, float[] walls, float[] rows) Layout(int i) {
        var b = Floor(i); bool rotate = Rotate(i);
        float low = (rotate ? b.min.z : b.min.x) + .15f, high = (rotate ? b.max.z : b.max.x) - .15f;
        float center = rotate ? b.center.x : b.center.z;
        var walls = Enumerable.Range(0, Gaps[i].Length).Select(n => center + (n - (Gaps[i].Length - 1) / 2f) * Pitch).ToArray();
        var rows = new float[walls.Length + 1];
        for (int n = 0; n < rows.Length; n++) {
            float a = n == 0 ? (rotate ? b.min.x : b.min.z) + .15f : walls[n - 1] + .15f;
            float z = n == walls.Length ? (rotate ? b.max.x : b.max.z) - .15f : walls[n] - .15f;
            rows[n] = (a + z) / 2;
        }
        return (low, high, walls, rows);
    }
    public static List<(string name, Vector3 a, Vector3 b)> Routes() {
        var routes = new List<(string, Vector3, Vector3)>();
        for (int i = 0; i < Names.Length; i++) {
            var l = Layout(i); var floor = Floor(i);
            for (int row = 0; row < l.rows.Length; row++)
                routes.Add((Names[i] + " row " + row, Point(i, l.low + 1.5f, l.rows[row]), Point(i, l.high - 1.5f, l.rows[row])));
            for (int wall = 0; wall < l.walls.Length; wall++) foreach (char side in new[] {'L', 'R'})
                if (Gaps[i][wall] == side || Gaps[i][wall] == 'B') {
                    float across = side == 'L' ? l.low + 1.5f : l.high - 1.5f;
                    routes.Add((Names[i] + " turn " + wall + side, Point(i, across, l.rows[wall]), Point(i, across, l.rows[wall + 1])));
                }
            foreach (bool north in new[] {false, true}) {
                float z = north ? floor.max.z : floor.min.z;
                var outside = new Vector3(floor.center.x, .035f, z + (north ? 3 : -3));
                var inside = new Vector3(floor.center.x, .035f, z + (north ? -1.65f : 1.65f));
                routes.Add((Names[i] + " door " + north, outside, inside));
                int row = Rotate(i) ? l.rows.Length / 2 - 1 : north ? l.rows.Length - 1 : 0;
                routes.Add((Names[i] + " vestibule " + north, inside, Point(i, Rotate(i) ? inside.z : floor.center.x, l.rows[row])));
            }
        }
        return routes;
    }
    public static List<Vector3> PosePoints() {
        var points = new List<Vector3>();
        var rack = GameObject.Find("Cinder entrance presentation").GetComponentInChildren<BoxCollider>().bounds;
        rack.Expand(new Vector3(.8f, 0, .8f));
        foreach (var r in Routes()) {
            points.Add(r.a); points.Add(r.b); points.Add((r.a + r.b) / 2);
        }
        // Sample four shoulder positions at every inside turn, including tight diagonal cargo views.
        for (int i = 0; i < Names.Length; i++) {
            var l = Layout(i);
            foreach (float along in l.rows) foreach (float across in new[] {l.low + 1.5f, l.high - 1.5f})
                foreach (float da in new[] {-.95f, .95f}) foreach (float dz in new[] {-.95f, .95f}) {
                    var point = Point(i, across + da, along + dz);
                    // Approach the retained rack from the aisle, never place the employee inside it.
                    if (i == 0 && rack.Contains(point + Vector3.up * .9f)) point.x = rack.max.x + .02f;
                    points.Add(point);
                }
        }
        return points.Distinct().ToList();
    }
    [MenuItem("NO RETURNS/Trials/Create Cinder Maze Review")]
    public static void Create() {
        if (EditorApplication.isPlaying || EditorSceneManager.GetActiveScene().isDirty) throw new Exception("Stop Play and save the scene first.");
        if (File.Exists(ScenePath)) throw new Exception("Maze review exists; preserve it before regenerating.");
        var scene = EditorSceneManager.OpenScene(CinderMapAppearanceBuild.ScenePath);
        EditorSceneManager.SaveScene(scene, ScenePath);
        var root = new GameObject("Cinder compact maze partitions").transform;
        for (int i = 0; i < Names.Length; i++) {
            var assembly = new GameObject(Names[i]).transform; assembly.SetParent(root);
            var l = Layout(i);
            for (int wall = 0; wall < l.walls.Length; wall++) {
                char gap = Gaps[i][wall];
                float a = l.low + (gap == 'L' || gap == 'B' ? Clearance : 0);
                float b = l.high - (gap == 'R' || gap == 'B' ? Clearance : 0);
                var partition = new GameObject("Partition " + wall + " / gap " + gap).transform; partition.SetParent(assembly);
                partition.position = Point(i, (a + b) / 2, l.walls[wall], 0);
                partition.rotation = Quaternion.Euler(0, Rotate(i) ? 90 : 0, 0);
                var collider = partition.gameObject.AddComponent<BoxCollider>(); collider.center = Vector3.up * 2; collider.size = new Vector3(b - a, 4, .3f);
                float remaining = b - a, offset = -(b - a) / 2;
                while (remaining > .001f) {
                    float size = Mathf.Min(1.2f, remaining);
                    string part = size > 1.199f ? "Wall_A" : "Wall_Fill" + Mathf.RoundToInt(size * 100).ToString("000");
                    string folder = part == "Wall_Fill030" ? "CinderMapFills01" : "CinderAppearance01";
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_NoReturns/Prefabs/" + folder + "/NR_Cinder_" + part + ".prefab");
                    if (!prefab) throw new Exception("Missing exact-width wall: " + part);
                    var obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab); obj.transform.SetParent(partition, false);
                    obj.transform.localPosition = new Vector3(offset + size / 2, 0, 0);
                    offset += size; remaining -= size;
                }
            }
        }
        Physics.SyncTransforms(); Validate(); EditorSceneManager.SaveScene(scene); AssetDatabase.SaveAssets();
    }
    [MenuItem("NO RETURNS/Trials/Validate Cinder Maze Review")]
    public static void Validate() {
        if (EditorSceneManager.GetActiveScene().path != ScenePath) throw new Exception("Open the maze scene.");
        var blockout = GameObject.Find("Cinder Depot editable primitive blockout").transform;
        if (CinderMapAppearanceBuild.ColliderState(blockout) != File.ReadAllText(Path.Combine(Output, "map-collision-baseline.json")).TrimEnd())
            throw new Exception("Original exterior/building/ship-blockout collision changed.");
        var maze = GameObject.Find("Cinder compact maze partitions");
        if (maze.GetComponentsInChildren<BoxCollider>().Length != Gaps.Sum(g => g.Length)) throw new Exception("Partition count changed.");
        foreach (Transform building in maze.transform) foreach (Transform wall in building) {
            var collider = wall.GetComponent<BoxCollider>(); var bounds = wall.GetComponentInChildren<Renderer>().bounds;
            foreach (var r in wall.GetComponentsInChildren<Renderer>()) bounds.Encapsulate(r.bounds);
            if (Vector3.Distance(bounds.min, collider.bounds.min) > .001f || Vector3.Distance(bounds.max, collider.bounds.max) > .001f)
                throw new Exception("Visual/collision mismatch: " + wall.name);
            foreach (var filter in wall.GetComponentsInChildren<MeshFilter>())
                if (Vector3.Distance(filter.transform.lossyScale, Vector3.one) > .0001f) throw new Exception("Stretched wall");
        }
        var parcel = GameObject.Find("Trial carried parcel").GetComponent<Collider>(); bool enabled = parcel.enabled; parcel.enabled = false;
        var results = new List<string>(); var probes = new List<CharacterController>();
        CharacterController Probe() {
            var obj = new GameObject("Maze passage probe"); var cc = obj.AddComponent<CharacterController>();
            cc.height = 1.8f; cc.radius = .34f; cc.center = Vector3.up * .9f; cc.skinWidth = .035f; cc.stepOffset = .32f; probes.Add(cc); return cc;
        }
        void Place(CharacterController cc, Vector3 p) { cc.enabled = false; cc.transform.position = p; cc.enabled = true; Physics.SyncTransforms(); }
        void Traverse(CharacterController cc, Vector3 end, string label) {
            for (int step = 0; step < 2500 && Vector2.Distance(new Vector2(cc.transform.position.x, cc.transform.position.z), new Vector2(end.x, end.z)) > .04f; step++) {
                Vector3 delta = end - cc.transform.position; delta.y = 0; cc.Move(Vector3.ClampMagnitude(delta, .08f) + Vector3.down * .04f);
            }
            var error = cc.transform.position - end; error.y = 0;
            if (error.magnitude > .08f) throw new Exception("Blocked: " + label + " error " + error.magnitude);
        }
        int fourLanes = 0;
        try {
            var single = Probe();
            foreach (var route in Routes()) foreach (bool reverse in new[] {false, true}) {
                Place(single, reverse ? route.b : route.a); Traverse(single, reverse ? route.a : route.b, route.name);
                results.Add("PASS " + route.name + (reverse ? " reverse" : " forward"));
            }
            UnityEngine.Object.DestroyImmediate(single.gameObject); probes.Clear();
            var crew = Enumerable.Range(0, 4).Select(_ => Probe()).ToArray();
            for (int i = 0; i < Names.Length; i++) {
                var l = Layout(i);
                for (int row = 1; row < l.rows.Length - 1; row++) {
                    for (int member = 0; member < 4; member++) Place(crew[member], Point(i, l.low + 1.5f, l.rows[row] + (member - 1.5f) * .71f));
                    for (int step = 0; step < Mathf.CeilToInt((l.high - l.low - 3) / .08f); step++)
                        for (int member = 0; member < 4; member++) {
                            Vector3 target = Point(i, l.high - 1.5f, l.rows[row] + (member - 1.5f) * .71f), delta = target - crew[member].transform.position; delta.y = 0;
                            crew[member].Move(Vector3.ClampMagnitude(delta, .08f) + Vector3.down * .04f);
                        }
                    for (int member = 0; member < 4; member++) {
                        Vector3 expected = Point(i, l.high - 1.5f, l.rows[row] + (member - 1.5f) * .71f), error = crew[member].transform.position - expected; error.y = 0;
                        if (error.magnitude > .08f) throw new Exception("Four-person lane blocked: " + Names[i] + " row " + row);
                    }
                    // Measure the opposed wall faces for this actual lane, not just the pitch constant.
                    var walls = maze.transform.Find(Names[i]).GetComponentsInChildren<BoxCollider>().OrderBy(c => Rotate(i) ? c.bounds.center.x : c.bounds.center.z).ToArray();
                    float width = Rotate(i) ? walls[row].bounds.min.x - walls[row - 1].bounds.max.x : walls[row].bounds.min.z - walls[row - 1].bounds.max.z;
                    if (Mathf.Abs(width - Clearance) > .001f) throw new Exception("Lane width changed");
                    fourLanes++; results.Add("PASS four bodies / " + Names[i] + " row " + row + " / clear " + width.ToString("F3") + "m");
                }
            }
        } finally { foreach (var p in probes) if (p) UnityEngine.Object.DestroyImmediate(p.gameObject); parcel.enabled = enabled; }
        File.WriteAllLines(Path.Combine(Output, "maze-passage-validation.txt"), results);
        var receipt = new {scene = ScenePath, partition_colliders = Gaps.Sum(g => g.Length), wall_modules = maze.GetComponentsInChildren<MeshFilter>().Length,
            triangles = maze.GetComponentsInChildren<MeshFilter>().Sum(m => m.sharedMesh.triangles.Length / 3), standard_clear_width_m = Clearance,
            door_clear_width_m = 3.2f, movement_segments = Routes().Count * 2, four_body_lanes = fourLanes, body_diameter_m = .68f,
            four_body_span_m = 2.81f, five_body_minimum_span_m = 3.4f, original_map_scene_preserved = true, human_four_player_review = false,
            buildings = Names.Select((name, i) => new {name, partitions = Gaps[i].Length, passage_gaps = Gaps[i], axis = Rotate(i) ? "X" : "Z"}),
            method = "Native CharacterController.Move both directions; four simultaneous controllers in every 3m internal straight lane. Human carrying/passing feel remains unreviewed."};
        File.WriteAllText(Path.Combine(Output, "maze-unity-validation.json"), Newtonsoft.Json.JsonConvert.SerializeObject(receipt, Newtonsoft.Json.Formatting.Indented) + "\n");
        Debug.Log("CINDER MAZE PASS: " + results.Count + " route/crew checks");
    }
}
}
