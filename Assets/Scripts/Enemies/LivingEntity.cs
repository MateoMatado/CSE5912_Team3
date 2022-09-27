using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentHealth -= damage;

        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //event listener
        if(onDeath != null)
        {
            onDeath();
        }
        isDead = true;
    }
}
