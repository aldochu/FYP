using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdControl : MonoBehaviour
{
    float speed = 1.2f;
    float rotSpeed = 4;

    private Transform goToLocation;
    public Transform[] path1;

    public GameObject Human1;

    private bool move = false;
    private bool reached = false;
    private int curPathCount = 1;

    CharacterController controller;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = Human1.GetComponent<Animator>();
        goToLocation = path1[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            move = true;
        }
        else if (move)
        {
            Vector3 direction = goToLocation.position - Human1.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            Human1.transform.rotation = Quaternion.Lerp(Human1.transform.rotation, rotation, rotSpeed * Time.deltaTime);

            float step = speed * Time.deltaTime;
            Human1.transform.position = Vector3.MoveTowards(Human1.transform.position, goToLocation.position, step);
            anim.SetFloat("speed", 1);
        }

        if (!reached)
        {
            if (Vector3.Distance(Human1.transform.position, goToLocation.position) < 0.05f)
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
