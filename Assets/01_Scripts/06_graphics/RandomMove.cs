using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour {

    float delaytimer;
    Vector3 pos;

    void Start()
    {
        getNewPosition(); // get initial targetpos
    }

    void Update()
    {
        delaytimer += Time.deltaTime;

        if (delaytimer > 10) // time to wait 
        {
            getNewPosition(); //get new position every 1 second
            delaytimer = 0f; // reset timer
        }
        transform.position = Vector3.MoveTowards(transform.position, pos, .5f);
    }

    void getNewPosition()
    {
        float x = Random.Range(0, 1300);
        float z = Random.Range(0, 800);

        pos = new Vector3(x, z, 0);
    }
}
