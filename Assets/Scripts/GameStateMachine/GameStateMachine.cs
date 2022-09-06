using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine 
{
    private static GameStateMachine instance = null;

    public static GameStateMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameStateMachine();
            }
            return instance;
        }
    }

    private GameState currentState;

    public GameState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    private GameStateMachine()
    {
        currentState = new DefaultState();
    }

    public void SwitchState(GameState newState)
    {
        currentState.Exit();
        currentState = newState;
        newState.Enter();
    }
}

public class DefaultState : GameState
{

}
