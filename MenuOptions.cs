using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour {

    private bool cleared = false;
    private string nextLocName;

    public void StartTele(string nextLoc)
    {
        Debug.Log("start tele");
        nextLocName = nextLoc;
        cleared = true;
        StartCoroutine(CheckTele());
    }

    private IEnumerator CheckTele()
    {
        //ensure that the person is still targeting the teleporter
        yield return new WaitForSeconds(3f);
        if (cleared)
        {
            SwitchScene();
        }
    }

    //in case you move cursor out of tele
    public void StopTele()
    {
        cleared = false;
    }

    private void SwitchScene()
    {
        if (nextLocName != null)
        {
            Debug.Log("Switch Scene");
            SceneManager.LoadScene(nextLocName);
        }
        else
        {
            Debug.Log("Scene null");
        }
    }
    private void Update()
    {
        Debug.Log(nextLocName);
    }
}
