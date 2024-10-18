using UnityEngine;

public class PatrolState : AIStateMachine
{
    // Start is called before the first frame update
    public override void EnterState(AiManager ai)
    {
        Debug.Log("Entered Patrol State");
        ai.Agent.speed = ai.patrolSpeed;
        ai.MovetoNextWaypoint();
    }

    // Update is called once per frame
    public override void UpdateState(AiManager ai)
    {
       if(!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            Debug.Log("Reached Waypoint, going to next waypoint");
            ai.MovetoNextWaypoint();
        }
        //ai.swarmBehaviour.Update();
    }
}
