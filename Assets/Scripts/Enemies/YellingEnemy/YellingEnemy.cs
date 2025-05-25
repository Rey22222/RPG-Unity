using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class YellingEnemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health = 15;
    public float maxHealth = 15;

    [Header("Combat")]
    public float attackCD = 3f;
    public float attackRange = 6f;
    public float aggroRange = 10f;
    public float dieScores = 20f;

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
        ChangeState(new IdleStateYellingEnemy(this));
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
            ChangeState(new FleeStateYellingEnemy(this));
        }

        currentState?.Update();
        Debug.Log(currentState);
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

    public void StartYell()
    {
        GetComponentInChildren<YellingEnemyDamageSystem>().StartYell();
    }
    public void EndYell()
    {
        GetComponentInChildren<YellingEnemyDamageSystem>().EndYell();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}