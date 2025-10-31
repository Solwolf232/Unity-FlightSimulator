using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAttendent_Trigger : MonoBehaviour
{

    [SerializeField] GameObject airPlaneDoors;

    public bool BoardingState;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") && GameManagerScript.instance.currentState == FlightState.BoardingState && BoardingState)
        {

            PlayerCamPlay playerCamPlay = other.gameObject.GetComponent<PlayerCamPlay>();
            playerCamPlay.PlayerLookAtNpc(true);
            
            if(airPlaneDoors != null)
            airPlaneDoors.SetActive(true);

            Destroy(this.gameObject);
            Debug.Log("Triggered");
        }
        else if(other.gameObject.CompareTag("Player") && GameManagerScript.instance.currentState == FlightState.LandedState && !BoardingState)
        {
            PlayerCamPlay.instance.PlayerLookAtNpc(true);
            FlightNpcSpeak.instance.IsSpeaking();



            Destroy(this.gameObject);


           
        }
    }
}
