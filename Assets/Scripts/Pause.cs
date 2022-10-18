using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Player;

namespace Team3
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private PlayerStateManager playerStateManager;

        private bool paused = false;

        // Start is called before the first frame update
        void Start()
        {
            playerStateManager = GameObject.Find("Player").GetComponent<PlayerStateManager>();
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
            if (playerStateManager.StateMachine.CurrentState is not CannonAimState)
            {
                GameStateMachine.Instance.CurrentState.HandlePause();
            }
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
