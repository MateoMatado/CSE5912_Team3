using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public bool IsTriggerOn
    { get; set; }
    [SerializeField] private LivingEntity bossPrefab;

    private void Start()
    {
        IsTriggerOn = false;
    }

    private void Update()
    {
        if (IsTriggerOn)
        {
            Spawn();
            IsTriggerOn = false;
        }
    }

    public void Spawn()
    {
        Instantiate(bossPrefab, transform.position, transform.rotation);
    }
}
