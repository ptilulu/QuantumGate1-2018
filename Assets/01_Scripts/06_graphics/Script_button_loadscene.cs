using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_button_loadscene : MonoBehaviour {

    public void Load(string Scene)
    {
        List<QCS.Circuit> circuits = new List<QCS.Circuit>
        {
            new QCS.Circuit(5, 3)
        };

        GameMode.nameGame = "";
        GameMode.circuits = circuits;
        GameMode.gates = new List<QCS.Gate>() { QCS.Gate.NOT, QCS.Gate.CONTROL, QCS.Gate.SWAP, QCS.Gate.HADAMARD };
        GameMode.customGates = new List<QCS.Gate>();
        SceneManager.LoadScene(Scene);
    }
}
