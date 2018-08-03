using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void ExitCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape");
            SceneManager.LoadScene("MainMenu");
        }
        //yield return new WaitForSeconds(.1f);
    }

    void SoundQuit()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (source.isPlaying)
                source.Stop();
            else
                source.Play();
            
        }
        //yield return new WaitForSeconds(.1f);
    }
}
