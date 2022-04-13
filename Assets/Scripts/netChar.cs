using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class netChar : NetworkBehaviour {

    public RenderTexture otherVeiw;
    public Camera cam;
    public Transform camtf;
    public Rigidbody rb;
    public AudioListener al;

    public float jumpStrength = 4;
    public float xRotation = 0f;
    public float lookSensitivity = 8f;
    public float walkspeed = 4f;
    public float walkaccel = 75f;
    public float flyspeed = 2f;
    public float flyaccel = 15f;

    public NetworkVariable<Vector3> pos = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> vel = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> mom = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> rot = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private bool ong;

    public void onNetUpdate(Vector3 last, Vector3 cur) {
        rb.position = pos.Value;
        rb.velocity = vel.Value;
        rb.angularVelocity = mom.Value;
        rb.rotation = Quaternion.Euler(rot.Value);
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            Cursor.lockState = CursorLockMode.Locked;
            rb.sleepThreshold = 0.0f;
        } else {
            vel.OnValueChanged += onNetUpdate;
            cam.targetTexture = otherVeiw;
            al.enabled = false;
        }
        syncPos();
    }

    //void Start() {
    //    if (IsOwner) {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        rb.sleepThreshold = 0.0f;
    //    } else {
    //        cam.targetTexture = otherVeiw;
    //        al.enabled = false;
    //    }
    //}

    void Update() {
        if (IsOwner) {
            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (lookSensitivity != 0) {
                camtf.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
            transform.Rotate(Vector3.up * mouseX);

            //float curspeed = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(rb.velocity.x), 2) + Mathf.Pow(Mathf.Abs(rb.velocity.y), 2));
            Vector3 mov = (Input.GetAxisRaw("Horizontal") * transform.right) + (Input.GetAxisRaw("Vertical") * transform.forward);

            if (ong) {
                //    float targxspeed = (mov.x * walkspeed);
                //if (Mathf.Abs(rb.velocity.x) - 0.1f < Mathf.Abs(targxspeed)) {
                //    rb.velocity = new Vector3(targxspeed, rb.velocity.y, rb.velocity.z);
                //}
                //
                //    float targzspeed = (mov.z * walkspeed);
                //if (Mathf.Abs(rb.velocity.z) - 0.1f < Mathf.Abs(targzspeed)) {
                //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, targzspeed);
                //}
                rb.velocity = new Vector3(
                    Mathf.Clamp(rb.velocity.x + (mov.x * walkaccel * Time.deltaTime), -walkspeed, walkspeed),
                    rb.velocity.y,
                    Mathf.Clamp(rb.velocity.z + (mov.z * walkaccel * Time.deltaTime), -walkspeed, walkspeed)
                    );
            } else {
                rb.velocity = new Vector3(
                    Mathf.Clamp(rb.velocity.x + (mov.x * flyaccel * Time.deltaTime), -flyspeed, flyspeed),
                    rb.velocity.y,
                    Mathf.Clamp(rb.velocity.z + (mov.z * flyaccel * Time.deltaTime), -flyspeed, flyspeed)
                    );
            }
        }
    }

    void FixedUpdate() {
        if (IsOwner) {
            if (transform.position.y < -100) {
                transform.position = Vector3.up;
            }

            ong = isGrounded();
            if (ong && (Input.GetAxis("Jump") >= 0.9f)) {
                rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
            }
        }
        syncPos();
    }

    void syncPos() {
        if (IsOwner) {
            pos.Value = rb.position;
            vel.Value = rb.velocity;
            vel.Value = rb.angularVelocity;
            rot.Value = rb.rotation.eulerAngles;
        } else {
            //rb.position = pos.Value;
            //rb.velocity = vel.Value;
            //rb.angularVelocity = mom.Value;
            //rb.rotation = Quaternion.Euler(rot.Value);
        }
    }

    bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.01f);
    }
}
