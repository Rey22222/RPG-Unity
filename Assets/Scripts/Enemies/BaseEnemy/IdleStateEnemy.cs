using UnityEngine;

public class IdleStateEnemy : IEnemyState
{
    private Enemy enemy;

    public IdleStateEnemy(Enemy enemy) 
    { 
        this.enemy = enemy; 
    }

    public void Enter() { }

    public void Update()
    {
        if (enemy.IsFleeing)
        {
            enemy.ChangeState(new FleeState(enemy));
            return;
        }

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance <= enemy.aggroRange)
        {
            enemy.ChangeState(new AgroState(enemy));
        }
    }

    public void Exit() { }
}
