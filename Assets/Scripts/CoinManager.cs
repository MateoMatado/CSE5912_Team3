using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private float maxOffset = 5f;

    void Start()
    {
        EventsPublisher.Instance.SubscribeToEvent("DeadEnemy", HandleDeadEnemy);
    }

    private void HandleDeadEnemy(object sender, object data)
    {
        GameObject enemy = (GameObject)sender;
        Vector3 pos = enemy.transform.position;
        var tuple = ((int, int))data;
        int minCoins = tuple.Item1;
        int maxCoins = tuple.Item2;
        int numCoins = Random.Range(minCoins, maxCoins);
        for (int i = 0; i < numCoins; i++)
        {
            Vector3 offset = new Vector3(
                2 * Random.value * maxOffset - maxOffset,
                2,
                2 * Random.value * maxOffset - maxOffset
            );
            if (Physics.Raycast(pos + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, 15, 8))
            {
                pos.y = hit.point.y;
            }
            Instantiate(coin, pos + offset, transform.rotation);
        }
    }
}
