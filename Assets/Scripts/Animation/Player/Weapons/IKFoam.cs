using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player.Weapons
{
    public class IKFoam : IKWeapon
    {
        float positionOffset = -0.15f;

        float lMaxHAngle = 45;
        float lMinHAngle = -135;
        float lMaxVAngle = 25;
        float lMinVAngle = -45;
        float rMaxHAngle = 135;
        float rMinHAngle = -45;
        float rMaxVAngle = 25;
        float rMinVAngle = -45;
        float length = 1;

        private Vector2 lHalfWH, lDefaultPos;
        private Vector2 rHalfWH, rDefaultPos;

        public IKFoam(GameObject sword)
        {
            Weapon = GameObject.Instantiate(sword);

            lHalfWH = new Vector2(lMaxHAngle - lMinHAngle, lMaxVAngle - lMinVAngle) / 2;
            lDefaultPos = new Vector2(lMaxHAngle + lMinHAngle, lMaxVAngle + lMinVAngle) / 2;
            rHalfWH = new Vector2(rMaxHAngle - rMinHAngle, rMaxVAngle - rMinVAngle) / 2;
            rDefaultPos = new Vector2(rMaxHAngle + rMinHAngle, rMaxVAngle + rMinVAngle) / 2;
        }

        protected override void RightBehavior(Animator anim, Vector2 inVector)
        {
            Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position;

            Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
            Vector3 result = anim.transform.forward * length;
            result = Quaternion.AngleAxis(-armAngles.y, anim.transform.right) * result;
            result = Quaternion.AngleAxis(armAngles.x, anim.transform.up) * result;
            result += shoulder;

            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, result);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.RightHand, Weapon.transform.rotation);
        }

        protected override void LeftBehavior(Animator anim, Vector2 inVector)
        {
            Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position;

            Vector2 armAngles = inVector * lHalfWH + lDefaultPos;
            Vector3 result = anim.transform.forward * length;
            result = Quaternion.AngleAxis(-armAngles.y, anim.transform.right) * result;
            result = Quaternion.AngleAxis(armAngles.x, anim.transform.up) * result;
            result += shoulder;

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, result);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, Weapon.transform.rotation);
        }

        public override void MoveWeapon(Animator anim, AvatarIKGoal hand)
        {
            if (hand == AvatarIKGoal.RightHand)
            {
                Weapon.transform.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position;
                Vector3 offset = Vector3.Normalize(anim.GetBoneTransform(HumanBodyBones.RightShoulder).position - anim.GetBoneTransform(HumanBodyBones.RightHand).position) * positionOffset;
                Weapon.transform.LookAt(Weapon.transform.position + offset * 100);
            }
            else
            {
                Weapon.transform.position = anim.GetBoneTransform(HumanBodyBones.LeftHand).position;
                Vector3 offset = Vector3.Normalize(anim.GetBoneTransform(HumanBodyBones.LeftShoulder).position - anim.GetBoneTransform(HumanBodyBones.LeftHand).position) * positionOffset;
                Weapon.transform.LookAt(Weapon.transform.position + offset * 100);
            }
        }
    }
}