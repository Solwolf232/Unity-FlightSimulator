using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement_Vr : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.0f; // Speed of player movement
    public XRNode inputSource = XRNode.LeftHand; // Joystick input from left controller
    public float gravity = -9.81f; // Gravity effect on the player

    [Header("Rotation Settings")]
    public XRNode rotationSource = XRNode.RightHand; // Joystick input from right controller
    public float rotationSpeed = 45.0f; // Rotation speed in degrees per second

    private Vector2 moveInputAxis; // Stores left joystick input
    private Vector2 rotationInputAxis; // Stores right joystick input
    private CharacterController characterController; // For handling movement
    private XROrigin xrOrigin; // Reference to XR Rig
    private Vector3 fallVelocity; // Handles falling effect

    [HideInInspector] public bool CanMove = true;
    public bool LooksAtAttendent = false;

    [Header("Foot Steps Sound")]
    [SerializeField] private AudioSource PlayeraudioSrc;
    [SerializeField] private AudioClip playerFootSteps;

    private void Start()
    {
        // Get references
        characterController = GetComponent<CharacterController>();
        xrOrigin = GetComponentInChildren<XROrigin>();

        CanMove = true;

        if (characterController == null)
        {
            Debug.LogError("CharacterController is missing from the player object.");
        }

        if (xrOrigin == null)
        {
            Debug.LogError("XROrigin is missing from the player object.");
        }
    }

    private void Update()
    {
        // Get joystick input for movement
        InputDevice moveDevice = InputDevices.GetDeviceAtXRNode(inputSource);
        moveDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out moveInputAxis);

        // Get joystick input for rotation
        
            InputDevice rotationDevice = InputDevices.GetDeviceAtXRNode(rotationSource);
            rotationDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationInputAxis);
        
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            MovePlayer();
            ApplyGravity();
        }

        RotatePlayer();
    }

    private void MovePlayer()
    {
        if (moveInputAxis == Vector2.zero)
        {
            if (PlayeraudioSrc.isPlaying)
            {
                PlayeraudioSrc.Stop();
            }
            return;
        }

        if (!PlayeraudioSrc.isPlaying)
        {
            PlayeraudioSrc.clip = playerFootSteps;
            PlayeraudioSrc.loop = true; // Loop the sound while moving
            PlayeraudioSrc.volume = 0.6f;
            PlayeraudioSrc.Play();
        }

        // Get forward and right directions relative to the headset
        Vector3 forward = xrOrigin.Camera.transform.forward;
        Vector3 right = xrOrigin.Camera.transform.right;

        // Flatten the directions to avoid upward/downward movement
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Calculate move direction
        Vector3 moveDirection = (forward * moveInputAxis.y + right * moveInputAxis.x) * moveSpeed;

        // Apply movement
        characterController.Move(moveDirection * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {


        if (rotationInputAxis.x == 0) return;

        // Calculate rotation amount
        float rotation = rotationInputAxis.x * rotationSpeed * Time.fixedDeltaTime;

        // Apply rotation to the XR Origin instead of the whole player
        xrOrigin.transform.Rotate(Vector3.up, rotation);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            fallVelocity = Vector3.zero; // Reset gravity effect if grounded
        }
        else
        {
            fallVelocity += Vector3.up * gravity * Time.fixedDeltaTime; // Apply gravity
        }

        characterController.Move(fallVelocity * Time.fixedDeltaTime);
    }
}
