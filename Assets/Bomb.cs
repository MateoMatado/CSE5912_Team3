using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    public LayerMask damageables;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    private float maxDamage = 120f;
    //private float explosionForce = 2000f;
    private float lifeTime = 5f;
    private float explosionRadius = 10f;
    private bool isTriggered = false;

    private float power = 1000f;
    //public Transform targetEntity;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        //Add Force with given Degree
        rigidbody.AddForce((transform.forward + transform.up) * power/2);

        StartCoroutine(Explosion());
        Destroy(gameObject,lifeTime);
    }

    //OnTriggerEnter?
    //private void OnTriggerEnter(Collider other)
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Bomb Collider" + other.gameObject.name);
        if(collision.gameObject.tag == "Player")
        {
            
        }
        //Debug.Log("Bomb----Player");
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageables);
        for (int i = 0; i < colliders.Length; i++)
        {
            //RigidBody not now...
            //Debug.Log(colliders[i].name);
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
