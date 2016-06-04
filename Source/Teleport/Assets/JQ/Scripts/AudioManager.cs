/*
AudioManager
Manages audio throughout the application. 

Copyright John M. Quick
*/

using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//volume limits
    public float bgmVolMin, bgmVolMax;

    //whether currently fading
    public bool bgmIsFading;

    //whether fading in or out
    public bool bgmIsFadingIn;
	
	//duration of fade, in seconds
    public float bgmFadeDuration;

    //time at which latest fade started
    private float _bgmFadeStartTime;

    //audio sources
    //defined in Unity Inspector
    public AudioSource bgmSource, sfxSource;

    //audio clips
    //defined in Unity Inspector
    //music
    public AudioClip bgm;

    //sound effects
    public AudioClip sfxSelect;
    public AudioClip sfxConfirm;

    //singleton instance
    private static AudioManager _Instance;

    //public getter
    public static AudioManager Instance {
        get { return _Instance; }
    }

    //awake
    void Awake() {

        //check for existing instance
        //if no instance
        if (_Instance == null) {

            //find object in scene
            _Instance = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
			
			//prevent destruction
			DontDestroyOnLoad(_Instance);
        }
    }

    //update
    void Update() {

        //if fading
        if (bgmIsFading == true) {

            //check fade
            CheckFade();
        }
    }

    //play an audio clip using a given source
    public void PlayClipFromSource(AudioClip theClip, AudioSource theSource) {

        //stop
        theSource.Stop();

        //set the clip
        theSource.clip = theClip;

        //play
        theSource.Play();
    }

    //play an audio clip using a given source
    //delay for transitions, animations, etc.
    public IEnumerator PlayClipFromSource(AudioClip theClip, AudioSource theSource, float theDelay) {

        //delay
        yield return new WaitForSeconds(theDelay);

        //stop
        theSource.Stop();

        //set the clip
        theSource.clip = theClip;

        //play
        theSource.Play();
    }

    //stop a given source
    public void StopSource(AudioSource theSource) {

        //stop
        theSource.Stop();
    }

    //toggle fade for music
    public void ToggleFade() {

        //reverse fade direction
        bgmIsFadingIn = !bgmIsFadingIn;

        //update start time
        _bgmFadeStartTime = Time.time;

        //toggle flag
        bgmIsFading = true;
    }

    //check fade for music
    private void CheckFade() {

        //calculate fade duration thus far
        float fadeDuration = Time.time - _bgmFadeStartTime;

        //convert to percentage
        float fadePct = Mathf.Clamp01(fadeDuration / bgmFadeDuration);

        //retrieve current volume
        float vol = bgmSource.volume;

        //check fade direction
        switch (bgmIsFadingIn) {

            //fade in
            case true:

                //current vol is less than max
                if (vol < bgmVolMax) {

                    //update vol
                    vol = fadePct;
                }

                //vol has reached max
                else {

                    //update vol
                    vol = bgmVolMax;

                    //stop fade
                    bgmIsFading = false;
                }

                break;

            //fade out
            case false:

                //current vol is greater than min
                if (vol > bgmVolMin) {

                    //update vol
                    vol = 1.0f - fadePct;
                }

                //vol has reached min
                else {

                    //update vol
                    vol = bgmVolMin;

                    //stop fade
                    bgmIsFading = false;
                }

                break;

            //default
            default:
                break;
        }

        //update vol
        bgmSource.volume = vol;
    }

} //end class