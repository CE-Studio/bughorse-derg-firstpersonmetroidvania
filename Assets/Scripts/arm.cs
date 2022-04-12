using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm : MonoBehaviour {

    public Transform wristpoint;
    public Transform elbowpoint;

    public Transform panel;
    public Transform horizSlide;
    public Transform vertSlide;
    public Transform wrist;
    public Transform wrjoint;
    public Transform wranch;
    public Transform boomExtension;

    public Transform armExtension;
    public Transform boom;
    public Transform elanch;

    public Transform armbase;
    public Transform r4arm;

    public GameObject light;

    private static readonly float sd = -0.08f;
    private Vector3 lastElbowPos;
    private Vector3 lastWristPos;
    private Vector3 lastWristRot;

    void Start() {
        lastElbowPos = elbowpoint.position;
        lastWristPos = wristpoint.position;
        lastWristRot = wristpoint.eulerAngles;
        calcmove();
    }

    public void calcmove() {
        lastElbowPos = elbowpoint.position;
        lastWristPos = wristpoint.position;
        lastWristRot = wristpoint.eulerAngles;

        horizSlide.localPosition = new Vector3(panel.localPosition.x, 0, sd);
        vertSlide.localPosition = new Vector3(0, panel.localPosition.y, sd);

        armbase.LookAt(elbowpoint, transform.up);
        armbase.localEulerAngles = new Vector3(0, armbase.localEulerAngles.y, 0);
        r4arm.LookAt(elbowpoint, transform.up);
        r4arm.localEulerAngles = new Vector3(r4arm.localEulerAngles.x, 0, 0);

        elanch.LookAt(armbase);

        armExtension.LookAt(wrist, elanch.forward * -1);
        armExtension.localEulerAngles = new Vector3(0, armExtension.localEulerAngles.y, 0);
        boom.LookAt(wrist, elanch.forward * -1);

        wrjoint.LookAt(elbowpoint, wranch.up);
        wrjoint.localEulerAngles = new Vector3(0, wrjoint.localEulerAngles.y, 0);
        boomExtension.LookAt(elbowpoint, wranch.up);
    }

    public void setLight(bool inp) {
        light.SetActive(inp);
    }

    void Update() {
        if ((lastElbowPos == elbowpoint.position) && (lastWristPos == wristpoint.position) && (lastWristRot == wristpoint.eulerAngles)) return;

        calcmove();
    }
}
