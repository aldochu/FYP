using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class AI : MonoBehaviour
{
    public int currentState = 1;
    public int newState = 1;


    public stateMachine<AI> stateMachine { get; set; }
  

    void Start()
    {
        stateMachine = new stateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
    }

    public void UpdateState(int state)
    {
        newState = state;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
