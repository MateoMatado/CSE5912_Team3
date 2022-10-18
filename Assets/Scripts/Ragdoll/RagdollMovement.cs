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

        private JointDrive[,] forces;

        bool rag = false;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("ToggleRagdoll", ToggleRagdoll);

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

            forces = new JointDrive[2,joints.Length];

            for (int i = 0; i < joints.Length; i++)
            {
                forces[0,i] = joints[i].angularYZDrive;
                forces[1,i] = joints[i].angularXDrive;
            }
        }

        private void OnDisable()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("ToggleRagdoll", ToggleRagdoll);
        }

        void ToggleRagdoll(object sender, object data)
        {
            if (rag)
            {
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].angularYZDrive = forces[0, i];
                    joints[i].angularXDrive = forces[1, i];
                }
            }
            else
            {
                for (int i = 0; i < joints.Length; i++)
                {
                    forces[0, i] = joints[i].angularYZDrive;
                    forces[1, i] = joints[i].angularXDrive;
                    joints[i].angularYZDrive = new JointDrive();
                    joints[i].angularXDrive = new JointDrive();
                }
            }
            rag = !rag;
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

                resultRotation *= Quaternion.Inverse(bones[i + 1].localRotation) * initialJointRotations[i];
                resultRotation *= worldToJointSpace;

                joints[i].targetRotation = resultRotation;
            }
        }
    }
}
