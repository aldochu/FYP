using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResult : MonoBehaviour
{

    public Transform target;

    public GameObject Pass, Fail;

    public Transform Table1, Table2, Table3, Table4;
    // Start is called before the first frame update

    public void DisplayResult(bool result, int seatNumber)
    {
        if (result)
        {
            Pass.SetActive(true);
        }
        else
        {
            Fail.SetActive(true);
        }

        if (seatNumber < 4)
        {
            if (seatNumber == 2)
            {
                transform.position = Table1.transform.position;
            }
            else
            {
                transform.position = Table2.transform.position;
            }
        }
        else
        {
            if (seatNumber == 5)
            {
                transform.position = Table4.transform.position;
            }
            else
            {
                transform.position = Table3.transform.position;
            }
        }
    }

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        //transform.LookAt(target);
        Vector3 targetPostition = new Vector3(target.position.x,
                                       this.transform.position.y,
                                       target.position.z);
        this.transform.LookAt(targetPostition);
        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        //transform.LookAt(target, Vector3.left);
    }
}
