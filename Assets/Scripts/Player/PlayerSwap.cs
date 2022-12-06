using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Scripts.Player
{
    public class PlayerSwap : MonoBehaviour
    {
        [SerializeField] List<string> names;
        [SerializeField] List<GameObject> prefabs;

        public GameObject current;
        
        Dictionary<string, GameObject> characterMap = new Dictionary<string, GameObject>();
        
        void Start()
        {
            try
            {
                for (int i = 0; i < names.Count; i++)
                {
                    characterMap.Add(names[i], prefabs[i]);
                }
            }
            catch
            {
                Debug.Log("List of names and list of prefabs different lengths");
            }

            Events.EventsPublisher.Instance.SubscribeToEvent("ChangePrefab", ChangePrefab);
            StartCoroutine(CreatePlayer());
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("ChangePrefab", ChangePrefab);
        }

        private void ChangePrefab(object sender, object data)
        {
            string name = (string)data;
            if (characterMap.ContainsKey(name))
            {
                Destroy(current);
                current = Instantiate(characterMap[name], this.transform.position, Quaternion.Euler(0, 0, 0));
                current.transform.parent = this.transform;
                current.transform.localScale = new Vector3(1, 1, 1);
                current.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Debug.Log("ERROR: No recognized prefab with name: " + name);
            }
        }

        IEnumerator CreatePlayer()
        {
            yield return new WaitForSeconds(0.5f);
            Events.EventsPublisher.Instance.PublishEvent("ChangePrefab", this, names[0]);
        }
    }
}
