using UnityEngine;

public interface IDamageable 
{
    //Vector3 hitPoint : the point that the attack hit 
    //Vector3 hitNormal : Direction of the hit surface
    void OnDamage(float damage);
}

// Every Damageable entities should inherit this IDamageable interface
// Instead of examining every object type that was attacked,
// if the object inherited IDamageable interface
// then it only needs to execute the OnDamage method