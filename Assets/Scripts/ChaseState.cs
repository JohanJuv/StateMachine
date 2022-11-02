using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    private StatePatternEnemy enemy;
    float searchTimer;
    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToTrackingState()
    {
        enemy.lastSeenTargetPos = GameObject.FindWithTag("Player").transform.position;
        enemy.currentState = enemy.trackingState;
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState; 
    }

    public void ToChaseState()
    {
        // Ei voida k‰ytt‰‰, koska ollaan jo Chase statessa.
    }

    public void ToPatrolState()
    {
        // Periaatteessa, ei k‰ytet‰. Paitsi jos vihollinen saisi pelaajan kiinni ja saisi sen "hoideltua", jolloin voisi palata
        // Patrol tilaan. 
    }

    void Look()
    {

        Vector3 enemyToTarget = enemy.chaseTarget.position - enemy.eye.position; // B-A eli suuntavektori silm‰st‰ kohteeseen


        // Debuggis‰de visualisointia varten
        Debug.DrawRay(enemy.eye.position, enemyToTarget, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemyToTarget, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            // Enemyn silm‰t on pelaajassa kiinni
            enemy.chaseTarget = hit.transform;

        }
        else
        {
            // t‰m‰ else toteutuu vain silloin, jos pelaaja katoaa n‰kyvist‰, esim sein‰n taakse.
            ToTrackingState();
        }
    }

    void Chase()
    {
        enemy.indicator.material.color = Color.red;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false; 
    }


}
