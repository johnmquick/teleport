/*
ButtonStates
Handles button states by 
manipulating audiovisual cues. 

Attach to an interactable 
GameObject with the appropriate 
components.

Copyright John M. Quick
*/

using UnityEngine;

public class ButtonStates : MonoBehaviour {

    //states
    public enum State {
        Normal = 0,
        Hover, 
        Release, 
        Disabled
    }

    //current state
    private State _currentState;

    //init
    void Start() {

        //set state
        _currentState = State.Normal;
    }

    //update
    void Update() {

        //if input enabled
        if (_currentState != State.Disabled) {

            //check state
            CheckState();
        }
    }

    //check state
    private void CheckState() {

        //raycast to selectable layer
        int selectMask = 1 << LayerMask.NameToLayer("Select");
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit selectHit;

        //if hit found
        if (Physics.Raycast(mouseRay, out selectHit, Mathf.Infinity, selectMask)) {

            //object hit
            if (selectHit.collider.gameObject == gameObject) {

                //left mouse button released
                if (Input.GetMouseButtonUp(0)) {

                    //change button state (release)
                    SetRelease();
                }

                //if in normal state, allow change to hover
                else if (_currentState == State.Normal) {

                    //change button state (hover)
                    SetHover();
                }
            }

            //different object hit
            else if (selectHit.collider.gameObject != gameObject) {

                //change button state (normal)
                SetNormal();
            }
        }

        //otherwise
        else {

            //change button state (normal)
            SetNormal();
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

    //hover
    private void SetHover() {

        //check state
        if (_currentState != State.Hover) {

            //set state
            _currentState = State.Hover;

            //highlight
            ShowHighlight();

            //audio
            AudioManager.Instance.PlayClipFromSource(
                AudioManager.Instance.sfxSelect,
                AudioManager.Instance.sfxSource
                );
        }
    }

    //release
    private void SetRelease() {

        //check state
        if (_currentState != State.Release) {

            //set state
            _currentState = State.Release;

            //highlight
            HideHighlight();

            //audio
            AudioManager.Instance.PlayClipFromSource(
                AudioManager.Instance.sfxConfirm,
                AudioManager.Instance.sfxSource
                );
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