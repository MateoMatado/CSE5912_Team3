using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f;
    public float currentHealth { get; set; }
    public bool isDead { get; set; }
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHealth = startingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
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
