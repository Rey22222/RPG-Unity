using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBigBoss : IEnemyState
{
    private BigBoss enemy;

    public IdleStateBigBoss(BigBoss enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance <= enemy.aggroRange)
        {
            enemy.ChangeState(new AgroStateBigBoss(enemy));
        }
    }

    public void Exit() { }
}