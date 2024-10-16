using UnityEngine;

public class ChaseState : AIStateMachine
{
    // Start is called before the first frame update

    //Declaring variables
    private Transform player;
    private Transform targetWaypoint;
    private bool biastoWaypoint;

    public ChaseState(Transform player, Transform targetWaypoint = null)
    {
        this.player = player;
        this.targetWaypoint = targetWaypoint;
        this.biastoWaypoint = targetWaypoint != null;
    }

    public override void EnterState(AiManager ai) 
    {
        Debug.Log("Entered ChaseState");
        ai.Agent.speed = ai.chaseSpeed;

        if (biastoWaypoint)
        {
            Debug.Log("Player detected at waypoint, moving to waypoint");
            ai.MoveToWaypoint(targetWaypoint.position);
        }

        else
        {
            Debug.Log("Player in Visual, Chasing Player");
            ai.MoveToWaypoint(player.position);
        }
    }

    // Update is called once per frame
    public override void UpdateState(AiManager ai)
    {
        Transform detectedPlayer;

        if (ai.sightDetection.CanSeePlayer(out detectedPlayer))
        {
            Debug.Log("Visual confirmation of Player");
            player = detectedPlayer;
            ai.MoveToWaypoint(player.position);
        }

        else if (!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            Debug.Log("Player not found near waypoint, going back to Patrol");
            ai.SwitchState(new PatrolState());
        }
    }
}
