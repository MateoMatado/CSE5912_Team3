using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState
{
    protected GameStateMachine stateMachine;
    
    public virtual void Enter()
    {
        Debug.Log("<color=#88ff7d>Entering Game State: </color><color=white>" + this.GetType().Name + "</color>");
    }

    public virtual void Exit()
    {
        Debug.Log("<color=#ff7d7d>Exiting Game State: </color><color=white>" + this.GetType().Name + "</color>");
    }
}
