using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightNpcSpeak : MonoBehaviour
{
    [Header("Npc Components")]
    private Transform playerTransform;
    private Animator npcAnimator;
    private NpcMovement NpcMovementScript;

    [Header("PlayerCamera")]
    PlayerCamPlay playerCameraPlayScript;
    PlayerController playerController;
    PlayerMovement_Vr playermovement_Vr;

    [Header("Sound")]
    [SerializeField] private AudioClip FlightAttendentSpeaking;

    public static FlightNpcSpeak instance;

    public bool IsSpeakingFlag = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        npcAnimator = GetComponentInChildren<Animator>();
        NpcMovementScript = GetComponent<NpcMovement>();
        GetPlayerComponenets();

    }

    // Update is called once per frame
    private void Update()
    {
        IsSpeaking();

    }

    private void LateUpdate()
    {
        if (playerController != null)
        {
            if (playerController.LooksAtAttendent)
            {
                NpcLooking(playerTransform);
            }
        }
        else if (playermovement_Vr != null)
        {
            if (playermovement_Vr.LooksAtAttendent)
            {
                NpcLooking(playerTransform);
            }
        }


    }

    private IEnumerator FinishTalkAnimation()
    {
        IsSpeakingFlag = true;
        yield return new WaitForSeconds(4.2f);
        IsSpeakingFlag = false;
    }

    public void IsSpeaking()
    {
        if (playerCameraPlayScript != null)
        {
            if (playerCameraPlayScript.IsLookingAtNpc)
            {
                StartCoroutine(FinishTalkAnimation()); // To Not Override Other Animations
                npcAnimator.Play("Talking");


                if (GameManagerScript.instance.currentState == FlightState.BoardingState) 
                    SoundManagerScript.instance.PlayNpcSpeak(FlightAttendentSpeaking);

                else if (GameManagerScript.instance.currentState == FlightState.LandedState)
                    SoundManagerScript.instance.PlayFlightAttendentLanding();

                playerCameraPlayScript.IsLookingAtNpc = false;

            }

        }

    }
    public void NpcLooking(Transform target)
    {
        Vector3 direction = target.position - transform.position; // Finding The Direction Of The Target
        float lookspeed = 3f; // Look Speed for Smooth Looking

        direction.y = 0; // Changing only the direciton of the x value so y will be 0

        Quaternion targetRotation = Quaternion.LookRotation(direction); // Finding the rotation of the distance so the player will look at

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookspeed); // smoothly rotating the view to the target 


        Invoke(nameof(StopSpeaking), 3.5f);
    }

    public void GetPlayerComponenets()
    {

        playerCameraPlayScript = FindObjectOfType<PlayerCamPlay>();
        if (playerCameraPlayScript != null)
        {
            
            playerController = playerCameraPlayScript.gameObject.GetComponent<PlayerController>();
            playermovement_Vr = playerCameraPlayScript.gameObject.GetComponent<PlayerMovement_Vr>();
            playerTransform = playerCameraPlayScript.gameObject.transform;
        }
        else
        {
            Debug.LogError("Player Is Not Found");
        }
    }

    private void StopSpeaking()
    {
        if(playerController != null)
        playerController.LooksAtAttendent = false;

        else if(playermovement_Vr != null)
        playermovement_Vr.LooksAtAttendent = false;


        NpcMovementScript.ShouldMove = true; // Start Walking After Dialouge Done
    }
}
