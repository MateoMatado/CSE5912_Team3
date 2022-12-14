using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Team3.Events;
using Team3.Input;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cutsceneCamera;
    [SerializeField] private float duration = 10f;
    [SerializeField] private float zoomOutSpeed = 0f;
    [SerializeField] private float orbitSpeed = 20f;
    [SerializeField] private float ySpeed = 0f;

    void Start()
    {
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        EventsPublisher.Instance.PublishEvent("StartCutscene", null, null);
        cutsceneCamera.Priority = 9999;
        var orbiter = cutsceneCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        // orbiter.m_FollowOffset = new Vector3(0, 30, -100);
        float timer = duration;
        while (timer > 0)
        {
            orbiter.m_FollowOffset += new Vector3(0, ySpeed, -zoomOutSpeed) * Time.deltaTime;
            orbiter.m_Heading.m_Bias += orbitSpeed * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
        cutsceneCamera.Priority = 0;
        yield return new WaitForSeconds(1f);
        EventsPublisher.Instance.PublishEvent("EndCutscene", null, null);
    }
}
