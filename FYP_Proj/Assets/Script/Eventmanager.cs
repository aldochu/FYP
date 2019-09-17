using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eventmanager : MonoBehaviour
{
    public GameObject myCamera;
    public Transform foodStall1,foodStall2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveToward()
    {
        Debug.Log("Went here");
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
            myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, foodStall1.position, 0.2f);
            yield return null;
        }
    }

    public void moveToFS1()
    {
        StartCoroutine("MoveToward");
    }

}
