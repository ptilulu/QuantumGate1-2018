using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    public GameObject ObjectAudio;
    public AudioClip clip;
    AudioSource audio;

    void initi()
    {
        audio = ObjectAudio.GetComponent<AudioSource>();
        audio.clip = clip;
    }

    public void Click()
    {
        initi();
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audio.enabled = true;
            audio.PlayOneShot(clip, 1f);
        }
            
    }
}
