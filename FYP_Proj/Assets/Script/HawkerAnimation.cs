﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkerAnimation : MonoBehaviour
{
    float speed = 1.2f;
    float rotSpeed = 2;

    public Transform backtarget;
    public Transform fronttarget;
    public Transform Looktarget;

    private Transform lookAtNow;
    private Transform goToTarget;

    private bool move = false;
    private bool rotate = true;
    private bool Updated = false;
    private bool CallCustomer = false;
    private bool Ordered = false;

    private bool customerVisited = false;
    private bool informCustomertoPoint = false;

    private int state = 0;

    private int RngTime;

    public GameObject GameManager;

    private int NumOfFood;

    CharacterController controller;
    Animator anim;

    public Canvas YesNoUI, ConfirmUI;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        goToTarget = fronttarget;
        lookAtNow = fronttarget;
        RngTime = Random.Range(8, 15);

        Invoke("moveVendor", RngTime);
    }



    public void customerVisit()
    {
        customerVisited = true;
    }

    public void customerNotVisit()
    {
        customerVisited = false;
    }

    private void moveVendor()
    {
        if (state == 0)
        {
            state = 1;
        }
        else if (state == 1 && !customerVisited)
        {
            state = 2;
        }
        else if (state == 2)
        {
            state = 1;
        }
        Updated = false;

        if (!(customerVisited&&state==1))
            Invoke("moveVendor", RngTime);
    }


    private void repeatAskingWhatCustomerWant()
    {
        //if customer haven't order any food after specific second ask them again
        if(Ordered == false)
            CallCustomer = false;
    }

    public void CustomerOrdered()
    {
        Ordered = true;
        YesNoUI.enabled = true;
        ConfirmUI.enabled = false;
    }

    public void CustomerOrderedCancel()
    {
        Ordered = false;
        YesNoUI.enabled = false;
        //Invoke("repeatAskingWhatCustomerWant", 5);
    }
    public void CustomerOrderConfirmation()
    {
        YesNoUI.enabled = false;
        ConfirmUI.enabled = true;
    }


    public void CustomerOrderedDone()
    {
        YesNoUI.enabled = false;
        ConfirmUI.enabled = false;
        //raise hand animation to ask for money
        anim.SetTrigger("placeOrder");
        GameManager.GetComponent<Eventmanager>().AskForMoney();
        

    }

    public void GoToBackOfStore(int NumOfFood)
    {
        this.NumOfFood = NumOfFood;
        state = 2;
        Updated = false;
        Debug.Log("Move to back");

        Invoke("GoToFrontofStore", 30); //this timing the hawker will go prepare food, so we can see whether the user will take this time to prepare the tray
    }
    public void GoToFrontofStore()
    {
        //carry food and go back to customer
        if(NumOfFood == 1)
            anim.SetBool("carryOneItem", true);
        else
            anim.SetBool("carryTwoItem", true);
        state = 1;
        Updated = false;
        Debug.Log("Move to Front");
    }

    public void DoneServingFood()
    {
        anim.SetBool("carryOneItem", false);
        anim.SetBool("carryTwoItem", false);
    }


    // Update is called once per frame
    void Update()
    {
        if (state == 1 && !Updated)
        {
            move = true;
            goToTarget = lookAtNow = fronttarget;
            Updated = true;
            anim.SetBool("reached", false);
        }

        else if (state == 2 && !Updated)
        {
            move = true;
            goToTarget = lookAtNow = backtarget;
            Updated = true;
            anim.SetBool("reached", false);
        }

        if (move)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goToTarget.position, step);
            anim.SetFloat("speed", 1);
        }

        if (Vector3.Distance(transform.position, goToTarget.position) < 0.1f)
        {
            move = false;
            anim.SetBool("reached", true);
            anim.SetFloat("speed", 0);
                rotate = false;
            lookAtNow = Looktarget;


            
        }

        if (!rotate)
        {
            Vector3 direction = lookAtNow.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        }

        //infront of customer
        if (state == 1 && customerVisited && !CallCustomer && !move)
        {
            if (!informCustomertoPoint)
            {
                GameManager.GetComponent<Eventmanager>().AskWhatFoodToOrder();
                GameManager.GetComponent<Eventmanager>().enableMenuSelection();
            }

            else
                GameManager.GetComponent<Eventmanager>().AskCustomerToPointAtMenu();
            informCustomertoPoint = true;
            CallCustomer = true; //this bool prevent update to keep on calling this function
            Invoke("repeatAskingWhatCustomerWant", 10);

        }
    }
}
