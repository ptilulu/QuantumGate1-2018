using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Show_hide_object : MonoBehaviour {

    public GameObject objet_a_cacher;
    public GameObject objet_a_montrer;

    public void process()
    {
        objet_a_cacher.SetActive(false);
        objet_a_montrer.SetActive(true);
    }
}
