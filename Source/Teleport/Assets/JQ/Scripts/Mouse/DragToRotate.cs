/*
DragToRotate

Hold mouse button to rotate 
the player's camera.

Copyright John M. Quick
*/

using UnityEngine;
using System.Collections;
using System;

public class DragToRotate : MonoBehaviour {

    //camera to control
    public Camera cam;

    //whether input is enabled
    public bool IsInputEnabled;

    //rotation speed
    public float speed;

    //initial input position
    private Vector3 _originInputPos;

    //initial camera rotation
    private Quaternion _originCamR;

    //init
    void Start() {

        //initial camera rotation
        _originCamR = cam.transform.rotation;
    }

    //update
    void Update() {

        //if input enabled
        if (IsInputEnabled == true) {

            //check input
            CheckInput();
        }
    }

    //check input
    private void CheckInput() {

        //if mouse button pressed
        if (Input.GetMouseButtonDown(1)) {

            //update origin
            _originInputPos = Input.mousePosition;
        }

        //if mouse button released
        if (Input.GetMouseButtonUp(1)) {

            //update rotation
            _originCamR = cam.transform.rotation;
        }

        //if mouse button held
        if (Input.GetMouseButton(1)) {

            //rotate camera
            RotateTo(Input.mousePosition);
        }
    }

    //rotate object
    public void RotateTo(Vector3 theMousePos) {

        //calculate change in mouse position
        //float deltaV = theMousePos.y - _originInputPos.y;
        float deltaH = theMousePos.x - _originInputPos.x;

        //apply speed
        //deltaV *= speed;
        deltaH *= speed;

        //check bounds
        //deltaV = ClampAngle(deltaV, -45.0f, 45.0f);
        deltaH = ClampAngle(deltaH, -360.0f, 360.0f);

        //update camera rotation
        cam.transform.rotation =
            _originCamR *
            Quaternion.AngleAxis(deltaH, Vector3.up)
            /*Quaternion.AngleAxis(deltaV, Vector3.left)*/;
    }

    //clamp angle
    private float ClampAngle(float theAngle, float theMin, float theMax) {

        //store angle
        float angle = theAngle % 360.0f;

        //ensure positive
        if (angle < 0.0f) {
            angle *= -1.0f;
        }

        //clamp angle
        angle = Mathf.Clamp(theAngle, theMin, theMax);

        //return
        return angle;
    }

} //end class