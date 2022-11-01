using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Ragdoll
{
    public class RotateTowardsInputWithCamera : MonoBehaviour
    {
        [SerializeField] Rigidbody body;
        [SerializeField] float torqueMagnitude = 500;

        GameObject camera;
        Transform cameraTarget;
        bool moving = false;

        InputAction moveAction;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerMove", Moved);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerStop", Stopped);
            StartCoroutine(RotateToMovement());
            cameraTarget = GetComponentInChildren<CameraTarget>().transform;
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", Moved);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", Stopped);
        }

        private void Moved(object sender, object data)
        {
            moveAction = (InputAction)data;
            moving = true;
        }

        private void Stopped(object sender, object data)
        {
            moving = false;
        }

        private IEnumerator RotateToMovement()
        {
            while (true)
            {
                if (moving && camera != null)
                {
                    /*Vector3 target = moveAction.ReadValue<Vector2>() * 500;
                    float angle = Mathf.Atan2(target.z, target.x) * Mathf.Rad2Deg;
                    Quaternion final = Quaternion.AngleAxis(-angle + 90, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, final, torqueMagnitude * Time.deltaTime);*/
                }
                body.AddTorque(new Vector3(0, -100000, 0));
                //Debug.Log("ASDFSADFDSA");
                //body.AddForce(new Vector3(0, 50000, 0));
                yield return null;
            }
        }

        public void SetCamera(GameObject inCamera)
        {
            camera = inCamera;
        }
    }
}
