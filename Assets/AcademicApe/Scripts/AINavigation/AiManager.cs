using UnityEngine;
using UnityEngine.AI;

public class AiManager : MonoBehaviour
{

    public Transform[] waypoints;
    public float waypointTolerance = 0.6f;
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 5.0f;

    [HideInInspector] public NavMeshAgent Agent;
    public AISight sightDetection;

    private int currentwaypointIndex = 0;
    private AIStateMachine currentState;

    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        foreach (Transform waypoint in waypoints)
        {
            WaypointDetection detector = waypoint.GetComponent<WaypointDetection>();
            detector.OnPlayerDetected += HandlePlayerDetectedAtWaypoint;
        }
        Debug.Log("going to first patrol state");
        currentState = new PatrolState();
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(AIStateMachine newstate)
    {
        currentState = newstate;
        currentState.EnterState(this);
    }

    private void HandlePlayerDetectedAtWaypoint(Transform waypoint)
    {
        SwitchState(new ChaseState(null, waypoint));
    }

    public void MovetoNextWaypoint()
    {
        currentwaypointIndex = (currentwaypointIndex + 1) % waypoints.Length;
        MoveToWaypoint(waypoints[currentwaypointIndex].position);
    }

    public void MoveToWaypoint(Vector3 position)
    {
        Agent.SetDestination(position);
    }


}
