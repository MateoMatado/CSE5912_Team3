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

        private void SetMove(object sender, object data)
        {
            anim.SetTrigger("ChangeRun");
        }

        private void SetStop(object sender, object data)
        {
            anim.SetTrigger("ChangeRun");
        }
    }
}