using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Ragdoll
{
    public class ActiveRagdoll : MonoBehaviour
    {
        [Header("ROOTS")]
        [SerializeField] GameObject animRoot;
        [SerializeField] GameObject physRoot;

        [Header("JOINT DRIVE")]
        [SerializeField] float maximumForce = 800;
        [SerializeField] float positionSpring = 1000;
        [SerializeField] float positionDamper = 50;

        [Header("BALANCING")]
        [Tooltip("Which body part should be upright")]
        [SerializeField] private Rigidbody verticalGoal;
        public float customTorsoAngularDrag = 0.05f;
        public float uprightTorque = 10000;
        [Tooltip("Defines how much torque percent is applied given the inclination angle percent [0, 1]")]
        public AnimationCurve uprightTorqueFunction;
        public float rotationTorque = 500;

        JointDrive defaultDrive;

        private Transform[] animTransforms;
        private ConfigurableJoint[] physJoints;
        private Quaternion[] initRotations;

        private Transform cameraFollow;
        private Camera cam;

        private InputAction moveAction;

        private ActiveRagdollToggle toggle;


        void Start()
        {
            SubToEvents();
            Events.EventsPublisher.Instance.PublishEvent("GrabCamera", null, cameraFollow);

            defaultDrive = new JointDrive();
            defaultDrive.maximumForce = maximumForce;
            defaultDrive.positionDamper = positionDamper;
            defaultDrive.positionSpring = positionSpring;

            physJoints = physRoot.GetComponentsInChildren<ConfigurableJoint>();
            TransformStore[] tStores = animRoot.GetComponentsInChildren<TransformStore>();
            animTransforms = new Transform[tStores.Length];
            for (int i = 0; i < tStores.Length; i++)
            {
                animTransforms[i] = tStores[i].transform;
            }
            foreach(ConfigurableJoint j in physJoints)
            {
                j.angularYZDrive = defaultDrive;
                j.angularXDrive = defaultDrive;
            }

            initRotations = new Quaternion[physJoints.Length];
            for (int i = 0; i < physJoints.Length; i++)
            {
                initRotations[i] = physJoints[i].transform.localRotation;
            }

            toggle = new ActiveRagdollToggle(physJoints);
        }

        private void OnDestroy()
        {
            UnsubToEvents();
        }

        void FixedUpdate()
        {
            UpdateCamera();

            for (int i = 0; i < physJoints.Length; i++)
            {
                Team3.ConfigurableJointExtensions.SetTargetRotationLocal(physJoints[i], animTransforms[i].localRotation, initRotations[i]);
            }




            if (moveAction != null)
            {
                Debug.DrawLine(verticalGoal.transform.position, verticalGoal.transform.position + 10 * ConvertToWorldInput(moveAction.ReadValue<Vector2>()), Color.blue);
            }

            Vector3 TargetDirection = (moveAction != null) ? -ConvertToWorldInput(moveAction.ReadValue<Vector2>()) : new Vector3();
            //Debug.DrawLine(verticalGoal.transform.position, verticalGoal.transform.position + 10 * Vector3.Normalize(TargetDirection), Color.cyan);

            /*         BALANCING         */
            Vector3 bodyUpVector = -verticalGoal.transform.right; // Frustratingly, this isn't always body.transform.up
            var balancePercent = Vector3.Angle(bodyUpVector, Vector3.up) / 180;
            balancePercent = uprightTorqueFunction.Evaluate(balancePercent);
            var rot = Quaternion.FromToRotation(bodyUpVector, Vector3.up).normalized;

            verticalGoal.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightTorque * balancePercent);

            Vector3 bodyForward = verticalGoal.transform.up;
            bodyForward = new Vector3(bodyForward.x, 0, bodyForward.z);
            DrawDebugLine(verticalGoal.position, bodyForward, Color.cyan);
            Quaternion moveTorque = Quaternion.FromToRotation(bodyForward, TargetDirection);
            Vector3 rotTorque = moveTorque.eulerAngles;
            if (rotTorque.magnitude <= 180)
            {
                rotTorque *= -1;
            }

            DrawDebugLine(verticalGoal.position, rotTorque, Color.red);
            verticalGoal.AddTorque(rotTorque * rotationTorque);
            Debug.Log("Torque Magnitude: " + rotTorque.magnitude);
        }

        private void DrawDebugLine(Vector3 startPos, Vector3 vec, Color col)
        {
            Debug.DrawLine(startPos, startPos + 10 * Vector3.Normalize(vec), col);
        }

        private void UpdateCamera()
        {
            int numCameraPoints = 12;
            Vector3 sum = new Vector3();
            for (int i = 0; i < numCameraPoints; i++)
            {
                sum += physJoints[i].transform.position;
            }
            sum /= numCameraPoints;
            cameraFollow.position = sum;
        }

        private Vector3 ConvertToWorldInput(Vector2 inVec)
        {
            Vector3 cameraForward = cam.transform.forward;
            Vector3 cameraRight = cam.transform.right;
            cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z);
            cameraRight = new Vector3(cameraRight.x, 0, cameraRight.z);

            return Vector3.Normalize((cameraForward * inVec.y) + (cameraRight * inVec.x));
        }

        #region EVENTS

        void SubToEvents()
        {

            Events.EventsPublisher.Instance.SubscribeToEvent("ReceiveCameraTransform", RecCam);
            Events.EventsPublisher.Instance.SubscribeToEvent("ReceiveCamera", RecCamObj);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerMove", GetInput);
        }

        void UnsubToEvents()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", GetInput);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReceiveCameraTransform", RecCam);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReceiveCamera", RecCamObj);
        }

        void GetInput(object sender, object data)
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", GetInput);
            moveAction = (InputAction)data;
        }

        void RecCam(object sender, object data)
        {
            cameraFollow = (Transform)data;
        }

        void RecCamObj(object sender, object data)
        {
            cam = (Camera)data;
        }

        #endregion

        IEnumerator CheckFreefall()
        {
            while (true)
            {
                yield return null;
            }
        }

        IEnumerator CheckGround()
        {
            while (true)
            {
                while (toggle.rag)
                {
                    yield return null;
                }
                while (!toggle.rag)
                {
                    yield return null;
                }
            }
        }
    }
}