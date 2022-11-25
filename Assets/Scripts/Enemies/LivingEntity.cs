using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Team3.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float currentHealth { get; set; }
    public bool isDead { get; set; }
    public event Action onDeath;        //trigger this event when dead

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startingHealth;
    }

    private void Awake()
    {
        StartCoroutine(EaseDamageMaterial());
    }

    public virtual void OnDamage(float damage)
    {
        //Debug.Log("Ahh~ ondamage");
        currentHealth -= damage;

        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private IEnumerator EaseDamageMaterial()
    {
        float pct = Math.Clamp(currentHealth / startingHealth, 0, 1);
        float lerpSpeed = 1f;
        while (!isDead)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            float healthPct = Math.Clamp(currentHealth / startingHealth, 0, 1);
            pct += (healthPct - pct) * lerpSpeed * Time.deltaTime;
            Debug.Log(gameObject.name + ' ' + pct);
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetFloat("_Health", pct);
            }
            yield return null;
        }
    }

    public virtual void Die()
    {
        //event listener
        EventsPublisher.Instance.PublishEvent("DeadEntity", null, gameObject);
        if(onDeath != null)
        {
            onDeath();
        }
        isDead = true;
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetFloat("_Health", 0);
        }
    }
}
