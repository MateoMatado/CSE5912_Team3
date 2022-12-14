using System.Collections;
using System.Collections.Generic;
// using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.AI;

public class GhoulSmallFast : LivingEntity
{
    private enum State
    {
        Patrol,
        Flee,
        Attack
    }

    private float startFleeDistance = 10f;
    private float fleeDisplacement = 30f;

    public GameObject landMinePrefab;


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



    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private float attackDamage = 30f;
    public float attackRadius = 3f;
    private float attackDistance;

    private float fieldOfView = 360f;
    private float viewDistance = 40f;
    private float lostDistance = 60f;
    private float patrolSpeed = 10f;
    private float fleeSpeed = 16f;



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
        currentHealth = 300f;
        StartCoroutine(UpdatePath());
        StartCoroutine(PlantMine());
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
                    state = State.Flee;
                    navMeshAgent.speed = fleeSpeed;
                }
                //navMeshAgent.SetDestination(targetEntity.position);
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
                }

                //first, move to random position
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
        if (state == State.Flee)
        {
            var distance = Vector3.Distance(targetEntity.position, transform.position);
            //if (distance <= attackDistance + 2f)  
            if (distance <= startFleeDistance)
            {
                BeginFlee();
            }

            //when flee enough from target
            if (distance >= lostDistance)
            {
                targetEntity = null;
                state = State.Patrol;
                navMeshAgent.speed = patrolSpeed;
            }
        }
        ghoulAnimator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }

    /*
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
    */
    public void BeginFlee()
    {
        state = State.Flee;
        Vector3 normDir = (targetEntity.position - transform.position).normalized;
        normDir = Quaternion.AngleAxis(110, Vector3.up) * normDir;
        var newPos = (transform.position - (normDir * fleeDisplacement));
        navMeshAgent.SetDestination(newPos);
    }
    
    IEnumerator PlantMine()
    {
        for(int i=0; i<1000; i++)
        {
            yield return new WaitForSeconds(1f);
            Vector3 offset = new Vector3(0, 0.5f, 0);
            Instantiate(landMinePrefab, transform.position+ offset, transform.rotation);
        }        
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

    /*
    public void EndAttack()
    {
        if (!isDead)
        {
            state = State.Chase;
            navMeshAgent.isStopped = false;
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
