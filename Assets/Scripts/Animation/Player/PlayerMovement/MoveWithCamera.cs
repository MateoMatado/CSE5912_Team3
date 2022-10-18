using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.PlayerMovement
{
    public class MoveWithCamera : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform cameraTarget;
        
        private Rigidbody body;
        private InputAction moveAction;
        private bool moving = false;
        private bool flying = false;

        // Start is called before the first frame update
        void Start()
        {
            body = GetComponent<Rigidbody>();
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerMove", StartMove);
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerStop", StopMove);
            StartCoroutine(Move());
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerMove", StartMove);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PlayerStop", StopMove);
        }

        public void StartFlying()
        {
            transform.Find("SmokeTrail").gameObject.SetActive(true);
            flying = true;
        }

        private void StartMove(object sender, object data)
        {
            moveAction = (InputAction)data;
            moving = true;
        }

        private void StopMove(object sender, object data)
        {
            moving = false;
            if (!flying)
                body.velocity = new Vector3(0, body.velocity.y, 0);
        }

        private IEnumerator Move()
        {
            while(true)
            {
                while (moving && !flying)
                {
                    Vector2 moveVector = moveAction.ReadValue<Vector2>() * speed * PlayerStatus.Instance.GetValue("Speed");
                    Vector3 cameraVector = Vector3.Normalize(new Vector3(cameraTarget.forward.x, 0, cameraTarget.forward.z));
                    float yVel = body.velocity.y;
                    body.velocity = moveVector.y * cameraVector;
                    //body.AddForce(moveVector.y * cameraVector, ForceMode.Impulse);
                    cameraVector = Vector3.Normalize(new Vector3(cameraTarget.right.x, 0, cameraTarget.right.z));
                    body.velocity += moveVector.x * cameraVector;
                    body.velocity += new Vector3(0, yVel, 0);
                    //body.AddForce(moveVector.x * currentCamera.transform.right, ForceMode.Impulse);
                    yield return null;
                }
                yield return null;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (flying)
            {
                flying = false;
                transform.Find("SmokeTrail").gameObject.SetActive(false);
                Events.EventsPublisher.Instance.PublishEvent("StopFlying", null, null);
            }
        }
    }
}