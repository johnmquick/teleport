/*
StateManagerMouse

Manages the game state.

Copyright John M. Quick
*/

using UnityEngine;
using System.Collections;

public class StateManagerMouse : MonoBehaviour {

    //whether input is enabled
    private bool _IsInputEnabled;

    //fade script
    public FadeScene fade;

    //user input scripts
    public ClickToMove move;
    public DragToRotate rotate;

    //buttons
    public ButtonStates[] buttons;

    //store previously selected object
    private GameObject _prevObj;

    //tutorial UI
    public Canvas canvasTutorial1, canvasTutorial2;

    //init
    void Start() {

        //disable all buttons
        DisableButtons();

        //identify starting point
        _prevObj = buttons[0].gameObject;

        //play music
        AudioManager.Instance.PlayClipFromSource(
            AudioManager.Instance.bgm,
            AudioManager.Instance.bgmSource
            );

        //fade music
        AudioManager.Instance.ToggleFade();

        //start turn
        StartCoroutine(Turn());
    }

    //update
    void Update() {

        //exit application on escape
        if (Input.GetKeyUp(KeyCode.Escape)) {

            //exit
            Application.Quit();
        }
    }

    //turn
    //a turn represents one complete interaction cycle 
    //fade in, select destination, fade out, update position
    private IEnumerator Turn() {

        //toggle flag
        _IsInputEnabled = true;
        rotate.IsInputEnabled = _IsInputEnabled;

        //fade in
        fade.ToggleFade();

        //disable previously selected object
        _prevObj.GetComponent<ButtonStates>().SetDisabled();

        //fade delay
        yield return new WaitForSeconds(fade.duration);

        //enable all buttons
        EnableButtons();

        //while
        while (_IsInputEnabled == true) {

            //store selected object
            GameObject obj = null;

            //select object
            yield return StartCoroutine(move.WaitForInput(value => obj = value));

            //check object
            if (obj != null && 
                obj.GetComponent<ButtonStates>().currentState != ButtonStates.State.Disabled) {

                //toggle flag
                _IsInputEnabled = false;
                rotate.IsInputEnabled = _IsInputEnabled;

                //disable all buttons
                DisableButtons();

                //fade out
                fade.ToggleFade();

                //fade delay
                yield return new WaitForSeconds(fade.duration);

                //move to object
                move.MoveTo(obj);

                //check tutorial visibility
                CheckTutorialVisibility(obj);

                //update previous object
                _prevObj = obj;                
            }
        }

        //start next turn
        StartCoroutine(Turn());
    }

    //disable all buttons
    private void DisableButtons() {

        //iterate through buttons
        for (int i = 0; i < buttons.Length; i++) {

            //disable
            buttons[i].SetDisabled();
        }
    }

    //enable all buttons
    private void EnableButtons() {

        //iterate through buttons
        for (int i = 0; i < buttons.Length; i++) {

            //enable
            buttons[i].SetEnabled();
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

        //if moving to center platform
        if (theObj.name == buttons[0].name) {

            //modify tutorial canvas visibility
            ShowCanvas(canvasTutorial1);
            HideCanvas(canvasTutorial2);
        }

        //if moving to front platform
        else if (theObj.name == buttons[1].name) {

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

} //end class