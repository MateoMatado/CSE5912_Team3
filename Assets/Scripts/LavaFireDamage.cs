using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class LavaFireDamage : MonoBehaviour
{
    [SerializeField] private float damageRate = 25;
    private bool playerInLava;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 3 && col.gameObject.name == "Player")
        {
            col.transform.Find("FireParticles").gameObject.SetActive(true);
            col.transform.Find("FireParticles").GetComponent<ParticleSystem>().Play();
            playerInLava = true;
            StartCoroutine(DamagePlayer());
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.layer == 3 && col.gameObject.name == "Player")
        {
            // col.transform.Find("FireParticles").gameObject.SetActive(false);
            col.transform.Find("FireParticles").GetComponent<ParticleSystem>().Stop();
            playerInLava = false;
        }
    }

    private IEnumerator DamagePlayer()
    {
        while (playerInLava)
        {
            EventsPublisher.Instance.PublishEvent("LavaDamagePlayer", null, damageRate * Time.deltaTime);
            yield return null;
        }
    }
}
