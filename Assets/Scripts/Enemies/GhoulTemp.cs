using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GhoulTemp : LivingEntity
{
    private enum State
    {
        Patrol,
        Chase,
        Attack
        // AttackBegin,        Attacking
    }

    private State state;
     
    private NavMeshAgent navMeshAgent;
    private Animator ghoulAnimator;
     
    //temp code for debug (to see the attack area and fov)
    public Transform attackRoot;
    public Transform eyeTransform;

    /*
        private AudioSource audioPlayer;
        public AudioClip chaseClip;
        public AudioClip deathClip;
     */

    public float chaseSpeed = 10f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // private float damage = 30f;
    public float attackRadius = 2f;
    public float attackDistance = 5f;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;
    public float patrolSpeed = 3f;


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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(attackRoot.position, attackRadius);
        }
        if (eyeTransform != null)
        {
            var leftEyeRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
            var leftRayDirection = leftEyeRotation * transform.forward;
            Handles.color = new Color(1, 0.92f, 0.016f, 0.3f);
            Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
        }
    }
#endif

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();        
        ghoulAnimator = GetComponent<Animator>();
        //audioPlayer = GetComponent<AudioSource>();

        
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;

        //navMeshAgent.stoppingDistance = 0.3f;
        //attackDistance = 5f;
        navMeshAgent.stoppingDistance = attackDistance;
        navMeshAgent.speed = patrolSpeed;        
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
                    Debug.Log("Enemy State : Chasing...");
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
                    Debug.Log("Enemy State : Patrol...");                    
                }

                //first, move to random position
                //Debug.Log("remainingDistance" + navMeshAgent.remainingDistance);
                if (navMeshAgent.remainingDistance <= remainingDistance)
                {
                    var patrolTargetPosition = EnemyUtility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
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
                        Debug.Log("Target Detected");
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
            if (distance <= attackDistance)
            {
                BeginAttack();
            }
        }
        //ghoulAnimator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
        ghoulAnimator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
    private void FixedUpdate()
    {
        if (state == State.Attack)
        {
            //turn smoothly to target
            var lookRotation = Quaternion.LookRotation(targetEntity.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;


            //KeepAttack();
        }

        
    }

    public void BeginAttack()
    {
        state = State.Attack;
        navMeshAgent.isStopped = true;
        ghoulAnimator.SetTrigger("Attack");


        //navMeshAgent.velocity = new Vector3(0, 0); //bad code

        isAlreadyAttacked = true;
        
        lastAttackTime = Time.time;
       // Debug.Log("State:Attacking...");
    }
    
    public void EndAttack()
    {
        state = State.Chase;
        navMeshAgent.isStopped = false;
    }


    public void KeepAttack()
    {
        //should change later
        //if the collided entity is the target, do attack
        if (!isDead && Time.time >= lastAttackTime + timeBetweenAttack)
        {
            lastAttackTime = Time.time;
            ghoulAnimator.SetTrigger("Attack");
            //Debug.Log("State:Keep Attacking...");

            //Add code here for damage calculation 
        }
    }


    /*
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Ontrigger");
        //if the collided entity is the target, do attack
        if (!isDead && Time.time >= lastAttackTime + timeBetweenAttack)
        {
            Transform attackTarget = other.transform;
            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                ghoulAnimator.SetTrigger("Attack");
                //Add code here for damage calculation 
            }
        }
    }
    */
}
