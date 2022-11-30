using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelfDestructMob : LivingEntity
{
    private enum State
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    [SerializeField] GameObject iDamageablePlayer;
    public ParticleSystem explosionParticle;
    public AudioClip explosionAudio;

    private State state;

    private NavMeshAgent navMeshAgent;
    private Animator ghoulAnimator;

    //temp code for debug (to see the attack area and fov)
    public Transform attackRoot;
    public Transform eyeTransform;

    //public AudioClip chaseClip;
    //public AudioClip deathClip;


    private AudioSource ghoulAudioPlayer;
    public ParticleSystem hitEffect;
    private ParticleSystem hitEffect2;
    public AudioClip deathSound;
    public AudioClip onDamageSound;
    public AudioClip chaseSound;


    [Range(0.01f, 2f)] public float turnSmoothTime = 1f;
    private float turnSmoothVelocity;

    private float attackDamage = 30f;
    public float attackRadius = 3f;
    private float attackDistance;

    private float fieldOfView = 360f;
    private float viewDistance = 40f;
    private float lostDistance = 60f;
    private float patrolSpeed = 6f;
    private float chaseSpeed = 12f;



    //public LivingEntity targetEntity;
    //temp code. should use upper one (livingEntity)
    public Transform targetEntity;
    public LayerMask whatIsTarget; //chase only the given layer object


    [SerializeField] float timeBetweenAttack = 1.3f;
    private float lastAttackTime;

    private bool isAlreadyAttacked = false;

    //will use RaycastHit[] to implement range based attack
    private const float waitTimeForCoroutine = 0.2f; //0.05
    private const float remainingDistance = 8f; //1




    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ghoulAnimator = GetComponent<Animator>();
        ghoulAudioPlayer = GetComponent<AudioSource>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        navMeshAgent.stoppingDistance = attackDistance;
        navMeshAgent.speed = patrolSpeed;

        hitEffect2 = GetComponentInChildren<ParticleSystem>();
    }


    private void Start()
    {
        StartCoroutine(UpdatePath());
    }
    private bool hasTarget
    {
        get
        {
            if (targetEntity != null)
            {
                return true;
            }
            return false;
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!isDead)
        {
            // if there is a target to chase, change state, update path to chase
            if (hasTarget)
            {
                if (state == State.Patrol)
                {
                    state = State.Chase;
                    navMeshAgent.speed = chaseSpeed;
                    //Debug.Log("Enemy State : Chasing...");
                }
                navMeshAgent.SetDestination(targetEntity.position);
            }
            // if no target, then patrol
            else
            {
                if (targetEntity != null)
                {
                    targetEntity = null;
                }
                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    navMeshAgent.speed = patrolSpeed;
                    //Debug.Log("Enemy State : Patrol...");                    
                }

                //first, move to random position
                //Debug.Log("remainingDistance" + navMeshAgent.remainingDistance);

                if (navMeshAgent.remainingDistance <= remainingDistance)
                {
                    //var patrolTargetPosition = GameObject.Find("EnemySpawnerType2").GetComponent<EnemyUtility>().randomPoint;
                    var patrolTargetPosition = GameObject.Find("FinalEnemySpawner").GetComponent<EnemyUtility>().randomPoint;
                    //var patrolTargetPosition = EnemyUtility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    navMeshAgent.SetDestination(patrolTargetPosition);
                }



                //Then, check nearby object whether it is target(player) by checking colliders nearby
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                foreach (var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform))
                    {
                        continue;
                    }
                    else
                    {
                        targetEntity = collider.transform;
                        //Debug.Log("Target Detected");
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(waitTimeForCoroutine);
        }
    }

    private bool IsTargetOnSight(Transform target)
    {
        var direction = target.position - eyeTransform.position;
        direction.y = eyeTransform.forward.y;

        if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        direction = target.position - eyeTransform.position;
        RaycastHit hit;
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        if (state == State.Chase)
        {
            var distance = Vector3.Distance(targetEntity.position, transform.position);
            //Debug.Log("current:" + distance+"   ,   attkDis:"+ attackDistance);
            if (distance <= attackDistance + 2f)  //this is to make 
            {
                BeginAttack();
            }

            //when lost target
            if (distance >= lostDistance)
            {
                targetEntity = null;
                state = State.Patrol;
                navMeshAgent.speed = patrolSpeed;
            }


        }

        ghoulAnimator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (state == State.Attack)
        {
            if (navMeshAgent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
        }

    }


    //Self-Destruct Mob will jump 
    public void BeginAttack()
    {
        state = State.Attack;
        navMeshAgent.isStopped = true;
        ghoulAudioPlayer.clip = chaseSound;
        ghoulAudioPlayer.PlayOneShot(chaseSound);
        ghoulAnimator.SetTrigger("Attack");
    }

    //After the end of Attack Animation, do self-destruct
    public void EndAttack()
    {
        isDead = true;
        iDamageablePlayer.GetComponent<IDamageable>().OnDamage(attackDamage);
        //Play Explode Effect 
        
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        ghoulAudioPlayer.clip = explosionAudio;
        ghoulAudioPlayer.PlayOneShot(explosionAudio);

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        ghoulAnimator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }



    //To affect damage to Player
    /*
    void OnCollisionEnter(Collision collision)
    {
        if (state == State.Attack)
        {
            Rigidbody hitTarget = collision.rigidbody;
            if (collision.collider.name == "Player")
            {
                collision.collider.GetComponent<IDamageable>().OnDamage(attackDamage);
            }
        }
    }
    */




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
        ghoulAnimator.SetTrigger("Die");
        ghoulAudioPlayer.clip = deathSound;
        ghoulAudioPlayer.PlayOneShot(deathSound);

        Collider ghoulCollider = GetComponent<Collider>();
        ghoulCollider.enabled = false;
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;
    }
}
