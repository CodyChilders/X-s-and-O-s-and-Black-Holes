using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;

    private void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        if (source == null)
        {
            //try to locate the audio source
            source = GetComponent<AudioSource>();
            if (source == null) //failed to locate for some reason
            {
                Debug.LogError("Unable to locate audio source component.");
                return;
            }
        }
        if (clip == null)
        {
            Debug.LogError("Unable to play a null audio clip.");
            return;
        }
        if (volume < 0f || volume > 1f)
        {
            Debug.LogWarningFormat("Volume for clip {0} is out of range [0, 1]: {1}", clip, volume);
            if (volume < 0f)
                volume = 0f;
            else
                volume = 1f;
        }
        source.PlayOneShot(clip, volume);
    }

    public AudioClip tttMusic;
    public void StartTTTMusic()
    {
        PlayOneShot(tttMusic);
    }

    public AudioClip swMusic;
    public void StartSWMusic()
    {
        PlayOneShot(swMusic);
    }

    public AudioClip placePiece;
    public void TTTPlacePiece() 
    { 
        PlayOneShot(placePiece); 
    }

    public AudioClip failPlacePiece;
    public void TTTFailPlacePiece() 
    { 
        PlayOneShot(failPlacePiece); 
    }

    public AudioClip edgeBoundry;
    public void TTTEdgeBoundry() 
    { 
        PlayOneShot(edgeBoundry); 
    }

    public AudioClip transitionToSW;
    public void TTTTransitionToSW() 
    { 
        PlayOneShot(transitionToSW); 
    }

    public AudioClip completeMatch;
    public void TTTCompleteMatch() 
    { 
        PlayOneShot(completeMatch); 
    }

    public AudioClip fireWeapon;
    public void SWFireWeapon() 
    { 
        PlayOneShot(fireWeapon); 
    }

    public AudioClip failFireWeapon;
    public void SWFailFireWeapon() 
    { 
        PlayOneShot(failFireWeapon);
    }

    public AudioClip transitionToTTT;
    public void SWTransitionToTTT() 
    { 
        PlayOneShot(transitionToTTT); 
    }

    public AudioClip incomingProjectileAlarm;
    public void SWIncomingProjectileAlarm() 
    { 
        PlayOneShot(incomingProjectileAlarm); 
    }

    public AudioClip projectileDetonateHarmlessly;
    public void SWProjectileDenotateHarmlessly() 
    {
        PlayOneShot(projectileDetonateHarmlessly);
    }

    public AudioClip engineThrottleUp;
    public void SWEngineThrottleUp() 
    {
        PlayOneShot(engineThrottleUp);
    }

    public AudioClip engineThrottleDown;
    public void SWEngineThrottleDown() 
    { 
        PlayOneShot(engineThrottleDown); 
    }

}
