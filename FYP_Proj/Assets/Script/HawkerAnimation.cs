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

    private bool move=false;
    private bool rotate = true;

    CharacterController controller;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            move = true;
        }

        if (move)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, fronttarget.position, step);
            anim.SetFloat("speed", 1);
        }

        if (Vector3.Distance(transform.position, fronttarget.position) < 0.1f)
        {
            move = false;
            anim.SetBool("reached", true);
            anim.SetFloat("speed", 0);
            rotate = false;
        }
        else if (!rotate)
        {
            Vector3 direction = Looktarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        }
        

    }
}
