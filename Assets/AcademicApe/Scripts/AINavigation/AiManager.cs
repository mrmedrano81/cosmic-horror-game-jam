using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{
    [Header ("Waypoint Parameters")]
    public float waypointTolerance = 0.6f;
    private WaypointDetection waypointdetectedplayer;
    [HideInInspector] public int currentwaypointIndex = 0;

    [Header("AI Speed Parameters")]
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 5.0f;
    public float attackMovementSpeed = 10f;
    public float minDistancetoPlayer = 2f;
    public float minDistancetoAttackPlayer = 2f;
    public float attackTrigger = 10f;
    public float aiRotationSpeed = 5f;
    [HideInInspector] public float timeinsight = 0f;

    [HideInInspector] public NavMeshAgent Agent;
    
    public AISight sightDetection;
    public List<WaypointDetection> Waypoints;
    public Animator spiderAnim;

    //AI States
    private AIStateMachine currentState;
    public PatrolState patrolState;
    public ChaseState chaseState;
    private AttackState attackState;

    [Header("AI Current State")]
    public bool IsPatrolState = false;
    public bool IsChaseState = false;
    public bool IsAttackState = false;
    public bool IsSearchState = false;


    [Header("Spider Step Interval")]
    public float chaseInterval;
    public float walkInterval;
    public float attackInterval;
    private float currentstepTime;

    public AudioSource footsteps;
    public AudioSource patrolchasesteps;
    public AudioSource lunge;
    public AudioSource idle;



    //handling navmesharea

    [Header("NavMeshSurface Set")]
    public string patrolAreaLayer = "Walkable";
    public string chaseAreaLayer = "Avoid Walls";

    [HideInInspector] public int patrolAreaMask;
    [HideInInspector] public int chaseAreaMask;


    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        sightDetection = GetComponent<AISight>();
        spiderAnim = GetComponentInChildren<Animator>();

        patrolAreaMask = 1 << NavMesh.GetAreaFromName(patrolAreaLayer);
        chaseAreaMask = 1 << NavMesh.GetAreaFromName(chaseAreaLayer);

        //Initialize States
        patrolState = new PatrolState();
        chaseState = new ChaseState();
        attackState = new AttackState();
        IsAttackState = false;


        //Debug.Log("going to first patrol state");
        currentState = patrolState;
        currentState.EnterState(this);

        StartCoroutine(TriggerIdleSoundRandomly());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        CheckForVisualonPlayer();
        
        waypointdetectedplayer = GetPlayerDetectedWaypoint();
        if (waypointdetectedplayer != null)
        {
            MovetoWaypoint(waypointdetectedplayer);
            //Debug.Log("Waypoint Detection Alert");
        }
        else
        {
            return;
        }

    }

    public void SwitchState(AIStateMachine newstate)
    {

        if(IsAttackState && !(newstate is AttackState))
        {
            //Debug.Log("SwitchState Blocked Due to AttackState");
            return;
        }
        if (currentState != newstate)
        {
            currentState = newstate;
            currentState.EnterState(this);
        }
    }

    private void CheckForVisualonPlayer()
    {
        if (sightDetection.CanSeePlayer(out Transform detectedPlayer))
        {
            chaseState.SetTarget(detectedPlayer);
            SwitchState(chaseState);
        }
    }

    public void AttackTarget(Transform player)
    {
        attackState.SetTarget(player);
        SwitchState(attackState);
    }

    public void MovetoWaypoint(WaypointDetection waypoint)
    {
        //Immediately Set Destination
        Agent.SetDestination(waypointdetectedplayer.transform.position);
        //Debug.Log($"Player detected at waypoint: {waypointdetectedplayer.name}");
    }
    public void MovetoNextWaypoint()
    {
        if(currentwaypointIndex <= Waypoints.Count)
        {
            //Debug.Log("Patrol Points not exhausted");
            currentwaypointIndex = (currentwaypointIndex + 1) % Waypoints.Count;
            Agent.SetDestination(Waypoints[currentwaypointIndex].transform.position);
            //Debug.Log($"Waypoint Index: {currentwaypointIndex}");
            //Debug.Log($"Waypoint reached. Moving to next waypoint: {Waypoints[currentwaypointIndex].name}");
        }
        else if(currentwaypointIndex > Waypoints.Count)
        {
            //Debug.Log("Patrol Points exhausted");
            currentwaypointIndex = 0;
            Agent.SetDestination(Waypoints[currentwaypointIndex].transform.position);
           //Debug.Log($"Waypoint reached. Moving to next waypoint: {Waypoints[currentwaypointIndex].name}");
        }

        
    }

    private WaypointDetection GetPlayerDetectedWaypoint()
    {
        foreach(WaypointDetection waypoint in Waypoints)
        {
            if(waypoint.IsPlayerNearby())
                return waypoint; 
        }
        return null;
    }


    public void ChaseAttackStepsAudio()
    {
        if (IsChaseState)
        {
            if (Time.time - currentstepTime > chaseInterval)
            {
                AudioManager.instance.PlaySFX(footsteps, EEnemySFX.SpiderFootstep, 0.7f, true);
                currentstepTime = Time.time;
            }
        }

        else if (IsAttackState)
        {
            if (Time.time - currentstepTime >attackInterval)
            {
                AudioManager.instance.PlaySFX(footsteps, EEnemySFX.SpiderFootstep, 0.7f, true);
                currentstepTime = Time.time;
            }
        }

    }

    public void LungeAttack()
    {
        AudioManager.instance.PlaySFX(lunge, EEnemySFX.SpiderAttack, 1f,false);
    }

    public void PatrolSearchStepsAudio()
    {
        if (IsPatrolState)
        {
            if (Time.time - currentstepTime > walkInterval)
            {
                AudioManager.instance.PlaySFX(patrolchasesteps, EEnemySFX.SpiderFootstep, 0.5f, true);
                currentstepTime = Time.time;
            }
        }
        if (IsSearchState)
        {
            if (Time.time - currentstepTime > walkInterval)
            {
                AudioManager.instance.PlaySFX(patrolchasesteps, EEnemySFX.SpiderFootstep, 0.5f, true);
                currentstepTime = Time.time;
            }
        }
    }


    private IEnumerator TriggerIdleSoundRandomly()
    {
        while (true)
        {
            float waitime = Random.Range(5f, 15f);
            yield return new WaitForSeconds(waitime);

            IdleSound();
        }
    }
    private void IdleSound()
    {
        AudioManager.instance.PlaySFX(idle, EEnemySFX.SpiderIdle, 0.7f, false);
    }
}
