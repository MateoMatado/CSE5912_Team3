using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUtility : MonoBehaviour
{
    private float delayTime = 3f;
    public Vector3 randomPoint;
    public static Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance, int areaMask)
    {
        var randomPos = Random.insideUnitSphere * distance + center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, distance, areaMask);
        return hit.position;
    }

    private void Start()
    {
        randomPoint = transform.position;
        StartCoroutine(getRandomPoint());
    }

    IEnumerator getRandomPoint()
    {
        while (true)
        {
            randomPoint = GetRandomPointOnNavMesh(transform.position, 30f, NavMesh.AllAreas);
            Debug.Log("RandomPoint: " + randomPoint);
            yield return new WaitForSeconds(delayTime);
        }      
        
    }
}
