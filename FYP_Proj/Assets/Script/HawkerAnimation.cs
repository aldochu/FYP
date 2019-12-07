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

    private bool move=false;
    private bool rotate = true;
    private bool Updated = false;

    private bool customerVisited = false;

    private int state = 0;

    private int RngTime;

    CharacterController controller;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        goToTarget = fronttarget;
        lookAtNow = fronttarget;
        RngTime = Random.Range(10, 20);

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
        else if (state == 1)
        {
            state = 2;
        }
        else if (!customerVisited)
        {
            state = 1;
        }
        Updated = false;
        Invoke("moveVendor", RngTime);
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

        else if ((state == 2 || Input.GetKey(KeyCode.Q)) && !Updated && !customerVisited)
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
        

    }
}
