/* Script permettant la gestion du son */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound_settings : MonoBehaviour
{

    public GameObject Text;

    // Test au lancement la valeur de son dans les prefs du joueur
    void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
            Text.SetActive(false);
        else
            Text.SetActive(true);
    }

    //Quand on clique sur le bouton, il faut désactiver/activer les sons
    public void Click()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            Text.SetActive(true);
        }

        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            Text.SetActive(false);
        }
    }
}
