using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.PlayerMovement
{
    public class MoveWithCamera : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Camera currentCamera;
        
        private Rigidbody body;
        private InputAction moveAction;
        private bool moving = false;

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

        private void StartMove(object sender, object data)
        {
            moveAction = (InputAction)data;
            moving = true;
        }

        private void StopMove(object sender, object data)
        {
            moving = false;
            body.velocity = new Vector3(0, body.velocity.y, 0);
        }

        private IEnumerator Move()
        {
            while(true)
            {
                while (moving)
                {
                    Vector2 moveVector = moveAction.ReadValue<Vector2>() * speed;
                    Vector3 cameraVector = Vector3.Normalize(new Vector3(currentCamera.transform.forward.x, 0, currentCamera.transform.forward.z));
                    float yVel = body.velocity.y;
                    body.velocity = moveVector.y * cameraVector;
                    //body.AddForce(moveVector.y * cameraVector, ForceMode.Impulse);
                    cameraVector = Vector3.Normalize(new Vector3(currentCamera.transform.right.x, 0, currentCamera.transform.right.z));
                    body.velocity += moveVector.x * cameraVector;
                    body.velocity += new Vector3(0, yVel, 0);
                    //body.AddForce(moveVector.x * currentCamera.transform.right, ForceMode.Impulse);
                    yield return null;
                }
                yield return null;
            }
        }
    }
}