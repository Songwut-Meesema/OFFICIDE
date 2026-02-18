using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IEnemyAI
{
    EnemyStates enemy;
    float timer;
    float patrolDuration = 2f;
    Vector3 randomPoint;

    public PatrolState(EnemyStates enemy)
    {
        this.enemy = enemy;
        randomPoint = enemy.transform.position;
    }

    public void UpdateActions()
    {
        Patrol();
        Watch();
    }

    void Patrol()
    {
        timer += Time.deltaTime;
        if (timer >= patrolDuration || Vector3.Distance(enemy.transform.position, randomPoint) < 1f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection += enemy.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
            {
                randomPoint = hit.position;
                enemy.navMeshAgent.SetDestination(randomPoint);
                enemy.navMeshAgent.isStopped = false;
            }
            timer = 0f;
        }
    }

    void Watch()
    {
        if (enemy.EnemySpotted())
        {
            ToChaseState();
        }
    }

    public void OnTriggerEnter(Collider enemy) { }

    public void ToAlertState() { enemy.currentState = enemy.alertState; }
    public void ToAttackState() { enemy.currentState = enemy.attackState; }
    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
    public void ToPatrolState() { }
}
