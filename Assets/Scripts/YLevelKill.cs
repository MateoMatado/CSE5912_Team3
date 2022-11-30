using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YLevelKill : MonoBehaviour
{
    [SerializeField] private float yLevelLimit = -50;
    private bool dead = false;
    
    void Update()
    {
        if (!dead && transform.position.y < yLevelLimit)
        {
            dead = true;
            GetComponent<LivingEntity>()?.Die();
        }
    }
}
