using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Input
{
    public enum InputType
    {
        MouseKeyboard, Gamepad
    }

    public class GameInputManager : MonoBehaviour
    {
        private GameInput inputs;

        public static InputType currentDevice = InputType.MouseKeyboard;

        void Awake()
        {
            inputs = new GameInput();
            inputs.Player.Move.started += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerMove", null, inputs.Player.Move); };
            inputs.Player.Move.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerStop", null, inputs.Player.Move); };
            inputs.Player.Jump.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerJump", null, inputs.Player.Jump); };
            inputs.Player.Pause.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("PauseUnpause", null, inputs.Player.Pause); };
            inputs.Player.ReloadScene.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LoadRunning", null, inputs.Player.Pause); };
            inputs.Player.LeftArmActivate.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LeftArmActivate", null, (inputs.Player.LeftArmActivate, inputs.Player.MoveArm, inputs.Player.MoveArmMouse)); };
            inputs.Player.LeftArmActivate.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("LeftArmDeactivate", null, inputs.Player.LeftArmActivate); };
            inputs.Player.RightArmActivate.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("RightArmActivate", null, (inputs.Player.RightArmActivate, inputs.Player.MoveArm, inputs.Player.MoveArmMouse)); };
            inputs.Player.RightArmActivate.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("RightArmDeactivate", null, inputs.Player.RightArmActivate); };
            //inputs.Player.MoveArm.started += (context) => { Events.EventsPublisher.Instance.PublishEvent("MoveArm", null, inputs.Player.MoveArm); };
            inputs.Player.MoveArmMouse.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("MoveArmMouse", null, inputs.Player.MoveArmMouse); };
            inputs.Player.LookMouse.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LookMouse", null, (inputs.Player.LookMouse, inputs.Player.LookPad)); currentDevice = InputType.MouseKeyboard; };
            inputs.Player.LookPad.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LookPad", null, (inputs.Player.LookMouse, inputs.Player.LookPad)); currentDevice = InputType.Gamepad; };
            inputs.Player.Target.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("Target", null, inputs.Player.Target); };
            inputs.Player.ChangeBanana.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("ChangePrefab", null, "banana"); };
            inputs.Player.ChangeBaby.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("ChangePrefab", null, "baby"); };

            //for temporary camera switching, by Jimmy 
            inputs.Player.CameraSwitch.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("CameraSwitch", null, inputs.Player.CameraSwitch); };

            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmActivate", EnableArm);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmActivate", EnableArm);
            Events.EventsPublisher.Instance.SubscribeToEvent("LeftArmDeactivate", DisableArm);
            Events.EventsPublisher.Instance.SubscribeToEvent("RightArmDeactivate", DisableArm);
        }

        private void OnEnable()
        {
            inputs.Player.Move.Enable();
            inputs.Player.Jump.Enable();
            inputs.Player.Pause.Enable();
            inputs.Player.ReloadScene.Enable();
            inputs.Player.LeftArmActivate.Enable();
            inputs.Player.RightArmActivate.Enable();
            inputs.Player.ChangeBanana.Enable();
            inputs.Player.ChangeBaby.Enable();

            inputs.Player.LookPad.Enable();
            inputs.Player.LookMouse.Enable();

            inputs.Player.Target.Enable();

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
            inputs.Player.MoveArm.Disable();
            inputs.Player.ChangeBanana.Disable();
            inputs.Player.ChangeBaby.Disable();

            inputs.Player.LookPad.Disable();
            inputs.Player.LookMouse.Disable();

            inputs.Player.Target.Disable();

            //for temporary camera switching, by Jimmy 
            inputs.Player.CameraSwitch.Disable();
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmActivate", EnableArm);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmActivate", EnableArm);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("LeftArmDeactivate", DisableArm);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("RightArmDeactivate", DisableArm);
        }


        int arms = 0;
        private void EnableArm(object sender, object data)
        {
            arms++;
            inputs.Player.MoveArm.Enable();
            inputs.Player.MoveArmMouse.Enable();
            inputs.Player.LookMouse.Disable();
            inputs.Player.LookPad.Disable();
        }

        private void DisableArm(object sender, object data)
        {
            arms--;
            if (arms < 1)
            {
                inputs.Player.MoveArm.Disable();
                inputs.Player.MoveArmMouse.Disable();
                inputs.Player.LookMouse.Enable();
                inputs.Player.LookPad.Enable();
            }
        }
    }
}