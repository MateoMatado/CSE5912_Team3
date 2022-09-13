using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Animation.Player
{
    public class RotateOnMove : MonoBehaviour
    {
        [SerializeField] float speed;

        InputAction movement;
        bool moving = false;

        // Start is called before the first frame update
        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerMove", Moved);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerStop", Stopped);
            StartCoroutine(RotateToMovement());
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", Moved);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", Stopped);
        }

        private void Moved(object sender, object data)
        {
            moving = true;
        }

        private void Stopped(object sender, object data)
        {
            moving = false;
        }

        IEnumerator RotateToMovement()
        {
            while(true)
            {
                while(moving)
                {
                    Debug.Log("Turning");
                    yield return null;
                }
                yield return null;
            }
        }
    }
}