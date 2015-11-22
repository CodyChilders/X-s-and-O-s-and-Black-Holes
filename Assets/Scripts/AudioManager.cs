using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public AudioClip tttMusic;
    public void StartTTTMusic()
    {

    }

    public AudioClip swMusic;
    public void StartSWMusic()
    {
        
    }

    public AudioClip placePiece;
    public void TTTPlacePiece() { }

    public AudioClip failPlacePiece;
    public void TTTFailPlacePiece() { }

    public AudioClip moveCursor;
    public void TTTMoveCursor() { }

    public AudioClip transitionToSW;
    public void TTTTransitionToSW() { }

    public AudioClip completeMatch;
    public void TTTCompleteMatch() { }

    public AudioClip fireWeapon;
    public void SWFireWeapon() { }

    public AudioClip failFireWeapon;
    public void SWFailFireWeapon() { }

    public AudioClip transitionToTTT;
    public void SWTransitionToTTT() { }

    public AudioClip incomingProjectileAlarm;
    public void SWIncomingProjectileAlarm() { }

    public AudioClip projectileDetonateHarmlessly;
    public void SWProjectileDenotateHarmlessly() { }

    public AudioClip engineThrottleUp;
    public void SWEngineThrottleUp() { }

    public AudioClip engineThrottleDown;
    public void SWEngineThrottleDown() { }

}
