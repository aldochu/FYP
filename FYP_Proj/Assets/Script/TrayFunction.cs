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
        if (other.gameObject.tag == "utensil1")
        {
            Stg4.GetComponent<Stg4>().utensil1Placed();
            other.transform.position = Utensil1.position;
            other.transform.parent = transform;
            Debug.Log("Got Utensil1");
        }
        else if (other.gameObject.tag == "utensil2")
        {
            Stg4.GetComponent<Stg4>().utensil2Placed();
            other.transform.position = Utensil2.position;
            other.transform.parent = transform;
            Debug.Log("Got Utensil2");
        }


    }
}
