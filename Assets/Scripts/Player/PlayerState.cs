using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public PlayerStateMachine stateMachine;
    
    public virtual void Enter()
    {
        Debug.Log("<color=#71f0b4>Entering Player State: </color><color=white>" + this.GetType().Name + "</color>");
    }

    public virtual void Exit()
    {
        Debug.Log("<color=#f071c7>Exiting Player State: </color><color=white>" + this.GetType().Name + "</color>");
    }
}
