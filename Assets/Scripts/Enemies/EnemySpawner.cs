using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float radius = 5f;
    [SerializeField] private int numberOfSpawn = 20;
    private bool isTriggerOn = false;
    private float nextSpawnTime = 3f;
    private float spawnTimer = 0;

    private void Update()
    {
        if (isTriggerOn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer < 0)
            {
                Spawn();
                spawnTimer = nextSpawnTime;
            }

        }
    }
    private void Spawn()
    {
        float nextAngle = 2 * Mathf.PI / numberOfSpawn;
        float angle = 0;

        for (int i = 0; i < numberOfSpawn; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sign(angle) * radius;

            Vector3 spawnPosition = new Vector3(x, transform.position.y, z);
            //GameObject objToSpawn = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            GameObject objToSpawn = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);

            angle += nextAngle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spawn Enemy!!!");
        Spawn();
        isTriggerOn = true;
    }
}