using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Input
{
    public class GameInputManager : MonoBehaviour
    {
        private GameInput inputs;

        void Awake()
        {
            inputs = new GameInput();
            inputs.Player.Move.started += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerMove", null, inputs.Player.Move); };
            inputs.Player.Move.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerStop", null, inputs.Player.Move); };
            inputs.Player.Jump.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerJump", null, inputs.Player.Jump); };
            inputs.Player.Pause.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("PauseUnpause", null, inputs.Player.Pause); };
            inputs.Player.ReloadScene.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LoadRunning", null, inputs.Player.Pause); };
            inputs.Player.LeftArmActivate.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LeftArmActivate", null, inputs.Player.LeftArmActivate); };
            inputs.Player.LeftArmActivate.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("LeftArmDeactivate", null, inputs.Player.LeftArmActivate); };
            inputs.Player.RightArmActivate.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("RightArmActivate", null, inputs.Player.RightArmActivate); };
            inputs.Player.RightArmActivate.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("RightArmDeactivate", null, inputs.Player.RightArmActivate); };

            //for temporary camera switching, by Jimmy 
            inputs.Player.CameraSwitch.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("CameraSwitch", null, inputs.Player.CameraSwitch); };
        }

        private void OnEnable()
        {
            inputs.Player.Move.Enable();
            inputs.Player.Jump.Enable();
            inputs.Player.Pause.Enable();
            inputs.Player.ReloadScene.Enable();
            inputs.Player.LeftArmActivate.Enable();
            inputs.Player.RightArmActivate.Enable();

            //for temporary camera switching, by Jimmy 
            inputs.Player.CameraSwitch.Enable();
        }

        private void OnDisable()
        {
            inputs.Player.Move.Disable();
            inputs.Player.Jump.Disable();
            inputs.Player.Pause.Disable();
            inputs.Player.ReloadScene.Disable();
            inputs.Player.LeftArmActivate.Disable();
            inputs.Player.RightArmActivate.Disable();

            //for temporary camera switching, by Jimmy 
            inputs.Player.CameraSwitch.Disable();
        }
    }
}