using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatBeltScript : MonoBehaviour
{
    private Transform originalSpot;
    private Quaternion originalRotation;

    private void Start()
    {
    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SeatBeltLeft"))
        {
            SeatScript seatScript = transform.parent.gameObject.GetComponentInParent<SeatScript>();
            if (seatScript != null)
            {
                seatScript.PutSeatBeltOnVr();
            }
       
                

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }


    public void OnSelectExited()
    {
        returnToSpot();
    }

    private void returnToSpot()
    {
        if (originalSpot != null && originalRotation != null)
        {
            transform.position = originalSpot.position;
            transform.rotation = originalSpot.rotation;
        }
    }


   
}
