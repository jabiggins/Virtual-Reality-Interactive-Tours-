using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// During the construction of WEBVR tours, one must determine whether or not the user is viewing it in a single screen, monoscopic,
/// mouse control mode, or in a split screen, stereoscopic, headset controlled mode. This script gauges the gamestate based off camera rotation.
/// 
/// Because of the lack of extensive support for unity web VR, in particular with the lack of ANY official support for GVR assets, 
/// much of this class functions as a lightswitch, merely turning things off and on again, which although crude, works minimalistically.
/// </summary>
public class StateManager2 : MonoBehaviour {

    GameObject leftCam, rightCam, mainCam, reticleL, reticleR, reticleM;
    Quaternion leftRot, mainRot;
    bool isSplitScreen =false;

    GameObject block;

    public GameObject Emulator1, System1, Emulator2, System2;

    /// <summary>
    /// Because this script is put on a gameobject that is a parent of all 3 cameras, it can find its own children and their respective reticles
    /// </summary>
    void Start () {
        //cameras and their rotations
        leftCam = this.transform.Find("CameraL").gameObject;
        rightCam = this.transform.Find("CameraR").gameObject;
        mainCam = this.transform.Find("CameraMain").gameObject;
        leftRot = leftCam.transform.rotation;
        mainRot = mainCam.transform.rotation;

        //reticles
        reticleL = leftCam.transform.Find("GvrReticlePointerL").gameObject;
        reticleR = rightCam.transform.Find("GvrReticlePointerR").gameObject;
        reticleM = mainCam.transform.Find("GvrReticlePointerM").gameObject;

        block = transform.Find("Blocker").gameObject;
        StartCoroutine(Block());

    }
	
	void Update () {
        //this checks if just switched from mono to stereo based on a change in camera rotation
        if (!isSplitScreen && leftRot != leftCam.transform.rotation)
        {
            isSplitScreen = true;
            mainRot = mainCam.transform.rotation;
            StereoSwitch();
            Debug.Log("Now you are stereo");
        }
        //this checks if just switched from stereo to mono based on a change in camera rotation
        if (isSplitScreen && mainRot != mainCam.transform.rotation)
        {
            isSplitScreen = false;
            leftRot = leftCam.transform.rotation;
            StereoSwitch();
            Debug.Log("Now you are mono");
        }
    }
        /// <summary>
        /// this method spawns a collider in front of the camera so the GVR reticle works as intended, but deletes
        /// </summary>
        /// <returns></returns>
    private IEnumerator Block()
    {
        block.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        block.SetActive(false);
    }
    /// <summary>
    /// by flipping the states of the various objects active/inactive in the scene, I can simulate the loading of the original assets, but under
    /// the desired stereoscopic and monoscopic states
    /// </summary>
    public void StereoSwitch()
    {
        //activate or deactivate mono assets
        Emulator1.SetActive(!Emulator1.activeSelf);
        System1.SetActive(!System1.activeSelf);
        reticleM.SetActive(!reticleM.activeSelf);

        //activate or deactivate stereo assets
        Emulator2.SetActive(!Emulator2.activeSelf);
        System2.SetActive(!System2.activeSelf);
        reticleL.SetActive(!reticleL.activeSelf);
        reticleR.SetActive(!reticleR.activeSelf);

        //Inititate blocker to reset/begin GVR interaction
        StartCoroutine(Block());
    }

}
