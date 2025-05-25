using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AgroStateBigBoss : IEnemyState
{
    private BigBoss enemy;
    private float newDestinationCD = 0f;
    public bool isHit;

    public AgroStateBigBoss(BigBoss enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        isHit = false;
    }

    public void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (distance > enemy.aggroRange)
        {
            enemy.ChangeState(new IdleStateBigBoss(enemy));
            return;
        }
        if (distance > enemy.attackRange)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
        }
        else
        {
            if (enemy.health <= enemy.maxHealth/2)
            {
                enemy.attackRange = 5f;
                isHit = true;
                
            }
            if (isHit && (distance <= enemy.attackRange))
            {
                enemy.ChangeState(new SecondAttackStateBigBoss(enemy));
                return;
            }
            if (!isHit)
            {
                enemy.ChangeState(new FirstAttackStateBigBoss(enemy));
                return;
            }
            
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
