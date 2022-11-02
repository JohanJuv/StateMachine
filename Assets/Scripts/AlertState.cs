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
        // Ei voida käyttää, ollaan jo Alert-tilassa. 
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

    void Search()
    {
        enemy.indicator.material.color = Color.yellow;
        enemy.navMeshAgent.isStopped = true; // Pystäytetään vihollinen alert tilassa
        enemy.transform.Rotate(0, enemy.searchTurningSpeed * Time.deltaTime, 0);
        searchTimer += Time.deltaTime; 

        if(searchTimer >= enemy.searchDuration)
        {
            // Enemy on etsinyt tarpeeksi kauan ja väsyy etsimiseen, joten se palaa patrol tilaan. 
            ToPatrolState();
        }

    }


}
