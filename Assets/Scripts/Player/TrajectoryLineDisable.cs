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
        EventsPublisher.Instance.SubscribeToEvent("LaunchedCannon", StopFlying);
    }

    private void StopFlying(object sender, object data)
    {
        if (line != null)
            line.positionCount = 0;
    }
}
