using System;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] float health = 15f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] GameObject ragdoll;

    [Header("Combat")]
    [SerializeField] float attackCD = 2f;
    [SerializeField] float attackRange = 8f;
    [SerializeField] float aggroRange = 15f;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;
    float timePassed;
    float newDestinationCD = 0.5f;
    bool isPlayerInRange;
    bool isAttacking;

    HealthSystem playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthSystem>();
        }
    }

    void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        if (player == null || playerHealth == null || playerHealth.IsDead)
        {
            if (isAttacking)
            {
                animator.ResetTrigger("attack");
                isAttacking = false;
            }

            agent.isStopped = true;
            animator.SetFloat("speed", 0);
            return;
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= aggroRange)
        {
            isPlayerInRange = true;

            if (distance > attackRange)
            {
                agent.isStopped = false;
                if (newDestinationCD <= 0)
                {
                    agent.SetDestination(player.transform.position);
                    newDestinationCD = 0.5f;
                }
                newDestinationCD -= Time.deltaTime;
            }
            else
            {
                agent.isStopped = true;
                transform.LookAt(player.transform);

                if (timePassed >= attackCD && !isAttacking)
                {
                    if (playerHealth != null && !playerHealth.IsDead)
                    {
                        animator.SetTrigger("attack");
                        isAttacking = true;
                        timePassed = 0;
                    }
                }
            }
        }
        else
        {
            isPlayerInRange = false;
            agent.isStopped = true;
        }

        timePassed += Time.deltaTime;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        animator.SetTrigger("damage");

        if (health <= 0)
        {
            Die();
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
        proj.GetComponent<Projectile>().Initialize(direction);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}

