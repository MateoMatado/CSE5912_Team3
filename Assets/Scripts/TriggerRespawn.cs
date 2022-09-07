using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Scripts
{
    public class TriggerRespawn : MonoBehaviour
    {
        [SerializeField] private Vector3 respawnLocation;

        // Start is called before the first frame update
        void Start()
        {
            Events.EventsPublisher.Instance.RegisterEvent("PlayerDeath");
        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = respawnLocation;
            Events.EventsPublisher.Instance.PublishEvent("PlayerDeath", null, null);
        }
    }
}