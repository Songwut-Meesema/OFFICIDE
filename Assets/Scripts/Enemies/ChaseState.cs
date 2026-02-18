using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyAI
{
    EnemyStates enemy;

    public ChaseState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateActions()
    {
        Watch();
        Chase();
    }

    void Watch()
    {
        if (!enemy.EnemySpotted())
        {
            ToAlertState();
        }
    }

    void Chase()
    {
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;

        if (enemy.navMeshAgent.remainingDistance <= enemy.attackRange && enemy.onlyMelee == true)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        }
        else if (enemy.navMeshAgent.remainingDistance <= enemy.shootRange && enemy.onlyMelee == false)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        }
    }

    public void OnTriggerEnter(Collider enemy)
    {
    }

    public void ToPatrolState()
    {
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
    }
}
