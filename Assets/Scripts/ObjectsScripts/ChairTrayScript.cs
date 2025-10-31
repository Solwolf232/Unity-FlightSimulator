using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairTrayScript : MonoBehaviour
{

    private Animator animator;
    private bool IsOpen;
    private void Start()
    {
        IsOpen = true;
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked Tray");

        IsOpen = !IsOpen;

        animator.SetBool("IsOpen",IsOpen);
    }

    public void OnSelectEntered()
    {
        IsOpen = !IsOpen;

        animator.SetBool("IsOpen", IsOpen);
    }
}
