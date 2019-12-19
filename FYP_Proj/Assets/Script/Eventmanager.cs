﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eventmanager : MonoBehaviour
{
    public GameObject myCamera;
    public Transform foodStall1, foodStall2;
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
    public AudioClip[] Conversation, RiceStallMenu, NoodleStallMenu, Quantity, price;
    private AudioSource audioSrc;

    private bool disableOtherFoodStall = false;
    private bool doneOrderingFood = false;
    private bool repeatAskAnythingElseAsked = false;


    public Canvas[] foodStallStage1, foodStallStage2;
    // Start is called before the first frame update
    private int[] OrderedFood;
    private int[] foodAmt;
    private int foodOrderSize = 0;
    private string OrdertextToPrint;

    private float[] Foodprice;


    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        foodStall2bool = foodStall3bool = false;
        OrderedFood = new int[2];
        foodAmt = new int[2];
        Foodprice = new float[17];
        constructFoodPrice();
    }


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

        for (int i = 0; i < foodStallStage1.Length; i++)
        {
            
            if (i == CurrentStall - 1)
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

    private void disableOtherStallUI()
    {
        for (int i = 0; i < foodStallStage1.Length; i++)
        {

            if (i != CurrentStall - 1)
            {
                foodStallStage1[i].enabled = false;
            }
        }
    }

    public void enableMenuSelection()
    {
        foodStallStage2[CurrentStall - 1].enabled = true;
    }


    public void SelectFood(int order)
    {

        if (!disableOtherFoodStall)
        {
            disableOtherFoodStall = true;
            disableOtherStallUI();
        }

        audioSrc.Stop();

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


            SayOutOrder();

        }
        else
        {
            //warn user that they can't purchase mroe than 2 food
            audioSrc.PlayOneShot(Conversation[3], 1);
        }
    }

    public void SayOutOrder()
    {
        doneOrderingFood = false; // this boolean is to enable to repeating asking customer what else do they want after 10 second of ordering food
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
                    audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);
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
        if(!repeatAskAnythingElseAsked)
        Invoke("RepeatAskAnythingElse", 10);
    }

    public void CancelOrder()
    {
        repeatAskAnythingElseAsked = false;
        foodOrderSize = 0;
        DoneOrdering();
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

    public void DoneOrdering()
    {
        repeatAskAnythingElseAsked = false;
        doneOrderingFood = true;
    }

    private void RepeatAskAnythingElse()
    {
        repeatAskAnythingElseAsked = true;
        if (!doneOrderingFood)
        {
            audioSrc.PlayOneShot(Conversation[2], 1);
            Invoke("RepeatAskAnythingElse", 5);
        }
    }

    public void AskForMoney()
    {
        float totalPrice = CalculateTotalPrice();
        audioSrc.Stop();

        switch (totalPrice) {
            case 3:
                audioSrc.PlayOneShot(price[0], 1);
                break;
            case 3.5f:
                audioSrc.PlayOneShot(price[1], 1);
                break;
            case 4:
                audioSrc.PlayOneShot(price[2], 1);
                break;
            case 6:
                audioSrc.PlayOneShot(price[3], 1);
                break;
            case 6.5f:
                audioSrc.PlayOneShot(price[4], 1);
                break;
            case 7:
                audioSrc.PlayOneShot(price[5], 1);
                break;
            case 7.5f:
                audioSrc.PlayOneShot(price[6], 1);
                break;
            case 8:
                audioSrc.PlayOneShot(price[7], 1);
                break;
        }
       
    }


    private float CalculateTotalPrice()
    {
        if (foodOrderSize < 2)
        {
            return Foodprice[OrderedFood[0]];
        }
        else
        {
            if (foodAmt[0] < 2)
            {
                return (Foodprice[OrderedFood[0]] + Foodprice[OrderedFood[1]]);
            }
            else
            {
                return (Foodprice[OrderedFood[0]]*2);
            }
        }
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


    private void constructFoodPrice()
    {
        Foodprice[0] = 3;
        Foodprice[1] = 3;
        Foodprice[2] = 4;
        Foodprice[3] = 3;
        Foodprice[4] = 3.5f;
        Foodprice[5] = 3.5f;
        Foodprice[6] = 3.5f;
        Foodprice[7] = 3.5f;
        Foodprice[8] = 4;
        Foodprice[9] = 4;
        Foodprice[10] = 3.5f;
        Foodprice[11] = 4;
        Foodprice[12] = 4;
        Foodprice[13] = 3.5f;
        Foodprice[14] = 3;
        Foodprice[15] = 3;
        Foodprice[16] = 3;
    }

}
