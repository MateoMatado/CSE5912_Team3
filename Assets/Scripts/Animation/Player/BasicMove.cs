using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player
{
    public class BasicMove : MonoBehaviour
    {
        [SerializeField] Animator anim;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerMove", SetMove);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerStop", SetStop);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerJump", SetJump);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerGrounded", SetGrounded);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", SetMove);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", SetStop);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerJump", SetJump);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerGrounded", SetGrounded);
        }

        private void SetJump(object sender, object data)
        {
            anim.ResetTrigger("SetGrounded");
            anim.SetTrigger("SetJump");
        }

        private void SetGrounded(object sender, object data)
        {
            anim.ResetTrigger("SetJump");
            anim.SetTrigger("SetGrounded");
        }

        private void SetMove(object sender, object data)
        {
            anim.ResetTrigger("SetIdle");
            anim.SetTrigger("SetRunning");
        }

        private void SetStop(object sender, object data)
        {
            anim.ResetTrigger("SetRunning");
            anim.SetTrigger("SetIdle");
        }
    }
}