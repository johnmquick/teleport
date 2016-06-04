/*
FixedRotate

Rotates player's view by 
a specified amout per 
interaction.

Copyright John M. Quick
*/

using UnityEngine;

public class FixedRotate : MonoBehaviour {

    //camera to control
    public Camera cam;

    //rotation amount, in degrees
    public float degrees;

    //rotate object
    public void RotateBy(float theDegrees) {

        //update camera rotation
        cam.transform.rotation *= Quaternion.AngleAxis(theDegrees, Vector3.up);
    }

    /*
    DEBUG TESTING
    MOUSE CONTROL OVERRIDE
    //update
    void Update() {
            //check input
            CheckInput();
    }

    //check input
    private void CheckInput() {

        //if mouse button released
        if (Input.GetMouseButtonUp(1)) {

            //rotate camera
            RotateBy(degrees);
        }
    }
    */
} //end class