using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public bool IsTriggerOn
    { get; set; }
    [SerializeField] private LivingEntity bossPrefab;
    bool bossSpawnedAlready;
    private void Start()
    {
        IsTriggerOn = false;
        bossSpawnedAlready = false;
    }

    private void Update()
    {
        if (IsTriggerOn && !bossSpawnedAlready)
        {
            Spawn();
            IsTriggerOn = false;
            bossSpawnedAlready = true;
        }
    }

    public void Spawn()
    {
        Instantiate(bossPrefab, transform.position, transform.rotation);
    }
}
