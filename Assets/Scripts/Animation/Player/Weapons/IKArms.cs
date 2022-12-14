using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Animation.Player
{
    public class IKArms : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] GameObject sword;
        [SerializeField] GameObject foamFinger;
        [SerializeField] GameObject hammer;
        [SerializeField] GameObject confettiGun;
        [SerializeField] float swingForce = 70;
        [SerializeField] float swingThreshold = 0.2f;
        [SerializeField] float mouseSensitivity = 0.01f;
        [SerializeField] float swingDelay = 0.2f;
        [SerializeField] Rigidbody body;
        [SerializeField] Transform leftHandObject;
        [SerializeField] Transform rightHandObject;
        [SerializeField] Animator physAnim;

        Vector2 inVector = new Vector2();
        Vector2 dPos = new Vector2();

        Weapons.IKWeapon left;
        Weapons.IKWeapon right;

        private InputAction leftAction;
        private InputAction rightAction;
        private InputAction stickAction;
        private InputAction mouseAction;

        #region START_STOP
        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", StartRight);
            Events.EventsPublisher.Instance.SubscribeToEvent("MoveArmMouse", StartMouse);
            Events.EventsPublisher.Instance.SubscribeToEvent("EquipWeapon", EquipWeapon);
            Events.EventsPublisher.Instance.SubscribeToEvent("SwapHands", SwapHands);

            StartCoroutine(PushOnSwing());

            //right = new Weapons.IKSword(sword);
            right = new Weapons.IKFoam(foamFinger);
            left = null;

            right?.SetParent(this.transform);
            left?.SetParent(this.transform);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArmMouse", StartMouse);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("EquipWeapon", EquipWeapon);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("SwapHands", SwapHands);
            //Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArm", StartStick);
        }
        #endregion START_STOP
        #region EVENT_HANDLING
        private void EquipWeapon(object sender, object data)
        {
            if ((Weapons.IKWeapon)data is Weapons.IKHammer)
            {
                right?.DisableWeapon();
                left?.DisableWeapon();
                right = (Weapons.IKHammer)data;
                left = right;
                right?.EnableWeapon();
                right.SetParent(transform);
            }
            else if ((Weapons.IKWeapon)data is Weapons.IKConfettiGun gun)
            {
                right?.DisableWeapon();
                left?.DisableWeapon();
                right = gun;
                left = right;
                right?.EnableWeapon();
                right.SetParent(transform);
            }
            else if ((AvatarIKGoal)sender == AvatarIKGoal.RightHand)
            {
                if (left == right)
                {
                    left?.DisableWeapon();
                    left = null;
                }
                right?.DisableWeapon();
                right = (Weapons.IKWeapon)data;
                right?.EnableWeapon();
                right.SetParent(transform);
            }
            else
            {
                if (left == right)
                {
                    right?.DisableWeapon();
                    right = null;
                }
                left?.DisableWeapon();
                left = (Weapons.IKWeapon)data;
                left?.EnableWeapon();
                left.SetParent(transform);
            }
        }

        private void SwapHands(object sender, object data)
        {
            Weapons.IKWeapon temp = right;
            right = left;
            left = temp;
        }

        private void StartLeft(object sender, object data)
        {
            leftAction = ((((InputAction, InputAction, InputAction))data).Item1);
            stickAction = ((((InputAction, InputAction, InputAction))data).Item2);
            mouseAction = ((((InputAction, InputAction, InputAction))data).Item3);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
        }

        private void StartRight(object sender, object data)
        {
            rightAction = ((((InputAction, InputAction, InputAction))data).Item1);
            stickAction = ((((InputAction, InputAction, InputAction))data).Item2);
            mouseAction = ((((InputAction, InputAction, InputAction))data).Item3);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
        }

        private void StartMouse(object sender, object data)
        {
            mouseAction = (InputAction)data;
            Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArmMouse", StartMouse);
        }

        /*private void StartStick(object sender, object data)
        {
            stickAction = (InputAction)data;
            Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArm", StartStick);
        }*/
        #endregion EVENT_HANDLING

        private void UpdateInvector()
        {
            Vector2 oldVector = inVector;

            if (stickAction.inProgress)
            {
                inVector = stickAction.ReadValue<Vector2>();
            }
            else
            {
                inVector += mouseAction.ReadValue<Vector2>() * mouseSensitivity;
                if (inVector.magnitude > 1)
                {
                    inVector = Vector2.ClampMagnitude(inVector, 1);
                }
            }
            dPos = oldVector - inVector;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            bool either = false;
            if (rightAction != null && rightAction.IsPressed() || (leftAction != null && leftAction.IsPressed() && left == right))
            {
                UpdateInvector(); // Must be inside of to verify that input exists
                right?.UpdateArm(anim, inVector, AvatarIKGoal.RightHand);
                either = true;
                anim.SetLookAtWeight(1);
                anim.SetLookAtPosition(anim.GetIKPosition(AvatarIKGoal.RightHand));
            }
            if ((leftAction != null && leftAction.IsPressed()) || (rightAction != null && rightAction.IsPressed() && left == right))
            {
                UpdateInvector();
                left?.UpdateArm(anim, inVector, AvatarIKGoal.LeftHand);
                if (!either)
                {
                    anim.SetLookAtWeight(1);
                    anim.SetLookAtPosition(anim.GetIKPosition(AvatarIKGoal.LeftHand));
                }
                either = true;
            }
            if (!either)
            {
                dPos = new Vector2(0, 0);
                anim.SetLookAtWeight(0);
            }
        }

        private IEnumerator PushOnSwing()
        {
            while (true)
            {
                while (dPos.magnitude >= swingThreshold && left != null && right != null && !(rightAction != null && rightAction.IsPressed() && right is Weapons.IKFoam) && !(leftAction != null && leftAction.IsPressed() && left is Weapons.IKFoam))
                {
                    Vector3 force = anim.GetBoneTransform(HumanBodyBones.RightHand).transform.position - anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position;
                    //body.AddForce(new Vector3(force.x, 0, force.z) * swingForce, ForceMode.Impulse);
                    body.velocity = new Vector3();
                    body.AddForce(body.transform.forward * swingForce, ForceMode.Impulse);
                    yield return new WaitForSeconds(swingDelay);
                }
                yield return null;
            }
        }

        private void LateUpdate()
        {
            right?.MoveWeapon(physAnim, AvatarIKGoal.RightHand);
            left?.MoveWeapon(physAnim, AvatarIKGoal.LeftHand);
            if (right != null)
            {
                right.Weapon.transform.parent = rightHandObject.transform;
            }
            if (left != null)
            {
                left.Weapon.transform.parent = leftHandObject.transform;
            }
        }
    }
}
