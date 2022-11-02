using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public LayerMask damageables;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    private float maxDamage = 120f;
    private float explosionForce = 1000f;
    private float lifeTime = 5f;
    private float explosionRadius = 50f;
    private bool isTriggered = false;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageables);
        for(int i=0; i<colliders.Length; i++)
        {
            //RigidBody not now...
            Debug.Log(colliders[i].name);
            IDamageable target = colliders[i].GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(maxDamage);
            }
        }
        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        explosionAudio.Play();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }
    
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(lifeTime-0.1f);
        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        explosionAudio.Play();
    }
}
