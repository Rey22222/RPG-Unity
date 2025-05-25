using UnityEngine;

public class AttackStateYellingEnemy : IEnemyState
{
    private YellingEnemy enemy;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    public AttackStateYellingEnemy(YellingEnemy enemy)
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
            Debug.Log(distance);
            enemy.agent.isStopped = false;
            enemy.ChangeState(new AgroStateYellingEnemy(enemy));
            return;
        }
        attackTimer += Time.deltaTime;
        if (attackTimer >= enemy.attackCD && !isAttacking)
        {
            Debug.Log(11111111111111111);
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
