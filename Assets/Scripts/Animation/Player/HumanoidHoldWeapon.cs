using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player
{
    public class HumanoidHoldWeapon : MonoBehaviour
    {
        [SerializeField] Transform weapon;
        [SerializeField] Transform holdTarget;
        [SerializeField] Animator anim;
        [SerializeField] float positionOffset;
        [SerializeField] Vector3 rotationOffset;

        void Start()
        {
            StartCoroutine(MoveToHand());
        }

        IEnumerator MoveToHand()
        {
            while(true)
            {
                Vector3 offset = Vector3.Normalize(anim.GetBoneTransform(HumanBodyBones.RightLowerArm).position - anim.GetBoneTransform(HumanBodyBones.RightHand).position) * positionOffset;
                weapon.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position + offset;
                weapon.LookAt(weapon.position + offset * 100);

                yield return null;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.RightHand, holdTarget.rotation);
        }
    }
}