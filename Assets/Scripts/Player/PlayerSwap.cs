using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team3.Scripts.Player
{
    public class PlayerSwap : MonoBehaviour
    {
        [SerializeField] List<string> names;
        [SerializeField] List<GameObject> prefabs;
        [SerializeField] GameObject cameraTarget;

        GameObject current;
        
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
            try
            {
                current = Instantiate(characterMap[names[0]], this.transform.position, Quaternion.Euler(0, 0, 0));
                current.transform.parent = this.transform;
                cameraTarget.transform.parent = current.GetComponentInChildren<CameraTarget>(false).transform;
                current.transform.localScale = new Vector3(1, 1, 1);
                current.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            catch
            {
                Debug.Log("No selected prefabs");
            }

            Events.EventsPublisher.Instance.SubscribeToEvent("ChangePrefab", ChangePrefab);
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
                cameraTarget.transform.parent = transform;
                Destroy(current);
                current = Instantiate(characterMap[name], this.transform.position, Quaternion.Euler(0, 0, 0));
                cameraTarget.transform.parent = current.GetComponentInChildren<CameraTarget>(false).transform;
                current.transform.parent = this.transform;
                current.transform.localScale = new Vector3(1, 1, 1);
                current.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Debug.Log("ERROR: No recognized prefab with name: " + name);
            }
        }
    }
}
