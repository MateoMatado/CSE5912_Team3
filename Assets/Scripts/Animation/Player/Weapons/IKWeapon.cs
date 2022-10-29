using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Animation.Player.Weapons
{
    public abstract class IKWeapon
    {
        public GameObject Weapon { get; set; }

        public void UpdateArm(Animator anim, Vector2 inVector, AvatarIKGoal hand)
        {
            if (hand == AvatarIKGoal.RightHand)
            {
                RightBehavior(anim, inVector);
            }
            else
            {
                LeftBehavior(anim, inVector);
            }
        }

        protected abstract void LeftBehavior(Animator anim, Vector2 inVector);

        protected abstract void RightBehavior(Animator anim, Vector2 inVector);

        public abstract void MoveWeapon(Animator anim, AvatarIKGoal hand);

        public void SetParent(Transform t)
        {
            if (Weapon != null)
            {
                Weapon.transform.parent = t;
            }
        }

        public void DisableWeapon()
        {
            if (Weapon != null)
            {
                Weapon.SetActive(false);
            }
        }

        public void EnableWeapon()
        {
            if (Weapon != null)
            {
                Weapon.SetActive(true);
            }
        }
    }
}