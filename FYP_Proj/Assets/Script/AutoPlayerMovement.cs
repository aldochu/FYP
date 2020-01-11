using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayerMovement : MonoBehaviour
{
    float speed = 10;
    public Transform[] path1;
    public Transform[] path2;
    public Transform[] path13;
    public Transform[] path46;
    public Transform[] seats;

    private Transform oneWay;

    private bool move = false;

    private bool VendorLocation = true;
    private bool Side = false; //true : left (seat1-3), false: right (seat4-6)

    private int seatsToGo; // this number must be from 0-5
    private Transform goToLocation;
    private int curPathCount;

    public GameObject myCam;

    public GameObject GameManager;

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
                goToLocation = path13[0];
                break;
            case 3:
                goToLocation = path46[0];
                break;
            case 4:
                goToLocation = oneWay;
                break;
            default:
                goToLocation = path46[0];
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
                goToLocation = path13[curPathCount];
                break;
            case 3:
                goToLocation = path46[curPathCount];
                break;
            case 4:
                goToLocation = oneWay;
                break;
            default:
                goToLocation = path46[curPathCount];
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
                return path13.Length;
            case 3:
                return path46.Length;
            case 4:
                return 1;
            default:
                return path46.Length;
        }
    }

    public void movePlayer(int location)
    {
        seatsToGo = location;
        UpdateGoToLocation();
        curPathCount = 1;
        move = true;
        VendorLocation = true;
    }

    public void movePlayerToSeat(int seatNumber)
    {
        curPathCount = 1;

        if (VendorLocation) //straightAway go find seats
        {
            oneWay = seats[seatNumber - 1]; //array starts from 0 and seat number is 1-6
            if (seatNumber < 4)
            {
                Side = true; //left side
            }
            else
            {
                Side = false; //right side
            }
            VendorLocation = false;
            seatsToGo = 4;

        }
        else
        {
            if (!CheckSameSide(seatNumber))//if different side
            {
                if (seatNumber < 4) // seat 1-3
                {
                    seatsToGo = 2;
                    path13[2] = seats[seatNumber - 1];
                    Side = true;//left side
                }
                else
                {
                    seatsToGo = 3;
                    path46[2] = seats[seatNumber - 1];
                    Side = false;//right side
                }
            }
            else//if same side
            {
                oneWay = seats[seatNumber - 1]; //one way
                seatsToGo = 4;
            }
            

        }


        UpdateGoToLocation();
        move = true;
    }


    private bool CheckSameSide(int seatNumber) //this function will check whether the seat going next belong to the same side
    {
        if (Side && seatNumber < 4)//left side
            return true;
        else if (!Side && seatNumber > 3)//right side
            return true;
        else
            return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move) //this section is to move the 1st in the queue
        {

            ////////Move the human towards the designated point
            float step = speed * Time.deltaTime;
            myCam.transform.position = Vector3.MoveTowards(myCam.transform.position, goToLocation.position, step);

        }

        if (move) // this section is to check whether the 1st in the queue have reacched the designated location
        {
            if (Vector3.Distance(myCam.transform.position, goToLocation.position) < 0.05f) //check the distance of the between the 1st in queue and the designated location
            {
                if (curPathCount < GetGoToLocationLength()) //path is make up of multiple point for the AI to walk towards to, so if the point isn't at the end yet, it should walk to the next point
                {
                    NextLocationToGO();
                    //goToLocation = path1[curPathCount];
                    curPathCount++;
                }
                else //reached the last point
                {
                    //reched
                    if (!(seatsToGo == 0 || seatsToGo == 1))
                    {
                        GameManager.GetComponent<Eventmanager>().CheckSeatAvailability(); //increment tray count
                    }
                       
                    move = false;
                }
            }
        }


    
    }
}
