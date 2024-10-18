using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{

    public float waypointTolerance = 0.6f;
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 5.0f;

    [HideInInspector] public NavMeshAgent Agent;
    public AISight sightDetection;
    //public SwarmBehaviour swarmBehaviour;
    public List<WaypointDetection> Waypoints;

    private int currentwaypointIndex = 0;
    private AIStateMachine currentState;
    private WaypointDetection waypointdetectedplayer;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        //swarmBehaviour = GetComponent<SwarmBehaviour>();
        sightDetection = GetComponent<AISight>();


        Debug.Log("going to first patrol state");
        currentState = new PatrolState();
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        Agent.updateUpAxis = false;
        currentState.UpdateState(this);
        CheckForVisualonPlayer();
        //swarmBehaviour.Update();
        waypointdetectedplayer = GetPlayerDetectedWaypoint();
        if (waypointdetectedplayer != null)
        {
            MovetoWaypoint(waypointdetectedplayer);
            Debug.Log("Waypoint Detection Alert");
        }
        else
        {
            return;
        }

    }

    public void SwitchState(AIStateMachine newstate)
    {
        currentState = newstate;
        currentState.EnterState(this);
    }

    private void CheckForVisualonPlayer()
    {
        Transform detectedPlayer;
        if(sightDetection.CanSeePlayer(out detectedPlayer))
        {
            Debug.Log("Player sighted! Switching to ChaseState");
            SwitchState(new ChaseState(detectedPlayer));

            //bool aisees = true;
        }
    }
    
    public void MovetoWaypoint(WaypointDetection waypoint)
    {
        //Immediately Set Destination
        Agent.SetDestination(waypointdetectedplayer.transform.position);
        Debug.Log($"Player detected at waypoint: {waypointdetectedplayer.name}");
    }
    public void MovetoNextWaypoint()
    {
            currentwaypointIndex = (currentwaypointIndex + 1) % Waypoints.Count;
            Agent.SetDestination(Waypoints[currentwaypointIndex].transform.position);
            Debug.Log($"Waypoint reached. Moving to next waypoint: {Waypoints[currentwaypointIndex].name}");
        
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


}
