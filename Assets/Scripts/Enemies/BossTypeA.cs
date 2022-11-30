using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossTypeA : LivingEntity
{
    private enum State
    {
        Chase,
        Faint,
        Spinning,
        DashAttack,
        Idle,
        Roar
    }
    private State state;

    private Animator bossAnimator;
    private AudioSource bossAudioPlayer;
    public ParticleSystem hitEffect;
    private ParticleSystem hitEffect2;
    public AudioClip deathSound;
    public AudioClip onDamageSound;
    public Transform targetEntity;


    private Vector3 oldTargetPosition;
    public float attackRadius = 3f;
    private float attackDistance;
    private const float dashRange = 70f;
    private const float spinStartDistance = 50f;    

    private const float faintTime = 3f;
    private const float spinTime = 4f;
    private const float roarTime = 1f;
    private const float waitingTimeBeforeDash = 5f;

    private float speed = 3f;
    private const float chaseSpeed = 10f;
    private const float dashSpeed = 20f;
    private const float spinTowardSpeed = 10f;
    private const float rotateSpeed = 720f;

    public Transform attackRoot;
    Rigidbody rb;

    float timer = 0f;
    

    private void Awake()
    {
        targetEntity = GameObject.Find("Player").transform;
        startingHealth = 1000f;
        speed = chaseSpeed;
        //GetComponentInParent!
        bossAnimator = GetComponent<Animator>();
        bossAudioPlayer = GetComponent<AudioSource>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        rb=GetComponent<Rigidbody>();

        hitEffect2 = GetComponentInChildren<ParticleSystem>();
    }

    
    void DoSpinning()
    {
        state = State.Spinning;
        timer += Time.deltaTime;

        //Perform Spinning
        if (timer < spinTime)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.Self);
            //transform.LookAt(targetEntity);
            
            speed = spinTowardSpeed;
            //transform.position += Vector3.forward * speed * Time.deltaTime;


            var targetPos = (targetEntity.position - transform.position).normalized;
            targetPos.y = 0;
            transform.position += targetPos * speed * Time.deltaTime;



            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //State: Spinning -> Faint
        else
        {
            timer = 0.0f;
            state = State.Faint;
            bossAnimator.SetTrigger("Die");
        }
    }


    void DoFaint()
    {
        state = State.Faint;
        speed = 0f;
        timer += Time.deltaTime;
        //State: Faint -> Chase
        if (timer > faintTime)
        {
            state = State.Chase;
            bossAnimator.SetTrigger("Chase");
            timer = 0.0f;
        }
    }

    void DoRoarBeforeDash()
    {
        //bossAnimator.SetTrigger("Angry");
        state = State.Roar;
        speed = 0f;
        timer += Time.deltaTime;
        transform.LookAt(targetEntity);
        if (timer > waitingTimeBeforeDash)
        {
            //Perform DashAttack
            //Look At Old Target Position
            oldTargetPosition = targetEntity.position; ////////////            
            bossAnimator.SetTrigger("Dash");
            state = State.DashAttack;            
            timer = 0.0f;
        }
    }
    void DoDash(Vector3 oldTargetPosition)
    {
        state = State.DashAttack;                
        speed = dashSpeed;
        //transform.position += Vector3.forward * speed * Time.deltaTime;

        var targetPos = (oldTargetPosition - transform.position).normalized;
        targetPos.y = 0;
        transform.position += targetPos * speed * Time.deltaTime;

        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    /*
    void DoDash2()
    {
        state = State.DashAttack;
        speed = dashSpeed;
        //transform.position += Vector3.forward * speed * Time.deltaTime;

        var targetPos = (targetEntity.position - transform.position).normalized;
        targetPos.y = 0;
        transform.position += targetPos * speed * Time.deltaTime;

        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    */

    private void Start()
    {
        bossAnimator.SetFloat("Speed", speed);
        state = State.Chase;        
    }

    void Update()
    {        
        //Debug.Log("BOSS STATE: " + state + "!!!!!!!!!!!");
        if (isDead)
        {
            return;
        }
        if (state == State.Chase)
        {
            //Chase To Target
            speed = chaseSpeed;
            //transform.LookAt(targetEntity);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Check Distance whether to Change State 
            var currentTargetdistance = Vector3.Distance(targetEntity.position, transform.position);
            if (currentTargetdistance > dashRange)  
            {
                state = State.Roar;
                bossAnimator.SetTrigger("Angry");
            }
            else if (currentTargetdistance < spinStartDistance)
            {                
                state = State.Spinning;
                bossAnimator.SetTrigger("Spin");
            }
        }
        else if(state == State.Roar)
        {
            DoRoarBeforeDash();
        }
        else if(state == State.DashAttack)
        {
            var oldTargetdistance = Vector3.Distance(oldTargetPosition, transform.position);            
            if (oldTargetdistance > 3f)
            {
                DoDash(oldTargetPosition); 
            }
            else
            {
                state = State.Chase;
                bossAnimator.SetTrigger("Chase");
            }            
        }
        else if(state == State.Spinning)
        {
            isInvincible = true;
            DoSpinning();
        }
        else if(state == State.Faint)
        {
            isInvincible = false;
            DoFaint();
        }

    }














    public override void OnDamage(float damage)
    {
        if (!isDead && !isInvincible)
        {
            hitEffect2.Play();
            bossAudioPlayer.clip = onDamageSound;
            bossAudioPlayer.PlayOneShot(onDamageSound);
        }

        //affect damage on hp
        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        bossAnimator.SetTrigger("Die");
        bossAudioPlayer.clip = deathSound;
        bossAudioPlayer.PlayOneShot(deathSound);

        //Collider bossCollider = GetComponent<Collider>();
        //bossCollider.enabled = false;


    }
}
