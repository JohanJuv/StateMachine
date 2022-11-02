using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{

    public float searchDuration; // Alert tilassa etsint�aika
    public float searchTurningSpeed; // Alert tilassa k��ntymisnopeus
    public float sightRange; // N�k�s�teen kantomatka. Raycast
    public Transform[] waypoints; // Patrol-tilan waypointit taulukossa. 
    public Transform eye; // Silm�n sijainti. T�st� l�htee n�k�s�de, raycast.
    public MeshRenderer indicator; // Laatikko vihollisen p��ll�, muutetaan t�m�n v�ri� tilan mukaan. Debuggity�kalu. 

    [HideInInspector] public Transform chaseTarget; // T�m� on pelaaja kun l�hdet��n jahtaamaan
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
Kotiteht�v�:
Tee nelj�s tila. Tilan nimi on TrackingState. Kun Enemy Chase tilassa ei en�� n�e pelaajaa, 
tallentaa se muistiin viimeisimm�n pelaajan sijainnin "nurkan takana". 
T�m�n j�lkeen enemy menee TrackinStateen ja liikkuu t�h�n sijaintiin. Kun p��see perille, menee Alert tilaan. 

*/