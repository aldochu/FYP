using UnityEngine;
using StateStuff;

public class FirstState : State<AI>
{
    private static FirstState _instance;
    private Eventmanager myEvent;

    private FirstState()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public static FirstState Instance
    {
        get {
            if(_instance == null)
            {
                new FirstState();
            }

            return _instance;

        }
    }
    public override void EnterState(AI _owner)
    {
       Debug.Log("Entering First State");
       _owner.gameObject.GetComponent<Eventmanager>().enabled = false;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exiting First State");
    }

    public override void UpdateState(AI _owner)
    {
        if (_owner.currentState != _owner.newState)
        {
            _owner.currentState = _owner.newState;
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
    }
}
