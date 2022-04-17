using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelController:MonoBehaviour {

    public netChar player;

    public GameObject e;
    public Transform ehead;
    public Transform elarm;
    public Transform erarm;
    public Transform elleg;
    public Transform erleg;

    public Transform elwing;
    public Transform erwing;

    public GameObject c;
    public Transform chead;
    public Transform clarm;
    public Transform crarm;
    public Transform clleg;
    public Transform crleg;

    public Transform clwing;
    public Transform crwing;

    void Start() {

    }

    void setLayerRecursively(GameObject obj, int newLayer) {
        if (null == obj) {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform) {
            if (null == child) {
                continue;
            }
            setLayerRecursively(child.gameObject, newLayer);
        }
    }


    public void updateShape() {
        if (player.charshape.Value) {
            e.SetActive(false);
            c.SetActive(true);
        } else {
            e.SetActive(true);
            c.SetActive(false);
        }

        if (player.IsLocalPlayer) {
            setLayerRecursively(transform.gameObject, 3);
            player.cam.cullingMask = 0b110111;
        } else {
            setLayerRecursively(transform.gameObject, 0);
            player.cam.cullingMask = 0b111111;
        }
    }

    void Update() {
        float cycletime = Time.realtimeSinceStartup;

        float camXRot = player.camtf.localEulerAngles.x;
        Vector3 hrot = new Vector3(camXRot < 180 ? -Mathf.Clamp(camXRot, 0, 50) : -Mathf.Clamp(camXRot, 310, 360), 0, 0);

        float limbrot = Mathf.Sin(cycletime * 6) * (player.curspeed * 6);

        Vector3 posrot = new Vector3(limbrot, 0, 0);
        Vector3 negrot = new Vector3(-limbrot, 0, 0);

        if (player.charshape.Value) {
            chead.localEulerAngles = hrot;

            clarm.localEulerAngles = posrot;
            crarm.localEulerAngles = -posrot;
            clleg.localEulerAngles = -posrot;
            crleg.localEulerAngles = posrot;
        } else {
            ehead.localEulerAngles = hrot;

            elarm.localEulerAngles = posrot;
            erarm.localEulerAngles = -posrot;
            elleg.localEulerAngles = -posrot;
            erleg.localEulerAngles = posrot;

            float swing0 = Mathf.Sin(cycletime * 1) * 2;
            float swing1 = (Mathf.Sin(cycletime * 1.42357f) + 1) * 2;

            elarm.localEulerAngles = new Vector3(elarm.localEulerAngles.x + swing0, elarm.localEulerAngles.y - swing1, elarm.localEulerAngles.z);
            erarm.localEulerAngles = new Vector3(erarm.localEulerAngles.x - swing0, erarm.localEulerAngles.y + swing1, erarm.localEulerAngles.z);
        }

    }
}
