using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.PlayerMovement
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] float jumpHeight;

        Rigidbody body;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerJump", OnJump);
            body = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerJump", OnJump);
        }

        void OnJump(object sender, object data)
        {
            body.velocity += new Vector3(0, Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight), 0);
        }
    }
}