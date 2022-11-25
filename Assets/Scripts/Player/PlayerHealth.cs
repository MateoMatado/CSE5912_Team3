using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : LivingEntity
{
    public static PlayerHealth Instance = null;

    //OnEnable is for when player revives  
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    
    private void Awake()
    {
        Instance = this;
        // currentHealth = 1000;
        Instance.currentHealth = HUDManager.Instance.hp;
        Instance.startingHealth = HUDManager.Instance.hp;
        StartCoroutine(EaseDamageMaterial());

    }
    private void Start()
    {
        // Instance.currentHealth = HUDManager.Instance.hp;

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
