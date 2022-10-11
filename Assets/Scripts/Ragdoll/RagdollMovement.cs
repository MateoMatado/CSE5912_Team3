using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Ragdoll
{
    public class RagdollMovement : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] GameObject ragRoot;
        [SerializeField] GameObject animRoot;

        private Transform[] bones;
        private ConfigurableJoint[] joints;
        private Quaternion[] initialJointRotations;

        void Start()
        {
            joints = ragRoot.GetComponentsInChildren<ConfigurableJoint>();
            TransformStore[] stores = animRoot.GetComponentsInChildren<TransformStore>();

            initialJointRotations = new Quaternion[joints.Length];
            for (int i = 0; i < joints.Length; i++)
            {
                initialJointRotations[i] = joints[i].transform.localRotation;
            }

            bones = new Transform[stores.Length];
            for (int i = 0; i < stores.Length; i++)
            {
                bones[i] = stores[i].transform;
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < joints.Length; i++)
            {
                // Quaternion code used from ConfigurableJointExtensions.cs on GitHub
                // Written by mstevenson
                Vector3 right = joints[i].axis;
                Vector3 forward = Vector3.Cross(right, joints[i].secondaryAxis).normalized;
                Vector3 up = Vector3.Cross(forward, right).normalized;
                Quaternion worldToJointSpace = Quaternion.LookRotation(forward, up);

                Quaternion resultRotation = Quaternion.Inverse(worldToJointSpace);

                //Debug.Log(bones[i + 1].localRotation);
                //Debug.Log(initialJointRotations[i]);
                resultRotation *= Quaternion.Inverse(bones[i + 1].localRotation) * initialJointRotations[i];
                //resultRotation *= initialJointRotations[i] * Quaternion.Inverse(bones[i + 1].localRotation);
                resultRotation *= worldToJointSpace;

                joints[i].targetRotation = resultRotation;
                //joints[i].targetRotation = Quaternion.Euler(45, 45, 45);
            }
        }
    }
}
