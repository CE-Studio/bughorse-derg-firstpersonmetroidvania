using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localChar : MonoBehaviour {

    public Transform cam;
    public float jumpStrength;

    private Rigidbody rb;

    void Start() {
        rb = transform.GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.0f;
    }

    bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.01f);
    }

private void FixedUpdate() {
        if ((Input.GetAxis("Jump") >= 0.9f) && isGrounded()) {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, rb.velocity.z);
        }
    }

    void Update() {
        
    }
}
