using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YLevelKill : MonoBehaviour
{
    [SerializeField] private float yLevelLimit = -50;
    [SerializeField] private float damage = 200;
    [SerializeField] private Transform islandsParent;
    private GameObject player;

    void Start()
    {
        StartCoroutine(CheckYLevel());
    }

    private IEnumerator CheckYLevel()
    {
        while(true)
        {
            if (player == null) player = GameObject.Find("Player");
            if (player != null && player.transform.position.y < yLevelLimit)
            {
                yield return DamageAndRespawn();
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator DamageAndRespawn()
    {
        player.GetComponent<LivingEntity>().OnDamage(damage);
        Transform island = FindClosestIsland();
        player.transform.position = island.position + new Vector3(0, 300, 0);
        yield return null;
    }

    private Transform FindClosestIsland()
    {
        Vector3 myPos = transform.position;
        myPos.y = 0;
        float minDistance = float.MaxValue;
        Transform closestIsland = null;
        foreach (Transform child in islandsParent)
        {
            if (child.tag == "IslandParent")
            {
                Vector3 pos = child.position;
                pos.y = 0;
                float distance = Vector3.Distance(myPos, pos);
                if (distance < minDistance) 
                {
                    minDistance = distance;
                    closestIsland = child;
                }
            }
        }
        return closestIsland;
    }
}
