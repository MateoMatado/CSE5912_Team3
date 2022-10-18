using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class PauseState : GameState
{
    public override void Enter()
    {
        base.Enter();
        EventsPublisher.Instance.PublishEvent("Pause", null, null);
        Time.timeScale = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.PublishEvent("Unpause", null, null);
        Time.timeScale = 1f;
    }

    public override void HandlePause()
    {
        stateMachine.SwitchState(GameStateMachine.RunningState);
    }
}
