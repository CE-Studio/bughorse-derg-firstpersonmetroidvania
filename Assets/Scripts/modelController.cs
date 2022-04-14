using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelController:MonoBehaviour {

    public netChar player;

    public Transform ehead;

    public Transform chead;

    void Start() {

    }

    
    void Update() {

        float camXRot = player.camtf.localEulerAngles.x;
        Vector3 hrot = new Vector3(camXRot < 180 ? -Mathf.Clamp(camXRot, 0, 50) : -Mathf.Clamp(camXRot, 310, 360), 0, 0);
        chead.localEulerAngles = hrot;
        ehead.localEulerAngles = hrot;
    }
}
