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
    public Transform[] foodPlacement;

    public GameObject[] Human;
    private GameObject tempFoodObj;

    public Transform firstPosition;

    private bool move = false;
    private bool reached = false;
    private int curPathCount = 1;

    CharacterController controller;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = Human[0].GetComponent<Animator>();
        goToLocation = path1[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            move = true;

            for (int i = 0; i < 3; i++)
            {
                Human[i + 1].GetComponent<MoveQueuer>().move(firstPosition, i ) ;
            }
        }
        else if (move)
        {
            Vector3 direction = goToLocation.position - Human[currentIndex].transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Human[currentIndex].transform.rotation = Quaternion.Lerp(Human[currentIndex].transform.rotation, rotation, rotSpeed * Time.deltaTime);
            tempFoodObj = Human[currentIndex].transform.GetChild(2).gameObject;
            tempFoodObj.SetActive(true);


            float step = speed * Time.deltaTime;
            Human[currentIndex].transform.position = Vector3.MoveTowards(Human[currentIndex].transform.position, goToLocation.position, step);
            anim.SetFloat("speed", 1);
        }

        if (!reached)
        {
            if (Vector3.Distance(Human[currentIndex].transform.position, goToLocation.position) < 0.05f)
            {
                /*
                move = false;
                anim.SetBool("reached", true);
                anim.SetFloat("speed", 0);
                rotate = false;
                */
                if (curPathCount < path1.Length)
                {
                    goToLocation = path1[curPathCount];
                    curPathCount++;
                }
                else
                {
                    tempFoodObj.transform.position = foodPlacement[0].transform.position;
                    tempFoodObj.transform.parent = null;
                    anim.SetFloat("speed", 0);
                    anim.SetTrigger("reached");
                    move = false;              
                    reached = false;
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
