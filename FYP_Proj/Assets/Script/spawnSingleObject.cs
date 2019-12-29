using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnSingleObject : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update

    void OnTriggerStay(Collider other)
    {

        // returns a float of the Hand Trigger’s current state on the Left Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        {
            if (other.gameObject.tag == "controller") //check whether the object can be grab
            {
                if (!other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus())
                {
                    other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                    GameObject spawnObject;
                    spawnObject = Instantiate(prefab, other.GetComponent<ManualControllerScript>().grabLocation.transform.position, other.GetComponent<ManualControllerScript>().grabLocation.transform.rotation) as GameObject;
                    spawnObject.transform.SetParent(other.transform);

                    other.gameObject.GetComponent<ManualControllerScript>().MyTempGameObject(spawnObject);
                }

            }
        }

        
        // returns a float of the Hand Trigger’s current state on the Right Oculus Touch controller.
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (other.gameObject.tag == "controller") //check whether the object can be grab
            {
                if (!other.gameObject.GetComponent<ManualControllerScript>().getGrabStatus())
                {
                    other.gameObject.GetComponent<ManualControllerScript>().Grabbing();
                    GameObject spawnObject;
                    spawnObject = Instantiate(prefab, other.GetComponent<ManualControllerScript>().grabLocation.transform.position, other.GetComponent<ManualControllerScript>().grabLocation.transform.rotation) as GameObject;
                    spawnObject.transform.SetParent(other.transform);

                    other.gameObject.GetComponent<ManualControllerScript>().MyTempGameObject(spawnObject);
                }

            }
        }


    }
}
