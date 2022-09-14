using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Animation.Player
{
    public class RotateOnMove : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] Rigidbody body;

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
                    Vector3 target = body.velocity;
                    float angle = Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg;
                    Quaternion final = Quaternion.AngleAxis(-angle+90, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, final, speed * Time.deltaTime);
                    yield return null;
                }
                yield return null;
            }
        }
    }
}