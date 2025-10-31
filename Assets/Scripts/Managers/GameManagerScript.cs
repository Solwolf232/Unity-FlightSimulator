using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FlightState
{
    BoardingState,
    TakeOffState,
    InFlightState,
    LandingState,
    LandedState
}
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject airPortModel;
    [SerializeField] public Transform flightAttendantSpawnPos;
    [SerializeField] private Transform playerSpawnPos;


    [HideInInspector] public FlightState currentState;
    private const float TakeoffDuration = 36f; // Time until plane is airborne
    private const float AnimationBuffer = 15f;  // Time until TakeOff Finishes


    [Header("Level Diffucly")]
    [HideInInspector] public LevelDiff LevelDiff;


    [Header("Timer")]
    [SerializeField] GameObject FlightTimer;

    [Header("Level Diffuclty")]
    [SerializeField] LevelDiffData LevelDiffData;


    [Header("Courotines References")]
    private Coroutine takeoffCoroutine;
    private Coroutine copilotSoundCoroutine;
    private Coroutine takeoffSoundCoroutine;

    [Header("Backgrounds")]
    [SerializeField] private List<Material> SkyBoxes;
    public Material currentSkyBox;
    [SerializeField] GameObject RainDrops;


   [HideInInspector] public float currentTimerTime;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); 
    }

    private void Start()
    {
        LevelDiff = LevelDiffData.currentLevelDiff;

        Debug.Log("The Level Diffculty is " + LevelDiff.ToString());
        ChangeCurrentState(FlightState.BoardingState);
        ChangeBackGround();


    }



    private void EnviormentSettings()
    {

    }

    private void ChangeBackGround()
    {
        switch (LevelDiff)
        {
            case LevelDiff.Easy:
               currentSkyBox = RenderSettings.skybox = SkyBoxes[0];
                RainDrops.SetActive(false);

                break;
            case LevelDiff.Medium:
              currentSkyBox = RenderSettings.skybox = SkyBoxes[1];
                RainDrops.SetActive(false);
                break;

            case LevelDiff.Hard:
               currentSkyBox = RenderSettings.skybox = SkyBoxes[1];
               RainDrops.SetActive(true);
                SoundManagerScript.instance.PlayRainSound();
                break;

        }
    }

    public void ChangeCurrentState(FlightState newState)
    {
        currentState = newState;
        Debug.Log($"Flight state changed to: {newState}");

      

        CheckFor_Restarts(); // Checks If A Restart Button Has Occured Before Changing To A New State


        switch (currentState)
        {
            case FlightState.BoardingState:
                HandleBoarding();
                break;
            case FlightState.TakeOffState:
                takeoffCoroutine = StartCoroutine(HandleTakeoffSequence()); // Store coroutine reference

                break;
            case FlightState.InFlightState:
                HandleInFlight();
                break;

            case FlightState.LandingState:
                StartOfTheLanding();
                StartCoroutine(WaitForSeatBelt());
                break;
        }
    }

    #region Flight States

    private void HandleBoarding()
    {
        SpawnBoardingObjects(); // Spawns The Plane , Player & Attendent
        PlayBoardingSounds(); // Playes The Boarding Sounds (AirPlane Jet Engine Noise)
    }

    private IEnumerator HandleTakeoffSequence()
    {
        PlayTakeoffSounds();
        yield return new WaitForSeconds(TakeoffDuration); // Wait for sounds & turbulence
        PlayTakeoffAnimation();
        yield return new WaitForSeconds(AnimationBuffer); // Wait for animation to finish
        ChangeCurrentState(FlightState.InFlightState);
    }

    private void HandleInFlight()
    {

        GameObject FlightTimerClone = Instantiate(FlightTimer);
        TimerScript timeScript = FlightTimerClone.GetComponentInChildren<TimerScript>();
        switch (LevelDiff)
        {
            case LevelDiff.Easy:
              currentTimerTime = timeScript.timerTime = 15f * 60f;
      
                break;
            case LevelDiff.Medium:
              currentTimerTime =  timeScript.timerTime = 30f * 60f;
                break;
            case LevelDiff.Hard:
                currentTimerTime = timeScript.timerTime = 60f * 60f;
                break;
        }
    }


    private void HandleLandingState()
    {
        FlightAttendent_Trigger LandingTrigger = FindObjectOfType<FlightAttendent_Trigger>();

        if(!LandingTrigger.isActiveAndEnabled)
        LandingTrigger.gameObject.SetActive(true); // Landing Trigger (Used To Play The Attendent Final Goodbye)

        Player_SeatManager.instance.gameObject.transform.SetParent(null, true); // Remove The Player From Kid in AirPlane To Indenpent
        NpcMovement.instance.ShouldMove = true;
        NpcMovement.instance.ShouldMoveToExit = true;

        SoundManagerScript.instance.StartCoroutine(SoundManagerScript.instance.PlayLandingSound(8.5f)); 

        if (airPortModel != null)
        {
            Animator airPortAnim = airPortModel.GetComponent<Animator>();
            airPortAnim.Play("AirPort_Landing");
        }
        
        StartCoroutine(unFastenSeatBelt());
    }

    #endregion

    #region Helper Methods

    private void SpawnBoardingObjects()
    {
        if (SpawnObjects.instance != null && flightAttendantSpawnPos != null && playerSpawnPos != null)
        {
            SpawnObjects.instance.BoardingPlaneSpawning(flightAttendantSpawnPos, playerSpawnPos);
        }
    }

    private void PlayBoardingSounds()
    {
        SoundManagerScript.instance?.PlayJetSound();
        SoundManagerScript.instance?.PlayPeopleChatterSfx();
    }

    private void PlayTakeoffSounds()
    {
        if (SoundManagerScript.instance != null)
        {
         copilotSoundCoroutine = SoundManagerScript.instance.StartCoroutine(SoundManagerScript.instance.PlayCoPilotSound(5f));
         takeoffSoundCoroutine = SoundManagerScript.instance.StartCoroutine(SoundManagerScript.instance.PlayTakeOffSound(TakeoffDuration));
        }
    }

    private void PlayLandingSounds()
    {
        if(SoundManagerScript.instance != null)
        {
            SoundManagerScript.instance.PlayCoPliotLanding();
        }
    }

    private void PlayTakeoffAnimation()
    {
        if (airPortModel != null)
        {
            Animator animator = airPortModel.GetComponent<Animator>();
            animator.Play("Air_Port_TakeOff");
        }
    }
    private void ResetAirportAnimation()
    {
        if (airPortModel != null)
        {
            Animator airPortAnim = airPortModel.GetComponent<Animator>();

            airPortAnim.Play("AirPort-Idle");
        }
    }

    private IEnumerator unFastenSeatBelt()
    {
        yield return new WaitForSeconds(30f);
        Player_SeatManager.instance.Is_Seat_Belt_On = false;
        ChangeCurrentState(FlightState.LandedState); // Landed Safely
    }

    private IEnumerator WaitForSeatBelt()
    {
        // Wait until the seat belt is on
        yield return new WaitUntil(() => Player_SeatManager.instance.Is_Seat_Belt_On);

        // Run the landing state function
        HandleLandingState();
    }


    private void StartOfTheLanding()
    {
        Player_SeatManager.instance.Is_Seat_Belt_On = false;

        Player_SeatManager.instance.gameObject.transform.parent = null;


        PlayLandingSounds();


        if (Player_SeatManager.instance.Is_Sitted) // If Player Is Already Sitted Then Spawn The Tutorial Again
        {
            SeatScript.instance.SeatBeltTutorialMethod(true);
            SeatScript.instance.SpawnSeatBelts_Vr();
        }
    }
    #endregion


    #region LevelsSpeciels
    private bool isTurbulenceRunning = false;
    private float timer = 10f;
    private float time;

    private void RandomizeAirPlaneTurbalance()
    {
        isTurbulenceRunning = true;
        time = UnityEngine.Random.Range(20, 120);


        bool ShouldTurblance = PlayerCamPlay.instance != null;

        if (ShouldTurblance)
        {
            PlayerCamPlay.instance.StartCoroutine(PlayerCamPlay.instance.AirPlane_Turbalance());
            Debug.Log("Turbalance");
            timer = time;
            isTurbulenceRunning = false;


        }
    }

    private void Update()
    {
        if (currentState == FlightState.InFlightState && !isTurbulenceRunning)
            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    RandomizeAirPlaneTurbalance();
                }
            }

        
    }
    private void CheckFor_Restarts()
    {

        if (takeoffCoroutine != null && currentState == FlightState.TakeOffState)
        {
            StopCoroutine(takeoffCoroutine);
            takeoffCoroutine = null;

            if (copilotSoundCoroutine != null)
            {
                SoundManagerScript.instance.StopCoroutine(copilotSoundCoroutine);
                copilotSoundCoroutine = null;
            }
            if (takeoffSoundCoroutine != null)
            {
                SoundManagerScript.instance.StopCoroutine(takeoffSoundCoroutine);
                takeoffSoundCoroutine = null;
            }

            ResetAirportAnimation();
        }

    }


    #endregion
}
