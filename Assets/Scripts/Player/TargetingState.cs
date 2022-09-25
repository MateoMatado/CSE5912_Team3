using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class TargetingState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        EventsPublisher.Instance.PublishEvent("EnterTargetingState", null, null);
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.PublishEvent("ExitTargetingState", null, null);
    }
}
