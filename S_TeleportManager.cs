

using System.Collections;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// Created by James Biggins @NIST ITL 2018
/// 
/// This is my stereoscopic teleport manager that moderates user interactions with my "teleporter" games objects
/// which allows the user to move around the tour from 360 location to 360 location. If a successful teleportation 
/// begins, it interacts with the stereoscopic samplefade script
/// There are a few commented out sections which provide functional code that was left out of the final project.  
/// The zoom transition was a little jarring for some users and the map implementation was left out on request from my mentor.
/// They are left in to show proof of funcitonal research.
/// </summary>
public class S_TeleportManager : MonoBehaviour
{

    [SerializeField]
    private GameObject currLocation;

    //[SerializeField]
    //private GameObject currFloor;

    private GameObject nextLoc;
    private Transform tele;

    private S_SampleFade rightFader, leftFader;

    private bool cleared;                           //moniters if you should teleport

    //zoom fields
    /*[SerializeField]
    private Transform cameraSystem;
    [SerializeField]
    private float zoomSpeed = 1f;
    [SerializeField]
    private float zoomDist, zoomDist2 = 0;
    [SerializeField]
    private float zoomDistMax = 60f;
    private Vector3 prevLoc, newLoc, diffMoved;
    */
    private bool lerp, lerp2 = false;

    private VideoPlayer vidPlayerL;
    private VideoPlayer vidPlayerR;


    void Start()
    {
        cleared = false;
        //allows multiple instances of SampleFade, one for each eye
        rightFader = new S_SampleFade();
        leftFader = new S_SampleFade();
    }
    /// <summary>
    /// An event trigger method activated by moving the GVR reticle over a teleporter, attached by ""event system"
    /// The location that the chosen teleporter leads to is passed in
    /// </summary>
    /// <param name="nextLocation"></param>
    public void StartTele(GameObject nextLocation)
    {
        cleared = true;
        nextLoc = nextLocation;
        StartCoroutine(CheckTele());
    }
    /// <summary>
    /// Method for zoom transitions, trigger activation method that updates desired teleporter to zoom toward
    /// </summary>
    /// <param name="teleporter"></param>
    public void GetTeleporter(Transform teleporter)
    {
        Debug.Log(teleporter);
        tele = teleporter;
    }
    /// <summary>
    /// Ensures that the user is mousing over the teleporter for X.X seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckTele()
    {
        //ensure that the person is still targeting the teleporter
        yield return new WaitForSeconds(1.5f);
        if (cleared)
        {
            //prevLoc = cameraSystem.position;
            //Debug.Log(prevLoc);
            //lerp = true;                      //indicates zoom transition start
            Activate();
        }
    }
    /// <summary>
    /// ensures that if the user accidently touches a teleporter, nothing happens. Uses trigger system specified above.
    /// </summary>
    public void StopTele()
    {
        cleared = false;
    }
    /// <summary>
    /// the main method that moderates the dissolve transtion from sphere to sphere. Calls methods in the stereoscopic sample fader
    /// </summary>
    public void Activate()
    {
        //deactivate tele marker to give user feedback and not interfere with zoom transition
        if (lerp)
            tele.Find("sprite arrow").gameObject.SetActive(false);

        //Activate the next location
        nextLoc.gameObject.SetActive(true);

        //finds the section of the sphere that each eye sees
        GameObject nextRSide = nextLoc.transform.Find("RSide").gameObject;
        GameObject nextLSide = nextLoc.transform.Find("LSide").gameObject;

        //tell the outer sphere what it should look like, run once for each side/eye, returns material to fade       
        Material rMat = rightFader.setTransition(nextRSide, currLocation.transform.Find("RSide").gameObject);
        Material lMat = leftFader.setTransition(nextLSide, currLocation.transform.Find("LSide").gameObject);

        //fade each side's inner sphere, because they are coroutines, they appear to happen simultaneously, uncomment the third parameter if zoom is desired
        StartCoroutine(rightFader.FadeOut(1.4f, rMat /*, diffMoved */));
        StartCoroutine(leftFader.FadeOut(1.4f, lMat /*, diffMoved */));

        ///dots on map that where the user is currently located in, be very careful of naming convention
        //currFloor = rightFader.switchDots(currLocation, nextLoc, currFloor);

        //bug fix: sometimes, if the user does not move reticle after teleporting, they can double teleport, this acts as a hard reset just in case
        StopTele();

        //I provided support for the addition of 360 stereoscopic videos, the following lines find them and start them
        vidPlayerL = nextLSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();
        vidPlayerR = nextRSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();

        if (vidPlayerL != null && vidPlayerR != null)
        {
            Debug.Log("video start:: " + vidPlayerL);
            vidPlayerL.Play();
            vidPlayerR.Play();
        }
        //deactive on delay
        StartCoroutine(Deactivate(nextLoc));
    }

    /// <summary>
    /// in the efforts to lower the overhead and required graphical rendering, I deactivate all of the sphere I am not currently looking at
    /// </summary>
    /// <param name="nextLoc"></param>
    /// <returns></returns>
    private IEnumerator Deactivate(GameObject nextLoc)
    {
        //delay for effect
        yield return new WaitForSeconds(1.5f);

        //re-activate tele marker
        if (lerp)
        {
            tele.Find("sprite arrow").gameObject.SetActive(true);
            lerp2 = true;
        }

        //deactivate curr location
        currLocation.gameObject.SetActive(false);

        //change current sphere so the next time this method runs, it access the children of the correct location
        currLocation = nextLoc;

    }
    /// <summary>
    /// update method only needed for zoom transitions
    /// </summary>
    /*private void Update()
    {
        if (lerp)
        {
            //Debug.Log("now we lerping: " + zoomDist);
            cameraSystem.position = Vector3.Lerp(cameraSystem.position, tele.position, zoomSpeed * Time.deltaTime);
            zoomDist++;
            if (zoomDist >= zoomDistMax)
            {
                Debug.Log("stop lerping");
                lerp = false;
                newLoc = cameraSystem.position;
                diffMoved = newLoc - prevLoc;
                Debug.Log(newLoc + "difference: " + diffMoved);
                zoomDist = 0;
            }
        }
        if (lerp2)
        {
            //Debug.Log("now we lerping: " + zoomDist);
            cameraSystem.position = Vector3.Lerp(cameraSystem.position, nextLoc.transform.Find("RSide").Find("SphereB").position,
                zoomSpeed * Time.deltaTime);
            zoomDist2++;
            if (zoomDist2 >= zoomDistMax)
            {
                Debug.Log("stop lerping2");
                lerp2 = false;
                //newLoc = cameraSystem.position;
                //diffMoved = newLoc - prevLoc;
                //Debug.Log(newLoc + "difference: " + diffMoved);
                zoomDist2 = 0;
            }
        }
    }
    */
}
