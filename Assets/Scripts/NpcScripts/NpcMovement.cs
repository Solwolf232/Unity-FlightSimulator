using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovement : MonoBehaviour
{

    public static NpcMovement instance;

    // ----- Booleans -----
    [Header("Booleans")]
    public bool ShouldMove;
    private bool ReturnsToSpot = false;
    [HideInInspector] public bool ShouldMoveToExit;
    [HideInInspector] public bool MovingTowardsPlayer;
    private bool ShouldGive; // Attendant should give water
    [SerializeField] bool moveToFront;
    

    // ----- Transforms -----
    [Header("Transforms")]
    public Transform targetBack; //Back Target
    public Transform targetFront; //Front Target
    private Transform originalPos;
    private Transform ButtonPos = null;

    // ----- Movement -----
    [Header("Movement")]
    public float moveSpeed;

    // ----- Components -----
    [Header("Components")]
    private Animator Npcanimator;
    private NavMeshAgent NavMeshAgentAi;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Npcanimator = GetComponentInChildren<Animator>();
        NavMeshAgentAi = GetComponent<NavMeshAgent>();
        originalPos = GameManagerScript.instance.flightAttendantSpawnPos;


        moveToFront = false;
    }

    private void Update()
    {
        if (!MovingTowardsPlayer && ShouldMove && !ShouldMoveToExit)
        {
            if (moveToFront)
            {
                MoveAiToTarget(targetFront);
            }
            else if(!moveToFront)
            {
                MoveAiToTarget(targetBack);
            }

        } //Returns To Spot

        else if (MovingTowardsPlayer && ShouldMove && !ShouldMoveToExit) //Moving Towrads The Player
        {
            MoveNpcToButton(ButtonPos);
        }
        if(!MovingTowardsPlayer && ShouldMoveToExit && ShouldMove) //Moving To The Exit
        {
            MoveNpcToExit(originalPos);
        }

        PlayNpcAnimations();
    }


    public void SetButton(Transform buttonPos)
    {
        ButtonPos = null;
        ButtonPos = buttonPos;
    }

    public void MoveAiToTarget(Transform target)
    {

        if (target != null) // If The Npc Stopped Talking
        {
            NavMeshAgentAi.SetDestination(target.position); // Setting Target

            if (!NavMeshAgentAi.pathPending &&
                NavMeshAgentAi.remainingDistance <= NavMeshAgentAi.stoppingDistance &&
                (!NavMeshAgentAi.hasPath || NavMeshAgentAi.velocity.sqrMagnitude == 0f))
            {
                ShouldMove = false;
                ReturnsToSpot = false;
            }
        }
        else
        {
            Debug.Log("target is Null");
        }
    }


    public void MoveNpcToButton(Transform Button)
    {
        if (Button != null)
        {

            NavMeshAgentAi.SetDestination(Button.position);

            if (!NavMeshAgentAi.pathPending &&
                NavMeshAgentAi.remainingDistance <= NavMeshAgentAi.stoppingDistance &&
                (!NavMeshAgentAi.hasPath || NavMeshAgentAi.velocity.sqrMagnitude == 0f))
            {
                ShouldMove = false;
                MovingTowardsPlayer = false;
                ShouldGive = true;
                Attendent_Call_Script.instance.GlassOfWaterAnim();

                StartCoroutine(NpcBackToWork());
            }
        }
    }
    public void MoveNpcToExit(Transform Exit)
    {
        if (Exit != null)
        {
           Debug.Log("Moving");

            NavMeshAgentAi.SetDestination(Exit.position);

            if (!NavMeshAgentAi.pathPending &&
                NavMeshAgentAi.remainingDistance <= NavMeshAgentAi.stoppingDistance &&
                (!NavMeshAgentAi.hasPath || NavMeshAgentAi.velocity.sqrMagnitude == 0f))
            {
                ShouldMove = false;
                
            }
        }
    }

    private IEnumerator NpcBackToWork()
    {
        yield return new WaitForSeconds(2.5f);
        ShouldMove = true;
        ReturnsToSpot = true;
        ShouldGive = false;
        moveToFront = !moveToFront;
    }


    private void PlayNpcAnimations()
    {
        if (ShouldMove && (!MovingTowardsPlayer || ShouldMoveToExit) && !ReturnsToSpot)
        {
            Npcanimator.Play("Flight_Attendent_Walk");

        }
        else if(!ShouldMove && !MovingTowardsPlayer && !ShouldGive && !FlightNpcSpeak.instance.IsSpeakingFlag)
        {
            Npcanimator.Play("FemaleIdle");
        }
        else if(ShouldMove && (MovingTowardsPlayer || ReturnsToSpot))
        {
            Npcanimator.Play("Flight_Attendent_Walk_Trolley");
        }
        else if(!ShouldMove && !MovingTowardsPlayer && ShouldGive)
        {
            Npcanimator.Play("Flight_Attendent_Give_Trolley");
        }
        
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetBack.position, 1f);
        Gizmos.DrawLine(transform.position, targetBack.position);

        Gizmos.DrawSphere(targetFront.position, 1f);
        Gizmos.DrawLine(transform.position,targetFront.position);
       
    }
}
