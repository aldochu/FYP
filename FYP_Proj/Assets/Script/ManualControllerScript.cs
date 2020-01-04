﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualControllerScript : MonoBehaviour
{

    // public variable that can be set to LTouch or RTouch in the Unity Inspector
    public OVRInput.Controller controller;

    public Transform grabLocation;

    private bool grab = false;

    private GameObject tempGameObject;

    void OnTriggerStay(Collider other)
    {

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller)) //when in collider and when press
        {
            if (!grab)
            {

                /*
                    if (other.gameObject.tag == "coin" || other.gameObject.tag == "tray") //check whether the object can be grab
                    {
                        //transform the food object to the hand and set the parent to the hand so that it will move and stay with the hand
                        other.transform.position = grabLocation.position;
                        other.transform.SetParent(transform);
                        grab = true;
                    }
                    */
                if (other.gameObject.tag == "coin") //check whether the object can be grab
                {
                    //transform the food object to the hand and set the parent to the hand so that it will move and stay with the hand
                    other.transform.position = grabLocation.position;
                    other.transform.SetParent(transform);

                    tempGameObject = other.gameObject;

                    grab = true;
                }
            }
            else
            {
                //if grabbed coin and want to change to other coin, exchange the position of both coin
                if (other.gameObject.tag == "coin" && tempGameObject.tag == "coin")
                {
                    //check whether the item currently grabbed is coin

                    //change the position of current holding coin to the coin that going to pick up
                    tempGameObject.transform.position = other.gameObject.transform.position;
                    tempGameObject.transform.rotation = other.gameObject.transform.rotation;
                    tempGameObject.transform.parent = null; //this will remove the connection with the hand

                    other.transform.position = grabLocation.position;
                    other.transform.SetParent(transform);
                    tempGameObject = other.gameObject;

                }
            }

            
        }

        if(other.gameObject.tag == "removeSpot")
        {
            if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller))//when not pressing
            {
                if (tempGameObject.tag == "utensil1" || tempGameObject.tag == "utensil2")
                {
                    //so far only these 2 object we want to delete to simulate that the player return the extra utensil back to where they took
                    Destroy(tempGameObject);
                    grab = false;
                }
            }
        }
    }

    public void MyTempGameObject(GameObject temp)
    {
        tempGameObject = temp;
    }


    public void removeObjectOnHand()
    {
        tempGameObject = null;
        grab = false;
    }

    public GameObject GetHandObject()
    {
        return tempGameObject;
    }

    public void NotGrabbing()
    {
        grab = false;
    }

    public void Grabbing()
    {
        grab = true;
    }

    public bool getGrabStatus()
    {
        return grab;
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
