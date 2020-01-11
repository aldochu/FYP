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


    private void OnTriggerStay(Collider other)
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {

            //this part is to grab tray, make sure the hand is not grabbing anything first
            if (!other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus())
            {
                if (Stg4.GetComponent<Stg4>().checkComplete()) //if the tray is complete
                {
                transform.position = other.GetComponent<ManualControllerScript>().grabLocation.transform.position;
                transform.parent = other.GetComponent<ManualControllerScript>().grabLocation.transform;
                Stg4.GetComponent<Stg4>().ProceedToStg5();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        //This part is for non vr test scene
        if (other.gameObject.tag == "xutensil1")
        {
            Stg4.GetComponent<Stg4>().utensil1Placed();
            other.transform.position = Utensil1.position;
            other.transform.rotation = Utensil1.rotation;
            other.transform.parent = transform;

            //other.transform.GetComponentInParent<ManualControllerScript>().removeObjectOnHand();


            Debug.Log("Got Utensil1");
        }
        else if (other.gameObject.tag == "xutensil2")
        {
            Stg4.GetComponent<Stg4>().utensil2Placed(); 
            other.transform.position = Utensil2.position;
            other.transform.rotation = Utensil2.rotation;
            other.transform.parent = transform;

            //other.transform.GetComponentInParent<ManualControllerScript>().removeObjectOnHand();

            Debug.Log("Got Utensil2");
        }
        //end 

        if (other.gameObject.tag == "controller")
        {
            //this part to is place utenils onto the tray
            if (other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus()) //only run this code if the hand is holding something
            {
                //check what thing the hand is holding
                GameObject temp = other.gameObject.GetComponent<ManualControllerScript>().GetHandObject();
                if (temp.tag == "utensil1")
                {
                    Stg4.GetComponent<Stg4>().utensil1Placed();
                    temp.transform.position = Utensil1.position;
                    temp.transform.rotation = Utensil1.rotation;
                    temp.transform.parent = transform; //this will unlink the connection between the object holded and the hand

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
