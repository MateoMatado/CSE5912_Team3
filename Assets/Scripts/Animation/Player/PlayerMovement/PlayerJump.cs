using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.PlayerMovement
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] float jumpHeight;
        [SerializeField] float rayLength;
        [SerializeField] Transform bottom;

        Ray ray;

        Rigidbody body;
        bool jumped = false;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerJump", OnJump);
            body = GetComponent<Rigidbody>();
            Debug.Log("Casting ray at: " + bottom.position);
            StartCoroutine(CheckGround());
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerJump", OnJump);
        }

        void OnJump(object sender, object data)
        {
            ray = new Ray(bottom.position, new Vector3(0, -1, 0));
            if (true || Physics.Raycast(ray, rayLength))
            {
                body.velocity = new Vector3(body.velocity.x, Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight), body.velocity.z);
                jumped = true;
            }
        }

        private IEnumerator CheckGround()
        {
            while (true)
            {
                while (jumped)
                {
                    ray = new Ray(bottom.position, new Vector3(0, -1, 0));
                    if (Physics.Raycast(ray, rayLength) && body.velocity.y <= 0)
                    {
                        jumped = false;
                        Events.EventsPublisher.Instance.PublishEvent("PlayerGrounded", null, null);
                    }
                    yield return null;
                }
                while (!jumped) { yield return null; }
            }
        }
    }
}