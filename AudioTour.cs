using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTour : MonoBehaviour {

    //[SerializeField]
    private GameObject backround;

    AudioClip sound;
    AudioSource musicSource;

    private bool canPlay = true;

    private void Start()
    {
        musicSource = this.gameObject.GetComponent<AudioSource>();
        sound = musicSource.clip;
    }

    public void PlaySound()
    {
        //backround = g;
        //backround.SetActive(true);          //enable mute
        //this.gameObject.SetActive(false);   //disablePlay
        Debug.Log("enabled");

        if (canPlay && !musicSource.isPlaying)
        {
            musicSource.PlayOneShot(sound);
            canPlay = false;
        }
    }

    public void StopMusic()
    {
        Debug.Log("stop music");
        //g.SetActive(false);                 //disable mute
        //this.gameObject.SetActive(true);    //enable play
        musicSource.Stop();
        canPlay = true;
    }
}
