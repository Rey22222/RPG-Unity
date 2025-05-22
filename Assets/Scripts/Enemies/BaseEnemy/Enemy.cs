using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health = 15;
    public float maxHealth = 15;

    [Header("Combat")]
    public float attackCD = 3f;
    public float attackRange = 1.5f;
    public float aggroRange = 4f;
    public float dieScores = 7f;

    [Header("UI")]
    public Image Bar;
    public float fill;
    public bool IsFleeing => hasFled;

    [HideInInspector] public GameObject player;
    [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    private IEnemyState currentState;
    public float attackTimer;
    public float destinationTimer;

    private bool hasFled = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fill = 1f;
        ChangeState(new IdleStateEnemy(this));
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
        if (!hasFled && health < maxHealth / 2f)
        {
            hasFled = true;
            ChangeState(new FleeState(this));
        }

        currentState?.Update();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
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

    public void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}