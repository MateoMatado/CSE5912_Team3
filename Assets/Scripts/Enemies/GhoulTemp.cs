using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GhoulTemp : LivingEntity
{
    private enum State
    {
        Patrol,
        Chase,
        Attack
    }

    private State state;

    //public LivingEntity targetEntity;
    //temp code. should use upper one (livingEntity)
    public Transform targetEntity;

    private NavMeshAgent navMeshAgent;
    private Animator ghoulAnimator;
    public LayerMask whatIsTarget; //chase the given layer object

    //temp for debug
    public Transform attackRoot;
    public Transform eyeTransform;
    public float attackRadius = 2f;
    public float fieldOfView = 50f;
    public float viewDistance = 10f;

    public float patrolSpeed = 10f;
    public float chaseSpeed = 20f;

    public float attackDistance;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
        
    [SerializeField] float timeBetweenAttack = 0.5f;
    private float lastAttackTime;

    private bool isAlreadyAttacked = false;

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

        /*
        var attackPivot = attackRoot.position;
        attackPivot.y = transform.position.y;
        attackDistance = Vector3.Distance(transform.position, attackPivot) + attackRadius;
        navMeshAgent.stoppingDistance = attackDistance;
        */
        attackDistance = 5f;
        navMeshAgent.stoppingDistance = 0.3f;
        
        navMeshAgent.speed = patrolSpeed;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        if (state == State.Chase)
        {
            var distance = Vector3.Distance(targetEntity.transform.position, transform.position);
            if (distance <= attackDistance)
            {
                BeginAttack();
            }
        }
        ghoulAnimator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
    }
    private void FixedUpdate()
    {
        if(state == State.Attack)
        {
            //turn smoothly to target
            var lookRotation = Quaternion.LookRotation(targetEntity.transform.position - transform.position);
            var targetAngleY = lookRotation.eulerAngles.y;

            targetAngleY = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY, ref turnSmoothVelocity, turnSmoothTime);
            transform.eulerAngles = Vector3.up * targetAngleY;


            KeepAttack();
        }
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
                    Debug.Log("State:Chasing...");
                    navMeshAgent.speed = chaseSpeed;
                }
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            // if no target, then patrol
            else
            {
                Debug.Log("No target...");
                if (targetEntity != null)
                {
                    targetEntity = null;
                }
                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    Debug.Log("State:Patrol...");
                    navMeshAgent.speed = patrolSpeed;
                }

                //first, move to random position
                //Debug.Log("remainingDistance" + navMeshAgent.remainingDistance);
                if (navMeshAgent.remainingDistance <= 10f)
                {
                    var patrolTargetPosition = EnemyUtility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    navMeshAgent.SetDestination(patrolTargetPosition);
                }

                //Then, check nearby object whether it is target(player) by checking colliders nearby
                var colliders = Physics.OverlapSphere(eyeTransform.position, viewDistance, whatIsTarget);
                foreach (var collider in colliders)
                {
                    if (IsTargetOnSight(collider.transform))
                    {
                        Debug.Log("Target Detected");
                        targetEntity = collider.transform;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
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

        RaycastHit hit;
        direction = target.position - eyeTransform.position;
        if (Physics.Raycast(eyeTransform.position, direction, out hit, viewDistance, whatIsTarget))
        {
            if (hit.transform == target)
            {                
                return true;
            }
        }

        return false;

    }

    public void BeginAttack()
    {
        state = State.Attack;
        navMeshAgent.velocity = new Vector3(0, 0); //bad code
        navMeshAgent.isStopped = true;
        isAlreadyAttacked = true;
        ghoulAnimator.SetTrigger("Attack");
        lastAttackTime = Time.time;
        Debug.Log("State:Attacking...");
    }

    public void KeepAttack()
    {
        //should change later
        //if the collided entity is the target, do attack
        if (!isDead && Time.time >= lastAttackTime + timeBetweenAttack)
        {
            lastAttackTime = Time.time;
            ghoulAnimator.SetTrigger("Attack");
            Debug.Log("State:Keep Attacking...");

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
