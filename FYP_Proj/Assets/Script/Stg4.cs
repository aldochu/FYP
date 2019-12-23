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


    private GameObject TrayObject;
    public void Start()
    {
        PlacedTray = utensil1 = utensil2 = false;
        audioSrc = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tray")
        {
            GameManager.GetComponent<Eventmanager>().IncrementNumOfTray(); //increment tray count

            if (PlacedTray)
            {
                //if already placed tray, destroy the new tray
                Destroy(other.gameObject);
            }
            else
            {
                PlacedTray = true;
                Debug.Log("Tray Placed");             
                other.transform.position = transform.position;
                TrayObject = other.gameObject;
                //need to put a check in collider so that every time a tray is place on the designated area it will tell the system that user have place the tray and the hawk can place the food on tray
                GameManager.GetComponent<Eventmanager>().placeFoodOnTray(TrayObject);
            }
            
        }
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
