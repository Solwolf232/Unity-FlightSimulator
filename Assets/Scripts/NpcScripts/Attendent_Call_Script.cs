using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Attendent_Call_Script : MonoBehaviour
{

    [SerializeField] Transform AttendentPos; // For The Attendent To Go To
    [SerializeField] GameObject WaterGlassPrefab;

    public static Attendent_Call_Script instance;

    private void Awake()
    {
        instance = this;
    }
    private void OnMouseDown()
    {
        bool ShouldCallAttendent = Player_SeatManager.instance.Is_Seat_Belt_On && this.enabled && GameManagerScript.instance.currentState == FlightState.InFlightState;

        if (ShouldCallAttendent)
        {
            Debug.Log("Clicked Button!");
            NpcMovement.instance.SetButton(AttendentPos.transform);
            NpcMovement.instance.MovingTowardsPlayer = true;
            NpcMovement.instance.ShouldMove = true;

            SoundManagerScript.instance.PlayAttendentButtonSound(); // Play The Sound
            StartCoroutine(DisableForAMin());
        }
       

    }

    public void OnSelectEntered()
    {
        bool ShouldCallAttendent = Player_SeatManager.instance.Is_Seat_Belt_On && this.enabled && GameManagerScript.instance.currentState == FlightState.InFlightState;

        if (ShouldCallAttendent)
        {
            Debug.Log("Clicked Button!");
            NpcMovement.instance.SetButton(AttendentPos.transform);
            NpcMovement.instance.MovingTowardsPlayer = true;
            NpcMovement.instance.ShouldMove = true;


            SoundManagerScript.instance.PlayAttendentButtonSound();

            StartCoroutine(DisableForAMin());
        }
    }

    private IEnumerator DisableForAMin() // Disabling The Abillty To Call The Flight Attendent Over and Over
    {
        this.enabled = false;
        yield return new WaitForSeconds(420f);
        Debug.Log("Can Call Again");
        this.enabled = true;
    }

    public void GlassOfWaterAnim()
    {
        PlayerCamPlay playerObject = PlayerCamPlay.instance.gameObject.GetComponent<PlayerCamPlay>();

        if(playerObject != null)
        {

            
            Animator playerAnim = playerObject.GetComponent<Animator>();

            if(playerAnim != null)
            playerAnim.Play("DrinkWater");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttendentPos.position, 0.2F);
        Gizmos.DrawLine(AttendentPos.position, transform.position);
    }
}
