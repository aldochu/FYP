using System.Collections;
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
    private int CurrentStall = 0;
    private bool updateText = false;

    private int CurrentStage = 0;

    public GameObject[] Hawkers;

    /// <summary>
    /// this part is for sound
    /// </summary>
    public AudioClip[] Conversation, RiceStallMenu, NoodleStallMenu, Quantity, price;
    private AudioSource audioSrc;

    private bool disableOtherFoodStall = false;
    private bool doneOrderingFood = false;
    private bool repeatAskAnythingElseAsked = false;
    private bool mainTalk = false;


    public Canvas[] foodStallStage1, foodStallStage2;
    // Start is called before the first frame update
    private int[] OrderedFood;
    private int[] foodAmt;
    private int foodOrderSize = 0;
    private string OrdertextToPrint;

    private float[] Foodprice;

    private float totalPrice, totalPaid;

   

    /// <summary>
    // For Stage 3 control
    /// </summary>
    private bool stage3 = false;
    public GameObject[] foodStallStage3;

    public GameObject[] foods;

    public Transform[] HawkerFoodLocation; //[0][1] hawker 1 , [2][3] hawker 2

    /// <summary>
    // For Stage 4 control
    /// </summary>
    private Tray myTray;
    private bool TrayCompleted = false;
    public GameObject[] foodStallStage4;
    private GameObject food1, food2;
    private bool foodplaced = false;


    public GameObject[] helper;


    /// <summary>
    // For Stage 5 and environment control
    /// </summary>
    public GameObject[] HumanQueue; //only 0,1
    public GameObject HumanSitting; //only 0,1

    private bool[] seatsOccupied; //There are 6 seats that is dynamic in the game. seats 0,1,3,4 is empty and 2,5 is occupied in the beginning
    private bool[] seatActionCalled; //there are total of 8 actions, [0-5] take seat and [6-7] leave seat and exit
    private int NoOfActionCalled = 0;
    private int RandomTimingBetweenCall = 10;
    int k;


    private Transform DesignatedPoint;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        foodStall2bool = foodStall3bool = false;
        OrderedFood = new int[2];
        foodAmt = new int[2];
        Foodprice = new float[17];
        constructFoodPrice();
        totalPrice = totalPaid = 0;
        myTray = new Tray();


        /// <summary>
        // For Stage 5 and environment control
        /// </summary>
        seatsOccupied = new bool[6];
        seatActionCalled = new bool[8];
        ConstructSeatVacancy();
        k = 6;//Random.Range(0, 8);
        Invoke("MovementAction", RandomTimingBetweenCall);
}


    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 1///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>


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

    IEnumerator MoveTowardPoint()
    {
        Debug.Log("Went here");
        float step;
        for (float ft = 1000; ft >= 0; ft -= 0.1f)
        {
                step = 6f * Time.deltaTime; // calculate distance to move
                myCamera.transform.position = Vector3.MoveTowards(myCamera.transform.position, DesignatedPoint.position, step);
                yield return null;

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
        CurrentStage = 1;

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
        //helper[CurrentStall - 1].GetComponent<GameHelper>().disableStg1();
        helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg2();
    }



    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 1///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>



    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 2///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    public void SelectFood(int order)
    {
        CurrentStage = 2;

        //Game helper
        

        if (!disableOtherFoodStall)
        {
            disableOtherFoodStall = true;
            disableOtherStallUI();

            helper[CurrentStall - 1].GetComponent<GameHelper>().disableStg2();
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

        mainTalk = true; // this bool is to prevent overlap sound
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
            Invoke("endOfMainTalk", 7);

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
                Invoke("endOfMainTalk", 7);
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
        if (!repeatAskAnythingElseAsked)
        {
            repeatAskAnythingElseAsked = true;
            Invoke("RepeatAskAnythingElse", 13);
        }
        
    }

    public void CancelOrder()
    {
        repeatAskAnythingElseAsked = false;
        foodOrderSize = 0;
        DoneOrdering();
    }

    public void SayOne()
    {
        if (!stage3)
        {
            audioSrc.Stop();
            audioSrc.PlayOneShot(Quantity[0], 1);
        }

    }

    public void SayTwo()
    {
        if (!stage3)
        {
            audioSrc.Stop();
            audioSrc.PlayOneShot(Quantity[1], 1);

        }
    }

    public void SayOutSecondOrder()
    {
        if (!stage3)
        {
            audioSrc.Stop();
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
            Invoke("endOfMainTalk", 7);
        }
    }

    public void AskAnythingElse()
    {
        if (!stage3)
        {
            audioSrc.Stop();
            audioSrc.PlayOneShot(Conversation[2], 1);
        }
    }

    private void endOfMainTalk()
    {
        mainTalk = false; // this bool is to prevent overlap sound
    }

    public void DoneOrdering()
    {
        repeatAskAnythingElseAsked = false;
        doneOrderingFood = true;
        
        
    }

    private void RepeatAskAnythingElse()
    {
        if (!doneOrderingFood)
        {
            if (!mainTalk)
            {
                audioSrc.PlayOneShot(Conversation[4], 1);
            }
                
            Invoke("RepeatAskAnythingElse", 10);
        }
    }

    public void AskForMoney()
    {
        totalPrice = CalculateTotalPrice();
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

    public void AskToMoneyFaster()
    {
        audioSrc.Stop();
        audioSrc.PlayOneShot(Conversation[5], 1);
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

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 2///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    /// 


    





    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 3///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    /// 

    public void BeginStageThree()
    {
        CurrentStage = 3;
        stage3 = true;
        foodStallStage2[CurrentStall - 1].enabled = false;
        int ArrayIndex = CurrentStall - 1;
        foodStallStage3[ArrayIndex].SetActive(true);
        foodStallStage3[ArrayIndex].GetComponent<Stg3>().BeginStgThree(foodOrderSize);

        //Game helper
        helper[CurrentStall - 1].GetComponent<GameHelper>().disableStg2();
        helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg3();

    }

    public void CustomerPay(float amt)
    {
        Debug.Log(amt);
        totalPaid += amt;
        Debug.Log(totalPaid);
    }


    public void DonePayment()
    {
        if (totalPrice > totalPaid)
        {
            audioSrc.Stop();
            audioSrc.PlayOneShot(Conversation[6], 1);
            //tell customer money is not enough
        }
        else
        {
            //give user feedback that their payment is done 
            audioSrc.PlayOneShot(Conversation[7], 1);

            //close stage 3 object which will also stop the repeat ask money function
            foodStallStage3[CurrentStall - 1].SetActive(false);
            Destroy(foodStallStage3[CurrentStall - 1]);

            //begin stage 4
            Invoke("BeginStageFour",1); //delay 3 second so that it will only move after saying out the feedback
        }
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 3///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    /// 



    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 4///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    /// 

    public void BeginStageFour()
    {
        

       Hawkers[CurrentStall - 1].GetComponent<HawkerAnimation>().GoToBackOfStore(foodOrderSize);
        //instantiate the prefab of food and make it appear at the food placement

       Invoke("InstantiateFood", 25);

        //Game helper
        helper[CurrentStall - 1].GetComponent<GameHelper>().disableStg3();
        helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg4(myTray);

    }

    private void InstantiateFood()
    {
        if (foodOrderSize < 2)
        {
            int spawnLocation = (CurrentStall - 1) * 2;
            //there's only one food
            //check what food it is
            food1 = Instantiate(foods[OrderedFood[0]], HawkerFoodLocation[spawnLocation].position, HawkerFoodLocation[spawnLocation].rotation) as GameObject;
            food1.transform.parent = HawkerFoodLocation[spawnLocation];

        }
        else
        {
            int spawnLocation1 = (CurrentStall - 1) * 2;
            int spawnLocation2 = ((CurrentStall - 1) * 2)+1;
            food1 = Instantiate(foods[OrderedFood[0]], HawkerFoodLocation[spawnLocation1].position, HawkerFoodLocation[spawnLocation1].rotation) as GameObject;
            food1.transform.parent = HawkerFoodLocation[spawnLocation1];

            if (foodAmt[0] < 2)
            {
                 food2 = Instantiate(foods[OrderedFood[1]], HawkerFoodLocation[spawnLocation2].position, HawkerFoodLocation[spawnLocation2].rotation) as GameObject;
               
            }
            else
            {
                 food2 = Instantiate(foods[OrderedFood[0]], HawkerFoodLocation[spawnLocation2].position, HawkerFoodLocation[spawnLocation2].rotation) as GameObject;
            }

            food2.transform.parent = HawkerFoodLocation[spawnLocation2];

        }

        CurrentStage = 4;
        Invoke("delayStage4Calling", 8);
    }


    private void delayStage4Calling()
    {
        int ArrayIndex = CurrentStall - 1;
        foodStallStage4[ArrayIndex].GetComponent<Stg4>().Stage4Begin();
    }

    //check whether there is tray at the desginated area, if no, inform the user to bring the tray
    public void IncrementNumOfTray()
    {
        myTray.tray++;
    }

    public void IncrementNumOfUtensil1()
    {
        myTray.utensil1++;

        //tell helper class that utensil1 is place
        if (CurrentStage == 4)
        {
            helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg4(myTray); // this will update the helper function
        }
    }

    public void IncrementNumOfUtensil2()
    {
        myTray.utensil2++;

        //tell helper class that utensil1 is place
        if (CurrentStage == 4)
        {
            helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg4(myTray); // this will update the helper function
        }
    }

    public Tray getTrayItemDetails()
    {
        return myTray;
    }

    public void AskForTray()
    {
        audioSrc.PlayOneShot(Conversation[8], 1);
    }

    public void placeFoodOnTray(GameObject TrayFoodLocation) //this code will check whether the hawker is holding food when the tray arrive, if yes then place the food. Else, when the food is ready place on the tray
    {
        if (CurrentStage == 4 && !foodplaced)
        {
            foodplaced = true;

            if (foodOrderSize < 2)
            {
                Transform foodtraylocation = TrayFoodLocation.GetComponent<TrayFunction>().foodPos[0];
                food1.transform.position = foodtraylocation.transform.position;
                food1.transform.parent = TrayFoodLocation.transform;
            }
            else
            {
                Transform foodtraylocation1 = TrayFoodLocation.GetComponent<TrayFunction>().foodPos[1];
                Transform foodtraylocation2 = TrayFoodLocation.GetComponent<TrayFunction>().foodPos[2];

                food1.transform.position = foodtraylocation1.position;
                food1.transform.parent = TrayFoodLocation.transform;

                food2.transform.position = foodtraylocation2.position;
                food2.transform.parent = TrayFoodLocation.transform;
            }

            //place the hand down
            Hawkers[CurrentStall - 1].GetComponent<HawkerAnimation>().DoneServingFood();
            
        }
    }

    //this function will be called by stg4 to signal that the system can go on to stg 5
    public void TrayComplete()
    {
        //
        helper[CurrentStall - 1].GetComponent<GameHelper>().disableStg4();
    }




    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 4///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    /// 


    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 5///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    /// 

    private void ConstructSeatVacancy()
    {
        for (int i = 0; i < 6; i++)
        {
            seatsOccupied[i] = false;
        }

        seatsOccupied[2] = true;
        seatsOccupied[5] = true;

        for (int i = 0; i < 8; i++)
        {
            seatActionCalled[i] = false;
        }
    }


    private void MovementAction()
    {
        

        if (NoOfActionCalled != 8)
        {
            /*
            bool Valid = false;
            while (!Valid) //loop until value == false
            {
                if (seatActionCalled[k])
                {
                    k = (k + 1) % 8;
                }
                else
                {
                    Valid = true;
                }    
            
            }
            */

            Debug.Log("k value: "+k + " NoOfActionCalled: "+ NoOfActionCalled);

            //seatActionCalled[k] = true;

            if (k < 3) //0,1,2
            {
                //if it's 0 or 1, ai can go take a seat
                if (k != 2)
                {
                    HumanQueue[0].GetComponent<CrowdControl>().MoveAI(k);
                    //mark the bool
                    seatsOccupied[k] = true;
                    seatActionCalled[k] = true;
                }

                else
                {
                    //if it's 2 
                    if (seatActionCalled[6]) //the AI seating on seat 2 has left
                    {
                        HumanQueue[0].GetComponent<CrowdControl>().MoveAI(k);
                        seatsOccupied[k] = true;
                        seatActionCalled[k] = true;
                    }
                    else //the AI seating on seat 2 hasn't left
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(0);
                        seatsOccupied[2] = false; //person left seat 2
                        seatActionCalled[6] = true;
                    }
                }

            }
            else if (k < 6)//3,4,5
            {
                int temp = k % 3;
                //if it's 3 or 4, ai can go take a seat
                if (k != 5)
                {

                    HumanQueue[1].GetComponent<CrowdControl>().MoveAI(temp);
                    //mark the bool
                    seatsOccupied[k] = true;
                    seatActionCalled[k] = true;
                }

                else
                {
                    //if it's 5 
                    if (seatActionCalled[7]) //the AI seating on seat 5 has left
                    {

                        HumanQueue[1].GetComponent<CrowdControl>().MoveAI(temp);
                        seatsOccupied[k] = true;
                        seatActionCalled[k] = true;
                    }
                    else //the AI seating on seat 2 hasn't left
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(1);
                        seatsOccupied[5] = false; //person left seat 5
                        seatActionCalled[7] = true;

                    }

                }
            }
            else //6,7
            {
                if (k == 6)
                {
                    if (!seatActionCalled[6])
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(0);
                        seatsOccupied[2] = false; //person left seat 2
                        seatActionCalled[6] = true;
                    }
                    else
                    {
                        HumanQueue[0].GetComponent<CrowdControl>().MoveAI(2);
                        seatsOccupied[2] = true;
                        seatActionCalled[k] = true;
                    }
                    
                }
                else
                {
                    if (!seatActionCalled[7])
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(1);
                        seatsOccupied[5] = false; //person left seat 5
                        seatActionCalled[7] = true;
                    }
                    else
                    {
                        HumanQueue[1].GetComponent<CrowdControl>().MoveAI(2);
                        seatsOccupied[5] = true;
                        seatActionCalled[k] = true;
                    }
                    
                }
            }

            NoOfActionCalled++;
            k = (k + 1) % 8;
            Invoke("MovementAction", RandomTimingBetweenCall);
        }
       
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 5///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    /// 
}




