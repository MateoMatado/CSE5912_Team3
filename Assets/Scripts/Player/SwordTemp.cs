using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTemp : MonoBehaviour
{
    int attackpoint = 100;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("============sword==============");
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            Debug.Log(other.name+" HIT!");
            target.OnDamage(attackpoint);
        }
    }

    
}
