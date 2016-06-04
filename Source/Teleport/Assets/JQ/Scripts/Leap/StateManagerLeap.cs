/*
StateManagerLeap

Manages the game state.

Copyright John M. Quick
*/

using UnityEngine;
using System.Collections;

public class StateManagerLeap : MonoBehaviour {

    //whether input is enabled
    public bool IsInputEnabled, IsTouchEnabled, IsRotateEnabled, IsTeleportEnabled;

    //fade script
    public FadeScene fade;

    //user input scripts
    public MoveOnInput move;
    public FixedRotate rotate;

    //interactive platforms
    public TouchStates[] platforms;

    //store selected platform object
    private GameObject _selectObj;

    //store platform object where player is currently positioned
    private GameObject _posObj;

    //tutorial UI
    public Canvas canvasTutorial1, canvasTutorial2;

    //init
    void Start() {

        //disable input
        DisableInput();

        //identify starting point
        _posObj = platforms[0].gameObject;
        _selectObj = null;

        //play music
        AudioManager.Instance.PlayClipFromSource(
            AudioManager.Instance.bgm,
            AudioManager.Instance.bgmSource
            );

        //fade music
        AudioManager.Instance.ToggleFade();

        //start turn
        StartCoroutine(StartTurn());
    }

    //update
    void Update() {
        
        //exit application on escape
        if (Input.GetKeyUp(KeyCode.Escape)) {

            //exit
            Application.Quit();
        }
    }

    //start turn
    public IEnumerator StartTurn() {

        //reset selected object
        _selectObj = null;

        //fade in
        fade.ToggleFade();

        //delay
        yield return new WaitForSeconds(fade.duration);

        //enable interactive objects
        EnableInteractives();

        //disable position object
        _posObj.GetComponent<TouchStates>().SetDisabled();

        //enable input
        IsInputEnabled = true;
    }

    //end turn
    public IEnumerator EndTurn() {

        //if valid object selected
        if (_selectObj != null) {

            //disable input
            DisableInput();

            //fade out
            fade.ToggleFade();

            //delay
            yield return new WaitForSeconds(fade.duration);

            //disable interactive objects
            DisableInteractives();

            //move to selected object
            move.MoveTo(_selectObj);

            //update position object
            _posObj = _selectObj;

            //check tutorial visibility
            CheckTutorialVisibility(_posObj);

            //start turn
            StartCoroutine(StartTurn());
        }
    }

    //disable all input
    public void DisableInput() {
        IsInputEnabled = false;
        IsTouchEnabled = false;
        IsRotateEnabled = false;
        IsTeleportEnabled = false;
    }

    //disable all interactive objects
    public void DisableInteractives() {

        //iterate through platforms
        for (int i = 0; i < platforms.Length; i++) {

            //disable
            platforms[i].SetDisabled();
        }
    }

    //enable all interactive objects
    public void EnableInteractives() {

        //iterate through platforms
        for (int i = 0; i < platforms.Length; i++) {

            //enable
            platforms[i].SetEnabled();
        }
    }

    //show canvas
    private void ShowCanvas(Canvas theCanvas) {

        //retrieve component
        CanvasGroup canvasGroup = theCanvas.GetComponent<CanvasGroup>();

        //set alpha
        canvasGroup.alpha = 1.0f;
    }

    //hide canvas
    private void HideCanvas(Canvas theCanvas) {

        //retrieve component
        CanvasGroup canvasGroup = theCanvas.GetComponent<CanvasGroup>();

        //set alpha
        canvasGroup.alpha = 0.0f;
    }

    //check tutorial visibility
    private void CheckTutorialVisibility(GameObject theObj) {

        //if on center platform
        if (theObj.name == platforms[0].name) {

            //modify tutorial canvas visibility
            ShowCanvas(canvasTutorial1);
            HideCanvas(canvasTutorial2);
        }

        //if on front platform
        else if (theObj.name == platforms[1].name) {

            //modify tutorial canvas visibility
            HideCanvas(canvasTutorial1);
            ShowCanvas(canvasTutorial2);
        }

        //otherwise
        else {

            //modify tutorial canvas visibility
            HideCanvas(canvasTutorial1);
            HideCanvas(canvasTutorial2);
        }
    }

    //event-based calls for interactions
    //associated with gesture detection objects in Unity Inpsector
    //disable input
    public void OnDisableTouch() {
        IsTouchEnabled = false;
    }
    public void OnDisableRotate() {
        IsRotateEnabled = false;
    }
    public void OnDisableTeleport() {
        IsTeleportEnabled = false;
    }

    //enable input
    public void OnEnableTouch() {
        IsTouchEnabled = true;

    }
    public void OnEnableRotate() {
        IsRotateEnabled = true;


    }
    public void OnEnableTeleport() {
        IsTeleportEnabled = true;
    }

    //rotate
    public void OnRotate() {

        //if enabled
        if (IsInputEnabled == true && IsRotateEnabled == true) {

            //disable subsequent input
            IsRotateEnabled = false;

            //rotate
            rotate.RotateBy(rotate.degrees);

            //audio
            AudioManager.Instance.PlayClipFromSource(
                AudioManager.Instance.sfxConfirm,
                AudioManager.Instance.sfxSource
                );
        }
    }

    //end turn
    public void OnTeleport() {

        //if enabled
        if (IsInputEnabled == true && IsTeleportEnabled == true) {

            //disable subsequent input
            IsTeleportEnabled = false;

            //end turn
            StartCoroutine(EndTurn());

            //audio
            AudioManager.Instance.PlayClipFromSource(
                AudioManager.Instance.sfxConfirm,
                AudioManager.Instance.sfxSource
                );
        }
    }

    //accessors
    public GameObject selectObj {
        set { _selectObj = value; }
    }
    public GameObject posObj {
        get { return _posObj; }
    }

} //end class