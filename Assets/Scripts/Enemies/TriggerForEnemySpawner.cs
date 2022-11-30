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

    private void Start()
    {
        //GameObject.Find("NavStartingIsland").g
    }
    private void OnTriggerEnter(Collider other)
    {
        this.name = "FinalEnemySpawner";
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
        this.name = "ExpiredSpawner";
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
