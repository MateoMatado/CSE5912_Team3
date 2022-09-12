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
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.PublishEvent("Unpause", null, null);
    }
}
