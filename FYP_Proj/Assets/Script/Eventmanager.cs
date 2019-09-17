using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eventmanager : MonoBehaviour
{
    public GameObject myCamera;
    public Transform foodStall1,foodStall2;
    public GameObject menu1, menu2;
    private Canvas SelectedMenu;

    // Start is called before the first frame update
    void Start()
    {

        if (Random.Range(0, 1f) > 0.5f)
        {
            SelectedMenu = menu1.GetComponent<Canvas>();
        }
        else
        {
            SelectedMenu = menu2.GetComponent<Canvas>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openMenu()
    {
        SelectedMenu.enabled = true;
    }

    public void closeMenu()
    {
        SelectedMenu.enabled = false;
    }
    IEnumerator MoveToward1()
    {
        Debug.Log("Went here");
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
            myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, foodStall1.position, 0.2f);
            yield return null;
        }
    }

    IEnumerator MoveToward2()
    {
        Debug.Log("Went here");
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
            myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, foodStall2.position, 0.2f);
            yield return null;
        }
    }

    public void moveToFS1()
    {
        StartCoroutine("MoveToward1");
    }

    public void moveToFS2()
    {
        StartCoroutine("MoveToward2");
    }

}
