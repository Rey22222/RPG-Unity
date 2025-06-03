using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BigBoss : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float health = 15f;
    public float maxHealth = 15f;

    [Header("Combat")]
    public float attackCD = 3f;
    public float attackRange = 6f;
    public float aggroRange = 10f;
    public float dieScores = 50f;

    [Header("UI")]
    public Image Bar;

    [HideInInspector] public GameObject player;
    [HideInInspector] public HealthSystem playerHealth;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;

    private IEnemyState currentState;
    private bool hasFled = false;

    public ElementType currentElement;
    public bool isPeacefulMode;
    public bool isAggroed;

    void Start()
    {
        isPeacefulMode = PlayerPrefs.GetInt("PeacefulMode", 0) == 1;
        isAggroed = !isPeacefulMode;

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentElement = (ElementType)Random.Range(0, 4);
        GetComponentInChildren<BossDamageDealer>().SetElement(currentElement);

        ChangeState(isAggroed ? new AgroStateBigBoss(this) : new IdleStateBigBoss(this));
    }

    void Update()
    {
   
        var statsController = FindObjectOfType<PlayerStatsController>();
        if (statsController != null)
        {
            isPeacefulMode = statsController.GetPeacefulMode();
        }

        if (player == null || playerHealth == null || playerHealth.IsDead)
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0);
            return;
        }

        Bar.fillAmount = health / maxHealth;
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

        if (isPeacefulMode && !isAggroed)
        {
            isAggroed = true;
            ChangeState(new AgroStateBigBoss(this));
        }

        if (health <= 0)
        {
            Die();
            ScoreSystem.Instance.RegisterKill(dieScores);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    // Для атак
    public void StartRoar() => GetComponentInChildren<BossDamageDealer>().StartRoar();
    public void EndRoar() => GetComponentInChildren<BossDamageDealer>().EndRoar();
    public void StartSwiping() => GetComponentInChildren<BossDamageDealer>().StartSwiping();
    public void EndSwiping() => GetComponentInChildren<BossDamageDealer>().EndSwiping();

    public bool IsFleeing => hasFled;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
