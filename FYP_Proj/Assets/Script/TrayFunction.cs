using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayFunction : MonoBehaviour
{

    public Transform[] foodPos;
    public Transform Utensil1;
    public Transform Utensil2;
    public Transform sauce;
    public GameObject Stg4;

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.tag == "utensil1")
        {
            Stg4.GetComponent<Stg4>().utensil1Placed();
            other.transform.position = Utensil1.position;
            other.transform.rotation = Utensil1.rotation;
            other.transform.parent = transform;

            other.transform.GetComponentInParent<ManualControllerScript>().removeObjectOnHand();


            Debug.Log("Got Utensil1");
        }
        else if (other.gameObject.tag == "utensil2")
        {
            Stg4.GetComponent<Stg4>().utensil2Placed(); 
            other.transform.position = Utensil2.position;
            other.transform.rotation = Utensil2.rotation;
            other.transform.parent = transform;

            other.transform.GetComponentInParent<ManualControllerScript>().removeObjectOnHand();

            Debug.Log("Got Utensil2");
        }
        */

        if (other.gameObject.tag == "controller")
        {
            if (other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus()) //only run this code if the hand is holding something
            {
                //check what thing the hand is holding
                GameObject temp = other.gameObject.GetComponent<ManualControllerScript>().GetHandObject();
                if (temp.tag == "utensil1")
                {
                    Stg4.GetComponent<Stg4>().utensil1Placed();
                    temp.transform.position = Utensil1.position;
                    temp.transform.rotation = Utensil1.rotation;
                    temp.transform.parent = transform;

                    other.transform.GetComponent<ManualControllerScript>().removeObjectOnHand();


                    Debug.Log("Got Utensil1");
                }
                else if (temp.tag == "utensil2")
                {
                    Stg4.GetComponent<Stg4>().utensil2Placed();
                    temp.transform.position = Utensil2.position;
                    temp.transform.rotation = Utensil2.rotation;
                    temp.transform.parent = transform;

                    other.transform.GetComponent<ManualControllerScript>().removeObjectOnHand();

                    Debug.Log("Got Utensil2");
                }
            }
        }


    }
}
