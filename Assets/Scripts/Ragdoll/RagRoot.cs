using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class RagRoot : MonoBehaviour
    {
        private Rigidbody _body;

        public Rigidbody body { get { return _body; } }

        // Used to denote the root of the ragdol's motion
        void Start()
        {
            _body = GetComponent<Rigidbody>();
        }
    }
}