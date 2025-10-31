using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AnimateHandController : MonoBehaviour
{
    [Header("Refrences")]
    public InputActionReference gripInputAction;
    public InputActionReference triggerInputAction;

    [Header("Values")]
    private float _gripValue;
    private float _triggerValue;

    [Header("Componenets")]
    private Animator hands_Animator;

    private void Start()
    {
        hands_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimateGrip();
        AnimateTrigger();
    }

    public void AnimateGrip()
    {
        _gripValue = gripInputAction.action.ReadValue<float>();
        hands_Animator.SetFloat("Grip",_gripValue);
    }

    public void AnimateTrigger()
    {
        _triggerValue = triggerInputAction.action.ReadValue<float>();
        hands_Animator.SetFloat("Trigger",_triggerValue);
    }
}
