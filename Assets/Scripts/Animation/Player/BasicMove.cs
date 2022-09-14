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
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", SetMove);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", SetStop);
        }

        private void SetMove(object sender, object data)
        {
            anim.SetTrigger("SetRunning");
        }

        private void SetStop(object sender, object data)
        {
            anim.SetTrigger("SetIdle");
        }
    }
}