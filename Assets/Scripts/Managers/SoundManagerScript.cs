using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffectDict
{
    public string npcName;
    public AudioClip audioClip;
}


public class SoundManagerScript : MonoBehaviour
{
    [Header("Sounds Effects Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource sfxSource2;
    [SerializeField] private AudioSource Background_sfxSource;
    [SerializeField] private AudioSource Weather_sfxSource;

    [Header("Sound Effects")]
    [SerializeField] public List<AudioClip> soundEffects;

    [Header("Background Music")]
    private bool Test;

    [Header("Objects Sound Effects")]
    private bool Test2;

    [Header("Instance")]
    public static SoundManagerScript instance;

    #region On Start Methods
    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }
    #endregion


    #region Npcs Sound
    public void PlayNpcSpeak(AudioClip npcSound)
    {
        if (sfxSource != null) // if the sound Effect source isnt null
        {
            sfxSource.PlayOneShot(npcSound);
        }
        else
        {
            Debug.LogWarning($"Sound {sfxSource} not found!");
        }
    }


    #endregion

    #region BackgroundSounds
    public void PlayJetSound()
    {
        if (soundEffects[0] != null)
        {
            Background_sfxSource.clip = soundEffects[0];
            Background_sfxSource.loop = true;           
            Background_sfxSource.Play();
            Background_sfxSource.volume = 0.5f;

        }
    }

    public void PlaySeatBeltSound()
    {
        if(soundEffects[1] != null)
        {
            sfxSource.clip = soundEffects[1];
            sfxSource.time = 1.2f;
            sfxSource.Play();
        }
    }

    public IEnumerator PlayCoPilotSound(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (soundEffects[2] != null)
        {
            sfxSource.clip = soundEffects[2];
            sfxSource.PlayOneShot(sfxSource.clip);
        }

    }

    public IEnumerator PlayTakeOffSound(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if(soundEffects[3] != null)
        {
            sfxSource.clip = soundEffects[3];
            sfxSource.time = 2f;
            sfxSource.Play();
        }
    }


    public void PlayAttendentButtonSound()
    {
        if (soundEffects[4] != null)
        {
            sfxSource.clip = soundEffects[4];
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }



    public void PlayCurtainSound()
    {
        if (soundEffects[5] != null)
        {
            sfxSource2.Stop(); 
            sfxSource2.clip = soundEffects[5];
            sfxSource2.time = 4.35f;
            sfxSource2.Play();
        }
    }


    public void PlayPeopleChatterSfx()
    {
        if(soundEffects[6] != null)
        {
            sfxSource.clip = soundEffects[6];
            sfxSource.Play();
        }
    }

    public void PlayAirPlaneTurblance()
    {
        if (soundEffects[7] != null)
        {
            sfxSource2.clip = soundEffects[7];
            sfxSource2.PlayOneShot(sfxSource2.clip);
        }
    }



    public void PlayCoPliotLanding()
    {
        if(soundEffects[8] != null)
        {
            sfxSource2.clip = soundEffects[8];
            sfxSource2.PlayOneShot(sfxSource2.clip);
        }
    }


    public void PlayFlightAttendentLanding()
    {
        if (soundEffects[9] != null)
        {
            sfxSource2.clip = soundEffects[9];
            sfxSource2.PlayOneShot(sfxSource2.clip);
        }
    }

    public void PlayRainSound()
    {
        if (soundEffects[10] != null)
        {
            Weather_sfxSource.clip = soundEffects[10];
            Weather_sfxSource.loop = true;
            Weather_sfxSource.Play();
            Weather_sfxSource.volume = 0.8f;

        }
    }

    public IEnumerator PlayLandingSound(float seconds)
    {

        yield return new WaitForSeconds(seconds);

        if (soundEffects[11] != null)
        {
            sfxSource.clip = soundEffects[11];
            sfxSource.PlayOneShot(sfxSource.clip);
        }
    }


    public void StopAllSounds()
    {
        if(sfxSource.clip != null)
        {
            sfxSource.Stop();
            sfxSource.clip = null;
        }

        if(sfxSource2.clip != null)
        {
            sfxSource.Stop();
            sfxSource2.clip = null;
        }
    }
        #endregion
    
}

