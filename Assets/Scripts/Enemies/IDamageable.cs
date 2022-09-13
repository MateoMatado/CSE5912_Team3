using UnityEngine;

public interface IDamageable 
{
    //hitPoint : the point that the attack hit 
    //hitNormal : Direction of the hit surface
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}

// Every Damageable entities should inherit this IDamageable interface
// Instead of examining every object type that was attacked,
// if the object inherited IDamageable interface
// then it only needs to execute the OnDamage method