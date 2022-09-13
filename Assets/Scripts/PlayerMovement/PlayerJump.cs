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

        void OnJump(object sender, object data)
        {
            body.velocity += new Vector3(0, Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight), 0);
        }
    }
}