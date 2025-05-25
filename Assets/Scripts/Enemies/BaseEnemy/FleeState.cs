using UnityEngine;
using UnityEngine.AI;

public class FleeState : IEnemyState
{
    private Enemy enemy;
    private float updateTimer = 0f;
    private float safeDistance = 10f;

    public FleeState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = false;
        SetFleeDestination();
        updateTimer = 1f;
    }

    public void Update()
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0f)
        {
            SetFleeDestination();
            updateTimer = 1f;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distanceToPlayer > safeDistance)
        {
            enemy.agent.SetDestination(enemy.transform.position);
        }
    }

    public void Exit()
    {

    }

    private void SetFleeDestination()
    {
        Vector3 fleeDirection = (enemy.transform.position - enemy.player.transform.position).normalized;
        Vector3 targetPosition = enemy.transform.position + fleeDirection * 10f;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            enemy.agent.SetDestination(hit.position);
        }
    }
}
