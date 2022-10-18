using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public static PlayerState   DefaultState = new DefaultPlayerState(),
                                IKState = new IKState(),
                                TargetingState = new TargetingState(),
                                CannonAimState = new CannonAimState();

    private PlayerState currentState;

    public PlayerState CurrentState 
    {
        get
        {
            return currentState;
        }
    }

    public PlayerStateMachine()
    {
        currentState = DefaultState;
        currentState.stateMachine = this;
    }

    // returns true on successful state change
    public bool SwitchState(PlayerState newState)
    {
        if (newState == CurrentState) return false;
        newState.stateMachine = this;
        currentState.Exit();
        currentState = newState;
        newState.Enter();
        return true;
    }
}

public class DefaultPlayerState : PlayerState
{

}
