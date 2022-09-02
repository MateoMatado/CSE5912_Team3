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

        }

        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = respawnLocation;
        }
    }
}