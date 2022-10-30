using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class TrajectoryLineDisable : MonoBehaviour
{
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        EventsPublisher.Instance.SubscribeToEvent("StopFlying", StopFlying);
    }

    private void StopFlying(object sender, object data)
    {
        line.positionCount = 0;
    }
}
