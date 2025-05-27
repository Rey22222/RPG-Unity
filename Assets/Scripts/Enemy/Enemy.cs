using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    public enum EnemyState { Idle, Chase, Attack, Flee }
    private EnemyState currentState = EnemyState.Idle;
    private EnemyState previousState = EnemyState.Idle;

    [SerializeField] float health = 15;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 3f;
    [SerializeField] float attackRange = 2f;
    [SerializeField] float aggroRange = 4f;
    [SerializeField] float fleeDistance = 7f;

    [Header("Flee Behavior")]
    [SerializeField] float fleeDuration = 8f;  
    private float fleeTimer = 0f;

    GameObject player;
    HealthSystem playerHealth;
    NavMeshAgent agent;
    Animator animator;
    float attackTimer;

    bool isAggroed = false;

    Vector3 fleeTargetPosition;
    bool hasFleeTarget = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();

        Debug.Log("[Enemy] Started with health: " + health);
    }

    void Update()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead)
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0);
            return;
        }

        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        UpdateState();
        ExecuteState();

        attackTimer += Time.deltaTime;
    }

    void UpdateState()
    {
        if (health <= 0)
        {
            currentState = EnemyState.Idle;
            return;
        }

        if (health < 10f)
        {
            if (currentState != EnemyState.Flee)  
            currentState = EnemyState.Flee;
            return;
        }

        if (!isAggroed)
        {
            if (currentState != EnemyState.Idle)
            currentState = EnemyState.Idle;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= aggroRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }

    void ExecuteState()
    {
        if (currentState != previousState)
        {
            OnStateEnter(currentState);
            previousState = currentState;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                hasFleeTarget = false;
                break;

            case EnemyState.Chase:
                agent.isStopped = false;
                hasFleeTarget = false;
                agent.SetDestination(player.transform.position);
                break;

            case EnemyState.Attack:
                agent.isStopped = true;
                hasFleeTarget = false;
                if (attackTimer >= attackCD)
                {
                    animator.SetTrigger("attack");
                    attackTimer = 0f;
                }
                break;

            case EnemyState.Flee:
                Flee();
                break;
        }
    }

    void OnStateEnter(EnemyState newState)
    {
        if (newState == EnemyState.Flee)
        {
            fleeTimer = 0f;

            Vector3 randomDirection = Random.insideUnitSphere * fleeDistance;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, fleeDistance, NavMesh.AllAreas))
            {
                fleeTargetPosition = hit.position;
                hasFleeTarget = true;
                agent.isStopped = false;
                agent.SetDestination(fleeTargetPosition);
            }
            else
            {
                hasFleeTarget = false;
                agent.isStopped = true;
            }
        }
    }

    void Flee()
    {
        fleeTimer += Time.deltaTime;

        if (fleeTimer >= fleeDuration)
        {
            agent.isStopped = true;
            hasFleeTarget = false;
            currentState = EnemyState.Idle;
            return;
        }

        if (!hasFleeTarget)
        {
            agent.isStopped = true;
            return;
        }

        if (agent.pathPending)
            return;

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
            hasFleeTarget = false;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("damage");

        if (health <= 0f)
        {
            Die();
            return;
        }

        isAggroed = true;

    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>()?.StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>()?.EndDealDamage();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
