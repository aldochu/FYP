﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eventmanager : MonoBehaviour
{
    public GameObject myCamera;
    public Transform foodStall1,foodStall2;
    private Transform myMoveTarget;
    //public GameObject menu1, menu2;
    private Canvas SelectedMenu;
    private bool foodStall2bool, foodStall3bool;
    private AI ai;
    private int CurrentStall = 0;
    private bool updateText = false;


    /// <summary>
    /// this part is for sound
    /// </summary>
    public AudioClip[] Conversation,RiceStallMenu,NoodleStallMenu,Quantity,price;
    private AudioSource audioSrc;



    public Canvas[] foodStallStage1, foodStallStage2;
    // Start is called before the first frame update
    private int[] OrderedFood;
    private int[] foodAmt;
    private int foodOrderSize = 0;
    private string OrdertextToPrint;


    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        foodStall2bool = foodStall3bool = false;
        OrderedFood = new int[2];
        foodAmt = new int[2];
    }

    /*
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



    public void openMenu()
    {
        SelectedMenu.enabled = true;
    }

    public void closeMenu()
    {
        SelectedMenu.enabled = false;
    }
    */

    IEnumerator MoveToward1()
    {
        Debug.Log("Went here");
        float step;
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
            if (foodStall2bool == true)
            {
                step = 6f * Time.deltaTime; // calculate distance to move
                myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, foodStall1.position, step);
                yield return null;
            }
        }
    }

    IEnumerator MoveToward2()
    {
        Debug.Log("Went here");
        float step;
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
            if (foodStall3bool == true)
            {
                step = 6f * Time.deltaTime; // calculate distance to move
                myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, foodStall2.position, step);
                yield return null;
            }
            
        }
    }

    public void moveToFS2()
    {
        foodStall3bool = false;
        foodStall2bool = true;
        StartCoroutine("MoveToward1");
        //myMoveTarget.position = foodStall1.position;
        CurrentStall = 1;
        changeStall();
    }

    public void moveToFS3()
    {
        foodStall2bool = false;
        foodStall3bool = true;
        StartCoroutine("MoveToward2");
        //myMoveTarget.position = foodStall2.position;
        CurrentStall = 2;
        changeStall();
    }


    public void changeStall()
    {
        for(int i = 0; i< foodStallStage1.Length; i++)
        {
            if (i == CurrentStall-1)
            {
                foodStallStage1[i].enabled = false;
                //foodStallStage2[i].enabled = true;
            }
            else
            {
                foodStallStage1[i].enabled = true;
                foodStallStage2[i].enabled = false;
            }
        }

    }

    public void enableMenuSelection()
    {
        foodStallStage2[CurrentStall - 1].enabled = true;
    }

    public void SelectFood(int order)
    {
        if (foodOrderSize < 2)
        {
            if (foodOrderSize == 0)
            {
                OrderedFood[foodOrderSize] = order;
                foodAmt[foodOrderSize++] = 1;
            }
            else
            {
                if (OrderedFood[0] == order)
                {
                    foodAmt[foodOrderSize++ - 1] = 2;
                }
                else
                {
                    OrderedFood[1] = order;
                    foodAmt[foodOrderSize++] = 1;
                }
            }


            //say out order after 3 second
            Invoke("SayOutOrder", 1);

        }
        else
        {
            //warn user that they can't purchase mroe than 2 food
            audioSrc.PlayOneShot(Conversation[3], 1);
        }
    }

    public void SayOutOrder()
    {
        //such long method is because the voice require delay so that it can be said line after line and not all voice played at once
        if (foodOrderSize == 1)
        {
            int temp = OrderedFood[0];
            if (temp < 8)
            {
                audioSrc.PlayOneShot(RiceStallMenu[temp], 1);
                Invoke("SayOne", 1.5f);

            }

            else
            {
                audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);
                Invoke("SayOne", 2);
            }

            Invoke("AskAnythingElse", 3);

        }       
        else
        {
            int temp = OrderedFood[0];
            if (foodAmt[0] == 2)
            {
                if (temp < 8)
                {
                    audioSrc.PlayOneShot(RiceStallMenu[temp], 1);
                    Invoke("SayTwo", 1.5f);
                }
                else
                {
                    audioSrc.PlayOneShot(NoodleStallMenu[temp-8], 1);
                    Invoke("SayTwo", 2);
                }

                Invoke("AskAnythingElse", 3);
            }
            else
            {
                if (temp < 8)
                {
                    audioSrc.PlayOneShot(RiceStallMenu[temp], 1);
                    Invoke("SayOne", 1.5f);
                }
                else
                {
                    audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);
                    Invoke("SayOne", 2);
                }

                Invoke("SayOutSecondOrder", 3);

            }                       
        }
    }

    public void SayOne()
    {
        audioSrc.PlayOneShot(Quantity[0], 1);

    }

    public void SayTwo()
    {
        audioSrc.PlayOneShot(Quantity[1], 1);
    }

    public void SayOutSecondOrder()
    {
        int temp = OrderedFood[1];
        if (temp < 8)
        {
            audioSrc.PlayOneShot(RiceStallMenu[temp], 1);
            Invoke("SayOne", 1.5f);
        }
        else
        {
            audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);
            Invoke("SayOne", 2);
        }

        Invoke("AskAnythingElse", 3);
    }

    public void AskAnythingElse()
    {
        audioSrc.PlayOneShot(Conversation[2], 1);
    }



    ///////////////////
    ///This part is the function to play the sound
    ///

    //communication

    public void AskWhatFoodToOrder()
    {
        audioSrc.PlayOneShot(Conversation[0], 1);
    }

    public void AskCustomerToPointAtMenu()
    {
        audioSrc.PlayOneShot(Conversation[1], 1);
    }
}
