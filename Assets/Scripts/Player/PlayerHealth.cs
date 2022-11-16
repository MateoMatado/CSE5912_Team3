using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    //[SerializeField] PlayerStatus playerStatus;
    //OnEnable is for when player revives  
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Start()
    {
        currentHealth = 1000;
    }

    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            //on damage sound effect code here
        }

        base.OnDamage(damage);

        Debug.Log("PLAYER HP:" + currentHealth);
        //change in HP UI code here
        //playerStatus.HealthChange(-damage);
    }

    public override void Die()
    {
        base.Die();

        //death sound and animation here

        //disable player movement here


    }

}
