using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayFunction : MonoBehaviour
{

    public Transform[] foodPos;
    public Transform Utensil1;
    public Transform Utensil2;
    public Transform sauce;
    public GameObject GameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "utensil1")
        {
            //let the gamemanager be the layer in the middle between trayfunction class and stg4class. Because there are multiple stg4 object so only manager class will know which one to
            //communicate with
        }
        else if (other.gameObject.tag == "utensil2")
        {
            //let the gamemanager be the layer in the middle between trayfunction class and stg4class. Because there are multiple stg4 object so only manager class will know which one to
            //communicate with
        }


    }
}
