using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapcon:MonoBehaviour {

    public static readonly int maxlines = 15;

    public TMPro.TMP_Text output;

    List<string> history = new List<string>();

    public static mapcon con;

    private bool conpulse = false;
    private bool contoggl = false;

    void Start() {
        con = this;
    }

    void Update() {
        if (Input.GetAxisRaw("console") > 0.9f) {
            if (!conpulse) {
                conpulse = true;
                if (contoggl) {
                    contoggl = false;
                    Cursor.lockState = CursorLockMode.Locked;
                } else {
                    contoggl = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        } else {
            conpulse = false;
        }
    }

    private void intlog(string inp) {
        history.Add(inp);

        string tempstring = "";

        if (history.Count > maxlines) {
            history.RemoveAt(0);
        }

        foreach (string i in history) {
            tempstring += (i + "\n");
        }

        output.text = tempstring;
    }

    public void submit(string inp) {
        cmdprocessor.submit(inp);
    }

    public static void log(string inp) {
        print(inp);
        con.intlog(inp);
    }
}
