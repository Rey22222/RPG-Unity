using UnityEngine;

public class IdleStateRangedEnemy : IEnemyState
{
    private RangedEnemy enemy;

    public IdleStateRangedEnemy(RangedEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeStateRangedEnemy(enemy));
            return;
        }
        float dist = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (dist <= enemy.aggroRange && !enemy.IsFleeing)
        {
            enemy.ChangeState(new AgroStateRangedEnemy(enemy));
        }
    }

    public void Exit() { }
}
