using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
// using UnityEditor.PackageManager;
#endif


//THIS IS FOR DEBUG by Jimmy

namespace Team3.PlayerMovement
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private Camera[] cameras;
        [SerializeField] private Camera defaultCamera;
        private int index = 0;

        void Start()
        {            
            index = 0;

            
            // Loop through each camera and disable it.            
            for (int i = index; i < cameras.Length; i++)
            {
                // If the camera is the defaultCamera, then enable it
                if (cameras[i].name.Equals(defaultCamera.name))
                {
                    cameras[i].enabled = true;
                    //this is to make sure next camera is not the default
                    index = i;
                }
                else
                    cameras[i].enabled = false;
            }
            Debug.Log("Default Camera Type: " + defaultCamera.name);
            
            Events.EventsPublisher.Instance.SubscribeToEvent("CameraSwitch", NextCamera);
        }

        private void OnDestroy()
        {
            Events.EventsPublisher.Instance.UnsubscribeToEvent("CameraSwitch", NextCamera);
        }

        public void NextCamera(object sender, object data)
        {
            int oldIndex = index;
            index++;
            index %= cameras.Length;

            // Enable the next camera
            cameras[index].enabled = true;
            Debug.Log("Switched to " + cameras[index].name);

            // then disable the current camera
            cameras[oldIndex].enabled = false;
        }
    }
}
