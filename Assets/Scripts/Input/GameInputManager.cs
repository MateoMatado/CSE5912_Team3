using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Team3.Input
{
    public class GameInputManager : MonoBehaviour
    {
        private GameInput inputs;
        private InputAction moveAction;

        // Start is called before the first frame update
        void Awake()
        {
            inputs = new GameInput();
            inputs.Player.Move.started += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerMove", null, inputs.Player.Move); };
            inputs.Player.Move.canceled += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerStop", null, inputs.Player.Move); };
            inputs.Player.Jump.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("PlayerJump", null, inputs.Player.Jump); };
            inputs.Player.Pause.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("Pause", null, inputs.Player.Pause); };
        }

        private void OnEnable()
        {
            Debug.Log(inputs);
            inputs.Player.Move.Enable();
            inputs.Player.Jump.Enable();
            inputs.Player.Pause.Enable();
        }

        private void OnDisable()
        {
            inputs.Player.Move.Disable();
            inputs.Player.Jump.Disable();
            inputs.Player.Pause.Disable();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}