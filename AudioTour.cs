using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A simple class that uses the GVR interactions with the music on and music off buttons to start the audio files
/// simulate telling information on a tour
/// </summary>
public class AudioTour : MonoBehaviour {
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
        if (canPlay && !musicSource.isPlaying)
        {
            musicSource.PlayOneShot(sound);
            canPlay = false;
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
        canPlay = true;
    }
}
