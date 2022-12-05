using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class SpawnEffect : MonoBehaviour
{
    [SerializeField] public float speed = .1f;
    private float amount = 1f;

    void Start()
    {
        StartCoroutine(DissolveChildren());
    }

    private IEnumerator DissolveChildren()
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetFloat("_DissolveAmount", amount);
        }
        while (amount > 0f)
        {
            amount -= speed * Time.deltaTime;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetFloat("_DissolveAmount", amount);
            }
            yield return null;
        }
    }
}
