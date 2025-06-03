using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health = 25f;
    public float maxHealth = 25f;

    [Header("Combat")]
    public float attackCD = 2f;
    public float attackRange = 8f;
    public float aggroRange = 15f;
    bool isAttacking;
    public float dieScores = 10f;
    [SerializeField] private ElementType elementType;
    [SerializeField] private GameObject projectileEffect;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    [Header("UI")]
    public Image Bar;
    public float fill;

    [HideInInspector] public GameObject player;
    [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    private IEnemyState currentState;

    public bool IsFleeing => hasFled;
    private bool hasFled = false;
    private PlayerStatsController statsController;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        statsController = FindObjectOfType<PlayerStatsController>();
        fill = 1f;
        ChangeState(new IdleStateRangedEnemy(this));

    }

    void Update()
    {
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
            if (!hasFled && health < maxHealth / 2f)
            {
                hasFled = true;
                ChangeState(new FleeStateRangedEnemy(this));
            }
            else if (!hasFled)
            {
                ChangeState(new IdleStateRangedEnemy(this)); 
            }

            return; 
        }

       
        if (!hasFled && health < maxHealth / 2f)
        {
            hasFled = true;
            ChangeState(new FleeStateRangedEnemy(this));
        }

        currentState?.Update();
        float distance = Vector3.Distance(transform.position, player.transform.position);
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        animator.SetTrigger("damage");
        if (health <= 0)
        {
            Die();
            ScoreSystem.Instance.RegisterKill(dieScores);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    public void FireProjectile()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead)
            return;

        if (projectilePrefab == null || projectileSpawnPoint == null)
        {
            Debug.LogWarning("Missing projectile reference!");
            return;
        }

        Vector3 direction = (player.transform.position - projectileSpawnPoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.LookRotation(direction));
        proj.GetComponent<Projectile>().Initialize(direction, elementType, projectileEffect);
    }
    public void EndAttack()
    {
        ChangeState(new IdleStateRangedEnemy(this));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}


