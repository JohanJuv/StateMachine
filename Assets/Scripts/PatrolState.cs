using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private StatePatternEnemy enemy;

    private int nextWaypoint; // kertoo mihin waypointiin menn‰‰n. Taulukon indeksinumero. 

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("joku osui triggeriin");
        if (other.CompareTag("Player"))
        {
            // Pelaaja on kuuloalueella
            ToAlertState();
        }
    }

    public void ToAlertState()
    {
        // siirryt‰‰n alert tilaan
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        // siirryt‰‰n chase tilaan
        enemy.currentState = enemy.chaseState; 
    }

    public void ToPatrolState()
    {
        // Ei voida ajaa t‰t‰, koska ollaan jo Patrol tilassa. 
    }

    public void ToTrackingState()
    {
        enemy.currentState = enemy.trackingState;
    }

    void Look()
    {
        // Debuggis‰de visualisointia varten
        Debug.DrawRay(enemy.eye.position, enemy.eye.forward * enemy.sightRange, Color.green);

        RaycastHit hit;
        if(Physics.Raycast(enemy.eye.position, enemy.eye.forward, out hit, enemy.sightRange) && hit.collider.CompareTag("Player"))
        {
            // t‰m‰ if toteutuu vain jos s‰de osuu pelaajaan.
            // Jos s‰de osuu pelaajaan, enemy menee chase tilaan ja se tiet‰‰, ett‰ chaseTarget on kappale johon n‰kˆs‰de osuu.
            enemy.chaseTarget = hit.transform;
            ToChaseState();
        }
    }

    void Patrol()
    {
        enemy.indicator.material.color = Color.green;
        enemy.navMeshAgent.destination = enemy.waypoints[nextWaypoint].position;
        enemy.navMeshAgent.isStopped = false; 

        // Vaihdetaan waypointia kun p‰‰st‰‰n nykyiseen waypointiin. Navmeshagentissa on t‰h‰n tyˆkalut. 
        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length; // looppaa waypointit l‰pi
        }
    }


}
