using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine stateMachine;

    public void SwitchState(GameState newState)
    {
        stateMachine.CurrentState = newState;
        Exit();
        newState.Enter();
    }
    
    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }
}
