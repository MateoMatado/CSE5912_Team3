using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Ragdoll
{
    public class ActiveRagdollToggle
    {
        private ConfigurableJoint[] joints;
        private (JointDrive, JointDrive)[] drives;
        private JointDrive zeroDrive;

        private bool _rag = false;
        public bool rag { get { return _rag; } }

        public ActiveRagdollToggle(ConfigurableJoint[] joints)
        {
            this.joints = joints;
            Events.EventsPublisher.Instance.SubscribeToEvent("ToggleRagdoll", ToggleRagdoll);

            zeroDrive = new JointDrive();
            zeroDrive.maximumForce = 0;
            zeroDrive.positionDamper = 0;
            zeroDrive.positionSpring = 0;

            drives = new (JointDrive, JointDrive)[joints.Length];
        }

        ~ActiveRagdollToggle()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ToggleRagdoll", ToggleRagdoll);
        }

        private void ToggleRagdoll(object data, object sender)
        {
            if (rag)
            {
                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i].angularXDrive = drives[i].Item1;
                    joints[i].angularYZDrive = drives[i].Item2;

                    //joints[i].angularXMotion = ConfigurableJointMotion.Free;
                    //joints[i].angularYMotion = ConfigurableJointMotion.Free;
                    //joints[i].angularZMotion = ConfigurableJointMotion.Free;
                }
                _rag = false;
            }
            else
            {
                for(int i = 0; i < joints.Length; i++)
                {
                    drives[i].Item1 = joints[i].angularXDrive;
                    drives[i].Item2 = joints[i].angularYZDrive;

                    joints[i].angularXDrive = zeroDrive;
                    joints[i].angularYZDrive = zeroDrive;

                    //joints[i].angularXMotion = ConfigurableJointMotion.Limited;
                    //joints[i].angularYMotion = ConfigurableJointMotion.Limited;
                    //joints[i].angularZMotion = ConfigurableJointMotion.Limited;
                }
                _rag = true;
            }
        }
    }
}
