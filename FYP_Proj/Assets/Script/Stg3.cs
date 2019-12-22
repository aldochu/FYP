using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stg3 : MonoBehaviour
{

    public Canvas YesUI;
    public GameObject CoinForOneOrder, CoinForTwoOrder, GameManager;
    private bool YesUIAppear = false;
    private bool PayingInProgress = false, PayingInProgressCheck = false; //should not rush the ppl to pay if they are already slowly paying, only not doing any action should they be rushed
    private int currentCoinAmt, PrevCoinAmt;


    public void Start()
    {
        currentCoinAmt = PrevCoinAmt = 0;
        Invoke("repeatAskMoney", 15);
    }

    public void repeatAskMoney()
    {
        if (!PayingInProgress)
        {
            GameManager.GetComponent<Eventmanager>().AskToMoneyFaster();
            Invoke("repeatAmount", 2);
        }
        
        //until the condition where play paid the money
        Invoke("repeatAskMoney", 15);
    }


    public void repeatAmount()
    {
        GameManager.GetComponent<Eventmanager>().AskForMoney();
    }


    public void BeginStgThree(int NumOfOrder)
    {
        if (NumOfOrder == 1)
        {
            CoinForOneOrder.SetActive(true);
        }
        else
        {
            CoinForTwoOrder.SetActive(true);
        }
    }

    public void CloseStageThree()
    {
        CoinForOneOrder.SetActive(false);
        CoinForOneOrder.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            PayingInProgress = true;
            currentCoinAmt++;

            if(!PayingInProgressCheck)
            {
                Invoke("noPayingAction", 10); // this code will check whether the player have not been doing anything after 10 second
                PayingInProgressCheck = true;
            }
           

            GameManager.GetComponent<Eventmanager>().CustomerPay(other.GetComponent<CoinValue>().getValue());
            Destroy(other.gameObject);
            if (!YesUIAppear)
            {
                YesUI.enabled = YesUIAppear = true;
            }
        }


    }

    private void noPayingAction()
    {

        if (currentCoinAmt == PrevCoinAmt)
            PayingInProgress = false;
        else
            PrevCoinAmt = currentCoinAmt;

        PayingInProgressCheck = false; //checked done, 

        Invoke("noPayingAction", 10); // this code will check whether the player have not been doing anything after 10 second
    }
}
