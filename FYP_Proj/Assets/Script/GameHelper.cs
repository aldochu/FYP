using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject stg2,stg3;


    //specific gameobject for stg 4
    public GameObject[] stg4; //0:tray, 1:Utensil1Tray, 2:Utensil2Tray, 3:LocationToPutTray
    public bool helperOn;


    public void enableStg2()
    {
        if (helperOn)
            stg2.SetActive(true);
    }

    public void disableStg2()
    {
        if (helperOn)
            stg2.SetActive(false);
    }

    public void enableStg3()
    {
        if (helperOn)
            stg3.SetActive(true);
    }

    public void disableStg3()
    {
        if (helperOn)
            stg3.SetActive(false);
    }


    public void enableStg4(Tray myTray)
    {
        if (helperOn)
        {
            stg4[3].SetActive(true); //location for tray
            //Check tray 
            if (myTray.tray < 1)
            {
                stg4[0].SetActive(true);//tray
            }
            else if (myTray.utensil1 < 1)
            {
                stg4[0].SetActive(false);//tray
                stg4[1].SetActive(true);//tray
            }
            else if (myTray.utensil2 < 1)
            {
                stg4[1].SetActive(false);//tray

                stg4[2].SetActive(true);//tray
            }
            else
            {
                stg4[1].SetActive(false);//tray
                stg4[2].SetActive(false);//tray
            }

        }
    }

    public void DisableStg4()
    {
        stg4[3].SetActive(false); //location for tray
    }
}
