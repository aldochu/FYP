using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Eventmanager : MonoBehaviour
{
    public GameObject myCamera;
    private Transform myMoveTarget;
    //public GameObject menu1, menu2;
    private Canvas SelectedMenu;
    private int CurrentStall = 0;
    private bool updateText = false;

    private int CurrentStage = 0;

    public GameObject[] Hawkers;

    private string[] FoodName;

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
    private int[] OrderedFood;
    private int[] foodAmt;
    private int foodOrderSize = 0;
    private string OrdertextToPrint;


    /// <summary>
    // For Stage 1 control
    /// </summary>
    public GameObject DisplayFoodMenu;
    private int NumOfFoodToBuy;
    private int[] FoodToBuy;
    private int[] FoodToBuyAmt;
    public GameObject[] AssistanceFunction;


    /// <summary>
    // For Stage 3 control
    /// </summary>
    private float[] Foodprice;

    private float totalPrice, totalPaid;

    private bool stage3 = false;
    public GameObject[] foodStallStage3;

    public GameObject[] foods;

    public Transform[] HawkerFoodLocation; //[0][1] hawker 1 , [2][3] hawker 2

    /// <summary>
    // For Stage 4 control
    /// </summary>
    private Tray myTray;
    public GameObject[] foodStallStage4;
    private GameObject food1, food2;
    private bool foodplaced = false;

    public GameObject[] PriceText;


    public GameObject[] helper;


    /// <summary>
    // For Stage 5 and environment control
    /// </summary>
    /// 
    public GameObject stage5;
    public GameObject[] HumanQueue; //only 0,1
    public GameObject HumanSitting; //only 0,1

    private bool[] seatsOccupied; //There are 6 seats that is dynamic in the game. seats 0,1,3,4 is empty and 2,5 is occupied in the beginning
    private bool[] seatActionCalled; //there are total of 8 actions, [0-5] take seat and [6-7] leave seat and exit
    private int NoOfActionCalled = 0;
    private int RandomTimingBetweenCall = 20;
    private bool FoundEmptySeat = false;
    private int PlayerSeatNumber = 9; //dummy value

    private int NoOfVacantSeat = 6;

    public GameObject[] TrayWithFood;

    int RandomAIMovement;



    /// <summary>
    // For Scoring
    /// </summary>
    /// 
    public GameObject DisplayResult;
    private Scores myScores;
    public GameObject DatabaseOnject;

    public System.DateTime PreviousTime, CurrentTime;


    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        OrderedFood = new int[2];
        foodAmt = new int[2];

        FoodToBuy = new int[4]; //maximum only 4 food in the list
        FoodToBuyAmt = new int[4]; //maximum only 4 food in the list

        Foodprice = new float[18];
        FoodName = new string[18];
        constructFoodName();
        constructFoodPrice();
        totalPrice = totalPaid = 0;
        myTray = new Tray();


        contructFoodToBuy();

        /// <summary>
        // For Stage 5 and environment control
        /// </summary>
        seatsOccupied = new bool[6];
        seatActionCalled = new bool[8];
        ConstructSeatVacancy();
        RandomAIMovement = Random.Range(0, 8); //0-7
        Invoke("MovementAction", RandomTimingBetweenCall);

        /// <summary>
        // For Scoring
        /// </summary>

        myScores = new Scores();

        ArrangeStage();

        PreviousTime = System.DateTime.MinValue;

        SayOutFoodToBuy1();
    }

    private void SayOutFoodToBuy1()
    {
        audioSrc.PlayOneShot(Conversation[11], 1);

        Invoke("SayOutFoodToBuy2", 1.2f);
    }

    private void SayOutFoodToBuy2()
    {
        

        if (NumOfFoodToBuy == 1)
        {
            SayOne();
            Invoke("SayfoodnameOne", 1);

        }
        else
        {
            if (FoodToBuyAmt[0] == 2)
            {
                SayTwo();
                Invoke("SayfoodnameOne", 1);

            }
            else
            {
                SayOne();
                Invoke("SayfoodnameOne", 1);

                Invoke("SayOutSecondFoodToBuy", 3);

            }
        }
    }

    public void SayfoodnameOne()
    {
        int temp = FoodToBuy[0];
        if (temp < 8)
        {
            audioSrc.PlayOneShot(RiceStallMenu[temp], 1);


        }

        else
        {
            audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);

        }
    }

    public void SayfoodnameTwo()
    {
        int temp = FoodToBuy[1];
        if (temp < 8)
        {
            audioSrc.PlayOneShot(RiceStallMenu[temp], 1);
        }

        else
        {
            audioSrc.PlayOneShot(NoodleStallMenu[temp - 8], 1);

        }
    }

    private void SayOutSecondFoodToBuy()
    {
        SayOne();
        Invoke("SayfoodnameTwo", 1.5f);
    }


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(PreviousTime == System.DateTime.MinValue);
            CurrentTime = System.DateTime.Now;
            GetTimeDifference(PreviousTime, CurrentTime);
        }
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 0///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// this section will randomly select food that require user to buy and display on the menu


    private void contructFoodToBuy()
    {
        NumOfFoodToBuy = Random.Range(1, 3); //this will return either 1 or 2

        if (!StaticVariable.PracticeMode)
        {
            NumOfFoodToBuy = 2; //real mode always 2
        }

        int foodMenu = Random.Range(1, 3); //menu 1 or 2

        for (int i = 0; i < NumOfFoodToBuy; i++)
        {
            if (i == 0)
            {
                if (foodMenu == 1)
                    FoodToBuy[0] = Random.Range(0, 8);
                else
                {
                    FoodToBuy[0] = Random.Range(9, 18);
                }
                FoodToBuyAmt[0] = 1;
            }          
            else if(i==1)
            {

                int k;

                if (foodMenu == 1)
                {
                     k = Random.Range(0, 8);
                }
                else
                {
                     k = Random.Range(9, 18);      
                }

                if (FoodToBuy[0] == k) //same food
                {
                    FoodToBuyAmt[0] = 2;
                }
                else
                {
                    FoodToBuy[1] = k;
                    FoodToBuyAmt[1] = 1;
                }

            }
        }

        //Debug.Log("Food to buy1: " + FoodToBuy[0] + " , Food to buy2: " + FoodToBuy[1]);

        DisplayFoodMenu.GetComponent<UpdateMenu>().UpdateFoodUI(FoodName[FoodToBuy[0]], FoodName[FoodToBuy[1]], FoodToBuyAmt[0],NumOfFoodToBuy);
        //once contructed the menu to buy, display it

        //update the assistance function that will help user in training mode to check the menu
        AssistanceFunction[0].GetComponent<AssistanceFunction>().setFood(FoodName[FoodToBuy[0]], FoodName[FoodToBuy[1]], FoodToBuyAmt[0], NumOfFoodToBuy);
        AssistanceFunction[1].GetComponent<AssistanceFunction>().setFood(FoodName[FoodToBuy[0]], FoodName[FoodToBuy[1]], FoodToBuyAmt[0], NumOfFoodToBuy);



    }

    private void ArrangeStage()
    {
        if (!StaticVariable.PracticeMode)
        {
            helper[0].GetComponent<GameHelper>().helperOn = false;
            helper[1].GetComponent<GameHelper>().helperOn = false;
            helper[2].GetComponent<GameHelper2>().helperOn = false;
            AssistanceFunction[0].SetActive(false);
            AssistanceFunction[1].SetActive(false);
            DisplayFoodMenu.SetActive(false);
            resetAllStallUI();
        }
    }

    public void BeginGame()
    {
        DisplayFoodMenu.SetActive(false);
        helper[2].GetComponent<GameHelper2>().EnableStg1();
        resetAllStallUI();
    }


    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End of Stage 0///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>

    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Stage 1///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>

    public void moveToFS2()
    {
        //StartCoroutine("MoveToward1");
        myCamera.GetComponent<AutoPlayerMovement>().movePlayer(0);
        //myMoveTarget.position = foodStall1.position;
        CurrentStall = 1;
        changeStall();
    }

    public void moveToFS3()
    {
        //StartCoroutine("MoveToward2");
        myCamera.GetComponent<AutoPlayerMovement>().movePlayer(1);
        //myMoveTarget.position = foodStall2.position;
        CurrentStall = 2;
        changeStall();
    }


    public void changeStall()
    {
        CurrentStage = 1;
        helper[2].GetComponent<GameHelper2>().DisableStg1();

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


        AssistanceFunction[0].GetComponent<AssistanceFunction>().enableSelection();
        AssistanceFunction[1].GetComponent<AssistanceFunction>().enableSelection();
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

    private void resetAllStallUI()
    {
        for (int i = 0; i < foodStallStage1.Length; i++)
        {
                foodStallStage1[i].enabled = true;
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
            Invoke("RepeatAskAnythingElse", 20);
        }

    }

    public void CancelOrder()
    {
        repeatAskAnythingElseAsked = false;
        foodOrderSize = 0;
        myScores.numOfCancelledOrder++;
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

            Invoke("RepeatAskAnythingElse", 20);
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
                return (Foodprice[OrderedFood[0]] * 2);
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
        Foodprice[17] = 3;
    }

    private void constructFoodName()
    {
        FoodName[0] = "柠檬鸡/\nLemon Chicken";
        FoodName[1] = "油菜/\nOyster Sauce Veg";
        FoodName[2] = "芝麻鸡/\nSesami Chicken";
        FoodName[3] = "玻璃鸡脚/\nChicken Feet";
        FoodName[4] = "叉烧烤肉/\nChar Siew Roasted Pork";
        FoodName[5] = "泰式豆腐/\nThai Style Beancurd";
        FoodName[6] = "烤鸡/\nRoasted Chicken";
        FoodName[7] = "白鸡/\nSteamed Chicken";
        FoodName[8] = "哥罗面/\nKolo Mee";
        FoodName[9] = "虾河粉/\nPrawn Hor Fun";
        FoodName[10] = "鱼丸咖喱面/\nFishball Curry Noodle";
        FoodName[11] = "咖喱鸡面/\nCurry Chicken Noodle";
        FoodName[12] = "杂锦汤面/\nAssorted Soup Noodle";
        FoodName[13] = "虾面/\nPrawn Noodle";
        FoodName[14] = "肉脞面/\nMinced Meat Noodle";
        FoodName[15] = "鱼丸肉脞面/\nFishball Minced Meat Noodle";
        FoodName[16] = "鱼丸鱼饼面/\nFishball & Fishcake Noodle";
        FoodName[17] = "鱼丸面/\nFishball Noodle";
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
        audioSrc.PlayOneShot(price[8], 1); //play feedback sound to inform player to coin been paid

        //update the price text
        PriceText[CurrentStall - 1].GetComponent<PriceText>().UpdateText(totalPaid);
    }


    public void DonePayment()
    {
        if (totalPrice > totalPaid)
        {
            audioSrc.Stop();
            audioSrc.PlayOneShot(Conversation[6], 1);
            //tell customer money is not enough
            myScores.numOfWrongPayment++;
        }
        else
        {
            myScores.payable = totalPrice;
            myScores.amtPaid = totalPaid;
            //give user feedback that their payment is done 
            audioSrc.PlayOneShot(Conversation[7], 1);

            //close stage 3 object which will also stop the repeat ask money function
            foodStallStage3[CurrentStall - 1].SetActive(false);
            Destroy(foodStallStage3[CurrentStall - 1]);

            //begin stage 4
            Invoke("BeginStageFour", 1); //delay 3 second so that it will only move after saying out the feedback
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
            int spawnLocation2 = ((CurrentStall - 1) * 2) + 1;
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

        
        Invoke("delayStage4Calling", 8);
    }


    private void delayStage4Calling()
    {
        CurrentStage = 4;
        int ArrayIndex = CurrentStall - 1;
        foodStallStage4[ArrayIndex].GetComponent<Stg4>().Stage4Begin();

        PreviousTime = System.DateTime.Now; //will start timing the time taken for the tray
    }

    //check whether there is tray at the desginated area, if no, inform the user to bring the tray
    public void IncrementNumOfTray()
    {
        myTray.tray++;
        if (myTray.tray > 1) //take more than 1 tray
        {
            audioSrc.PlayOneShot(Conversation[9], 1);
        }
        helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg4(myTray);

        if (CurrentStage != 4)
        {
            myScores.timeToGetTray = 0;
        }
        else if (myTray.tray == 1)
        {
            CurrentTime = System.DateTime.Now;
            myScores.timeToGetTray = GetTimeDifference(PreviousTime, CurrentTime);

            PreviousTime = System.DateTime.Now; //will start timing the time taken for the utensil1
        }
    }

    public void IncrementNumOfUtensil1()
    {
        myTray.utensil1++;

        //tell helper class that utensil1 is place
        if (CurrentStage == 4)
        {
            helper[CurrentStall - 1].GetComponent<GameHelper>().enableStg4(myTray); // this will update the helper function
        }

        if (CurrentStage != 4)
        {
            myScores.timeToGetUtenil1 = 0;
        }
        else if (myTray.utensil1 == 1)
        {
            CurrentTime = System.DateTime.Now;
            myScores.timeToGetUtenil1 = GetTimeDifference(PreviousTime, CurrentTime);

            PreviousTime = System.DateTime.Now; //will start timing the time taken for the utensil1
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

        if (CurrentStage != 4)
        {
            myScores.timeToGetUtensil2 = 0;
        }
        else if (myTray.utensil2 == 1)
        {
            CurrentTime = System.DateTime.Now;
            myScores.timeToGetUtensil2 = GetTimeDifference(PreviousTime, CurrentTime);
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

    public void beginStage5()
    {
        stage5.SetActive(true);
        helper[CurrentStall - 1].GetComponent<GameHelper>().DisableStg4();
    }

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

    public void GoToSeat(int seatNumber)
    {
        if (FoundEmptySeat) //user current seat is empty but player want to change seat, so need to free this current seat so that AI can seat it
        {
            seatsOccupied[PlayerSeatNumber - 1] = false; //free the seat
        }

        PlayerSeatNumber = seatNumber;


        if (!seatsOccupied[seatNumber - 1])//if seat is empty
        {
            FoundEmptySeat = true;
            seatsOccupied[seatNumber - 1] = true; //player took the seat
        }
        else
        {
            FoundEmptySeat = false;
        }
        myCamera.GetComponent<AutoPlayerMovement>().movePlayerToSeat(seatNumber);
        helper[2].GetComponent<GameHelper2>().DisableStg5(seatNumber);
    }

    public void CheckSeatAvailability() //if player go to a seat that doesn't belong to anyone, then the game is complete, else play message that seats taken
    {
        if (FoundEmptySeat)
        {
            stage5.SetActive(false);
            //place the food on the table
            if (PlayerSeatNumber < 4) //1-3
            {
                TrayWithFood[CurrentStall - 1].transform.position = HumanQueue[0].GetComponent<CrowdControl>().foodPlacement[PlayerSeatNumber - 1].position;
                TrayWithFood[CurrentStall - 1].transform.rotation = HumanQueue[0].GetComponent<CrowdControl>().foodPlacement[PlayerSeatNumber - 1].rotation;
            }
            else
            {
                TrayWithFood[CurrentStall - 1].transform.position = HumanQueue[1].GetComponent<CrowdControl>().foodPlacement[(PlayerSeatNumber - 1) % 3].position;
                TrayWithFood[CurrentStall - 1].transform.rotation = HumanQueue[1].GetComponent<CrowdControl>().foodPlacement[(PlayerSeatNumber - 1) % 3].rotation;
            }
            TrayWithFood[CurrentStall - 1].transform.parent = null;
            CalculateResults();
            //game complete, check result

        }
        else
        {
            myScores.numOfFailedSeatPick++;
            audioSrc.PlayOneShot(Conversation[10], 1); //tell player to find other seat
        }
    }


    private void MovementAction() //this is for ai movement
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

            //seatActionCalled[k] = true;

            if (RandomAIMovement < 3) //0,1,2
            {
                //if it's 0 or 1, ai can go take a seat
                if (RandomAIMovement != 2)
                {
                    if (!seatsOccupied[RandomAIMovement] && NoOfVacantSeat!=1) //if no 1 seat and there is more than 1 vacant seat
                    {
                        HumanQueue[0].GetComponent<CrowdControl>().MoveAI(RandomAIMovement);
                        seatsOccupied[RandomAIMovement] = true;
                        NoOfVacantSeat--;

                    }
                        
                    //mark the bool
                    
                    seatActionCalled[RandomAIMovement] = true;
                }

                else
                {
                    //if it's 2 
                    if (seatActionCalled[6]) //the AI seating on seat 2 has left
                    {
                        if (!seatsOccupied[RandomAIMovement] && NoOfVacantSeat != 1) //if no 1 seat and there is more than 1 vacant seat
                        {
                            HumanQueue[0].GetComponent<CrowdControl>().MoveAI(RandomAIMovement);
                            seatsOccupied[RandomAIMovement] = true;
                            NoOfVacantSeat--;
                        }
                           
                        
                        seatActionCalled[RandomAIMovement] = true;
                    }
                    else //the AI seating on seat 2 hasn't left
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(0);
                        seatsOccupied[2] = false; //person left seat 2
                        seatActionCalled[6] = true;
                    }
                }

            }
            else if (RandomAIMovement < 6)//3,4,5
            {
                int temp = RandomAIMovement % 3;
                //if it's 3 or 4, ai can go take a seat
                if (RandomAIMovement != 5)
                {
                    if (!seatsOccupied[RandomAIMovement] && NoOfVacantSeat != 1) //if no 1 seat and there is more than 1 vacant seat
                    {
                        HumanQueue[1].GetComponent<CrowdControl>().MoveAI(temp);
                        seatsOccupied[RandomAIMovement] = true;
                        NoOfVacantSeat--;
                    }
                        
                    //mark the bool
                    seatActionCalled[RandomAIMovement] = true;
                }

                else
                {
                    //if it's 5 
                    if (seatActionCalled[7]) //the AI seating on seat 5 has left
                    {
                        if (!seatsOccupied[RandomAIMovement] && NoOfVacantSeat != 1) //if no 1 seat and there is more than 1 vacant seat
                        {
                            HumanQueue[1].GetComponent<CrowdControl>().MoveAI(temp);
                            seatsOccupied[RandomAIMovement] = true;
                            NoOfVacantSeat--;
                        }
                            
                        seatActionCalled[RandomAIMovement] = true;
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
                if (RandomAIMovement == 6)
                {
                    if (!seatActionCalled[6])
                    {
                        HumanSitting.GetComponent<SeaterCrowdControl>().MoveAI(0);
                        seatsOccupied[2] = false; //person left seat 2
                        seatActionCalled[6] = true;
                    }
                    else
                    {
                        if (NoOfVacantSeat != 1)
                        {
                            HumanQueue[0].GetComponent<CrowdControl>().MoveAI(2);
                            seatsOccupied[2] = true;
                            NoOfVacantSeat--;
                        }
                        
                        seatActionCalled[RandomAIMovement] = true;
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
                        if (NoOfVacantSeat != 1)
                        {
                            HumanQueue[1].GetComponent<CrowdControl>().MoveAI(2);
                            seatsOccupied[5] = true;
                            NoOfVacantSeat--;
                        }

                        
                        seatActionCalled[RandomAIMovement] = true;
                    }

                }
            }

            NoOfActionCalled++;
            RandomAIMovement = (RandomAIMovement + 1) % 8;
            Invoke("MovementAction", RandomTimingBetweenCall);
        }

        Debug.Log("Seat Left: " + NoOfVacantSeat);

    }

    private void CalculateResults()
    {
        bool[] check;//this is food 2 menu check
        check = new bool[2];
        check[0] = check[1] = false;

        if (NumOfFoodToBuy == foodOrderSize)
        {
            if (NumOfFoodToBuy < 2)
            {
                myScores.FoodToOrder1 = FoodName[FoodToBuy[0]];
                myScores.FoodOrdered1 = FoodName[OrderedFood[0]];



                if (OrderedFood[0] == FoodToBuy[0])
                {
                    check[0] = true;
                    myScores.Result = "Pass";

                    myScores.numOfWrongOrdered = 0;
                }
                else
                    myScores.numOfWrongOrdered = 1;

            }
            else //2 food
            {              
                if (foodAmt[0] > 1)
                {
                    myScores.FoodToOrder1 = FoodName[FoodToBuy[0]];
                    myScores.FoodOrdered1 = FoodName[OrderedFood[0]];

                    if (OrderedFood[0] == FoodToBuy[0])
                    {
                        check[0] = check[1] = true;
                        myScores.Result = "Pass";
                        myScores.numOfWrongOrdered = 0;
                    }
                    else
                    {
                        myScores.numOfWrongOrdered = 2;
                    }
                }
                else
                {
                    myScores.FoodToOrder1 = FoodName[FoodToBuy[0]];
                    myScores.FoodOrdered1 = FoodName[OrderedFood[0]];

                    myScores.FoodToOrder2 = FoodName[FoodToBuy[1]];
                    myScores.FoodOrdered2 = FoodName[OrderedFood[1]];

                    int NumOfWrongFoodPicked = 2;

                    //check the array
                    for (int i = 0; i < 2; i++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            if (OrderedFood[k] == FoodToBuy[i])
                            {
                                check[i] = true;
                                myScores.Result = "Pass";
                                NumOfWrongFoodPicked--;
                            }
                        }
                    }

                    myScores.numOfWrongOrdered = NumOfWrongFoodPicked;
                }
            }
            
        }

        myScores.updateTrayValue(myTray);

        if (!StaticVariable.PracticeMode)
            DatabaseOnject.GetComponent<DatabaseFunction>().UploadScores(myScores);

        DisplayResult.SetActive(true);
        if (NumOfFoodToBuy < 2)
        {
                DisplayResult.GetComponent<UpdateResult>().DisplayResult(check[0], PlayerSeatNumber);
        }
        else
        {
            DisplayResult.GetComponent<UpdateResult>().DisplayResult(check[0] && check[1], PlayerSeatNumber);
        }

    }   
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////End Of Stage 5///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    /// 


    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////Reset For 2nd Round///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    /// 


    public float GetTimeDifference(System.DateTime Previous, System.DateTime Current)
    {
        System.TimeSpan diff = Current - Previous;
        return (float)(System.Math.Round(diff.TotalSeconds, 2));
    }


    public void backToHome()
    {
        SceneManager.LoadScene("Login");
    }
}




