using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player.Weapons
{
    public class IKHammer : IKWeapon
    {
        float positionOffset = -0.15f;        

        float rMaxHAngle = 15;
        float rMinHAngle = -15;
        float rMaxVAngle = 140;
        float rMinVAngle = -15;
        float length = 0.75f;

        private Vector2 lHalfWH, lDefaultPos;
        private Vector2 rHalfWH, rDefaultPos;

        private Vector3 hammerPosition = new Vector3();

        public IKHammer(GameObject hammer)
        {
            Weapon = GameObject.Instantiate(hammer);

            rHalfWH = new Vector2(rMaxHAngle - rMinHAngle, rMaxVAngle - rMinVAngle) / 2;
            rDefaultPos = new Vector2(rMaxHAngle + rMinHAngle, rMaxVAngle + rMinVAngle) / 2;
        }

        protected override void RightBehavior(Animator anim, Vector2 inVector)
        {
            Vector3 shoulderMid = (anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position) / 2;
            shoulderMid = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - shoulderMid;

            Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
            Vector3 result = anim.transform.forward * length;
            result = Quaternion.AngleAxis(-armAngles.y, anim.transform.right) * result;
            result = Quaternion.AngleAxis(armAngles.x, anim.transform.up) * result;
            result += shoulderMid;
            hammerPosition = result;

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, result);
            //anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            //anim.SetIKRotation(AvatarIKGoal.RightHand, Weapon.transform.rotation);
        }

        protected override void LeftBehavior(Animator anim, Vector2 inVector)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, hammerPosition);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, Weapon.transform.rotation);
        }

        public override void MoveWeapon(Animator anim, AvatarIKGoal hand)
        {
            if (hand == AvatarIKGoal.RightHand && Input.GameInputManager.InIK)
            {
                Weapon.transform.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position;

                Vector3 shoulderMid = (anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position) / 2;
                shoulderMid = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - shoulderMid;
                Weapon.transform.LookAt(shoulderMid + 5*(Weapon.transform.position-shoulderMid) * 100);
            }
            else if (hand == AvatarIKGoal.RightHand && !Input.GameInputManager.InIK)
            {
                Weapon.transform.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position;

                Vector3 shoulderMid = (anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position) / 2;
                shoulderMid = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position - shoulderMid;
                Weapon.transform.LookAt(shoulderMid + 5 * (Weapon.transform.position - shoulderMid) * 100);

                Vector3 left = anim.GetBoneTransform(HumanBodyBones.LeftHand).transform.position - anim.GetBoneTransform(HumanBodyBones.LeftLowerArm).transform.position;
                Vector3 right = anim.GetBoneTransform(HumanBodyBones.RightHand).transform.position - anim.GetBoneTransform(HumanBodyBones.RightLowerArm).transform.position;
                Weapon.transform.LookAt(anim.GetBoneTransform(HumanBodyBones.RightHand).transform.position - 100 * Vector3.Cross(left, right));
            }
        }
    }
}