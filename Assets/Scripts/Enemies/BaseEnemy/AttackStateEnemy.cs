using UnityEngine;

public class AttackStateEnemy : IEnemyState
{
    private Enemy enemy;

    public AttackStateEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.attackTimer = 0;
    }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeState(enemy));
            return;
        }

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance > enemy.attackRange)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeState(new AgroState(enemy));
            return;
        }
        if (!enemy.IsFleeing)
        {
            enemy.transform.LookAt(enemy.player.transform);
        }
        enemy.attackTimer += Time.deltaTime;
        if (enemy.attackTimer >= enemy.attackCD)
        {
            enemy.animator.SetTrigger("attack");
            enemy.attackTimer = 0;
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
