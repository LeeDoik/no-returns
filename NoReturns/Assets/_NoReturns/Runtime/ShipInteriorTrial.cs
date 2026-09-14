using UnityEngine;
using UnityEngine.InputSystem;

namespace NoReturns.Trials {
public sealed class ShipInteriorTrial : MonoBehaviour {
    CharacterController body;
    Camera eye;
    float yaw, pitch, vertical;
    bool english;
    GameObject parcel;
    bool carrying;
    void Start() {
        gameObject.layer=2; // Exclude the holder from the trial cargo obstruction cast.
        body = gameObject.AddComponent<CharacterController>();
        body.height=1.8f; body.radius=.34f; body.center=Vector3.up*.9f;
        body.skinWidth=.035f; body.stepOffset=.32f;
        var view=new GameObject("Trial eye"); view.transform.SetParent(transform,false);
        view.transform.localPosition=Vector3.up*1.57f;
        eye=view.AddComponent<Camera>(); eye.nearClipPlane=.06f; eye.farClipPlane=100;
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
        vertical=body.isGrounded?-2:vertical-20*Time.deltaTime;
        body.Move((transform.TransformDirection(Vector3.ClampMagnitude(move,1))*3+Vector3.up*vertical)*Time.deltaTime);
        if(transform.position.y<-3){body.enabled=false;transform.position=new Vector3(0,.1f,-8);body.enabled=true;}
        if(parcel&&k.eKey.wasPressedThisFrame&&!carrying&&Vector3.Distance(eye.transform.position,parcel.transform.position)<2.4f){
            carrying=true;parcel.GetComponent<Rigidbody>().isKinematic=true;parcel.GetComponent<Collider>().enabled=false;
        }
        if(parcel&&carrying){
            Vector3 desired=eye.transform.TransformPoint(new Vector3(0,-.54f,1.1f));
            Vector3 offset=desired-eye.transform.position;
            if(Physics.BoxCast(eye.transform.position,new Vector3(.4f,.325f,.325f),offset.normalized,out var hit,eye.transform.rotation,offset.magnitude,1<<0,QueryTriggerInteraction.Ignore))
                desired=eye.transform.position+offset.normalized*Mathf.Max(.15f,hit.distance-.04f);
            parcel.transform.SetPositionAndRotation(desired,eye.transform.rotation);
            if(k.qKey.wasPressedThisFrame){carrying=false;parcel.GetComponent<Collider>().enabled=true;parcel.GetComponent<Rigidbody>().isKinematic=false;}
        }
    }
    void OnGUI(){
        GUI.Box(new Rect(16,16,590,76),english?"FLATBED STRUCTURE TRIAL — not the main game":"FLATBED 구조 시험 — 본 게임과 별도 장면");
        GUI.Label(new Rect(28,43,565,24),english?"WASD move / Mouse look / E carry / Q drop / F1 한국어 / Esc cursor":"WASD 이동 / 마우스 시야 / E 들기 / Q 놓기 / F1 English / Esc 커서");
    }
}
}
