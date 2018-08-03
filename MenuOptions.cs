using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// in the pc version, instead of a webpage acting as a main menu, I created a scene that allows the user to select a tour using GVR interaction
/// with triggered objects
/// </summary>
public class MenuOptions : MonoBehaviour {
    
    //variable that ensures the user mouses over teleporter for a sufficient amount of time before teleportation to another scene is commenced
    private bool cleared = false;

    /// <summary>
    /// starts teleportation process
    /// </summary>
    /// <param name="nextLoc"></param>
    public void StartTele(Object nextLoc)
    {
        cleared = true;
        StartCoroutine(CheckTele(nextLoc));
    }
    /// <summary>
    /// ensures that the user is still wants to teleport after a couple of seconds
    /// </summary>
    /// <param name="nextLoc"></param>
    /// <returns></returns>
    private IEnumerator CheckTele(Object nextLoc)
    {
        //ensure that the person is still targeting the teleporter
        yield return new WaitForSeconds(3f);
        if (cleared)
        {
            SceneManager.LoadScene(nextLoc.name);
        }
    }
    //in case you move cursor out of tele
    public void StopTele()
    {
        cleared = false;
    }
}
