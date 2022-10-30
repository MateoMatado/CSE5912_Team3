using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int lowest;
    [SerializeField] int highest;
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        HUDManager.Instance.GetCoin(Random.Range(lowest, highest));
        Destroy(gameObject);
    }
}
