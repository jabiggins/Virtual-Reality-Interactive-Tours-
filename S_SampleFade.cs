using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class covers the specifics of the dissolve transition by changing the alpha of the inner sphere.  Also the map functionality is implemented but not enabled for the  final build of the project
/// </summary>
public class S_SampleFade : MonoBehaviour
{
    private GameObject currentSphere, nextSphere, outerSphere;
    private Material innerMat, outerMat, nextOuterMat;
    private static float orginalAlpha;
    /// <summary>
    /// Basically preps everything for the transition, takes in sphere you are currently in and the sphere you are going to
    /// </summary>
    /// <param name="nSide"></param>
    /// <param name="cSide"></param>
    /// <returns></returns>
    public Material setTransition(GameObject nSide, GameObject cSide)
    {
        currentSphere = cSide.transform.Find("SphereS").gameObject;
        nextSphere = nSide.transform.Find("SphereS").gameObject;
        outerSphere = cSide.transform.Find("SphereB").gameObject;

        //make the outer material be the same as the sphere that you will teleport to
        GameObject nextOuterSphere = nextSphere.transform.parent.Find("SphereB").gameObject;    //find next outside sphere        
        nextOuterMat = nextOuterSphere.GetComponent<Renderer>().material;                       // find material (mat) of next outside sphere
        outerMat = outerSphere.GetComponent<Renderer>().material;                               //save the current mat for later

        outerSphere.GetComponent<Renderer>().material = nextOuterMat;                    //make THIS outersphere have the mat of the NEXT outer sphere
        outerSphere.transform.rotation = nextOuterSphere.transform.rotation;             //make THIS outersphere have the roatation of the NEXT outer sphere
        innerMat = currentSphere.GetComponent<Renderer>().material;                      //find and store mat of inner sphere to decrease alpha later
        
        //update minimap, needs to happen before the Coroutine or else it will not run
        //switchDots();

        return innerMat;
    }
    /// <summary>
    /// needs to be in a IEnumerator so that I can remove alpha periodically and get a nice dissolve transition, requires texture to 
    /// be a default sprite texture however. Takes in the materials of the left and right side inner sphere, because both need ot be fade
    /// </summary>
    /// <param name="time"></param>
    /// <param name="mat"></param>
    /// <returns></returns>
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
        //Change the camera position to the NEXT location
        Camera.main.transform.parent.position = nextSphere.transform.position;
        Debug.Log("Moved");
        Transition();
    }
    /// <summary>
    /// overloaded method that adjusts teleportation location based on the difference the cameras are zoomed, not called without uncommenting
    /// </summary>
    /// <param name="time"></param>
    /// <param name="mat"></param>
    /// <param name="differenceMoved"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time, Material mat, Vector3 differenceMoved)
    {
        //store original alpha
        orginalAlpha = mat.color.a;

        //While we are still visible, remove some of the alpha colour
        while (mat.color.a > 0.0f)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - (Time.deltaTime / time));

            yield return null;
        }

        //Change the camera position to the NEXT location
        Camera.main.transform.parent.position = nextSphere.transform.position + differenceMoved;
        Debug.Log("Moved");

        Transition();

    }

    /// <summary>
    /// after the fade the spheres need to transition back to how they were before. The material alpha and rotation need to be adjusted back
    /// to their previous values
    /// </summary>
    public void Transition()
    {
        //reset alpha (for next fade)
        innerMat.color = new Color(innerMat.color.r, innerMat.color.g, innerMat.color.b, orginalAlpha);

        //revert the MAT and ROTATION of the outside sphere to what it was before
        outerSphere.GetComponent<Renderer>().material = outerMat;

        Vector3 eulerRotation = new Vector3(currentSphere.transform.eulerAngles.x, 
            currentSphere.transform.transform.eulerAngles.y, currentSphere.transform.eulerAngles.z);
        outerSphere.transform.rotation = Quaternion.Euler(eulerRotation);
    }


    /// <summary>
    /// this method handles which minimap circle is "active" or colored and which floor is currently displayed
    /// </summary>
    /// <param name="currLocation"></param>
    /// <param name="nextLocation"></param>
    /// <param name="currFloor"></param>
    /// <returns></returns>
    /*public GameObject switchDots (GameObject currLocation, GameObject nextLocation, GameObject currFloor)
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
    */
}

	

