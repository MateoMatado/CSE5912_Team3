using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] int lowest;
    [SerializeField] int highest;
    private float timeToDestroy = 1f;
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        HUDManager.Instance.GetCoin(Random.Range(lowest, highest));
        GetComponent<AudioSource>().Play();
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(DestroyInTime());
    }

    private IEnumerator DestroyInTime()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
