using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class netChar:NetworkBehaviour {

    public RenderTexture otherVeiw;
    public Camera cam;
    public Transform camtf;
    public Rigidbody rb;
    public AudioListener al;
    public SimpleCameraController sc;
    public CapsuleCollider col;
    public modelController mc;

    public float jumpStrength = 4;
    public float xRotation = 0f;
    public float lookSensitivity = 8f;
    public float walkspeed = 4f;
    public float walkaccel = 75f;
    public float flyspeed = 2f;
    public float flyaccel = 15f;

    public bool freecamming = false;
    private bool fctrack = false;

    //public NetworkVariable<Vector3> pos = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> vel = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<Vector3> mom = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<Vector3> rot = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> camrot = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> charshape = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private bool ong;

    public static netChar localplayer;

    public void updateShape(bool last = false, bool cur = false) {
        if (charshape.Value) {
            col.center = new Vector3(0, 0.65f, 0);
            col.radius = 0.29f;
            col.height = 1.3f;
            camtf.localPosition = new Vector3(0, 1.1f, 0);
        } else {
            col.center = new Vector3(0, 1.06f, 0);
            col.radius = 0.4f;
            col.height = 2.12f;
            camtf.localPosition = new Vector3(0, 1.92f, 0);
        }
        mc.updateShape();
    }

    //public void onPosUpdate(Vector3 last, Vector3 cur) {
    //    rb.position = pos.Value;
    //    rb.MovePosition(new Vector3(
    //            rb.position.x + ((cur.x - rb.position.x) / 2),
    //            rb.position.y + ((cur.y - rb.position.y) / 2),
    //            rb.position.z + ((cur.z - rb.position.z) / 2)
    //        ));
    //}

    public void onVelUpdate(Vector3 last, Vector3 cur) {
        rb.velocity = vel.Value;
    }

    //public void onAVelUpdate(Vector3 last, Vector3 cur) {
    //    rb.angularVelocity = mom.Value;
    //}

    //public void onRotUpdate(Vector3 last, Vector3 cur) {
    //    rb.rotation = Quaternion.Euler(rot.Value);
    //}

    public void onCamRotUpdate(Vector3 last, Vector3 cur) {
        camtf.rotation = Quaternion.Euler(camrot.Value);
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            localplayer = this;
            Cursor.lockState = CursorLockMode.Locked;
            rb.sleepThreshold = 0.0f;
            charshape.Value = netActions.playmode;
            mapcon.log("Loaded local player.");
        } else {
            //pos.OnValueChanged += onPosUpdate;
            vel.OnValueChanged += onVelUpdate;
            //mom.OnValueChanged += onAVelUpdate;
            //rot.OnValueChanged += onRotUpdate;
            camrot.OnValueChanged += onCamRotUpdate;
            charshape.OnValueChanged += updateShape;
            cam.targetTexture = otherVeiw;
            al.enabled = false;
            rb.isKinematic = true;
            mapcon.log("Loaded remote player.");
        }
        syncPos();
        updateShape();
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

            if (Input.GetAxisRaw("freecam") > 0.9f) {
                if (fctrack) {

                } else {
                    fctrack = true;
                    freecamming = !freecamming;
                    sc.enabled = freecamming;
                    if (freecamming) {
                        Cursor.lockState = CursorLockMode.None;
                        cam.cullingMask = 0b111111;
                    } else {
                        Cursor.lockState = CursorLockMode.Locked;
                        updateShape();
                        camtf.localEulerAngles = Vector3.zero;
                    }
                }
            } else {
                fctrack = false;
            }

            float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            if (lookSensitivity != 0) {
                camtf.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
            transform.Rotate(Vector3.up * mouseX);

            if (!freecamming) {
                Vector3 rvel = transform.InverseTransformDirection(rb.velocity);
                if (ong) {
                    if (Input.GetAxisRaw("Vertical") > 0.9f) {
                        if (!(rvel.z > walkspeed)) {
                            rvel.z += walkaccel * Time.deltaTime;
                        }
                    } else if (Input.GetAxisRaw("Vertical") < -0.9f) {
                        if (!(rvel.z < -walkspeed)) {
                            rvel.z -= walkaccel * Time.deltaTime;
                        }
                    }

                    if (Input.GetAxisRaw("Horizontal") > 0.9f) {
                        if (!(rvel.x > walkspeed)) {
                            rvel.x += walkaccel * Time.deltaTime;
                        }
                    } else if (Input.GetAxisRaw("Horizontal") < -0.9f) {
                        if (!(rvel.x < -walkspeed)) {
                            rvel.x -= walkaccel * Time.deltaTime;
                        }
                    }
                } else {
                    if (Input.GetAxisRaw("Vertical") > 0.9f) {
                        if (!(rvel.z > flyspeed)) {
                            rvel.z += flyaccel * Time.deltaTime;
                        }
                    } else if (Input.GetAxisRaw("Vertical") < -0.9f) {
                        if (!(rvel.z < -flyspeed)) {
                            rvel.z -= flyaccel * Time.deltaTime;
                        }
                    }

                    if (Input.GetAxisRaw("Horizontal") > 0.9f) {
                        if (!(rvel.x > flyspeed)) {
                            rvel.x += flyaccel * Time.deltaTime;
                        }
                    } else if (Input.GetAxisRaw("Horizontal") < -0.9f) {
                        if (!(rvel.x < -flyspeed)) {
                            rvel.x -= flyaccel * Time.deltaTime;
                        }
                    }
                }
                rb.velocity = transform.TransformDirection(rvel);
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
            //pos.Value = rb.position;
            vel.Value = rb.velocity;
            //vel.Value = rb.angularVelocity;
            //rot.Value = rb.rotation.eulerAngles;
            camrot.Value = camtf.rotation.eulerAngles;
        } else {
            //rb.position = pos.Value;
            rb.velocity = vel.Value;
            //rb.angularVelocity = mom.Value;
            //rb.rotation = Quaternion.Euler(rot.Value);
        }
    }

    bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.01f);
    }

    public float curspeed {
        get {
            return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(vel.Value.x), 2) + Mathf.Pow(Mathf.Abs(vel.Value.z), 2));
        }
    }

}
