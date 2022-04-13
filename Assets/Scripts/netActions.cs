using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class netActions:MonoBehaviour {

    public static bool playmode = false;
    public NetworkManager nm;
    public NetworkTransport nt;

    void Start() {

    }


    void OnGUI() {
        if (nm.IsServer! && nm.IsClient!) {
            
        }
    }
}
