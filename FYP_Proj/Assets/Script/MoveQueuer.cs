using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveQueuer : MonoBehaviour
{

    private bool moveNow = false;
    private Transform PlaceToGo;
    float speed = 1.2f;
    float reduction;
    Animator anim;
    // Start is called before the first frame update

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void move(Transform t, float reduction)
    {
        PlaceToGo = t;
        this.reduction = reduction;
        moveNow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveNow)
        {

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlaceToGo.position + new Vector3(0,0,reduction), step);
            anim.SetFloat("speed", 1);

            if (Vector3.Distance(transform.position, PlaceToGo.position + new Vector3(0,0,reduction)) < 0.05f)
            {
                anim.SetFloat("speed", 0);
                moveNow = false;
            }
        }
    }

    
}
