using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class RagRoot : MonoBehaviour
    {
        private Rigidbody _body;
        private Rigidbody[] _bodies;

        public Rigidbody rootBody { get { return _body; } }
        public Rigidbody[] bodies { get { return _bodies; } }

        // Used to denote the root of the ragdol's motion
        void Start()
        {
            _body = GetComponent<Rigidbody>();
            _bodies = _body.transform.parent.GetComponentsInChildren<Rigidbody>();
        }
    }
}