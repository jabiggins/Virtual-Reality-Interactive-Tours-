using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_SampleFade : MonoBehaviour
{
    private GameObject currentSphere, nextSphere, outerSphere;
    private Material innerMat, outerMat, nextOuterMat;
    private static float orginalAlpha;

    //private Transform trueOuterTransform;


    //GameObject leftCam, rightCam;

    private void Start()
    {

        //leftCam = this.transform.Find("CamL").gameObject;
        //rightCam = this.transform.Find("CamR").gameObject;
    }

    public Material setTransition(GameObject nSide, GameObject cSide)
    {
        currentSphere = cSide.transform.Find("SphereS").gameObject;
        nextSphere = nSide.transform.Find("SphereS").gameObject;
        outerSphere = cSide.transform.Find("SphereB").gameObject;

        //make the outer material be the same as the sphere that you will teleport to
        GameObject nextOuterSphere = nextSphere.transform.parent.Find("SphereB").gameObject;    //find next outside sphere        
        nextOuterMat = nextOuterSphere.GetComponent<Renderer>().material;                       // find material (mat) of next outside sphere
        outerMat = outerSphere.GetComponent<Renderer>().material;                               //save the current mat for later
        //trueOuterTransform = outerSphere.transform;                                           //save current transform for later, not needed because you can simply refer to the inner sphere, which although had undergone changes in alpha, had not undergone changes in transform 
        //Debug.Log("true rotation:  " + currentSphere.transform.rotation.ToString());

        outerSphere.GetComponent<Renderer>().material = nextOuterMat;                    //make THIS outersphere have the mat of the NEXT outer sphere
        outerSphere.transform.rotation = nextOuterSphere.transform.rotation;             //make THIS outersphere have the roatation of the NEXT outer sphere
        innerMat = currentSphere.GetComponent<Renderer>().material;                      //find and store mat of inner sphere to decrease alpha later
        //Debug.Log("new rotation:  " + outerSphere.transform.rotation.ToString());


        return innerMat;

        //update minimap, needs to happen before the Coroutine or else it will not run
        //switchDots();

        //Start the fading process of the inner sphere
        //StartCoroutine(FadeOut(1.4f, innerMat));


    }

    public IEnumerator FadeOut(float time, Material rMat, Material lMat)
    {
        //store original alpha
        orginalAlpha = rMat.color.a;

        //While we are still visible, remove some of the alpha colour
        while (rMat.color.a > 0.0f)
        {
            rMat.color = new Color(rMat.color.r, rMat.color.g, rMat.color.b, rMat.color.a - (Time.deltaTime / time));
            lMat.color = new Color(lMat.color.r, lMat.color.g, lMat.color.b, lMat.color.a - (Time.deltaTime / time));
            yield return null;
        }
        Debug.Log("fade out");

        //Change the camera position
        Camera.main.transform.parent.position = nextSphere.transform.position;
        Debug.Log("Moved");



    }

    public IEnumerator FadeOut(float time, Material mat)
    {
        //store original alpha
        orginalAlpha = mat.color.a;

        //While we are still visible, remove some of the alpha colour
        while (mat.color.a > 0.0f)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / time));

            yield return null;
        }
        Debug.Log("fade out");

        //Change the camera position
        Camera.main.transform.parent.position = nextSphere.transform.position;
        Debug.Log("Moved");

        Transition();

    }


    public void Transition()
    {


        //reset alpha (for next fade)
        innerMat.color = new Color(innerMat.color.r, innerMat.color.g, innerMat.color.b, orginalAlpha);


            //revert the MAT and ROTATION of the outside sphere to what it was before
            outerSphere.GetComponent<Renderer>().material = outerMat;

            Vector3 eulerRotation = new Vector3(currentSphere.transform.eulerAngles.x, currentSphere.transform.transform.eulerAngles.y, currentSphere.transform.eulerAngles.z);
            outerSphere.transform.rotation = Quaternion.Euler(eulerRotation);

        Debug.Log("Done trans");


    }

    //this method handles which minimap circle is "active" or colored and which floor is currently displayed
    public GameObject switchDots (GameObject currLocation, GameObject nextLocation, GameObject currFloor)
    {
        //get sphere numbers
        string currLocNum = currLocation.name.Substring(currLocation.name.Length - 2);         
        string nextLocNum = nextLocation.name.Substring(nextLocation.name.Length - 2);
        Debug.Log("curr: " + currLocNum + ", next: " + nextLocNum);

        GameObject currDot = null;
        GameObject nextDot = null;

        //get dots corresponding to spheres
        foreach (Transform child in currFloor.transform)
        {
            if (child.gameObject.name.Substring(child.gameObject.name.Length - 2) == currLocNum)
                currDot = child.gameObject;
            else if (child.gameObject.name.Substring(child.gameObject.name.Length - 2) == nextLocNum)
                nextDot = child.gameObject;
        }

        Debug.Log("currDot: " + currDot + ", nextDot: " + nextDot);
        //if BOTH dots corresponding to these spheres exist on the current floor
        if (currDot != null && nextDot!=null)
        {
            Debug.Log("Same floor");
            Material currM = currDot.GetComponent<Renderer>().material;
            Material nextM = nextDot.GetComponent<Renderer>().material;
             
            currDot.GetComponent<Renderer>().material = nextM;
            nextDot.GetComponent<Renderer>().material = currM;
        }
        //if this runs, we cant find a destination on the current floor--> we must be trying to switch floors
        else
        {
            Debug.Log("move floor");
            currFloor.SetActive(false);
            //here we call a script that MUST exist on EACH teleporter that moves camera "between floors"
            currFloor = currLocation.GetComponentInChildren<MoveFloor>().getDest();
            currFloor.SetActive(true);
        }
        return currFloor;
    }
}

	

