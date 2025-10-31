using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Menu_SoundManager : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioSource audiosrc;
    [SerializeField] private AudioClip clip;

    [Header("UI Buttons")]
    [SerializeField] private AudioSource UiAudioSrc;
    [SerializeField] private AudioClip buttonClip;
    private void Start()
    {
        PlayBackGroundMusic();
    }
    private void PlayBackGroundMusic()
    {
        if (clip != null && audiosrc != null)
        {
            Debug.Log("Playing!");
            audiosrc.clip = clip;
            audiosrc.Play();
        }
    }


    private void PlayButtonSfx()
    {
           if(buttonClip != null)
           {
            UiAudioSrc.clip = buttonClip;
            UiAudioSrc.Play();
           }
       
        
    }
 
}
