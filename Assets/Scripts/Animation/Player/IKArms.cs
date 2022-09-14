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

        private InputAction leftAction;
        private InputAction rightAction;

        // For testing
        float rot = 0;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", StartRight);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
        }

        private void StartLeft(object sender, object data)
        {
            leftAction = (InputAction)data;
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
        }

        private void StartRight(object sender, object data)
        {
            rightAction = (InputAction)data;
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if(leftAction != null && leftAction.IsPressed())
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(0, 0, 0));
                anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Euler(rot, rot, rot));
                rot = (rot + 5) % 360;
            }

            if (rightAction != null && rightAction.IsPressed())
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, pickup.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, pickup.rotation);
            }
        }
    }
}