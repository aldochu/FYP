using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualControllerScript : MonoBehaviour
{

    // public variable that can be set to LTouch or RTouch in the Unity Inspector
    public OVRInput.Controller controller;

    public Transform grabLocation;

    void OnTriggerStay(Collider other)
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller)) //when in collider and when press
        {
            if (other.gameObject.tag == "food" || other.gameObject.tag == "tray") //check whether the object can be grab
            {
                //transform the food object to the hand and set the parent to the hand so that it will move and stay with the hand
                other.transform.position = grabLocation.position;
                other.transform.SetParent(transform);
            }
        }

    }


    /*
    // Update is called once per frame
    void Update()
    {
        // returns a float of the Hand Trigger’s current state on the Left Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
        }

        // returns a float of the Hand Trigger’s current state on the Right Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
        }
    }
    */
}
