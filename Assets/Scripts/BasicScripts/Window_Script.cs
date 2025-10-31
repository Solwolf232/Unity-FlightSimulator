using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR;

public class Window_Script : MonoBehaviour
{
    private Animator window_Animator;
    private bool is_Open= false;
    private bool CanOpen = true;

    private float timer;
    private float time = 1.3f;
    private void Start()
    {
        window_Animator = GetComponentInParent<Animator>();
       
    }
    

    public void OnSelectEntered()
    {
        if (!is_Open && CanOpen)
        {
            window_Animator.Play("CurtainOpen");
            is_Open = true;

            CanOpen = false; // Delay For Opening And Closing
            timer = time;


            SoundManagerScript.instance.PlayCurtainSound();
        }
        else if(is_Open && CanOpen)
        {
            window_Animator.Play("CurtainClosed");
            is_Open = false;

            CanOpen = false;
            timer = time;
            SoundManagerScript.instance.PlayCurtainSound();

        }
    }


    private void OnMouseDown()
    {
        Debug.Log("Clicked WIndow");
        if (!is_Open && CanOpen)
        {
            window_Animator.Play("CurtainOpen");
            is_Open = true;

            CanOpen = false; // Delay For Opening And Closing
            timer = time;

            SoundManagerScript.instance.PlayCurtainSound();

        }
        else if(is_Open && CanOpen)
        {
            window_Animator.Play("CurtainClosed");
            is_Open = false;

            CanOpen = false;
            timer = time;
            SoundManagerScript.instance.PlayCurtainSound();

        }
    }


    public void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                CanOpen = true;
            }
        }
    }
}
