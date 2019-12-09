using System.Collections;
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

    CharacterController controller;
    Animator anim;

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
    }

    // Update is called once per frame
    void Update()
    {
        if ((state == 1 || Input.GetKey(KeyCode.W)) && !Updated)
        {
            move = true;
            goToTarget = lookAtNow = fronttarget;
            Updated = true;
        }

        else if ((state == 2 || Input.GetKey(KeyCode.Q)) && !Updated)
        {
            move = true;
            goToTarget = lookAtNow = backtarget;
            Updated = true;
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
        else if (!rotate)
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
