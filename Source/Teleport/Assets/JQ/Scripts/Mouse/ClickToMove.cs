/*
ClickToMove

Click on an object to move 
the player's camera there.

Copyright John M. Quick
*/

using UnityEngine;
using System.Collections;
using System;

public class ClickToMove : MonoBehaviour {

    //wait for input
    public IEnumerator WaitForInput(Action<GameObject> theObj) {

        //whether input has been received
        bool isInputReceived = false;

        //while input not received
        while (isInputReceived == false) {

            //check input
            GameObject obj = CheckInput();

            //if input received
            if (obj != null) {

                //toggle flags
                isInputReceived = true;

                //move to object
                theObj(obj);
            }

            //pause
            yield return 0;
        }
    }

    //check input
    private GameObject CheckInput() {

        //store selected object
        GameObject obj = null;

        //raycast to selectable layer
        int selectMask = 1 << LayerMask.NameToLayer("Select");
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit selectHit;

        //if hit found
        if (Physics.Raycast(mouseRay, out selectHit, Mathf.Infinity, selectMask)) {

            //left mouse button released
            if (Input.GetMouseButtonUp(0)) {

                //store selected object
                obj = selectHit.collider.gameObject;
            }
        }

        //return
        return obj;
    }

    //move to an object
    public void MoveTo(GameObject theObj) {

        //retrieve object position
        Vector3 movePos = theObj.transform.position;

        //offset move position
        movePos.y += 1.0f + theObj.GetComponent<Collider>().bounds.size.y / 2;

        //move camera
        Camera.main.transform.position = movePos;
    }

} //end class