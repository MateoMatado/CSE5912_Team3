using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Team3.Events;

public class CannonCutscene : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cutsceneCamera;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float zoomOutSpeed = 10f;
    [SerializeField] private float orbitSpeed = 20f;
    [SerializeField] private float ySpeed = 2f;

    void Start()
    {
        EventsPublisher.Instance.SubscribeToEvent("CannonCutscene", HandleCutscene);
    }

    void OnDestroy()
    {
        EventsPublisher.Instance.UnsubscribeToEvent("CannonCutscene", HandleCutscene);
    }

    private void HandleCutscene(object sender, object data)
    {
        if (((GameObject)data) == gameObject) 
        {
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        cutsceneCamera.Priority = 999;
        var orbiter = cutsceneCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        orbiter.m_FollowOffset = new Vector3(0, 30, -100);
        float timer = duration;
        while (timer > 0)
        {
            orbiter.m_FollowOffset += new Vector3(0, ySpeed, -zoomOutSpeed) * Time.deltaTime;
            orbiter.m_Heading.m_Bias += orbitSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        cutsceneCamera.Priority = 0;
    }
}
