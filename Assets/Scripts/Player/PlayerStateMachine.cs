using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public static PlayerState DefaultState = new DefaultPlayerState();

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
    }

    public void SwitchState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }
}

public class DefaultPlayerState : PlayerState
{

}
