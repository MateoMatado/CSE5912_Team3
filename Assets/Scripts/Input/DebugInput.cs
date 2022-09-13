using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Input
{
    public class DebugInput : MonoBehaviour
    {
        private GameInput inputs;

        void Awake()
        {
            inputs = new GameInput();
            inputs.Player.ReloadScene.performed += (context) => { Events.EventsPublisher.Instance.PublishEvent("LoadRunning", null, inputs.Player.Pause); };
        }

        private void OnEnable()
        {
            inputs.Player.ReloadScene.Enable();
        }

        private void OnDisable()
        {
            inputs.Player.ReloadScene.Disable();
        }
    }
}