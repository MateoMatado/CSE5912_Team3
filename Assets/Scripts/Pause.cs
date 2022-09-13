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

        private void CheckPause(object sender, object data)
        {
            if (!paused)
            {
                Events.EventsPublisher.Instance.PublishEvent("Pause", null, null);
            }
            else
            {
                Events.EventsPublisher.Instance.PublishEvent("Unpause", null, null);
            }
        }

        private void StartPause(object sender, object data)
        {
            //GameStateMachine.Instance.SwitchState(new PauseState());
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }

        private void EndPause(object sender, object data)
        {
            //GameStateMachine.Instance.SwitchState(new RunningState());
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
        }
    }
}
