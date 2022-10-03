using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Team3.Player;
using Cinemachine;

// My changes to this file are kinda ugly code wise but it works for both the mouse and the keyboard and should be fine until I remake the input system
namespace Team3
{
    public class CameraRotationWithMouse : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float padSensitivity;

        private Vector2 look = Vector2.zero;
        private PlayerStateManager stateManager;

        private InputAction mouse, gamepad;

        void Awake()
        {
            stateManager = GetComponent<PlayerStateManager>();
            Team3.Events.EventsPublisher.Instance.SubscribeToEvent("LookMouse", LookStart);
            Team3.Events.EventsPublisher.Instance.SubscribeToEvent("LookPad", LookStart);
            StartCoroutine(Look());
        }

        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void UnsubscribeToEvents()
        {
            Team3.Events.EventsPublisher.Instance.UnsubscribeToEvent("LookMouse", LookStart);
            Team3.Events.EventsPublisher.Instance.UnsubscribeToEvent("LookPad", LookStart);
        }

        private void LookStart(object sender, object data)
        {
            mouse = (((InputAction, InputAction))data).Item1;
            gamepad = (((InputAction, InputAction))data).Item2;
            UnsubscribeToEvents();
        }

        private IEnumerator Look()
        {
            while (true)
            {
                while (mouse != null &&  Input.GameInputManager.currentDevice == Input.InputType.MouseKeyboard)
                {
                    if (stateManager.StateMachine.CurrentState is TargetingState) break;
                    if (stateManager.StateMachine.CurrentState is IKState) break;
                    if (GameStateMachine.Instance.CurrentState is PauseState) break;
                    look = mouse.ReadValue<Vector2>() * mouseSensitivity;
                    MoveCamera(look);                    
                    yield return null;
                }
                while (gamepad != null && Input.GameInputManager.currentDevice == Input.InputType.Gamepad)
                {
                    if (stateManager.StateMachine.CurrentState is TargetingState) break;
                    if (stateManager.StateMachine.CurrentState is IKState) break;
                    if (GameStateMachine.Instance.CurrentState is PauseState) break;
                    look = gamepad.ReadValue<Vector2>() * padSensitivity;
                    MoveCamera(look);
                    yield return null;
                }

                yield return null;
            }
        }

        private void MoveCamera(Vector2 look)
        {
            cameraTarget.transform.rotation *= Quaternion.AngleAxis(look.x, Vector3.up);
            cameraTarget.transform.rotation *= Quaternion.AngleAxis(-look.y, Vector3.right);

            var angles = cameraTarget.transform.localEulerAngles;
            angles.z = 0;
            var angle = angles.x;
            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            // transform.rotation = Quaternion.Euler(0, cameraTarget.transform.rotation.eulerAngles.y, 0);
            cameraTarget.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);
        }
    }
}