using UnityEngine;

public class AttackStateRangedEnemy : IEnemyState
{
    private RangedEnemy enemy;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    public AttackStateRangedEnemy(RangedEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.agent.isStopped = true;
        isAttacking = false;
        attackTimer = 0f;
    }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeStateRangedEnemy(enemy));
            return;
        }

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (distance > enemy.attackRange)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeState(new AgroStateRangedEnemy(enemy));
            return;
        }
        if (!enemy.IsFleeing)
        {
            enemy.transform.LookAt(enemy.player.transform);
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= enemy.attackCD && !isAttacking)
        {
            enemy.animator.SetTrigger("attack");
            isAttacking = true;
            attackTimer = 0f;
        }
    }

    public void Exit()
    {
        enemy.animator.ResetTrigger("attack");
    }
}
