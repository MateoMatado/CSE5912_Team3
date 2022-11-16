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

    public virtual void OnDamage(float damage)
    {
        //Debug.Log("Ahh~ ondamage");
        currentHealth -= damage;

        if(currentHealth <= 0 && !isDead)
        {
            Die();
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
    }
}
