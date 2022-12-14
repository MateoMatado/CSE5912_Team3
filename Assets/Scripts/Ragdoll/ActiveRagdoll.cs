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
        public GameObject PlayerObject;
        private Player.PlayerStateManager stateManager;

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

        [Header("ROLLING")]
        [SerializeField] public float rollForce = 100;

        JointDrive defaultDrive;
        JointDrive zeroDrive;

        private Transform[] animTransforms;
        private ConfigurableJoint[] physJoints;
        private Quaternion[] initRotations;

        private Transform cameraFollow;
        private Camera cam;

        private InputAction moveAction;

        public ActiveRagdollToggle toggle;

        private Transform effectTarget;
        private Transform enemyTarget;


        void Start()
        {
            SubToEvents();
            GrabStores();

            defaultDrive = new JointDrive();
            defaultDrive.maximumForce = maximumForce;
            defaultDrive.positionDamper = positionDamper;
            defaultDrive.positionSpring = positionSpring;

            zeroDrive = new JointDrive();
            zeroDrive.maximumForce = 0;
            zeroDrive.positionDamper = 0;
            zeroDrive.positionSpring = 0;

            physJoints = physRoot.GetComponentsInChildren<ConfigurableJoint>();
            List<ConfigurableJoint> tempList = new List<ConfigurableJoint>(physJoints);
            tempList.RemoveAt(0);
            physJoints = tempList.ToArray();
            physRoot.GetComponent<ConfigurableJoint>().xDrive = zeroDrive;
            physRoot.GetComponent<ConfigurableJoint>().yDrive = zeroDrive;
            physRoot.GetComponent<ConfigurableJoint>().zDrive = zeroDrive;

            TransformStore[] tStores = animRoot.GetComponentsInChildren<TransformStore>();
            animTransforms = new Transform[tStores.Length];
            for (int i = 0; i < tStores.Length; i++)
            {
                animTransforms[i] = tStores[i].transform;
            }
            foreach (ConfigurableJoint j in physJoints)
            {
                j.angularYZDrive = defaultDrive;
                j.angularXDrive = defaultDrive;
            }

            initRotations = new Quaternion[physJoints.Length];
            for (int i = 0; i < physJoints.Length; i++)
            {
                initRotations[i] = physJoints[i].transform.localRotation;
            }

            PlayerObject = transform.parent.gameObject;
            stateManager = PlayerObject.GetComponent<Player.PlayerStateManager>();
            transform.parent = transform.parent.parent;

            toggle = new ActiveRagdollToggle(physJoints, physRoot, rollForce);
            toggle.GetStateMachine(stateManager);
            Events.EventsPublisher.Instance.PublishEvent("GrabCamera", null, cameraFollow);
            //StartCoroutine(CheckGround());
            //StartCoroutine(CheckFreefall());
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
            if (!(stateManager.StateMachine.CurrentState == PlayerStateMachine.CannonAimState))
            {
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
            //cameraFollow.position = sum;
            //enemyTarget.position = sum;
            PlayerObject.transform.position = sum;
            if (effectTarget != null) { effectTarget.position = verticalGoal.position; }
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
            Events.EventsPublisher.Instance.SubscribeToEvent("ManualMove", MoveTo);
            Events.EventsPublisher.Instance.SubscribeToEvent("GrabRag", GetGrabbed);
            Events.EventsPublisher.Instance.SubscribeToEvent("ReleaseRag", GetReleased);
        }

        void UnsubToEvents()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", GetInput);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReceiveCameraTransform", RecCam);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReceiveCamera", RecCamObj);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ManualMove", MoveTo);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReleaseRag", GetReleased);
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
            toggle.GetCamera(cam);
        }

        #endregion

        void GrabStores()
        {
            Transform par = this.transform.parent;
            effectTarget = par.GetComponentInChildren<EffectStore>()?.transform;
            enemyTarget = par.GetComponentInChildren<EnemyTargetStore>().transform;
        }

        void MoveTo(object sender, object data)
        {
            Vector3 diff = (Vector3)data - physRoot.transform.position;
            physRoot.transform.position += diff;

            // foreach (ConfigurableJoint j in physJoints)
            // {
            //     j.transform.position += diff;
            // }
        }

        Rigidbody prevHeadLock;
        Vector3 prevHeadAnchor;
        Vector3 prevAnchor;
        void GetGrabbed(object sender, object data)
        {
            (Rigidbody, Rigidbody) rData = ((Rigidbody, Rigidbody))data;
            Rigidbody headLock = rData.Item2;
            Rigidbody pelvisLock = rData.Item1;

            ConfigurableJoint head = physRoot.GetComponentInChildren<HeadStore>().gameObject.GetComponent<ConfigurableJoint>();
            ConfigurableJoint pelvis = physRoot.GetComponent<ConfigurableJoint>();

            prevHeadLock = head.connectedBody;
            pelvis.connectedBody = pelvisLock;
            //head.connectedBody = headLock;
            prevHeadAnchor = head.anchor;
            //head.autoConfigureConnectedAnchor = false;
            //head.connectedAnchor = new Vector3();
            prevAnchor = head.anchor;

            physRoot.GetComponent<ConfigurableJoint>().xDrive = defaultDrive;
            physRoot.GetComponent<ConfigurableJoint>().yDrive = defaultDrive;
            physRoot.GetComponent<ConfigurableJoint>().zDrive = defaultDrive;
        }

        void GetReleased(object sender, object data)
        {
            ConfigurableJoint head = physRoot.GetComponentInChildren<HeadStore>().gameObject.GetComponent<ConfigurableJoint>();
            ConfigurableJoint pelvis = physRoot.GetComponent<ConfigurableJoint>();

            physRoot.GetComponent<ConfigurableJoint>().xDrive = zeroDrive;
            physRoot.GetComponent<ConfigurableJoint>().yDrive = zeroDrive;
            physRoot.GetComponent<ConfigurableJoint>().zDrive = zeroDrive;

            //head.connectedAnchor = prevHeadAnchor;
            //head.connectedBody = prevHeadLock;
            //head.anchor = prevAnchor;
            //head.autoConfigureConnectedAnchor = true;

            pelvis.connectedBody = null;
        }

        public void Dampen()
        {
            Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody r in bodies)
            {
                r.velocity = Vector3.zero;
            }
        }
    }
}