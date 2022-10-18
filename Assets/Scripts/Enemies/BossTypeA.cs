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
        SpinAttack,
        DashAttack,
        Idle
    }
    private State state;

    private Animator bossAnimator;
    private AudioSource ghoulAudioPlayer;
    public ParticleSystem hitEffect;
    private ParticleSystem hitEffect2;
    public AudioClip deathSound;
    public AudioClip onDamageSound;

    public float attackRadius = 3f;
    private float attackDistance;
    private const float dashRange = 8f;   

    public Transform targetEntity;
    [SerializeField] float timeBetweenAttack = 1.3f;
    private float lastAttackTime;

    private bool isAlreadyAttacked = false;
    private const float waitTimeForCoroutine = 0.2f; //0.05
    private const float remainingDistance = 8f; //1

    private const float faintTime = 4f;
    private const float spinTime = 2f;

    private float speed = 3f;
    private const float walkSpeed = 5f;
    private const float dashSpeed = 15f;
    private const float rotateSpeed = 10f;
    float dashDuration = 0.5f;
    public Transform attackRoot;
    //Transform orientation;
    private Rigidbody rb;


    float timer = 0f;
    float waitingTimeBeforeDash = 4f;

    private void Awake()
    {
        speed = walkSpeed;
        bossAnimator = GetComponent<Animator>();
        //bossAudioPlayer = GetComponent<AudioSource>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        rb=GetComponent<Rigidbody>();
        //orientation = GetComponent<Transform>();

        //hitEffect2 = GetComponentInChildren<ParticleSystem>();
    }

    void SpinAttack()
    {
        bossAnimator.SetTrigger("Spin");
        DoSpinning();

        DoFaint();
        state = State.Idle;
    }
    IEnumerator DoSpinning()
    {
        transform.Rotate(0f, rotateSpeed*Time.deltaTime, 0f, Space.Self);
        transform.Translate(Vector3.forward * Time.deltaTime);
        yield return new WaitForSeconds(spinTime);
    }


    IEnumerator DoFaint()
    {
        bossAnimator.SetTrigger("Faint");
        yield return new WaitForSeconds(faintTime);
    }
    void DashAttack()
    {
        bossAnimator.SetTrigger("Dash");
        speed = dashSpeed;
        transform.LookAt(targetEntity);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //Vector3 force = transform.forward;
        //rb.AddForce(force, ForceMode.Impulse);
    }

    void ResetDash()
    {

    }
    void DoChase()
    {

    }

    void DoIdle()
    {

    }

    private void Start()
    {
        bossAnimator.SetFloat("Speed", speed);
        //bossAnimator.SetTrigger("Faint");
       
        
        state = State.Chase;        
    }


    void Update()
    {
        Debug.Log("BOSS STATE: " + state + "!!!!!!!!!!!");
        //Debug.Log("SPEED:" + speed + ";;;" + bossAnimator.GetFloat("Speed"));
        if (isDead)
        {
            return;
        }
        if (state == State.Chase)
        {
            transform.LookAt(targetEntity);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            var distance = Vector3.Distance(targetEntity.position, transform.position);

            //Change State to "DashAttack"
            if (distance > dashRange)  
            {
                speed = 0f;
                bossAnimator.SetFloat("Speed", speed);
                timer += Time.deltaTime;
                //Debug.Log("TIMER:" + timer);
                if (timer > waitingTimeBeforeDash)
                {
                    state = State.DashAttack;
                    DashAttack();
                    timer = 0;
                }
                
            }
            //Change State to "SpinAttack"
            else if (distance < attackDistance )
            {
                state = State.SpinAttack;
                SpinAttack();
            }
            

        }
        else if(state == State.DashAttack)
        {

        }

    }














    public override void OnDamage(float damage)
    {
        if (!isDead)
        {
            hitEffect2.Play();
            ghoulAudioPlayer.clip = onDamageSound;
            ghoulAudioPlayer.PlayOneShot(onDamageSound);
        }

        //affect damage on hp
        base.OnDamage(damage);
    }

    public override void Die()
    {
        base.Die();
        bossAnimator.SetTrigger("Die");
        ghoulAudioPlayer.clip = deathSound;
        ghoulAudioPlayer.PlayOneShot(deathSound);

        Collider ghoulCollider = GetComponent<Collider>();
        ghoulCollider.enabled = false;


    }
}
