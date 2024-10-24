using UnityEngine;

public class PatrolState : AIStateMachine
{
    // Start is called before the first frame update
    public override void EnterState(AiManager ai)
    {
        //Debug.Log("Entered Patrol State");
        //ai.Agent.areaMask = ai.GetAreaMaskforPatrol();
        ai.spiderAnim.CrossFadeInFixedTime("Armature_SpiderWalk_Anim", 0.1f);
        ai.Agent.areaMask = ai.patrolAreaMask;
        ai.Agent.speed = ai.patrolSpeed;
        if (!ai.Agent.pathPending || ai.Agent.remainingDistance < ai.waypointTolerance)
        {
            ai.Agent.SetDestination(ai.Waypoints[ai.currentwaypointIndex].transform.position);
        }

        //Update State bools
        ai.IsChaseState = false;
        ai.IsSearchState = false;
        ai.IsPatrolState = true;
    }

    // Update is called once per frame
    public override void UpdateState(AiManager ai)
    {

       ai.PatrolSearchStepsAudio();

       if(!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            //Debug.Log("Reached Waypoint, going to next waypoint");
            ai.MovetoNextWaypoint();
        }
        //ai.swarmBehaviour.Update();
    }
}
