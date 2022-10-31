using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(2.2f);
        Destroy(gameObject);
        for(int i = -1; i < 2; i++)
        {
            for (int n = -1; n < 2; n++)
            { 
                GameObject obj = ItemsFactory.Instance.GetRandomObject(Random.Range(0, 20));
                Vector3 newPosition = transform.position + (transform.forward * i * 6) + (transform.right * n * 6) + (transform.up * 2f);
                Instantiate(obj, newPosition, obj.transform.rotation);
            }
            
        }
    }
}
