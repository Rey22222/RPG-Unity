using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float health = 15;
    [SerializeField] private float maxHealth = 15;

    [Header("Combat")]
    [SerializeField] internal float attackCD = 3f;
    [SerializeField] internal float attackRange = 2f;
    [SerializeField] internal float aggroRange = 4f;
    [SerializeField] internal float fleeDistance = 7f;
    [SerializeField] private float dieScores = 7f;

    [Header("Flee Behavior")]
    [SerializeField] private float fleeDuration = 8f;
    private float fleeTimer = 0f;

    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject ragdoll;

    [Header("UI")]
    public Image Bar;
    public float fill;
    public bool IsFleeing => hasFled;

    private bool hasFled = false;
    private bool isAggroed = false;
    private IEnemyState currentStateFSM;

    internal GameObject player;
    internal HealthSystem playerHealth;
    internal NavMeshAgent agent;
    internal Animator animator;

    internal float attackTimer;
    private float destinationTimer;

    private EnemyState currentState = EnemyState.Idle;
    private EnemyState previousState = EnemyState.Idle;

    private Vector3 fleeTargetPosition;
    private bool hasFleeTarget = false;

    private enum EnemyState { Idle, Chase, Attack, Flee }
    private PlayerStatsController statsController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        statsController = FindObjectOfType<PlayerStatsController>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        fill = 1f;

        Debug.Log("[Enemy] Started with health: " + health);
    }

    void Update()
    {
        if (Bar != null)
            Bar.fillAmount = health / maxHealth;

        if (player == null || playerHealth == null || playerHealth.IsDead)
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0);
            return;
        }

        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);


        if (statsController != null && statsController.GetPeacefulMode())
        {
            isAggroed = false;

            if (!hasFled && health < maxHealth / 2f)
            {
                hasFled = true;
                currentState = EnemyState.Flee;
            }
            else if (!hasFled)
            {
                currentState = EnemyState.Idle;
            }

            ExecuteState();
            return;
        }

        if (!hasFled && health < maxHealth / 2f)
        {
            hasFled = true;
            currentState = EnemyState.Flee;
        }

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


        if (!isAggroed)
        {
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
    public void ChangeState(IEnemyState newState)
    {
        currentStateFSM?.Exit();
        currentStateFSM = newState;
        currentStateFSM.Enter();
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
            ScoreSystem.Instance?.RegisterKill(dieScores);
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