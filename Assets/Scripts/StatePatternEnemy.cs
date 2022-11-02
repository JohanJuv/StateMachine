using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{

    public float searchDuration; // Alert tilassa etsintäaika
    public float searchTurningSpeed; // Alert tilassa kääntymisnopeus
    public float sightRange; // Näkösäteen kantomatka. Raycast
    public Transform[] waypoints; // Patrol-tilan waypointit taulukossa. 
    public Transform eye; // Silmän sijainti. Tästä lähtee näkösäde, raycast.
    public MeshRenderer indicator; // Laatikko vihollisen päällä, muutetaan tämän väriä tilan mukaan. Debuggityökalu. 

    [HideInInspector] public Transform chaseTarget; // Tämä on pelaaja kun lähdetään jahtaamaan
    [HideInInspector] public Vector3 lastSeenTargetPos;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public TrackingState trackingState;

    [HideInInspector] public NavMeshAgent navMeshAgent; 

    private void Awake()
    {
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        chaseState = new ChaseState(this);
        trackingState = new TrackingState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = patrolState; 
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}

/*
Kotitehtävä:
Tee neljäs tila. Tilan nimi on TrackingState. Kun Enemy Chase tilassa ei enää näe pelaajaa, 
tallentaa se muistiin viimeisimmän pelaajan sijainnin "nurkan takana". 
Tämän jälkeen enemy menee TrackinStateen ja liikkuu tähän sijaintiin. Kun pääsee perille, menee Alert tilaan. 

*/