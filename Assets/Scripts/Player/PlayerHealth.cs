using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{   

    //OnEnable is for when player revives  
    protected override void OnEnable()
    {
        base.OnEnable();
        
        //add health UI code here


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
    }

    public override void Die()
    {
        base.Die();

        //death sound and animation here

        //disable player movement here


    }

}
