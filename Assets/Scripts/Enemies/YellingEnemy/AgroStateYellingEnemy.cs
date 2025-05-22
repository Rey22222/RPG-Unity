using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AgroStateYellingEnemy : IEnemyState
{
    private YellingEnemy enemy;

    public AgroStateYellingEnemy(YellingEnemy enemy)
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
            enemy.ChangeState(new FleeStateYellingEnemy(enemy));
            return;
        }

        if (distance > enemy.aggroRange)
        {
            enemy.ChangeState(new IdleStateYellingEnemy(enemy));
            return;
        }

        if (distance > enemy.attackRange)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
        }
        else
        {
            enemy.agent.SetDestination(enemy.transform.position);
            enemy.transform.LookAt(enemy.player.transform);
            enemy.attackTimer += Time.deltaTime;

            if (enemy.attackTimer >= enemy.attackCD)
            {
                enemy.attackTimer = 0;
                enemy.ChangeState(new AttackStateYellingEnemy(enemy));
                
            }
        }
    }

    public void Exit()
    {
        enemy.attackTimer = 0;
    }
}
