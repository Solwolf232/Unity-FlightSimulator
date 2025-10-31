using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video_Script : MonoBehaviour
{

    [Header("Button Essentails")]
    private bool is_Video_Open = false;
    private Animator buttonAnim;

    [SerializeField] private GameObject Video_gm;
  //  [SerializeField] private Transform Video_pos;

    private GameObject Video;
    private void Start()
    {
        buttonAnim = GetComponent<Animator>();
        Video_gm.SetActive(false);
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked Button");
        if (!is_Video_Open)
        {
            Video_gm.SetActive(true);
            is_Video_Open =true;    
        }
        else
        {
            Video_gm.SetActive(false);
            is_Video_Open=false;
        }
    }

   

    public void OnSelectEntered()
    {
        if (!is_Video_Open)
        {
            Video_gm.SetActive(true);
            is_Video_Open = true;
        }
        else
        {
            Video_gm.SetActive(false);
            is_Video_Open = false;
        }
    }
}
