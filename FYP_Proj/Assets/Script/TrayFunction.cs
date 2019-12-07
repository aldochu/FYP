using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayFunction : MonoBehaviour
{

    public Transform[] foodPos;
    public Transform spoon;
    public Transform fork;
    public Transform sauce;
    private int numOfFood;
    private bool foodOnTray = false; // this counter is for the case where the user brought 2 food. there's a specific placement for the 1st and 2nd food.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "food")
        {
            if (numOfFood == 1)
            {
                other.transform.position = foodPos[0].position;
                other.transform.SetParent(transform);
            }
            else
            {
                if (!foodOnTray)
                {
                    foodOnTray = true;
                    other.transform.position = foodPos[1].position;
                    other.transform.SetParent(transform);
                }
                else
                {
                    other.transform.position = foodPos[2].position;
                    other.transform.SetParent(transform);
                }
            }
            
        }


    }

    public void GetnumOfFood(int value)
    {
        numOfFood = value;
    }
}
