using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLanding : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //for put utensil into tray
    {
        if (other.gameObject.tag == "coin")//when release
        {
            if (!other.gameObject.GetComponent<CoinValue>().getStatus()) //if it's not hold by hand
            {
                /*
                other.gameObject.transform.SetParent(transform); //when stg 3 is over the coin will also disappear
                Rigidbody gameObjectsRigidBody = other.gameObject.GetComponent<Rigidbody>(); // Get the rigidbody.
                BoxCollider gameObjectBoxCollider = other.gameObject.GetComponent<BoxCollider>(); //Get the box collider

                gameObjectBoxCollider.isTrigger = true; //this will enable object to be on the floor
                gameObjectsRigidBody.useGravity = false;
                gameObjectsRigidBody.isKinematic = true;
                */

                other.gameObject.transform.SetParent(transform); //when stg 3 is over the coin will also disappear
                other.gameObject.GetComponent<CoinValue>().dropComplete();
            }
                           
        }
    }
}
