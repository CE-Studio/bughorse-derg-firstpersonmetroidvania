using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class netActions:MonoBehaviour {

    public static bool playmode = false;
    public NetworkManager nm;
    public UnityTransport nt;

    private static string address = "127.0.0.1";

    void Start() {

    }

    void OnGUI() {
        if ((!nm.IsServer) && (!nm.IsClient)) {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (GUILayout.Button("Start Host")) {
                nm.StartHost();
            }
            address = GUILayout.TextField(address);
            if (GUILayout.Button("Start Client")) {
                nt.SetConnectionData(address, 7777);
                nm.StartClient();
            }
            playmode = GUILayout.Toggle(playmode, "Character");

            GUILayout.EndArea();
        }
    }
}
