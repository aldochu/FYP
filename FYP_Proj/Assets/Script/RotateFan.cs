﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFan : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(0, 5, 0, Space.Self);
    }
}
