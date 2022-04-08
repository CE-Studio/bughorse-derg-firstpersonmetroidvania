using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localChar : MonoBehaviour {

    public Transform cam;
    public float jumpStrength;
    public float xRotation = 0f;
    public float lookSensitivity = 8f;
    public float walkspeed = 7.0f;
    public float flyspeed = 2f;
    
    private bool ong;

    private Rigidbody rb;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = transform.GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.0f;
    }

    bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.01f);
    }

private void FixedUpdate() {
        ong = isGrounded();
        if (ong && (Input.GetAxis("Jump") >= 0.9f)) {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
        }

        
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (lookSensitivity != 0) {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        transform.Rotate(Vector3.up * mouseX);

        float curspeed = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(rb.velocity.x), 2) + Mathf.Pow(Mathf.Abs(rb.velocity.y), 2));
        Vector3 mov = (Input.GetAxisRaw("Horizontal") * transform.right) + (Input.GetAxisRaw("Vertical") * transform.forward);

        if (ong) {
                float targxspeed = (mov.x * walkspeed);
            if (Mathf.Abs(rb.velocity.x) + 0.1f < Mathf.Abs(targxspeed)) {
                rb.velocity = new Vector3(targxspeed, rb.velocity.y, rb.velocity.z);
            }

                float targzspeed = (mov.z * walkspeed);
            if (Mathf.Abs(rb.velocity.z) - 0.1f < Mathf.Abs(targzspeed)) {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, targzspeed);
            }
        } else {
                float targxspeed = (mov.x * flyspeed);
            if (Mathf.Abs(rb.velocity.x) - 0.1f < Mathf.Abs(targxspeed)) {
                rb.velocity = new Vector3(targxspeed, rb.velocity.y, rb.velocity.z);
            }

                float targzspeed = (mov.z * flyspeed);
            if (Mathf.Abs(rb.velocity.z) - 0.1f < Mathf.Abs(targzspeed)) {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, targzspeed);
            }
        }
    }
}
