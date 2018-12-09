/* Script permettant la gestion des vibrations */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vibration_settings : MonoBehaviour {

    public GameObject Text;

	// Test au lancement la valeur de vibration dans les prefs du joueur
	void Start () {
        if (PlayerPrefs.GetInt("vibration") == 0)
           Text.SetActive(false);
        else
           Text.SetActive(true);
    }
	
    //Quand on clique sur le bouton, il faut désactiver/activer les vibrations
    public void Click()
    {
        if (PlayerPrefs.GetInt("vibration") == 0){
            PlayerPrefs.SetInt("vibration", 1);
            Text.SetActive(true);
        }

        else
        {
            PlayerPrefs.SetInt("vibration", 0);
            Text.SetActive(false);
        }       
    }
}
