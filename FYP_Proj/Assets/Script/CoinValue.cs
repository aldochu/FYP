using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinValue : MonoBehaviour
{
    // Start is called before the first frame update

    public float value;
    private bool held;
    private bool drop;

    private void Start()
    {
        held = false;
        drop = false;
    }

    public float getValue()
    {
        return value;
    }

    public void hold()
    {
        held = true;
    }

    public void release()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
        held = false;
        drop = true;
    }

    public bool getStatus()
    {
        return held;
    }

    public void dropComplete()
    {
        drop = false;
    }

    private void Update()
    {
        if (drop)
        {
            transform.Translate(0, -Time.deltaTime, 0, Space.World);
        }
    }
}
