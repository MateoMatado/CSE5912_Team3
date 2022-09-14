using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Animation.Player
{
    public class IKArms : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] Transform pickup;

        [SerializeField] float lMaxHAngle;
        [SerializeField] float lMinHAngle;
        [SerializeField] float lMaxVAngle;
        [SerializeField] float lMinVAngle;
        [SerializeField] float rMaxHAngle;
        [SerializeField] float rMinHAngle;
        [SerializeField] float rMaxVAngle;
        [SerializeField] float rMinVAngle;
        [SerializeField] float length;

        private Vector2 lHalfWH, lDefaultPos;
        private Vector2 rHalfWH, rDefaultPos;

        private InputAction leftAction;
        private InputAction rightAction;
        private InputAction stickAction;

        // For testing
        float rot = 0;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", StartRight);

            lHalfWH = new Vector2(lMaxHAngle - lMinHAngle, lMaxVAngle - lMinVAngle) / 2;
            lDefaultPos = new Vector2(lMaxHAngle + lMinHAngle, lMaxVAngle + lMinVAngle) / 2;
            rHalfWH = new Vector2(rMaxHAngle - rMinHAngle, rMaxVAngle - rMinVAngle) / 2;
            rDefaultPos = new Vector2(rMaxHAngle + rMinHAngle, rMaxVAngle + rMinVAngle) / 2;
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
            //Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArm", StartStick);
        }

        private void StartLeft(object sender, object data)
        {
            leftAction = ((((InputAction, InputAction))data).Item1);
            stickAction = ((((InputAction, InputAction))data).Item2);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
        }

        private void StartRight(object sender, object data)
        {
            rightAction = ((((InputAction, InputAction))data).Item1);
            stickAction = ((((InputAction, InputAction))data).Item2);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
        }

        /*private void StartStick(object sender, object data)
        {
            stickAction = (InputAction)data;
            Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArm", StartStick);
        }*/

        private void OnAnimatorIK(int layerIndex)
        {
            if(leftAction != null && leftAction.IsPressed())
            {
                Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position;
                Vector2 inVector = stickAction.ReadValue<Vector2>();

                Vector2 armAngles = inVector * lHalfWH + lDefaultPos;
                Vector3 result = transform.forward * length;
                result = Quaternion.AngleAxis(-armAngles.y, transform.right) * result;
                result = Quaternion.AngleAxis(armAngles.x, transform.up) * result;
                result += shoulder;

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, result);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Euler(rot, rot, rot));
                rot = (rot + 5) % 360;
            }

            if (rightAction != null && rightAction.IsPressed())
            {
                Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position;
                Vector2 inVector = stickAction.ReadValue<Vector2>();

                Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
                Vector3 result = transform.forward * length;
                result = Quaternion.AngleAxis(-armAngles.y, transform.right) * result;
                result = Quaternion.AngleAxis(armAngles.x, transform.up) * result;
                result += shoulder;

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, result);
                anim.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.Euler(rot, rot, rot));
                rot = (rot + 5) % 360;
            }
        }
    }
}