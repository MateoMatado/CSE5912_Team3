using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I don't know why this is necessary but I couldn't find any other solution
namespace Team3.Ragdoll
{
    public class SetConnectedBody : MonoBehaviour
    {
        [SerializeField] Rigidbody body;
        [SerializeField] ConfigurableJoint joint;

        void Start()
        {
            joint.connectedBody = null;
            joint.connectedBody = body;
        }
    }
}