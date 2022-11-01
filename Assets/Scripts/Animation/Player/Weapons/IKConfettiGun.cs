using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player.Weapons
{
    public class IKConfettiGun : IKWeapon
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

        private float shootInterval = 3f;
        private float shootTime = 0f;

        public IKConfettiGun(GameObject confettiGun)
        {
            Weapon = GameObject.Instantiate(confettiGun);

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

            Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
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
            Vector3 adjust = Weapon.transform.Find("RightHandPosition").position - Weapon.transform.position;
            Weapon.transform.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position - adjust;
            Transform rFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
            Transform rHand = anim.GetBoneTransform(HumanBodyBones.RightHand);
            Transform rShoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
            if (Input.GameInputManager.InIK)
            {
                Weapon.transform.LookAt(rHand.position + (anim.transform.forward) * 100);
                shootTime -= Time.deltaTime;
                if (shootTime <= 0)
                {
                    Shoot();
                    shootTime = shootInterval;
                }
            }
            else
            {
                Weapon.transform.LookAt(rHand.position + (rFoot.position - rHand.position) * 100);
            }
        }

        public void Shoot()
        {
            Weapon.GetComponentInChildren<ParticleSystem>().Play();
            Vector3 firePoint = Weapon.transform.Find("FirePoint").position;
            float radius = 5;
            RaycastHit[] hits = Physics.SphereCastAll(firePoint, radius, Weapon.transform.forward, radius * 2, 6);
            firePoint.y -= radius;
            float force = 500;
            foreach (RaycastHit hit in hits)
            {
                Vector3 direction = (hit.collider.transform.position - firePoint).normalized;
                hit.collider.gameObject.GetComponentInChildren<Rigidbody>().AddForce(direction * force);
            }
        }

    }
}
