using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float radius = 10f;
    [SerializeField] private int numberOfSpawn = 20;
    //private bool wasTriggeredOn = false;
    private bool isTriggerOn = false;
    private float nextSpawnTime = 3f;
    private float spawnTimer = 0;
    private int totalSpawnCount = 2;

    
    private void Update()
    {
        if (isTriggerOn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0)
            {
                totalSpawnCount --;
                if(totalSpawnCount > 0)
                {
                    Spawn();
                }
                spawnTimer = nextSpawnTime;
            }

        }
    }
    private void Spawn()
    {
        for (int i = 0; i < numberOfSpawn; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSpawn;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = transform.position + new Vector3(x, 0, z);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);

            Instantiate(enemyToSpawn, spawnPosition, rot);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spawn Enemy!!!");
        Spawn();
        isTriggerOn = true;        
    }
}