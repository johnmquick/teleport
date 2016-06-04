/*
MoveOnInput

Moves the player to a 
specified location on input.

Copyright John M. Quick
*/

using UnityEngine;

public class MoveOnInput : MonoBehaviour {

    //camera to control
    public Camera cam;

    //move to an object
    public void MoveTo(GameObject theObj) {

        //if valid
        if (theObj != null) {

            //retrieve object position
            Vector3 movePos = theObj.transform.position;

            //offset move position
            movePos.y += 3.0f + theObj.GetComponent<Collider>().bounds.size.y / 2;

            //move camera
            cam.transform.position = movePos;
        }
    }

} //end class