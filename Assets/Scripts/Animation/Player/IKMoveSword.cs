using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Animation.Player
{
    public class IKMoveSword : MonoBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] Transform weapon;
        [SerializeField] Transform IKTarget;
        [SerializeField] Transform hand;
        [SerializeField] float positionOffset;

        [SerializeField] float mouseSensitivity;
        [SerializeField] float swingThreshold = 0.2f;
        [SerializeField] float swingDelay = 0.2f;
        [SerializeField] float swingForce = 80;
        Rigidbody body;

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
        Vector2 inVector = new Vector2();
        Vector2 dPos = new Vector2();

        private InputAction leftAction;
        private InputAction rightAction;
        private InputAction stickAction;
        private InputAction mouseAction;

        private float rot = 0;

        #region START_STOP
        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", StartRight);
            Events.EventsPublisher.Instance.SubscribeToEvent("MoveArmMouse", StartMouse);

            lHalfWH = new Vector2(lMaxHAngle - lMinHAngle, lMaxVAngle - lMinVAngle) / 2;
            lDefaultPos = new Vector2(lMaxHAngle + lMinHAngle, lMaxVAngle + lMinVAngle) / 2;
            rHalfWH = new Vector2(rMaxHAngle - rMinHAngle, rMaxVAngle - rMinVAngle) / 2;
            rDefaultPos = new Vector2(rMaxHAngle + rMinHAngle, rMaxVAngle + rMinVAngle) / 2;

            body = transform.parent.parent.gameObject.GetComponent<Rigidbody>();

            StartCoroutine(MoveSword());
            StartCoroutine(PushOnSwing());
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", StartLeft);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", StartRight);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArmMouse", StartMouse);
            //Events.EventsPublisher.Instance.UnsubscribeToEvent("MoveArm", StartStick);
        }
#endregion START_STOP
        #region EVENT_HANDLING
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

        private IEnumerator MoveSword()
        {
            while (true)
            {
                if (rightAction != null && rightAction.IsPressed())
                {
                    UpdateInvector();
                    Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position;

                    Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
                    Vector3 result = transform.forward * length;
                    result = Quaternion.AngleAxis(-armAngles.y, transform.right) * result;
                    result = Quaternion.AngleAxis(armAngles.x, transform.up) * result;
                    result += shoulder;

                    weapon.position = result;
                    Vector3 offset = Vector3.Normalize(anim.GetBoneTransform(HumanBodyBones.RightShoulder).position - anim.GetBoneTransform(HumanBodyBones.RightHand).position) * positionOffset;
                    weapon.LookAt(weapon.position + offset * 100);
                }
                else
                {
                    Vector3 offset = Vector3.Normalize(anim.GetBoneTransform(HumanBodyBones.RightShoulder).position - anim.GetBoneTransform(HumanBodyBones.RightHand).position) * positionOffset;
                    weapon.position = anim.GetBoneTransform(HumanBodyBones.RightHand).position + offset;
                    weapon.LookAt(weapon.position + offset * 100);
                }
                // Looks fun but screws with ragdoll too much
                //rot = (rot + 10) % 360;
                yield return null;
            }
        }

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
            /*if (leftAction != null && leftAction.IsPressed())
            {
                UpdateInvector();
                Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder).transform.position;

                Vector2 armAngles = inVector * lHalfWH + lDefaultPos;
                Vector3 result = transform.forward * length;
                result = Quaternion.AngleAxis(-armAngles.y, transform.right) * result;
                result = Quaternion.AngleAxis(armAngles.x, transform.up) * result;
                result += shoulder;

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, result);
            }*/

            /*if (rightAction != null && rightAction.IsPressed())
            {
                UpdateInvector();
                Vector3 shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position;

                Vector2 armAngles = inVector * rHalfWH + rDefaultPos;
                Vector3 result = transform.forward * length;
                result = Quaternion.AngleAxis(-armAngles.y, transform.right) * result;
                result = Quaternion.AngleAxis(armAngles.x, transform.up) * result;
                result += shoulder;

                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, result);
            }*/
            if (rightAction != null && rightAction.IsPressed())
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, IKTarget.position);
                anim.SetLookAtPosition(IKTarget.position);
                anim.SetLookAtWeight(1);
            }
            else
            {
                dPos = new Vector2(0, 0);
            }

            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKRotation(AvatarIKGoal.RightHand, IKTarget.rotation);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Euler(rot, rot, rot));
        }

        private IEnumerator PushOnSwing()
        {
            while(true)
            {
                while (dPos.magnitude >= swingThreshold)
                {
                    Vector3 force = anim.GetBoneTransform(HumanBodyBones.RightHand).transform.position - anim.GetBoneTransform(HumanBodyBones.RightShoulder).transform.position;
                    //body.AddForce(new Vector3(force.x, 0, force.z) * swingForce, ForceMode.Impulse);
                    body.velocity = new Vector3();
                    body.AddForce(body.transform.forward * swingForce, ForceMode.Impulse);
                    yield return new WaitForSeconds(0.2f);
                }
                while (dPos.magnitude < swingThreshold) { yield return null; }
            }
        }
    }
}
