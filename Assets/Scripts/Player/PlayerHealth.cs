using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : LivingEntity
{
    public static PlayerHealth Instance = new PlayerHealth();
    private float currentHealth = 1000;

    //OnEnable is for when player revives  
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    
    private void Awake()
    {
        //Instance = this;
        Instance.currentHealth = 1000;

    }
    private void Start()
    {
        Instance.currentHealth = HUDManager.Instance.hp;

    }



    private void Update()
    {
        Debug.Log("PlayerHealth.cs : " + Instance.currentHealth);
    }

    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            //on damage sound effect code here
        }

        //base.OnDamage(damage);
        //OnDamage(damage);
        Instance.currentHealth -= damage;

        if (Instance.currentHealth <= 0 && !isDead)
        {
            Die();
        }

        Debug.Log("PLAYER HP:" + Instance.currentHealth);
        //change in HP UI code here
        //playerStatus.HealthChange(-damage);
        //slider.value = currentHealth;
    }

    public override void Die()
    {
        base.Die();

        //death sound and animation here

        //disable player movement here


    }

    public float GetHP()
    {
        return Instance.currentHealth;
    }


}
