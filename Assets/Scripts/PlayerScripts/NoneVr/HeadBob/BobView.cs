using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BobView : MonoBehaviour
{

    [Header("Bobbing Settings")]
    public float bobbingSpeed = 5f; // Speed of the bobbing
    public float bobbingAmount = 0.1f; // Intensity of the bobbing
    public float movementThreshold = 0.1f; // Minimum speed to trigger bobbing
    public float smoothStopSpeed = 5f; // Speed of smooth stopping

    private float defaultYPos;
    private float timer = 0f;

    private CharacterController characterController;

    void Start()
    {
        // Save the starting Y position of the camera
        defaultYPos = transform.localPosition.y;
        characterController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        Vector3 currentPos = transform.localPosition;

        if (characterController != null && characterController.velocity.magnitude > movementThreshold)
        {
            // Update the bobbing timer
            timer += Time.deltaTime * bobbingSpeed;

            // Calculate the new Y position
            float newY = defaultYPos + Mathf.Sin(timer) * bobbingAmount;

            // Apply the bobbing
            transform.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
        }
        else
        {
            // Smoothly return to the default position
            float newY = Mathf.Lerp(currentPos.y, defaultYPos, Time.deltaTime * smoothStopSpeed);
            transform.localPosition = new Vector3(currentPos.x, newY, currentPos.z);

            // Reset the timer slowly for smoother restarting
            timer = Mathf.Lerp(timer, 0f, Time.deltaTime * smoothStopSpeed);
        }
    }
}
