﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBodyCollision : MonoBehaviour
{
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.detectCollisions = false;
    }

}
