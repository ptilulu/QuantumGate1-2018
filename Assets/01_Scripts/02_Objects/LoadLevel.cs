using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load une scene suivant son nom
/// </summary>
public class LoadLevel : MonoBehaviour
{
    [Tooltip("Niveau à charger")]
    public Level level;

    public void OnClick()
    {

        Debug.Log("Number of level : " + level.id);
        Debug.Log("Number of columns : " + level.colonnes.Count);

        List<QCS.Circuit> circuits = new List<QCS.Circuit>
        {
            new QCS.Circuit(level.ColonneHeight, level.NbColonnes)
        };

        GameMode.nameGame = "";
        GameMode.circuits = circuits;
        GameMode.level = level;
        GameMode.gates = new List<QCS.Gate>() { QCS.Gate.NOT, QCS.Gate.CONTROL, QCS.Gate.SWAP, QCS.Gate.HADAMARD };
        GameMode.customGates = new List<QCS.Gate>();
        for (int i = 0; i < level.NbColonnes; i++)
        {
            if (level.colonnes[i].BitDefaut == 0)
                GameMode.circuits[0].SetEntry(i, QCS.Qubit.Zero);
            else
                GameMode.circuits[0].SetEntry(i, QCS.Qubit.One);
            for (int j = 0; j < level.colonnes[i].Portes.Count; j ++ )
            {
                Debug.Log("Type of gate : " + level.colonnes[i].Portes[j]);
                switch(level.colonnes[i].Portes[j])
                {
                    case 1:
                        if (!(GameMode.circuits[0].PutGate(j, i, QCS.Gate.HADAMARD)))
                            Debug.Log("error");
                        break;
                    case 2:
                        if (!(GameMode.circuits[0].PutGate(j, i, QCS.Gate.SWAP)))
                            Debug.Log("error");
                        break;
                    case 3:
                        if (!(GameMode.circuits[0].PutGate(j, i, QCS.Gate.NOT)))
                            Debug.Log("error");
                        break;
                    case 4:
                        if (!(GameMode.circuits[0].PutGate(j, i, QCS.Gate.CONTROL)))
                            Debug.Log("error");
                        break;
                }
            }
            
            
        }
        Debug.Log("Expected results : " + level.Resultats);

        SceneManager.LoadScene("CampaignLevel", LoadSceneMode.Single);
    }
}
