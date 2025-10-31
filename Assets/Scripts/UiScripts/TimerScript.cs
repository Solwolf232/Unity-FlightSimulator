using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TimerScript : MonoBehaviour
{
    [HideInInspector] public float timerTime;
    [HideInInspector] public float time_spent;

    private float totaltime;
    private TextMeshProUGUI timerText;

    public static TimerScript instance;

    private void Awake()
    {
        totaltime = GameManagerScript.instance.currentTimerTime;

        instance = this;
    }

    private void Start()
    {
        if(isActiveAndEnabled)
            timerText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            if(timerTime > 0)
            {
                timerTime -= Time.deltaTime;
                
                time_spent += Time.deltaTime;

                // Convert seconds to minutes
                int minutes = Mathf.FloorToInt(timerTime / 60);
                int seconds = Mathf.FloorToInt(timerTime % 60); // Get remaining seconds

                // Display in MM:SS format
                timerText.text = $"Flight Time: {minutes:D2}:{seconds:D2} Min";

            }
            else if (timerTime <= 0)
            {
                FlightState finalState = FlightState.LandingState;
                GameManagerScript.instance.ChangeCurrentState(finalState);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
