using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cmdprocessor : MonoBehaviour
{
    static cmdprocessor cmd;
    
    // Start is called before the first frame update
    void Start()
    {
        cmd = this;
    }

    private void run(string command) {
        string[] inp = command.Split(' ');

        if (inp.Length < 1) {
            mapcon.log("Error: empty command");
            return;
        }

        switch (inp[0]) {
            case "help":
                mapcon.log("setchar <e, c> ... Set player character.");
                mapcon.log("prefab [name] position[x, y, z] rotation[x, y, z] ... Spawn prefab.");
                mapcon.log("wire [id1] [id2] ... Connect input id1 to output id2.");
                break;
            case "setchar":
                if (inp.Length < 2) {
                    mapcon.log("Error: \"setchar\" command is missing arguments.");
                    return;
                } else {
                    if (inp[1] == "c") {
                        netChar.localplayer.charshape.Value = true;
                        netChar.localplayer.updateShape();
                        mapcon.log("Swapped to Clarence.");
                    } else if (inp[1] == "e") {
                        netChar.localplayer.charshape.Value = false;
                        netChar.localplayer.updateShape();
                        mapcon.log("Swapped to Epsilon.");
                    } else {
                        mapcon.log($"Error: Argument must be \"c\" or \"e\", not \"{inp[1]}\".");
                    }
                }
                break;
            default:
                mapcon.log($"Error: \"{inp[0]}\" is an unknown command.");
                break;
        }
    }

    public static void submit(string command) {
        cmd.run(command);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
