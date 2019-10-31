using System.Collections;
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
    public Text Ordertext;
    private bool updateText = false;


    public Canvas[] foodStallStage1, foodStallStage2;
    // Start is called before the first frame update
    private string[] OrderedFood;
    private int[] foodAmt;
    private int foodOrderSize = 0;
    private string OrdertextToPrint;


    void Start()
    {
        foodStall2bool = foodStall3bool = false;
        OrderedFood = new string[5];
        foodAmt = new int[5];
    }

    void Update()
    {
        if (updateText)
        {
                Ordertext.text = OrdertextToPrint;
            updateText = false;
        }
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
                foodStallStage2[i].enabled = true;
            }
            else
            {
                foodStallStage1[i].enabled = true;
                foodStallStage2[i].enabled = false;
            }
        }

    }

    public void SelectFood(string order)
    {
        Debug.Log(foodOrderSize);
        if (foodOrderSize < 5)
        {
            if (foodOrderSize == 0)
            {
                OrderedFood[foodOrderSize] = order;
                foodAmt[foodOrderSize++] = 1;
            }
            else
            {
                bool exist = false;
                for (int i = 0; i < foodOrderSize; i++)
                {
                    if (OrderedFood[i] == order)
                    {
                        foodAmt[i]++;
                        exist = true;
                    }
                }

                if (!exist)
                {
                    OrderedFood[foodOrderSize] = order;
                    foodAmt[foodOrderSize++] = 1;
                }


            }


            string combineText = "";

            for (int i = 0; i < foodOrderSize; i++)
            {
                combineText += OrderedFood[i] + " x " + foodAmt[i] + "\n";
                //Debug.Log()
            }
            OrdertextToPrint = combineText;
            updateText = true;


        }

        

    }


}
