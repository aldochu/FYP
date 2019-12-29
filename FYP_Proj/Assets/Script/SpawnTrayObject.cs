using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrayObject : MonoBehaviour
{

    public GameObject TrayWithCode;
    public GameObject Tray;

    private bool TrayWithCodeTaken = false;


    void OnTriggerStay(Collider other)
    {

        // returns a float of the Hand Trigger’s current state on the Left Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            if (other.gameObject.tag == "controller") //check whether the object can be grab
            {
                if (!other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus())
                {
                    if (!TrayWithCodeTaken)
                    {
                        other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                        TrayWithCode.transform.position = other.GetComponent<ManualControllerScript>().grabLocation.transform.position;
                        TrayWithCode.transform.SetParent(other.transform);
                        TrayWithCodeTaken = true;
                    }
                    else
                    {
                        other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                        GameObject spawnObject;
                        spawnObject = Instantiate(Tray, other.GetComponent<ManualControllerScript>().grabLocation.transform.position, other.GetComponent<ManualControllerScript>().grabLocation.transform.rotation) as GameObject;
                        spawnObject.transform.parent = other.transform;
                    }
                }
                           
            }
        }

        // returns a float of the Hand Trigger’s current state on the Right Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))
        {
            if (other.gameObject.tag == "controller") //check whether the object can be grab
            {
                if (!other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus())
                {
                    if (!TrayWithCodeTaken)
                    {
                        other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                        TrayWithCode.transform.position = other.GetComponent<ManualControllerScript>().grabLocation.transform.position;
                        TrayWithCode.transform.SetParent(other.transform);
                        TrayWithCodeTaken = true;
                    }
                    else
                    {
                        other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                        GameObject spawnObject;
                        spawnObject = Instantiate(Tray, other.GetComponent<ManualControllerScript>().grabLocation.transform.position, other.GetComponent<ManualControllerScript>().grabLocation.transform.rotation) as GameObject;
                        spawnObject.transform.parent = other.transform;
                    }
                }

            }
        }

    }
}
