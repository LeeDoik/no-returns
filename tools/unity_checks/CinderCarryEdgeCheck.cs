using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NoReturns.Trials;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Run through Unity CLI run_script in CinderStructureReview Play mode. Stop Play afterward.
public static class CinderCarryEdgeCheck {
    public static object Main() {
        if (!EditorApplication.isPlaying || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "CinderStructureReview")
            throw new InvalidOperationException("Open CinderStructureReview and enter Play mode first.");
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
        var failures = new List<object>();
        int samples = 0;
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
                var overlaps = Physics.OverlapBox(parcel.transform.position, new Vector3(.4f,.325f,.325f),
                    parcel.transform.rotation, 1 << 0, QueryTriggerInteraction.Ignore);
                samples++;
                if (overlaps.Length > 0) failures.Add(new {point=new[]{point.x,point.y,point.z},yaw,pitch,
                    cargo=new[]{parcel.transform.position.x,parcel.transform.position.y,parcel.transform.position.z},collider=overlaps[0].name});
            }
        bool pickup = false, forward = false, backward = false, drop = false, jump = false;
        if (failures.Count == 0) {
            Action<Key[]> keys = pressed => {
                InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(pressed));
                InputSystem.Update();
            };
            body.enabled = false; walk.transform.SetPositionAndRotation(new Vector3(-18.6f,.035f,-11.4f),Quaternion.identity); body.enabled = true;
            typeof(CinderBlockoutWalk).GetField("yaw",flags).SetValue(walk,0f);
            typeof(CinderBlockoutWalk).GetField("pitch",flags).SetValue(walk,0f);
            typeof(CinderBlockoutWalk).GetField("vertical",flags).SetValue(walk,0f);
            typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,false);
            parcel.transform.position = new Vector3(-18.1f,.35f,-10.6f);
            parcel.GetComponent<Collider>().enabled = true;
            keys(new[]{Key.E}); update.Invoke(walk,null);
            pickup = (bool)typeof(CinderBlockoutWalk).GetField("carrying",flags).GetValue(walk);
            int steps = Mathf.CeilToInt(9f / (3f * Time.deltaTime));
            keys(new[]{Key.W}); for(int i=0;i<steps;i++) update.Invoke(walk,null);
            forward = walk.transform.position.z > -2.6f && walk.transform.position.z < -2.2f;
            keys(new[]{Key.S}); for(int i=0;i<steps;i++) update.Invoke(walk,null);
            backward = Mathf.Abs(walk.transform.position.z + 11.4f) < .2f;
            keys(new[]{Key.Q}); update.Invoke(walk,null);
            drop = !(bool)typeof(CinderBlockoutWalk).GetField("carrying",flags).GetValue(walk) && parcel.GetComponent<Collider>().enabled;
            float start = walk.transform.position.y, peak = start;
            keys(new[]{Key.Space}); update.Invoke(walk,null); keys(new Key[0]);
            for(int i=0;i<Mathf.CeilToInt(1.1f/Time.deltaTime);i++) {
                update.Invoke(walk,null); peak = Mathf.Max(peak,walk.transform.position.y);
            }
            jump = peak-start > .55f;
            if (pickup && forward && backward && drop && jump) {
                var camera = walk.GetComponentInChildren<Camera>();
                typeof(CinderBlockoutWalk).GetField("carrying",flags).SetValue(walk,true);
                parcel.GetComponent<Collider>().enabled = false;
                parcel.GetComponent<Rigidbody>().isKinematic = true;
                var views = new[] {
                    new {name="carry-edge-door.png",point=new Vector3(-19.7f,.035f,-8.4f),yaw=45f},
                    new {name="carry-edge-wall.png",point=new Vector3(-25.275f,.035f,-2.4f),yaw=270f}
                };
                foreach(var view in views) {
                    body.enabled=false;walk.transform.position=view.point;body.enabled=true;
                    typeof(CinderBlockoutWalk).GetField("yaw",flags).SetValue(walk,view.yaw);
                    update.Invoke(walk,null);
                    var target=new RenderTexture(1280,720,24);
                    var pixels=new Texture2D(1280,720,TextureFormat.RGB24,false);
                    var oldTarget=camera.targetTexture;var oldActive=RenderTexture.active;
                    try {
                        camera.targetTexture=target;camera.Render();RenderTexture.active=target;
                        pixels.ReadPixels(new Rect(0,0,1280,720),0,0);pixels.Apply();
                        File.WriteAllBytes(Path.GetFullPath("../art/cinder-kit-01/"+view.name),pixels.EncodeToPNG());
                    } finally {
                        camera.targetTexture=oldTarget;RenderTexture.active=oldActive;
                        target.Release();UnityEngine.Object.DestroyImmediate(target);UnityEngine.Object.DestroyImmediate(pixels);
                    }
                }
            }
        }
        string output = Path.GetFullPath("../art/cinder-kit-01/carry-edge-validation.json");
        var result = new {scene="CinderStructureReview",samples,overlap_count=failures.Count,failures,
            input_checks=new {pickup,forward,backward,drop,jump},
            method="Play mode, API-set poses, reflected actual CinderBlockoutWalk.Update; not human controls"};
        File.WriteAllText(output, Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
        if (failures.Count > 0) throw new Exception("Cargo overlaps in " + failures.Count + " / " + samples + " samples; see " + output);
        if (!(pickup && forward && backward && drop && jump)) throw new Exception("Carry input regression; see " + output);
        return result;
    }
}
