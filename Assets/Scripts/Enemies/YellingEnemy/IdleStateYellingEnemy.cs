using UnityEngine;

public class IdleStateYellingEnemy : IEnemyState
{
    private YellingEnemy enemy;

    public IdleStateYellingEnemy(YellingEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeStateYellingEnemy(enemy));
            return;
        }

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance <= enemy.aggroRange)
        {
            enemy.ChangeState(new AgroStateYellingEnemy(enemy));
        }
    }

    public void Exit() { }
}
