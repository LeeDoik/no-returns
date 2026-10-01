using UnityEngine;
using UnityEngine.InputSystem;

namespace NoReturns.Trials {
public static class TrialCargoPose {
    public static Vector3 Position(Vector3 eye, Quaternion rotation, BoxCollider shape) {
        var half = Vector3.Scale(shape.size * .5f, shape.transform.lossyScale);
        var start = eye + rotation * new Vector3(0, -.54f, 0);
        bool enabled = shape.enabled;
        int layer = shape.gameObject.layer;
        shape.enabled = true; // ComputePenetration needs an enabled collider in this Editor version.
        shape.gameObject.layer = 2; // Keep the query shape out of its own casts.
        try {
            // ponytail: four separation passes cover trial box corners; review this bound for compound geometry.
            for (int pass = 0; pass < 4; pass++) {
                var contacts = Physics.OverlapBox(start, half, rotation, 1 << 0, QueryTriggerInteraction.Ignore);
                if (contacts.Length == 0) break;
                foreach (var obstacle in contacts) {
                    if (obstacle == shape) continue;
                    if (Physics.ComputePenetration(shape, start, rotation, obstacle, obstacle.transform.position,
                        obstacle.transform.rotation, out var direction, out var distance))
                        start += direction * (distance + .04f);
                }
            }
            var desired = eye + rotation * new Vector3(0, -.54f, 1.1f);
            var offset = desired - start;
            if (Physics.BoxCast(start, half, offset.normalized, out var hit, rotation, offset.magnitude,
                1 << 0, QueryTriggerInteraction.Ignore))
                desired = start + offset.normalized * Mathf.Max(0, hit.distance - .04f);
            return desired;
        } finally {
            shape.enabled = enabled;
            shape.gameObject.layer = layer;
        }
    }
}
public sealed class CinderBlockoutWalk : MonoBehaviour {
    public const float JumpSpeed=5f, Gravity=18f;
    CharacterController body;
    Camera eye;
    float yaw, pitch, vertical;
    Vector3 spawn;
    bool english;
    GameObject parcel;
    bool carrying;
    void Start() {
        spawn=transform.position; yaw=transform.eulerAngles.y;
        gameObject.layer=2; // Exclude the holder from the trial cargo obstruction cast.
        body = gameObject.AddComponent<CharacterController>();
        body.height=1.8f; body.radius=.34f; body.center=Vector3.up*.9f;
        body.skinWidth=.035f; body.stepOffset=.32f;
        var view=new GameObject("Trial eye"); view.transform.SetParent(transform,false);
        view.transform.localPosition=Vector3.up*1.57f;
        eye=view.AddComponent<Camera>(); eye.nearClipPlane=.06f; eye.farClipPlane=250;
        eye.fieldOfView=80; view.AddComponent<AudioListener>();
        parcel=GameObject.Find("Trial carried parcel");
        Cursor.lockState=CursorLockMode.Locked; Cursor.visible=false;
    }
    void Update() {
        var k=Keyboard.current;var m=Mouse.current;if(k==null||m==null)return;
        if(k.escapeKey.wasPressedThisFrame){Cursor.lockState=CursorLockMode.None;Cursor.visible=true;}
        if(m.leftButton.wasPressedThisFrame){Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;}
        if(k.f1Key.wasPressedThisFrame)english=!english;
        if(Cursor.lockState!=CursorLockMode.Locked)return;
        var delta=m.delta.ReadValue();yaw+=delta.x*.12f;pitch=Mathf.Clamp(pitch-delta.y*.12f,-80,80);
        transform.rotation=Quaternion.Euler(0,yaw,0);eye.transform.localRotation=Quaternion.Euler(pitch,0,0);
        Vector3 move=new Vector3((k.dKey.isPressed?1:0)-(k.aKey.isPressed?1:0),0,(k.wKey.isPressed?1:0)-(k.sKey.isPressed?1:0));
        if(body.isGrounded&&vertical<0)vertical=-2;
        if(k.spaceKey.wasPressedThisFrame&&body.isGrounded&&!carrying)vertical=JumpSpeed;
        vertical-=Gravity*Time.deltaTime;
        body.Move((transform.TransformDirection(Vector3.ClampMagnitude(move,1))*(k.leftShiftKey.isPressed&&!carrying?5f:3f)+Vector3.up*vertical)*Time.deltaTime);
        if(transform.position.y<-3){body.enabled=false;transform.position=spawn;body.enabled=true;}
        if(parcel&&k.eKey.wasPressedThisFrame&&!carrying&&Vector3.Distance(eye.transform.position,parcel.transform.position)<2.4f){
            carrying=true;parcel.GetComponent<Rigidbody>().isKinematic=true;parcel.GetComponent<Collider>().enabled=false;
        }
        if(parcel&&carrying){
            Vector3 desired=TrialCargoPose.Position(eye.transform.position,eye.transform.rotation,parcel.GetComponent<BoxCollider>());
            parcel.transform.SetPositionAndRotation(desired,eye.transform.rotation);
            if(k.qKey.wasPressedThisFrame){carrying=false;parcel.GetComponent<Collider>().enabled=true;parcel.GetComponent<Rigidbody>().isKinematic=false;}
        }
    }
    void OnGUI(){
        GUI.Box(new Rect(16,16,740,76),english?"CINDER DEPOT BLOCKOUT / Listener markers only":"CINDER DEPOT 배치 시험 / 리스너는 위치 표식");
        GUI.Label(new Rect(28,43,715,24),english?"WASD move / Mouse look / E carry / Q drop / Shift sprint / Space jump / F1 한국어 / Esc cursor":"WASD 이동 / 마우스 시야 / E 들기 / Q 놓기 / Shift 달리기 / Space 점프 / F1 English / Esc 커서");
    }
}
}
