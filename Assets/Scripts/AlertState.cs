using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    private StatePatternEnemy enemy;
    float searchTimer; 

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Search();
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void ToAlertState()
    {
        // Ei voida k�ytt��, ollaan jo Alert-tilassa. 
    }

    public void ToChaseState()
    {
        searchTimer = 0;
        enemy.currentState = enemy.chaseState;
    }

    public void ToPatrolState()
    {
        searchTimer = 0;
        enemy.currentState = enemy.patrolState;

    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }

    void Look()
    {
        // Debuggis�de visualisointia varten
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            // t�m� if toteutuu vain jos s�de osuu pelaajaan.
            // Jos s�de osuu pelaajaan, enemy menee chase tilaan ja se tiet��, ett� chaseTarget on kappale johon n�k�s�de osuu.
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Search()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true; // Pyst�ytet��n vihollinen alert tilassa
        enemy.transform.Rotate(0, enemy.searchTurningSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime; 

        if(searchTimer >= enemy.searchDuration)
        {
            // Enemy on etsinyt tarpeeksi kauan ja v�syy etsimiseen, joten se palaa patrol tilaan. 
            ToPatrolState();
        }

    }


}
