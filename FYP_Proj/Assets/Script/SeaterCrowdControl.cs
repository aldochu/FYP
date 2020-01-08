using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaterCrowdControl : MonoBehaviour
{
    float speed = 1.2f;
    float rotSpeed = 4;


    public GameObject[] Human; //this human are those seating on the designated seat, there will only be 2 human

    public Transform[] path1; //path 1 for human 1
    public Transform[] path2; //path 2 for human 2

    public int humanNum;
    private Transform goToLocation;
    private int curPathCount;

    private bool move = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateGoToLocation()
    {
        switch (humanNum)
        {
            case 0:
                goToLocation = path1[0];
                break;
            case 1:
                goToLocation = path2[0];
                break;
        }
    }

    private void NextLocationToGO()
    {
        switch (humanNum)
        {
            case 0:
                goToLocation = path1[curPathCount];
                break;
            case 1:
                goToLocation = path2[curPathCount];
                break;
        }
    }

    private int GetGoToLocationLength()
    {
        switch (humanNum)
        {
            case 0:
                return path1.Length;
            case 1:
                return path2.Length;
            default:
                return path2.Length;
        }
    }

    private void startWalking() //this delay is need so that it can play the standing animation 1st before it starts moving
    {
        move = true;
    }


    // Update is called once per frame
    void Update()
    {

        //Move the queue Upward, 2nd to last person will move up
        if (Input.GetKey(KeyCode.K)) //this part of the code will only be called once
        {
            //reset value to move 
           
            anim = Human[humanNum].GetComponent<Animator>();
            //goToLocation = path1[0];
            UpdateGoToLocation();
            curPathCount = 1;
            //

            anim.SetTrigger("GoOff");//playing walking animation

            Invoke("startWalking",1.5f);

        }
        else if (move) //this section is to move the 1st in the queue
        {
            Vector3 direction = goToLocation.position - Human[humanNum].transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Human[humanNum].transform.rotation = Quaternion.Lerp(Human[humanNum].transform.rotation, rotation, rotSpeed * Time.deltaTime); //this part is to rotate the human

            ////////Move the human towards the designated point
            float step = speed * Time.deltaTime;
            Human[humanNum].transform.position = Vector3.MoveTowards(Human[humanNum].transform.position, goToLocation.position, step);

        }

        if (move) // this section is to check whether the 1st in the queue have reacched the designated location
        {
            if (Vector3.Distance(Human[humanNum].transform.position, goToLocation.position) < 0.05f) //check the distance of the between the 1st in queue and the designated location
            {
                /*
                move = false;
                anim.SetBool("reached", true);
                anim.SetFloat("speed", 0);
                rotate = false;
                */
                if (curPathCount < GetGoToLocationLength()) //path1 is make up of multiple point for the AI to walk towards to, so if the point isn't at the end yet, it should walk to the next point
                {
                    NextLocationToGO();
                    //goToLocation = path1[curPathCount];
                    curPathCount++;
                }
                else //reached the last point
                {
                    //destroy the human
                    Destroy(Human[humanNum]);
                    move = false;
                }
            }
        }
        /*
        else if (!reached)
        {
            Vector3 direction = Looktarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        }
        */

    }
}
