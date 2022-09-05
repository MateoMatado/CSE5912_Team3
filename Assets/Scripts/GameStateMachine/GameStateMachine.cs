using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine 
{
    private static GameStateMachine instance;

    public static GameStateMachine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new();
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
        set
        {
            currentState = value;
        }
    }

    private GameStateMachine()
    {
        
    }
}
