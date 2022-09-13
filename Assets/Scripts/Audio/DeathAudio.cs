using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Audio
{
    public class DeathAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private List<AudioClip> audioList;
        [SerializeField] private string eventName;

        // Start is called before the first frame update
        void Start()
        {
            Events.EventsPublisher.Instance.SubscribeToEvent(eventName, PlaySound);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent(eventName, PlaySound);
        }

        private void PlaySound(object sender, object data)
        {
            if (audioList.Count > 0)
            {
                source.PlayOneShot(audioList[Random.Range(0, audioList.Count)]);
                Debug.Log("PlayingDeathEffect");
            }
        }
    }
}