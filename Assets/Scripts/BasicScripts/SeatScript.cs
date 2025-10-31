using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class SeatScript : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private Transform chairTransform;
    [SerializeField] private Transform SpawnNpcTransform;
    private Transform playerTransform;

    [Header("Variables")]
    private bool isAvailable = true;
    private Vector3 originalPlayerSize;

    [Header("Npc Related")]
    [SerializeField] private List<GameObject> customerNpcModels;

    [Header("Vr Tutorials")]
    [SerializeField] private GameObject seatBeltTutorial;
    private GameObject currentSeatBeltTutorial;




    [Header("Vr Related")]
    [SerializeField] GameObject SeatBeltRight;
    [SerializeField] GameObject SeatBeltLeft;


    [Header("References_Vr")]
    [SerializeField] private GameObject SeatBR;
    [SerializeField] private GameObject SeatBL;

    [Header("Instance")]
    public static SeatScript instance;


    [Header("Player Related")]
    [SerializeField] GameObject PlayerSittingModel;
    private GameObject PlayerSittingModel_Refrence; // To Be Deleted Later

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke(nameof(SetPlayerTransform), 0.3f);

        if (customerNpcModels != null)
        {
            RandomizeNpcSpawn();
        }
    }

    public void SetPlayerTransform()
    {
        PlayerCamPlay playerCameraPlayScript = FindObjectOfType<PlayerCamPlay>();
        if (playerCameraPlayScript != null)
        {
            playerTransform = playerCameraPlayScript.gameObject.transform;
            originalPlayerSize = playerTransform.localScale;
        }
        else
        {
            Debug.LogError("Player Is Not Found");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Chair Clicekd");
        bool ShouldSit = isAvailable && !Player_SeatManager.instance.Is_Sitted && GameManagerScript.instance.currentState != FlightState.LandedState;

        if (ShouldSit)
        {
            Player_SeatManager.instance.Is_Sitted = true;
            SitOnChair();
        }
    }



    public void OnSelectEntered()
    {
        if (isAvailable && !Player_SeatManager.instance.Is_Sitted && GameManagerScript.instance.currentState != FlightState.LandedState)
        {
            Player_SeatManager.instance.Is_Sitted = true;
            SitOnChair();
        }
        else if (Player_SeatManager.instance.Is_Sitted && GameManagerScript.instance.currentState == FlightState.LandedState)
        {
            Player_SeatManager.instance.Is_Sitted = false;
            SitOnChair();

        }
    }

    private void Update()
    {
        GetUpFromChair();
        PutSeatBeltOn_NonVr();


    }

    private void SitOnChair()
    {
        PlayerController playerMovement = playerTransform.GetComponent<PlayerController>();
        PlayerMovement_Vr playerMovement_Vr = playerTransform.GetComponent<PlayerMovement_Vr>();

   

        if (Player_SeatManager.instance.Is_Sitted)
        {
            DisableMovement(playerMovement, playerMovement_Vr);
            HandleSit();
        }
        else
        {
            EnableMovement(playerMovement, playerMovement_Vr);
            HandleGettingUp();
        }
    }

    private void HandleSit()
    {
        playerTransform.position = chairTransform.position;
        playerTransform.rotation = chairTransform.rotation;
        playerTransform.localScale = new Vector3(0.3f, 0.6f, 0.3f);

        SpawnSittingModel();

        currentSeatBeltTutorial = SpawnTutorial();
        currentSeatBeltTutorial.transform.SetParent(transform, true);
        SpawnSeatBelts_Vr();
    }

    private void HandleGettingUp()
    {
        // Moving player slightly outside the chair's position
        playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z);

        if (currentSeatBeltTutorial != null)
            DestroyTutorial(currentSeatBeltTutorial);


    }

    private void DisableMovement(PlayerController playerMovement, PlayerMovement_Vr playerMovement_Vr)
    {
        if (playerMovement != null)
            playerMovement.canMove = false;
        else if (playerMovement_Vr != null)
            playerMovement_Vr.CanMove = false;
    }

    private void EnableMovement(PlayerController playerMovement, PlayerMovement_Vr playerMovement_Vr)
    {
        playerTransform.localScale = originalPlayerSize;

        if (playerMovement != null)
            playerMovement.canMove = true;
        else if (playerMovement_Vr != null)
            playerMovement_Vr.CanMove = true;
    }

    private void RandomizeNpcSpawn()
    {
        int spawnNum;
        int randomPerson = Random.Range(0, 3);
        LevelDiff currentDifficulty = GameManagerScript.instance.LevelDiff;

        switch (currentDifficulty)
        {
            case LevelDiff.Easy:
                spawnNum = Random.Range(0, 25);
                if (spawnNum > 0 && spawnNum < 5)
                    SpawnNpc(customerNpcModels[randomPerson]);
                break;

            case LevelDiff.Medium:
                spawnNum = Random.Range(0, 10);
                if (spawnNum > 0 && spawnNum < 5)
                    SpawnNpc(customerNpcModels[randomPerson]);
                break;

            case LevelDiff.Hard:
                spawnNum = Random.Range(0, 7);
                if (spawnNum > 0 && spawnNum < 5)
                    SpawnNpc(customerNpcModels[randomPerson]);
                break;
        }
    }

    public void SeatBeltTutorialMethod(bool isSitting)
    {
        if (isSitting && currentSeatBeltTutorial == null)
        {
            Debug.Log("Tutorial Spawned!");
            currentSeatBeltTutorial = SpawnTutorial();
        }
    }

    private void PutSeatBeltOn_NonVr()
    {
        if (Input.GetKeyDown(KeyCode.L) && Player_SeatManager.instance.Is_Sitted)
        {
            if (currentSeatBeltTutorial != null)
            {
                DestroyTutorial(currentSeatBeltTutorial);
                Player_SeatManager.instance.Is_Seat_Belt_On = true;
                currentSeatBeltTutorial = null;

                SoundManagerScript.instance.PlaySeatBeltSound();


                GameObject airplane = transform.root.gameObject;
                Player_SeatManager.instance.transform.SetParent(airplane.transform, true);

                if (GameManagerScript.instance.currentState == FlightState.BoardingState) // Only Change State When First Seat Belted
                    Player_SeatManager.instance.OnSeatBeltOn();


            }
        }
    }

    public void PutSeatBeltOnVr()
    {

        if (currentSeatBeltTutorial != null)
        {
            DestroyTutorial(currentSeatBeltTutorial);

            //GameObject airplane = transform.root.gameObject;
            //Player_SeatManager.instance.transform.SetParent(airplane.transform, true);
        }

        Player_SeatManager.instance.Is_Seat_Belt_On = true;

        SoundManagerScript.instance.PlaySeatBeltSound();

        if (GameManagerScript.instance.currentState == FlightState.BoardingState) // Only Change State When Its First Time SeatBelting
            Player_SeatManager.instance.OnSeatBeltOn(); // TakeOff


    }






    private void GetUpFromChair()
    {
        bool shouldGetUp = Input.GetMouseButtonDown(1) && Player_SeatManager.instance.Is_Sitted && !Player_SeatManager.instance.Is_Seat_Belt_On;

        if (shouldGetUp)
        {
            GameObject myObject = GameObject.FindWithTag("BodyTorso");
            if(myObject != null)
                Destroy(myObject);


            Player_SeatManager.instance.Is_Sitted = false;
            SitOnChair();
        }
    }

    public void GetUpFromChairVr()
    {
        if (Player_SeatManager.instance.Is_Seat_Belt_On || Player_SeatManager.instance.Is_Sitted) return;


        
        Destroy(PlayerSittingModel_Refrence);
        Player_SeatManager.instance.Is_Sitted = false;
        SitOnChair();
    }


    public void SpawnSeatBelts_Vr()
    {
        GameObject Player = Player_SeatManager.instance.gameObject;

        if (Player == null)
            return;

        Vector3 spawnPosR = new Vector3(Player.transform.position.x - 0.3f, Player.transform.position.y + 0.08f, Player.transform.position.z - 0.8f);
        Vector3 spawnPosL = new Vector3(Player.transform.position.x + 0.3f, Player.transform.position.y + 0.08f, Player.transform.position.z - 0.8f);



        SeatBR = Instantiate(SeatBeltRight, spawnPosR, Quaternion.identity);
        SeatBR.transform.SetParent(transform, true);

        SeatBL = Instantiate(SeatBeltLeft, spawnPosL, Quaternion.identity);
        SeatBL.transform.SetParent(transform, true);
        Debug.Log("Spawned belts Successfuly");

    }

    public void DestorySeatBelts_Vr(GameObject SeatBl, GameObject SeatBr)
    {
        Destroy(SeatBr);
        Destroy(SeatBl);
    }



    private GameObject SpawnTutorial()
    {
        Vector3 tutorialSpawnPoint = new Vector3(playerTransform.position.x, playerTransform.position.y+0.2f, playerTransform.position.z - 1f);
        return Instantiate(seatBeltTutorial, tutorialSpawnPoint, Quaternion.identity);
    }

    private void DestroyTutorial(GameObject tutorial)
    {
        if (tutorial != null)
        {
            currentSeatBeltTutorial = null;
            Destroy(tutorial);
            DestorySeatBelts_Vr(SeatBR, SeatBL);
            Debug.Log("Destroyed Tutorial! & Belts");

        }
        else
        {
            Debug.Log("Tutorial is Null" + tutorial.ToString());
        }
    }


    private void SpawnSittingModel()
    {
        Vector3 NewPosition = new Vector3(playerTransform.position.x, playerTransform.position.y - 0.8f, playerTransform.position.z);
        Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);

        PlayerSittingModel_Refrence = Instantiate(PlayerSittingModel, NewPosition, rotation);
    }

    private void SpawnNpc(GameObject npcModel)
    {
        Quaternion desiredRotation = Quaternion.Euler(0, 180, 0);
        GameObject newCustomerNpc = Instantiate(npcModel, SpawnNpcTransform.position, desiredRotation);
        isAvailable = false;
        newCustomerNpc.transform.SetParent(transform, true);
        Debug.Log("Customer Spawned!");
    }
}