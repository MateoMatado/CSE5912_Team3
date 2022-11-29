using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // The code relating to balancing was modified from https://github.com/sergioabreu-g/active-ragdolls
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


        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("ReceiveCamera", RecCam);
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
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ReceiveCamera", RecCam);
        }

        void FixedUpdate()
        {
            UpdateCamera();
            for (int i = 0; i < physJoints.Length; i++)
            {
                Team3.ConfigurableJointExtensions.SetTargetRotationLocal(physJoints[i], animTransforms[i].localRotation, initRotations[i]);
            }



            /*         BALANCING         */
            Vector3 bodyUpVector = -verticalGoal.transform.right; // Frustratingly, this isn't always body.transform.up
            var balancePercent = Vector3.Angle(bodyUpVector, Vector3.up) / 180;
            balancePercent = uprightTorqueFunction.Evaluate(balancePercent);
            var rot = Quaternion.FromToRotation(bodyUpVector, Vector3.up).normalized;

            verticalGoal.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightTorque * balancePercent);

            Debug.DrawLine(verticalGoal.transform.position, verticalGoal.transform.position+10 * bodyUpVector, Color.yellow);
            Debug.DrawLine(verticalGoal.transform.position, verticalGoal.transform.position+10 * Vector3.up, Color.blue);
            Debug.DrawLine(verticalGoal.transform.position, verticalGoal.transform.position + new Vector3(rot.x, rot.y, rot.z) * balancePercent * 10, Color.red);

            /*foreach (ConfigurableJoint j in physJoints)
            {
                Rigidbody part = j.GetComponent<Rigidbody>();
                part?.AddTorque(new Vector3(rot.x, rot.y, rot.z) * uprightTorque * balancePercent);
                //part?.AddForce(Vector3.up * 10);
            }*/
            Debug.Log("bPercent:" + balancePercent);
            /*var directionAnglePercent = Vector3.SignedAngle(verticalGoal.transform.forward,
                                TargetDirection, Vector3.up) / 180;
            verticalGoal.AddRelativeTorque(0, directionAnglePercent * rotationTorque, 0);*/
        }

        void RecCam(object sender, object data)
        {
            cameraFollow = (Transform)data;
        }

        private void UpdateCamera()
        {
            Vector3 sum = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                sum += physJoints[i].transform.position;
            }
            sum /= 3;
            cameraFollow.position = sum;
        }
    }
}