/*
SetMaterialColor

Set the rgb values of a material color in 
the Unity Inspector. Attach to an object 
with a valid renderer and material.

Copyright John M. Quick
*/

using UnityEngine;

public class SetMaterialColor : MonoBehaviour {

    //color values
    public float r, g, b;

    //init
    void Start() {

        //set color
        gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

} //end class