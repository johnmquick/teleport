/*
TouchStates
Handles interactiveo object states 
by manipulating audiovisual cues. 

Attach to an interactable GameObject 
with the appropriate components.

Copyright John M. Quick
*/

using UnityEngine;

public class TouchStates : MonoBehaviour {

    //states
    public enum State {
        Normal = 0,
        Selected, 
        Disabled
    }

    //current state
    private State _currentState;

    //init
    void Start() {

        //set state
        SetDisabled();
    }

    //update
    void Update() {

        //if input enabled
        if (_currentState != State.Disabled) {

            //check state
            CheckState();
        }
    }

    //check input
    private void CheckState() {

        //retrieve index finger tips
        GameObject tipL = GameObject.FindWithTag("IndexTipL");
        GameObject tipR = GameObject.FindWithTag("IndexTipR");

        //raycast to selectable layer
        int selectMask = 1 << LayerMask.NameToLayer("Select");

        //retrieve state manager
        StateManagerLeap stateManager = GameObject.FindWithTag("StateManager").GetComponent<StateManagerLeap>();

        //left hand
        //if valid and input enabled
        if (tipL != null && stateManager.IsTouchEnabled == true) {

            //raycast to selectable layer
            Vector3 touchPointL = Camera.main.WorldToScreenPoint(tipL.transform.position);
            Ray touchRayL = Camera.main.ScreenPointToRay(touchPointL);
            RaycastHit touchHitL;

            //if hit found
            if (Physics.Raycast(touchRayL, out touchHitL, Mathf.Infinity, selectMask)) {

                //retrieve hit object
                GameObject hitObjL = touchHitL.collider.gameObject;

                //object hit, not current position
                if (hitObjL == gameObject && gameObject != stateManager.posObj) {

                    //change button state (selected)
                    SetSelected();
                }

                //different object hit, not current position
                else if (hitObjL != gameObject && hitObjL != stateManager.posObj) {

                    //change button state (normal)
                    SetNormal();
                }
            }
        }

        //right hand
        //if valid and input enabled
        if (tipR != null && stateManager.IsTouchEnabled == true) {

            //raycast to selectable layer
            Vector3 touchPointR = Camera.main.WorldToScreenPoint(tipR.transform.position);
            Ray touchRayR = Camera.main.ScreenPointToRay(touchPointR);
            RaycastHit touchHitR;

            //if hit found
            if (Physics.Raycast(touchRayR, out touchHitR, Mathf.Infinity, selectMask)) {

                //retrieve hit object
                GameObject hitObjR = touchHitR.collider.gameObject;

                //object hit, visible to camera, and not current position
                if (hitObjR == gameObject && gameObject != stateManager.posObj) {

                    //change button state (selected)
                    SetSelected();
                }

                //different object hit, visible to camera, and not current position
                else if (hitObjR != gameObject && hitObjR != stateManager.posObj) {

                    //change button state (normal)
                    SetNormal();
                }
            }
        }
    }

    //normal
    private void SetNormal() {

        //check state
        if (_currentState != State.Normal) {

            //set state
            _currentState = State.Normal;

            //highlight
            HideHighlight();
        }       
    }

    //release
    private void SetSelected() {

        //check state
        if (_currentState != State.Selected) {

            //set state
            _currentState = State.Selected;

            //highlight
            ShowHighlight();

            //audio
            AudioManager.Instance.PlayClipFromSource(
                AudioManager.Instance.sfxSelect,
                AudioManager.Instance.sfxSource
                );

            //update state manager
            GameObject.FindWithTag("StateManager").GetComponent<StateManagerLeap>().selectObj = gameObject;
        }
    }

    //show highlight
    private void ShowHighlight() {
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = true;
    }

    //hide highlight
    private void HideHighlight() {
        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        halo.enabled = false;
    }

    //enable
    public void SetEnabled() {

        //enable
        SetNormal();
    }

    //disable
    public void SetDisabled() {

        //hide highlight
        HideHighlight();

        //disable
        _currentState = State.Disabled;
    }

    //accessors
    public State currentState {
        get { return _currentState; }
    }

} //end class