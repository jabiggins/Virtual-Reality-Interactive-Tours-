using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class S_TeleportManager : MonoBehaviour {

    [SerializeField]
    private GameObject currLocation;

    [SerializeField]
    private GameObject currFloor;

    [SerializeField]
    Transform tele, tripod;

    private S_SampleFade2 rightFader, leftFader;

    private bool cleared = false;       //moniters if you should teleport

                                        //zooming vars
    //private bool lerp, shouldZoom;            // moniters if its time to lerp and whether or not you are performing zoom
    //private int i = 0;
    //private GvrReticlePointer pointer;
    //private Vector3 targetLoc;

    private VideoPlayer vidPlayerL;
    private VideoPlayer vidPlayerR;

    void Start () {
        cleared = false;
        rightFader = new S_SampleFade2();
        leftFader = new S_SampleFade2();
        //pointer = tripod.Find("CamL").GetComponentInChildren<GvrReticlePointer>();
    }

    public void StartTele(GameObject nextLoc)
    {
        Debug.Log("start tele");
        cleared = true;
        //zoom.ExecuteZoom();
        StartCoroutine(CheckTele(nextLoc));
    }

    private IEnumerator CheckTele(GameObject nextLoc)
    {
        //ensure that the person is still targeting the teleporter
        yield return new WaitForSeconds(1.5f);
        if (cleared)
        {
            //zoom
            //targetLoc = pointer.targetLocation;
            //Debug.Log("targetLoc: " + targetLoc + "teleLoc" + tele.position);
            //lerp = true;

            //tele and fade
            Activate(nextLoc);
        }
    }

    //in case you move cursor out of tele
    public void StopTele()
    {
        Debug.Log("stop tele");
        cleared = false;
    }

    public void Activate(GameObject nextLoc)
    {
        //Activate the next location
        nextLoc.gameObject.SetActive(true);

        GameObject nextRSide = nextLoc.transform.Find("RSide").gameObject;
        GameObject nextLSide = nextLoc.transform.Find("LSide").gameObject;
        //GameObject currRSide = nextLoc.transform.Find("RSide").gameObject;      //can delete
        //GameObject currLSide = nextLoc.transform.Find("LSide").gameObject;      //can delete

        //tell the outer sphere what it should look like, run once for each side/eye, returns material to fade       
        Material rMat = rightFader.setTransition(nextRSide, currLocation.transform.Find("RSide").gameObject);
        Material lMat = leftFader.setTransition(nextLSide, currLocation.transform.Find("LSide").gameObject);

        /*                                            trials for video fading, delete whenever
        vidPlayerL = nextLSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();
        vidPlayerR = nextRSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();
        if (vidPlayerL != null && vidPlayerR != null)
        {
            VideoController();
        }
        */

        //zoom.ExecuteZoom();

        //fade each side's innder sphere
        StartCoroutine(rightFader.FadeOut(1.4f, rMat));
        StartCoroutine(leftFader.FadeOut(1.4f, lMat));
        Debug.Log("sample fade done");

        //move and reset
        //rightFader.Transition();
        //leftFader.Transition();

                    //dots on map
        //currFloor = rightFader.switchDots(currLocation, nextLoc, currFloor);

        StopTele();

        vidPlayerL = nextLSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();
        vidPlayerR = nextRSide.transform.Find("SphereS").gameObject.GetComponent<VideoPlayer>();

        if (vidPlayerL!=null && vidPlayerR!=null)
        {
            Debug.Log("video start");
            vidPlayerL.Play();
            vidPlayerR.Play();
        }


        //deactive on delay
        StartCoroutine(Deactivate(nextLoc));
        
    }


    private IEnumerator Deactivate(GameObject nextLoc)
    {
        //delay for effect
        yield return new WaitForSeconds(3f);

        //deactivate curr location
        currLocation.gameObject.SetActive(false);

        //change current sphere
        currLocation = nextLoc;

    }
    public bool GetCleared()
    {
        Debug.Log(cleared);
        return cleared;
    }

    //private void Update()
    //{
    //    if (lerp)
    //    {
    //        Debug.Log("now we lerping: " + i);
    //        tripod.position = Vector3.Lerp(tripod.position, tele.position, 1f * Time.deltaTime);         //
    //        i++;
    //        if (i >= 120)
    //        {
    //            Debug.Log("stop lerping");
    //            lerp = false;
    //        }
    //    }
    //}
}
    