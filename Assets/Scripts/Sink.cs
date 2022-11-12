using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class Sink : MonoBehaviour
{
    public float sinkSpeed = 1f;
    private float sinkTime = 0f;
    [SerializeField] private string eventName = "Sink";

    void Start()
    {
        EventsPublisher.Instance.SubscribeToEvent(eventName, HandleDissolve);
    }

    void OnDestroy()
    {
        EventsPublisher.Instance.UnsubscribeToEvent(eventName, HandleDissolve);
    }

    private void HandleDissolve(object sender, object data)
    {
        if ((GameObject)data == gameObject && sinkTime == 0)
        {
            StartCoroutine(SinkCoroutine());
        }
    }

    private IEnumerator SinkCoroutine()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        Vector3 pos = transform.position;
        while (true)
        {
            sinkTime += sinkSpeed * Time.deltaTime;
            transform.position = pos + .5f * Physics.gravity * (sinkTime * sinkTime);
            yield return null;
        }
    }
}
