using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NoReturns.Trials;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Run through Unity CLI run_script in a Cinder structure/appearance/map/maze review Play. Stop Play afterward.
public static class CinderCarryEdgeCheck {
    public static async Task<object> Main() {
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        bool site = scene == "CinderCompactSiteReview";
        bool props = site && GameObject.Find(NoReturns.Editor.CinderSitePropsBuild.RootName);
        bool maze = scene == "CinderMazeReview";
        bool map = site || maze || scene == "CinderMapAppearanceReview";
        bool appearance = map || scene == "CinderAppearanceReview";
        if (!EditorApplication.isPlaying || (!appearance && scene != "CinderStructureReview"))
            throw new InvalidOperationException("Open a Cinder structure/appearance review and enter Play first.");
        string prefix = props ? "props-" : site ? "site-" : maze ? "maze-" : map ? "map-" : appearance ? "production-" : "";
        var walk = UnityEngine.Object.FindAnyObjectByType<CinderBlockoutWalk>();
        var body = walk.GetComponent<CharacterController>();
        var parcel = GameObject.Find("Trial carried parcel");
        int parcelLayer = parcel.layer;
        var flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var update = typeof(CinderBlockoutWalk).GetMethod("Update", flags);
        walk.enabled = false;
        InputSystem.ResetDevice(Keyboard.current);
        InputSystem.ResetDevice(Mouse.current);
        Cursor.lockState = CursorLockMode.Locked;
        typeof(CinderBlockoutWalk).GetField("carrying", flags).SetValue(walk, true);
        parcel.GetComponent<Rigidbody>().isKinematic = true;
        parcel.GetComponent<Collider>().enabled = false;
        var points = new List<Vector3>();
        foreach (float x in new[] {-19.7f, -18.6f, -17.5f})
            foreach (float z in new[] {-9f, -8.4f, -7.8f, -2.4f}) points.Add(new Vector3(x, .035f, z));
        points.Add(new Vector3(-25.275f, .035f, -2.4f));
        points.Add(new Vector3(-11.925f, .035f, -2.4f));
        if (appearance) { // Approach the new reserved rack volume from the aisle and both ends.
            points.Add(new Vector3(-24.5f, .035f, -5.4f));
            points.Add(new Vector3(-24.52f, .035f, -5.4f));
            points.Add(new Vector3(-25.275f, .035f, -3.825f));
            points.Add(new Vector3(-25.275f, .035f, -6.975f));
        }
        var buildings = new[] {"SIDE OFFICE", "BAY 04", "C SERVICE", "STORAGE"};
        if (map) foreach (string name in buildings) {
            var floor = GameObject.Find("Cinder Depot editable primitive blockout").transform.Find(name).Find(name + " floor").GetComponent<Renderer>().bounds;
            foreach (float z in new[] {floor.min.z, floor.max.z}) foreach (float x in new[] {-1.1f, 0f, 1.1f})
                foreach (float offset in new[] {-.6f, 0f, .6f}) points.Add(new Vector3(floor.center.x + x, .035f, z + offset));
            foreach (float x in new[] {floor.min.x + .525f, floor.max.x - .525f})
                points.Add(new Vector3(x, .035f, floor.center.z));
        }
        if (maze) points = NoReturns.Editor.CinderMazeBuild.PosePoints();
        if (site) points = NoReturns.Editor.CinderCompactSiteBuild.PosePoints();
        if (props) points = NoReturns.Editor.CinderSitePropsBuild.PosePoints();
        var failures = new List<object>();
        int samples = 0, touching_contacts = 0, carrying_segments = 0;
        if (maze || site) foreach (var point in points) {
            var center = point + body.center;
            float capOffset = body.height / 2 - body.radius;
            if (Physics.CheckCapsule(center - Vector3.up * capOffset, center + Vector3.up * capOffset, body.radius, 1 << 0, QueryTriggerInteraction.Ignore))
                throw new Exception("Cargo sample is not a free body pose: " + point);
        }
        void InspectCargo(Vector3 point, float yaw, float pitch) {
            var overlaps = Physics.OverlapBox(parcel.transform.position, new Vector3(.4f,.325f,.325f),
                    parcel.transform.rotation, 1 << 0, QueryTriggerInteraction.Ignore);
            samples++;
            var shape = parcel.GetComponent<BoxCollider>();
            shape.enabled = true; // Native penetration checks require the enabled query shape.
            try {
                foreach (var obstacle in overlaps) {
                    if (Physics.ComputePenetration(shape, parcel.transform.position, parcel.transform.rotation,
                        obstacle, obstacle.transform.position, obstacle.transform.rotation, out var direction, out var depth) && depth > .00001f)
                        failures.Add(new {point=new[]{point.x,point.y,point.z},yaw,pitch,depth,
                            cargo=new[]{parcel.transform.position.x,parcel.transform.position.y,parcel.transform.position.z},collider=obstacle.name});
                    else touching_contacts++; // OverlapBox includes touching bounds; these are not penetrating volumes.
                }
            } finally { shape.enabled = false; }
        }
        foreach (var point in points) foreach (float yaw in new[] {0f,45f,90f,135f,180f,225f,270f,315f})
            foreach (float pitch in new[] {-80f,0f,80f}) {
                body.enabled = false; walk.transform.position = point; body.enabled = true;
                typeof(CinderBlockoutWalk).GetField("yaw", flags).SetValue(walk, yaw);
                typeof(CinderBlockoutWalk).GetField("pitch", flags).SetValue(walk, pitch);
                typeof(CinderBlockoutWalk).GetField("vertical", flags).SetValue(walk, 0f);
                Physics.SyncTransforms();
                update.Invoke(walk, null); // Exercise the existing carrying implementation, not a copied pose formula.
                Physics.SyncTransforms();
                if (parcel.GetComponent<Collider>().enabled || parcel.layer != parcelLayer)
                    throw new Exception("Carry query changed parcel collider/layer state.");
                InspectCargo(point, yaw, pitch);
            }
        bool pickup = false, forward = false, backward = false, drop = false, jump = false;
        if (failures.Count == 0) {
            Action<Key[]> keys = pressed => {
                InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(pressed));
                InputSystem.Update();
            };
            var smokeStart = site ? NoReturns.Editor.CinderCompactSiteBuild.Spawn : new Vector3(-18.6f,.035f,-11.4f);
            body.enabled = false; walk.transform.SetPositionAndRotation(smokeStart,Quaternion.identity); body.enabled = true;
            typeof(CinderBlockoutWalk).GetField("yaw",flags).SetValue(walk,site ? 90f : 0f);
            typeof(CinderBlockoutWalk).GetField("pitch",flags).SetValue(walk,0f);
            typeof(CinderBlockoutWalk).GetField("vertical",flags).SetValue(walk,0f);
            typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,false);
            parcel.transform.position = smokeStart + new Vector3(.5f,.315f,.8f);
            parcel.GetComponent<Collider>().enabled = true;
            keys(new[]{Key.E}); update.Invoke(walk,null);
            pickup = (bool)typeof(CinderBlockoutWalk).GetField("carrying",flags).GetValue(walk);
            float travel = maze || site ? 4.8f : 9f;
            int steps = Mathf.CeilToInt(travel / (3f * Time.deltaTime));
            keys(new[]{Key.W}); for(int i=0;i<steps;i++) update.Invoke(walk,null);
            var smokeEnd = smokeStart + (site ? Vector3.right : Vector3.forward) * travel;
            var smokeError = walk.transform.position - smokeEnd; smokeError.y = 0; forward = smokeError.magnitude < .2f;
            keys(new[]{Key.S}); for(int i=0;i<steps;i++) update.Invoke(walk,null);
            smokeError = walk.transform.position - smokeStart; smokeError.y = 0; backward = smokeError.magnitude < .2f;
            keys(new[]{Key.Q}); update.Invoke(walk,null);
            drop = !(bool)typeof(CinderBlockoutWalk).GetField("carrying",flags).GetValue(walk) && parcel.GetComponent<Collider>().enabled;
            float start = walk.transform.position.y, peak = start;
            keys(new[]{Key.Space}); update.Invoke(walk,null); keys(new Key[0]);
            for(int i=0;i<Mathf.CeilToInt(1.1f/Time.deltaTime);i++) {
                update.Invoke(walk,null); peak = Mathf.Max(peak,walk.transform.position.y);
            }
            jump = peak-start > .55f;
            if ((maze || site) && pickup && forward && backward && drop && jump) {
                typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,true);
                parcel.GetComponent<Collider>().enabled=false; parcel.GetComponent<Rigidbody>().isKinematic=true;
                foreach (var route in site ? NoReturns.Editor.CinderCompactSiteBuild.Routes() : NoReturns.Editor.CinderMazeBuild.Routes()) foreach (bool reverse in new[] {false,true}) {
                    var a = reverse ? route.b : route.a; var b = reverse ? route.a : route.b;
                    float yaw = Mathf.Atan2(b.x-a.x,b.z-a.z)*Mathf.Rad2Deg;
                    body.enabled=false; walk.transform.position=a; body.enabled=true;
                    typeof(CinderBlockoutWalk).GetField("yaw",flags).SetValue(walk,yaw);
                    typeof(CinderBlockoutWalk).GetField("pitch",flags).SetValue(walk,0f);
                    typeof(CinderBlockoutWalk).GetField("vertical",flags).SetValue(walk,0f);
                    keys(new[]{Key.W});
                    int ticks = Mathf.CeilToInt(Vector3.Distance(a,b) / (3 * Time.deltaTime));
                    for (int tick=0; tick<ticks; tick++) {
                        update.Invoke(walk,null); Physics.SyncTransforms(); InspectCargo(walk.transform.position,yaw,0);
                    }
                    var error=walk.transform.position-b; error.y=0;
                    if(error.magnitude>.2f) throw new Exception("Actual carrying route blocked: "+route.name);
                    carrying_segments++;
                }
                keys(new Key[0]);
            }
            if (pickup && forward && backward && drop && jump) {
                var camera = walk.GetComponentInChildren<Camera>();
                typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,true);
                parcel.GetComponent<Collider>().enabled = false;
                parcel.GetComponent<Rigidbody>().isKinematic = true;
                var views = new List<(string name, Vector3 point, float yaw, bool carry)> {
                    ("carry-edge-door.png",new Vector3(-19.7f,.035f,-8.4f),45f,true),
                    ("carry-edge-wall.png",new Vector3(-25.275f,.035f,-2.4f),270f,true)
                };
                if (appearance) {
                    views.Add(("entry-empty.png",new Vector3(-18.6f,.035f,-11.4f),0f,false));
                    views.Add(("entry-carry.png",new Vector3(-18.6f,.035f,-11.4f),0f,true));
                    views.Add(("inside-rear-carry.png",new Vector3(-18.6f,.035f,-2.4f),180f,true));
                    views.Add(("rack-empty.png",new Vector3(-22.6f,.035f,-4.0f),240f,false));
                }
                if (site) {
                    views.Clear();
                    views.Add(("landing-empty.png",NoReturns.Editor.CinderCompactSiteBuild.Spawn,180,false));
                    views.Add(("west-covered.png",new Vector3(-22.95f,.035f,-14.1f),0,false));
                    views.Add(("office-link.png",new Vector3(-20.7f,.035f,13.65f),90,false));
                    views.Add(("central-junction.png",new Vector3(-5.25f,.035f,-4.35f),90,false));
                    views.Add(("central-carry.png",new Vector3(4.125f,.035f,2.55f),0,true));
                    views.Add(("north-covered.png",new Vector3(-12,.035f,25.95f),90,false));
                    views.Add(("bay-carry.png",new Vector3(12.75f,.035f,5.85f),0,true));
                    views.Add(("service-loop.png",new Vector3(21.3f,.035f,-6.9f),0,false));
                    views.Add(("overview.png",new Vector3(3,60,-61),0,false));
                    if(props) {
                        views.Add(("warehouse-racks.png",new Vector3(-17.4f,.035f,2.5f),285,false));
                        views.Add(("office-workbenches.png",new Vector3(-14.1f,.035f,19.8f),270,false));
                        views.Add(("bay-dispatch.png",new Vector3(12.75f,.035f,15.9f),90,false));
                        views.Add(("service-equipment.png",new Vector3(13.35f,.035f,-10.2f),270,false));
                        views.Add(("storage-freight.png",new Vector3(12f,.035f,-24.9f),110,false));
                        views.Add(("exterior-machinery.png",new Vector3(30,18,-35),319,false));
                        views.Add(("facility-sign.png",new Vector3(16.2f,.035f,4.5f),0,false));
                    }
                } else if (maze) {
                    views.Clear();
                    views.Add(("entry-empty.png", new Vector3(-18.6f,.035f,-7.1f), 90, false));
                    views.Add(("entry-carry.png", new Vector3(-18.6f,.035f,-7.1f), 90, true));
                    views.Add(("turn-carry.png", new Vector3(-13.05f,.035f,-3.15f), 270, true));
                    views.Add(("junction-empty.png", new Vector3(-18.6f,.035f,.15f), 270, false));
                    views.Add(("office-empty.png", new Vector3(-2.85f,.035f,19.05f), 0, false));
                    views.Add(("bay-04-carry.png", new Vector3(32.7f,.035f,10.125f), 90, true));
                    views.Add(("service-empty.png", new Vector3(26.1f,.035f,-12.75f), 90, false));
                    views.Add(("storage-empty.png", new Vector3(3.15f,.035f,-31.35f), 0, false));
                } else if (map) {
                    foreach (string name in buildings) {
                        var floor = GameObject.Find("Cinder Depot editable primitive blockout").transform.Find(name).Find(name + " floor").GetComponent<Renderer>().bounds;
                        string label = name.ToLowerInvariant().Replace(' ', '-');
                        views.Add((label + "-empty.png", new Vector3(floor.center.x, .035f, floor.min.z - 3), 0, false));
                        views.Add((label + "-carry.png", new Vector3(floor.center.x, .035f, floor.min.z + 2), 0, true));
                    }
                    views.Add(("overview.png", new Vector3(50, 55, -55), 315, false));
                }
                foreach(var view in views) {
                    body.enabled=false;walk.transform.position=view.point;body.enabled=true;
                    typeof(CinderBlockoutWalk).GetField("yaw",flags).SetValue(walk,view.yaw);
                    typeof(CinderBlockoutWalk).GetField("pitch",flags).SetValue(walk,view.name == "overview.png" ? 40f : view.name == "exterior-machinery.png" ? 25f : 0f);
                    typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,view.carry);
                    parcel.GetComponent<Renderer>().enabled=view.carry;
                    update.Invoke(walk,null);
                    Physics.SyncTransforms();
                    await Task.Delay(100); // Let Unity submit changed renderer transforms before the camera capture.
                    var target=new RenderTexture(1280,720,24);
                    var pixels=new Texture2D(1280,720,TextureFormat.RGB24,false);
                    var oldTarget=camera.targetTexture;var oldActive=RenderTexture.active;
                    try {
                        camera.targetTexture=target;camera.Render();RenderTexture.active=target;
                        pixels.ReadPixels(new Rect(0,0,1280,720),0,0);pixels.Apply();
                        File.WriteAllBytes(Path.GetFullPath("../art/cinder-kit-01/"+prefix+view.name),pixels.EncodeToPNG());
                    } finally {
                        camera.targetTexture=oldTarget;RenderTexture.active=oldActive;
                        target.Release();UnityEngine.Object.DestroyImmediate(target);UnityEngine.Object.DestroyImmediate(pixels);
                    }
                }
                if (maze || site) {
                    // Cutaway is an inspection capture only. Restore the native scene renderers afterward.
                    var roofs = new List<Renderer>();
                    foreach (var renderer in UnityEngine.Object.FindObjectsByType<Renderer>())
                        if (renderer.enabled && renderer.name != "Compact yard and utility roofs" && (renderer.name.Contains("roof") || renderer.transform.GetComponentsInParent<Transform>().Any(t => t.name.StartsWith("NR_Cinder_Ceiling_") || t.name == "Roof machinery")))
                            roofs.Add(renderer);
                    MeshFilter compactSurface = site ? GameObject.Find("Compact yard and utility roofs").GetComponent<MeshFilter>() : null;
                    Mesh originalSurface = compactSurface ? compactSurface.sharedMesh : null;
                    Mesh cutawaySurface = null;
                    var oldPosition = camera.transform.position; var oldRotation = camera.transform.rotation; bool ortho = camera.orthographic;
                    float oldSize = camera.orthographicSize; var target = new RenderTexture(1280,1280,24); var pixels = new Texture2D(1280,1280,TextureFormat.RGB24,false);
                    var oldTarget = camera.targetTexture; var oldActive = RenderTexture.active;
                    try {
                        foreach (var roof in roofs) roof.enabled = false;
                        if (compactSurface) {
                            cutawaySurface = UnityEngine.Object.Instantiate(originalSurface);
                            var vertices = cutawaySurface.vertices; var indices = cutawaySurface.triangles; var ground = new List<int>();
                            for(int n=0;n<indices.Length;n+=3) if(vertices[indices[n]].y<.01f) ground.AddRange(new[]{indices[n],indices[n+1],indices[n+2]});
                            cutawaySurface.triangles=ground.ToArray();compactSurface.sharedMesh=cutawaySurface;
                        }
                        parcel.GetComponent<Renderer>().enabled = false;
                        camera.orthographic = true;
                        foreach (string name in site ? new[]{"FIELD"} : new[] {"A WAREHOUSE","SIDE OFFICE","BAY 04","C SERVICE","STORAGE"}) {
                            var field = NoReturns.Editor.CinderCompactSiteBuild.Field;
                            var floor = site ? new Bounds(new Vector3(field.center.x,0,field.center.y),new Vector3(field.width,0,field.height)) : GameObject.Find("Cinder Depot editable primitive blockout").transform.Find(name).Find(name+" floor").GetComponent<Renderer>().bounds;
                            camera.transform.SetPositionAndRotation(new Vector3(floor.center.x,40,floor.center.z), Quaternion.Euler(90,0,0));
                            camera.orthographicSize = Mathf.Max(floor.size.x,floor.size.z)/2+1.8f;
                            await Task.Delay(100); camera.targetTexture = target; camera.Render(); RenderTexture.active = target;
                            pixels.ReadPixels(new Rect(0,0,1280,1280),0,0); pixels.Apply();
                            string label = site ? "field" : name == "A WAREHOUSE" ? "warehouse" : name.ToLowerInvariant().Replace(' ','-');
                            File.WriteAllBytes(Path.GetFullPath("../art/cinder-kit-01/"+prefix+label+"-cutaway.png"), pixels.EncodeToPNG());
                        }
                    } finally {
                        if (compactSurface) compactSurface.sharedMesh=originalSurface;
                        if (cutawaySurface) UnityEngine.Object.DestroyImmediate(cutawaySurface);
                        foreach (var roof in roofs) roof.enabled = true;
                        camera.transform.SetPositionAndRotation(oldPosition,oldRotation); camera.orthographic = ortho; camera.orthographicSize = oldSize;
                        camera.targetTexture = oldTarget; RenderTexture.active = oldActive; target.Release();
                        UnityEngine.Object.DestroyImmediate(target); UnityEngine.Object.DestroyImmediate(pixels);
                    }
                }
                parcel.GetComponent<Renderer>().enabled=true;
            }
        }
        string output = Path.GetFullPath("../art/cinder-kit-01/"+prefix+"carry-edge-validation.json");
        var result = new {scene,static_pose_count=points.Count*24,carrying_segments,samples,overlap_count=failures.Count,touching_contacts,penetration_tolerance_m=.00001f,failures,
            input_checks=new {pickup,forward,backward,drop,jump},
            method="Play mode, API-set poses, reflected actual CinderBlockoutWalk.Update; not human controls"};
        File.WriteAllText(output, Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
        if (failures.Count > 0) throw new Exception("Cargo overlaps in " + failures.Count + " / " + samples + " samples; see " + output);
        if (!(pickup && forward && backward && drop && jump)) throw new Exception("Carry input regression; see " + output);
        return result;
    }
}
