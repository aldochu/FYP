using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stg4 : MonoBehaviour
{
    public GameObject GameManager;
    // Start is called before the first frame update

    private bool PlacedTray, utensil1, utensil2;

    public AudioClip[] utensil;
    private AudioSource audioSrc;

    public bool isVR;


    private GameObject TrayObject;
    public void Start()
    {
        PlacedTray = utensil1 = utensil2 = false;
        audioSrc = GetComponent<AudioSource>();
    }

    public bool checkComplete()
    {
        if (PlacedTray && utensil1 && utensil2)
        {
            return true;
        }
        else
            return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tray")
        {
            GameManager.GetComponent<Eventmanager>().IncrementNumOfTray(); //increment tray count

                
                Debug.Log("Tray Placed");
                if (isVR)
                    other.gameObject.GetComponentInParent<ManualControllerScript>().NotGrabbing(); //free the hand that grab this

            if (!PlacedTray) //this condition is required so that player will be able to move the tray after the food is complete
            {
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;
                other.transform.SetParent(transform);
                TrayObject = other.gameObject;
                PlacedTray = true;
            }
                
                

                //need to put a check in collider so that every time a tray is place on the designated area it will tell the system that user have place the tray and the hawk can place the food on tray
                GameManager.GetComponent<Eventmanager>().placeFoodOnTray(TrayObject);
            
            
        }

        if (other.gameObject.tag == "xtray")//extra tray
        {
            GameManager.GetComponent<Eventmanager>().IncrementNumOfTray(); //increment tray count
                //if already placed tray, destroy the new tray
                if (isVR)
                    other.gameObject.GetComponentInParent<ManualControllerScript>().NotGrabbing(); //free the hand that grab this
                Destroy(other.gameObject);
        }
    }

    public void utensil1Placed()
    {
        utensil1 = true;
        GameManager.GetComponent<Eventmanager>().IncrementNumOfUtensil1();
    }

    public void utensil2Placed()
    {
        utensil2 = true;
        GameManager.GetComponent<Eventmanager>().IncrementNumOfUtensil2();
    }


    public void Stage4Begin()
    {
        //first check whether there's tray place on the counter, check with the manager class
        Tray myTray = GameManager.GetComponent<Eventmanager>().getTrayItemDetails();
        if (myTray.tray != 0)
        {
            PlacedTray = true;
            if (myTray.utensil1 != 0)
            {
                utensil1 = true;
            }

            if (myTray.utensil2 != 0)
            {
                utensil2 = true;
            }
        }

        if(PlacedTray) //if tray have been place before stage 4, then can skip the asking user to get tray
            GameManager.GetComponent<Eventmanager>().placeFoodOnTray(TrayObject);

        repeatAskingUserAction();

    }


    private void repeatAskingUserAction()
    {
        //if no tray
        if (!PlacedTray)
        {
            //ask for tray
            GameManager.GetComponent<Eventmanager>().AskForTray();
        }
        else
        {
            if (!utensil1)
            {
                audioSrc.PlayOneShot(utensil[0], 1);
            }
            else if (!utensil2)
            {
                audioSrc.PlayOneShot(utensil[1], 1);
            }
            else
            {
                // if PlacedTray & utensil1 & utensil2 == true, mean can proceed to stage 5
                //the hawker will also place the food
            }
        }

        if (!(PlacedTray && utensil1 && utensil2))
        {
            Invoke("repeatAskingUserAction", 10);
        }
    }

}
