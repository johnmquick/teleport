/*
TitleManager

Manages the title scene.

Copyright John M. Quick
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	//load scenes based on button presses
    //mouse
    public void OnMouseMode() {
        SceneManager.LoadScene(1);
    }

    //leap
    public void OnLeapMode() {
        SceneManager.LoadScene(2);
    }

} //end class