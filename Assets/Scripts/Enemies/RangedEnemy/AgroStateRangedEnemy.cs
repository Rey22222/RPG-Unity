using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AgroStateRangedEnemy : IEnemyState
{
    private RangedEnemy enemy;
    private float newDestinationCD = 0f;

    public AgroStateRangedEnemy(RangedEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = false;
    }

    public void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeStateRangedEnemy(enemy));
            return;
        }

        if (distance > enemy.aggroRange)
        {
            enemy.ChangeState(new IdleStateRangedEnemy(enemy));
            return;
        }

        if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(new AttackStateRangedEnemy(enemy));
            return;
        }

        newDestinationCD -= Time.deltaTime;
        if (newDestinationCD <= 0)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
            newDestinationCD = 0.5f;
        }

        enemy.animator.SetFloat("speed", enemy.agent.velocity.magnitude / enemy.agent.speed);
    }

    public void Exit()
    {
        enemy.agent.isStopped = true;
        enemy.animator.SetFloat("speed", 0);
    }
}

