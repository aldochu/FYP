using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper2 : MonoBehaviour
{
    public GameObject stg1;

    public GameObject[] stg5; 
    public bool helperOn;
    private int previousDisabled=0;

    public void EnableStg1()
    {
        if (helperOn)
            stg1.SetActive(true);
    }

    public void DisableStg1()
    {
        if (helperOn)
            stg1.SetActive(false);
    }


    public void DisableStg5(int seat)
    {
        stg5[previousDisabled].SetActive(true);
        previousDisabled = seat - 1;
        stg5[previousDisabled].SetActive(false);
    }
}
