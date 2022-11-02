using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerForEnemySpawner : MonoBehaviour
{
    // This class is to check whether Player landed at the island
    // If player enters the trigger, then order Beacon to spawn Enemies

    private bool isTriggerOn = false;
    [SerializeField] List<EnemySpawner> enemySpawner = new List<EnemySpawner>();
    [SerializeField] BossSpawner bossSpawner;
    //Check Player enters Beacon Area. If then, start spawning enemies
    private void OnTriggerEnter(Collider other)
    {
        isTriggerOn = true;
        foreach (EnemySpawner spawner in enemySpawner)
        {
            spawner.IsTriggerOn = true;
        }
        if(bossSpawner != null)
        {
            bossSpawner.IsTriggerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggerOn = false;
        foreach (EnemySpawner spawner in enemySpawner)
        {
            spawner.IsTriggerOn = false;
        }
        if (bossSpawner != null)
        {
            bossSpawner.IsTriggerOn = false;
        }
    }
}
