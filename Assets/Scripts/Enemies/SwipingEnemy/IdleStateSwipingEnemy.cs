using UnityEngine;

public class IdleStateSwipingEnemy : IEnemyState
{
    private SwipingEnemy enemy;

    public IdleStateSwipingEnemy(SwipingEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeStateSwipingEnemy(enemy));
            return;
        }

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance <= enemy.aggroRange)
        {
            enemy.ChangeState(new AgroStateSwipingEnemy(enemy));
        }
    }

    public void Exit() { }
}
