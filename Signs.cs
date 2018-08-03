using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class contains trigger methods activated by the GVR reticle system. I put any words or pictures that are needed to explain 
/// the tour as a material on a "backround object"
/// </summary>
public class Signs : MonoBehaviour
{
    private GameObject backround;

    [SerializeField]
    AudioClip sound;
    [SerializeField]
    AudioSource musicSource;

    private bool canPlay = true;

    public void DisplayText(GameObject bround)
    {
        backround = bround;
        backround.SetActive(true);
        Debug.Log("enabled");

        if(canPlay)
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
