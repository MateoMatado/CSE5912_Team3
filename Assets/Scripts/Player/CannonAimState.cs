using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class CannonAimState : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        EventsPublisher.Instance.PublishEvent("EnterCannonAimState", null, null);
    }

    public override void Exit()
    {
        base.Exit();
        EventsPublisher.Instance.PublishEvent("ExitCannonAimState", null, null);
    }
}
