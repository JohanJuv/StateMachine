using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingState : IEnemyState
{
    private StatePatternEnemy enemy;
   
    public TrackingState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Track();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;

    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }
    
    void Look()
    {
        // Debuggisäde visualisointia varten
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            // tämä if toteutuu vain jos säde osuu pelaajaan.
            // Jos säde osuu pelaajaan, enemy menee chase tilaan ja se tietää, että chaseTarget on kappale johon näkösäde osuu.
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Track()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.destination = enemy.lastSeenTargetPos; 
        enemy.navMeshAgent.isStopped = false;

        if (!enemy.navMeshAgent.pathPending)
        {
            if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
            {
                if (!enemy.navMeshAgent.hasPath || enemy.navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    ToPatrolState();
                }
            }
        }
    }


}
