using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelController:MonoBehaviour {

    public netChar player;

    public GameObject e;
    public Transform ehead;

    public GameObject c;
    public Transform chead;

    void Start() {

    }

    public void updateShape() {
        if (player.charshape.Value) {
            e.SetActive(false);
            c.SetActive(true);
        } else {
            e.SetActive(true);
            c.SetActive(false);
        }
    }
    
    void Update() {

        float camXRot = player.camtf.localEulerAngles.x;
        Vector3 hrot = new Vector3(camXRot < 180 ? -Mathf.Clamp(camXRot, 0, 50) : -Mathf.Clamp(camXRot, 310, 360), 0, 0);
        chead.localEulerAngles = hrot;
        ehead.localEulerAngles = hrot;
    }
}
