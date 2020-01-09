using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    float speed = 1.2f;
    float rotSpeed = 4;

    int currentIndex = 0;

    private Transform goToLocation;
    public Transform[] path1;
    public Transform[] path2;
    public Transform[] path3;
    public Transform[] foodPlacement;

    public GameObject[] Human;
    private GameObject tempFoodObj;

    public Transform firstPosition;

    private int seatsToGo; // this number must be from 0-2

    private bool move = false;
    private bool functionCalled = false;
    //private bool reached = false;
    private int curPathCount;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //anim = Human[0].GetComponent<Animator>();
        //goToLocation = path1[0];
    }


    private void UpdateGoToLocation()
    {
        switch (seatsToGo)
        {
            case 0:
                goToLocation = path1[0];
                break;
            case 1:
                goToLocation = path2[0];
                break;
            case 2:
                goToLocation = path3[0];
                break;
            default:
                goToLocation = path3[0];
                break;
        }
    }

    private void NextLocationToGO()
    {
        switch (seatsToGo)
        {
            case 0:
                goToLocation = path1[curPathCount];
                break;
            case 1:
                goToLocation = path2[curPathCount];
                break;
            case 2:
                goToLocation = path3[curPathCount];
                break;
            default:
                goToLocation = path3[curPathCount];
                break;
        }
    }

    private int GetGoToLocationLength()
    {
        switch (seatsToGo)
        {
            case 0:
                return path1.Length;
            case 1:
                return path2.Length;
            case 2:
                return path3.Length;
            default:
                return path3.Length;
        }
    }

    public void MoveAI(int i)
    {
        seatsToGo = i;
        functionCalled = true;
    }

    // Update is called once per frame
    void Update()
    {

        //Move the queue Upward, 2nd to last person will move up
        if (Input.GetKey(KeyCode.L) || functionCalled) //this part of the code will only be called once
        {
            functionCalled = false; //so that this part will only run once

            //reset value to move 
            move = true;
            anim = Human[currentIndex].GetComponent<Animator>();
            //goToLocation = path1[0];
            UpdateGoToLocation();
            curPathCount = 1;
            //

            for (int i = currentIndex; i < 3; i++)
            {
                Human[i + 1].GetComponent<MoveQueuer>().move(firstPosition, i - currentIndex ) ;
            }

            anim.SetFloat("speed", 1);//playing walking animation

            /////////make the food appear
            tempFoodObj = Human[currentIndex].transform.GetChild(2).gameObject;
            tempFoodObj.SetActive(true);
        }
        else if (move) //this section is to move the 1st in the queue
        {
            Vector3 direction = goToLocation.position - Human[currentIndex].transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            Human[currentIndex].transform.rotation = Quaternion.Lerp(Human[currentIndex].transform.rotation, rotation, rotSpeed * Time.deltaTime); //this part is to rotate the human

            ////////Move the human towards the designated point
            float step = speed * Time.deltaTime;
            Human[currentIndex].transform.position = Vector3.MoveTowards(Human[currentIndex].transform.position, goToLocation.position, step);
            
        }

        if (move) // this section is to check whether the 1st in the queue have reacched the designated location
        {
            if (Vector3.Distance(Human[currentIndex].transform.position, goToLocation.position) < 0.05f) //check the distance of the between the 1st in queue and the designated location
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
                    tempFoodObj.transform.position = foodPlacement[seatsToGo].transform.position; //place the food on the table
                    tempFoodObj.transform.rotation = foodPlacement[seatsToGo].transform.rotation; //place the food on the table
                    tempFoodObj.transform.parent = null;//disconnect the food with the AI else it will keep on moving due to the animation
                    anim.SetTrigger("reached");
                    anim.SetFloat("speed", 0);
                   
                    move = false;

                    currentIndex++; 
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
