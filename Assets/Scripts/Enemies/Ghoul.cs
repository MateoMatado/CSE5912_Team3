using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Playables;
#endif


public class Ghoul : LivingEntity
{
    private enum State
    {
        Patrol,
        Tracking,
        AttackBegin,
        Attacking
    }
    private State state;

    private NavMeshAgent navMeshAgent;
    private Animator ghoulAnimator;

    //temp for debug
    public Transform attackRoot;
    public Transform eyeTransform;

    private AudioSource ghoulAudioPlayer;
    public AudioClip hitSound; 
    public AudioClip deathSound;
    
    private Renderer ghoulRenderer; //to differentiate ghoul's color by its damage value

    [SerializeField] float chaseSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField] float damage = 20f;
    public float attackRadius = 2f;
    private float attackDistance;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;

    //This is temporary code, should be the player, not Boss
    public LivingEntity targetEntity;
    public LayerMask whatIsTarget; //chase the given layer object

    
    private RaycastHit[] hits = new RaycastHit[10];
    private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();


    //temp
    public ParticleSystem hitEffect; //particle effect when get hit
    [SerializeField] float timeBetweenAttack = 0.5f;
    private float lastAttackTime;




#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
        }
        if(eyeTransform != null)
        {
            var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            var leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1f, 1f, 1f, 0.2f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }
        
    }
#endif

    private bool hasTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.isDead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ghoulAnimator = GetComponent<Animator>();
        ghoulAudioPlayer = GetComponent<AudioSource>();

        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;
        ghoulRenderer = GetComponentInChildren<Renderer>();

        navMeshAgent.stoppingDistance = attackDistance;
        navMeshAgent.speed = patrolSpeed;
    }


    public void Setup(float health, float damage, float chaseSpeed, float patrolSpeed)
    {
        startingHealth = health;
        this.damage = damage;
        this.chaseSpeed = chaseSpeed;
        this.patrolSpeed = patrolSpeed;
    }
    
    private void Start()
    {
        //after awake, start chasing routine
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        if(state == State.Tracking)
        {
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            if(distance <= attackDistance)
            {
                BeginAttack();
            }
        }
        ghoulAnimator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        if(state == State.AttackBegin || state == State.Attacking)
        {
            //turn smoothly to target
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;
        }

        if(state == State.Attacking)
        {
            var direction = transform.forward;
            var deltaDistance = navMeshAgent.velocity.magnitude * Time.deltaTime;

            var size = Physics.SphereCastNonAlloc(attackRoot.position, attackRadius, direction, hits, deltaDistance, whatIsTarget);

            /*
            for(int i=0; i<size; i++)
            {
                var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();
                
                if(attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
                {
                    var message = new DamageMessage();
                    message.amount = damage;
                    message.damager = gameObject;

                    if (hits[i].distance <= 0f)
                    {
                        message.hitPoint = attackRoot.position;
                    }
                    else
                    {
                        message.hitPoint = hits[i].point;
                    }
                    message.hitNormal = hits[i].normal;
                    attackTargetEntity.ApplyDamage(message);
                    lastAttackedTargets.Add(attackTargetEntity);
                    break;

                }
            }
            */
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!isDead)
        {
            if (hasTarget)
            {
                // if there is a target to chase, update path and move
                if(state == State.Patrol)
                {
                    state = State.Tracking;
                    navMeshAgent.speed = chaseSpeed;
                }
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                if (targetEntity != null)
                {
                    targetEntity = null;
                }
                if(state != State.Patrol)
                {
                    state = State.Patrol;
                    navMeshAgent.speed = patrolSpeed;
                }
                if(navMeshAgent.remainingDistance <= 1f)
                {
                    var patrolTargetPosition = EnemyUtility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    navMeshAgent.SetDestination(patrolTargetPosition);
                }

                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                foreach(var collider in colliders)
                {
                    if (!IsTargetOnSight(collider.transform)){
                        continue;
                    }
                    var livingEntity = collider.GetComponent<LivingEntity>();
                    if(livingEntity != null && !livingEntity.isDead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }


   private bool IsTargetOnSight(Transform target)
    {
        var direction = target.position - eyeTransform.position;
        direction.y = eyeTransform.forward.y;

        if(Vector3.Angle(direction,eyeTransform.forward) > fieldOfView * 0.5f)
        {
            return false;
        }

        RaycastHit hit;
        direction = target.position - eyeTransform.position;
        if(Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if(hit.transform == target)
            {
                return true;
            }
        }

        return false;
        
    }




    public void BeginAttack()
    {
        state = State.AttackBegin;
        navMeshAgent.isStopped = true;
        ghoulAnimator.SetTrigger("Attack");
    }
    public void EnableAttack()
    {
        state = State.Attacking;
        lastAttackedTargets.Clear();
    }
    public void DisableAttack()
    {
        state = State.Tracking;
        navMeshAgent.isStopped = false;
    }



    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            ghoulAudioPlayer.PlayOneShot(hitSound);
        }
        base.OnDamage(damage,hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        //to disable all the colliders not to interfere other AI.
        Collider[] ghoulColliders = GetComponents<Collider>();
        for(int i=0; i < ghoulColliders.Length; i++)
        {
            ghoulColliders[i].enabled = false;
        }
        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        ghoulAnimator.SetTrigger("Die");
        ghoulAudioPlayer.PlayOneShot(deathSound);
    }

    private void OnTriggerStay(Collider other)
    {
        //if the collided entity is the target, do attack
        if(!isDead && Time.time >= lastAttackTime + timeBetweenAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if(attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                Vector3 hitPoint = other.ClosestPoint(transform.position);  
                Vector3 hitNormal = transform.position - other.transform.position; ;

                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
        }
    }
}
