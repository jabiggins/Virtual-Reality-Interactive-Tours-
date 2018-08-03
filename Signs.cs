using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Signs : MonoBehaviour
{

    //[SerializeField]
    private GameObject backround;

    [SerializeField]
    AudioClip sound;

    [SerializeField]
    AudioSource musicSource;

    private bool canPlay = true;

    public void DisplayText(GameObject g)
    {
        backround = g;
        backround.SetActive(true);
        Debug.Log("enabled");

        if(canPlay && !musicSource.isPlaying)
        {
            musicSource.PlayOneShot(sound);
            canPlay = false;
        }
    }
    public void ClearText()
    {
        backround.SetActive(false);
        canPlay = true;
    }

}
