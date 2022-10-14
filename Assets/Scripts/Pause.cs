using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;

        private bool paused = false;

        // Start is called before the first frame update
        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent("PauseUnpause", CheckPause);
            Events.EventsPublisher.Instance.SubscribeToEvent("Pause", StartPause);
            Events.EventsPublisher.Instance.SubscribeToEvent("Unpause", EndPause);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("PauseUnpause", CheckPause);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("Pause", StartPause);
            Events.EventsPublisher.Instance.UnsubscribeToEvent("Unpause", EndPause);
        }

        private void CheckPause(object sender, object data)
        {
            GameStateMachine.Instance.CurrentState.HandlePause();
        }

        private void StartPause(object sender, object data)
        {
            pauseMenu.SetActive(true);
            paused = true;
        }

        private void EndPause(object sender, object data)
        {
            pauseMenu.SetActive(false);
            paused = false;
        }
    }
}
