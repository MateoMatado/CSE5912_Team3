using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Input
{
    public class MoveWithInput : MonoBehaviour
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Camera camera;

        private GameInput inputs;
        private InputAction moveAction;
        private Rigidbody rBody;


        private void Awake()
        {
            inputs = new GameInput();
            rBody = GetComponent<Rigidbody>();
            Events.EventsPublisher.Instance.SubscribeToEvent("PlayerDeath", OnDeath);
        }

        private void OnEnable()
        {
            moveAction = inputs.Player.Move;
            moveAction.Enable();

            inputs.Player.Jump.performed += Jump;
            inputs.Player.Jump.Enable();

            inputs.Player.Pause.performed += QuitGame;
            inputs.Player.Pause.Enable();
        }

        private void OnDisable()
        {
            moveAction.Disable();
        }

        private void FixedUpdate()
        {
            Vector2 moveVector = moveAction.ReadValue<Vector2>() * moveSpeed;
            Vector3 cameraVector = Vector3.Normalize(new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z));
            rBody.AddForce(moveVector.y*cameraVector, ForceMode.Impulse);
            cameraVector = Vector3.Normalize(new Vector3(camera.transform.right.x, 0, camera.transform.right.z));
            rBody.AddForce(moveVector.x*camera.transform.right, ForceMode.Impulse);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            rBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void QuitGame(InputAction.CallbackContext context)
        {
            GameStateMachine.Instance.SwitchState(new MainMenuState());
        }

        private void OnDeath(object sender, object data)
        {
            rBody.velocity = new Vector3(0, 0, 0);
        }
    }
}