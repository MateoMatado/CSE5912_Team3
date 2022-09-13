using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team3
{
    public class LoadScene : MonoBehaviour
    {
        [SerializeField] string eventName;
        [SerializeField] string sceneName;

        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent(eventName, Load);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent(eventName, Load);
        }

        private void Load(object sender, object data)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}