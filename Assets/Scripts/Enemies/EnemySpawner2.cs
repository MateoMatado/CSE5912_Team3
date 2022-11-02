using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2 : MonoBehaviour
{
    [SerializeField] private GhoulTemp ghoulPrefab;
    private List<GhoulTemp> ghoulsList = new List<GhoulTemp>();
    [SerializeField] private int spawnAmount = 20;


    private void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            CreateGhouls();
        }
    }

    private void CreateGhouls()
    {        
        GhoulTemp ghoul = Instantiate(ghoulPrefab, transform.position, transform.rotation);
        ghoulsList.Add(ghoul);

        ghoul.onDeath += () => ghoulsList.Remove(ghoul);
        ghoul.onDeath += () => Destroy(ghoul.gameObject, 10f);

    }
}
