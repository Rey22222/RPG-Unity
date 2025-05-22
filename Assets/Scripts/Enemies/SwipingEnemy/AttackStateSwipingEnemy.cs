using UnityEngine;

public class AttackStateSwipingEnemy : IEnemyState
{
    private SwipingEnemy enemy;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    public AttackStateSwipingEnemy(SwipingEnemy enemy)
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
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        if (distance > enemy.attackRange)
        {
            enemy.agent.isStopped = false;
            enemy.ChangeState(new AgroStateSwipingEnemy(enemy));
            return;
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