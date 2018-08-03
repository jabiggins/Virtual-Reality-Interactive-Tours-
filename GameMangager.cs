using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Oftentimes, keyboard input needs to be given to change different game variables such as backround music playing, or exiting to the main menu
/// </summary>
public class GameMangager : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    // Update is called once per frame
    void Update()
    {
        ExitCheck();
        SoundQuit();
    }
    /// <summary>
    /// moniter if the current tour should stop and return the user to the main menu, unsupported in webVR 
    /// </summary>
    void ExitCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            SceneManager.LoadScene("MainMenu");
        }
    }
    /// <summary>
    /// stops the backround music
    /// </summary>
    void SoundQuit()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (source.isPlaying)
                source.Stop();
            else
                source.Play();
            
        }
    }
}
