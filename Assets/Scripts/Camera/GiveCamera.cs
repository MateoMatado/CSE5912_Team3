using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3
{
    public class GiveCamera : MonoBehaviour
    {
        [SerializeField] Transform cameraFollow;
        [SerializeField] Camera cam;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("GrabCamera", GiveCam);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("GrabCamera", GiveCam);
        }

        void GiveCam(object sender, object data)
        {
            Events.EventsPublisher.Instance.PublishEvent("ReceiveCameraTransform", null, cameraFollow);
            Events.EventsPublisher.Instance.PublishEvent("ReceiveCamera", null, cam);
        }
    }
}