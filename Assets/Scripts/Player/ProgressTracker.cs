using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;
using Cinemachine;
using Team3.Input;

public class ProgressTracker : MonoBehaviour
{
    private int deaths = 0;
    private int clears = 0;
    [SerializeField] private int killsForBossIsland;
    [SerializeField] private int islandClearsForBossIsland;
    [SerializeField] private CinemachineVirtualCamera cutsceneCam;
    private bool done = false;
    private float shrinkSpeed = 100f;

    void Start()
    {
        EventsPublisher.Instance.SubscribeToEvent("DeadEntity", HandleDeadEntity);
        EventsPublisher.Instance.SubscribeToEvent("LaunchedCannon", HandleIslandClear);
    }

    void OnDestroy()
    {
        EventsPublisher.Instance.UnsubscribeToEvent("LaunchedCannon", HandleIslandClear);
    }

    private void HandleDeadEntity(object sender, object data)
    {
        deaths++;
        if (deaths >= killsForBossIsland && clears >= islandClearsForBossIsland && !done)
        {
            StartCoroutine(ShrinkForceField());
            done = true;
        }
    }

    private void HandleIslandClear(object sender, object data)
    {
        clears++;
        if (deaths >= killsForBossIsland && clears >= islandClearsForBossIsland && !done)
        {
            StartCoroutine(ShrinkForceField());
            done = true;
        }
    }

    private IEnumerator ShrinkForceField()
    {
        GameObject.Find("GameManager").GetComponent<GameInputManager>().DisableMove();
        cutsceneCam.Priority = 1000;
        while (transform.localScale.x > 1)
        {
            float scale = transform.localScale.x;
            scale -= shrinkSpeed * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        cutsceneCam.Priority = 0;
        GameObject.Find("GameManager").GetComponent<GameInputManager>().EnableMove();
        Destroy(gameObject);
    }
}
