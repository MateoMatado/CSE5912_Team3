using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Ragdoll
{
    public class ActiveRagdollToggle
    {
        GameObject root;
        private Rigidbody[] bodies;
        Camera cam;
        private ConfigurableJoint[] joints;
        private (JointDrive, JointDrive)[] drives;
        private JointDrive zeroDrive;

        private Player.PlayerStateManager stateMachine;

        private float rollForce;

        private bool _rag = false;
        public bool rag { get { return _rag; } }

        private bool infiniteJump = false;

        public ActiveRagdollToggle(ConfigurableJoint[] joints, GameObject bodyRoot, float rollForce)
        {
            this.joints = joints;
            this.rollForce = rollForce;
            bodies = bodyRoot.GetComponentsInChildren<Rigidbody>();
            Events.EventsPublisher.Instance.SubscribeToEvent("ToggleRagdoll", ToggleRagdoll);
            Events.EventsPublisher.Instance.SubscribeToEvent("Roll", Roll);
            Events.EventsPublisher.Instance.SubscribeToEvent("InfiniteJump", ToggleInfiniteJump);

            zeroDrive = new JointDrive();
            zeroDrive.maximumForce = 0;
            zeroDrive.positionDamper = 0;
            zeroDrive.positionSpring = 0;

            drives = new (JointDrive, JointDrive)[joints.Length];
        }

        ~ActiveRagdollToggle()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ToggleRagdoll", ToggleRagdoll);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("Roll", Roll);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("InfiniteJump", ToggleInfiniteJump);
        }

        private void ToggleRagdoll(object sender, object data)
        {
            if (data != null)
            {
                _rag = !((bool)data);
            }
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
            else if (stateMachine.StateMachine.CurrentState != PlayerStateMachine.CannonAimState)
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

        void Roll(object sender, object data)
        {
            if ((!rag || infiniteJump) && stateMachine.StateMachine.CurrentState != PlayerStateMachine.CannonAimState)
            {
                Vector3 force = ConvertToWorldInput(((InputAction)data).ReadValue<Vector2>()) + new Vector3(0, 1, 0);
                force = force.normalized * rollForce;

                foreach (Rigidbody body in bodies)
                {
                    body.AddForce(force, ForceMode.Impulse);
                }

                if (!rag)
                {
                    Events.EventsPublisher.Instance.PublishEvent("ToggleRagdoll", null, null);
                }
            }
        }

        public void GetCamera(Camera cam)
        {
            this.cam = cam;
        }

        public void GetStateMachine(Player.PlayerStateManager machine)
        {
            stateMachine = machine;
        }


        private Vector3 ConvertToWorldInput(Vector2 inVec)
        {
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z);
            cameraRight = new Vector3(cameraRight.x, 0, cameraRight.z);

            return Vector3.Normalize((cameraForward * inVec.y) + (cameraRight * inVec.x));
        }

        private void ToggleInfiniteJump(object sender, object data)
        {
            if (infiniteJump)
            {
                infiniteJump = false;
            }
            else
            {
                infiniteJump = true;
            }
        }
    }
}
