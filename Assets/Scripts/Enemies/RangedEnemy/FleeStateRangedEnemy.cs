using UnityEngine;
using UnityEngine.AI;

public class FleeStateRangedEnemy : IEnemyState
{
    private RangedEnemy enemy;
    private Vector3 fleeTarget;
    private float updateTimer = 0f;
    private float safeDistance = 10f;

    public FleeStateRangedEnemy(RangedEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetTrigger("flee");
        SetFleeDestination();
    }

    public void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            SetFleeDestination();
            updateTimer = 1f;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distanceToPlayer > safeDistance)
        {
            enemy.agent.isStopped = true;
            enemy.animator.SetFloat("speed", 0);
        }
    }

    public void Exit()
    {
        enemy.animator.ResetTrigger("flee");
    }

    private void SetFleeDestination()
    {
        Vector3 fleeDirection = (enemy.transform.position - enemy.player.transform.position).normalized;
        fleeTarget = enemy.transform.position + fleeDirection * 10f;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            enemy.agent.SetDestination(hit.position);
        }
    }
}
