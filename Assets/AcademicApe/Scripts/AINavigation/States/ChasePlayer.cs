using UnityEngine;

public class ChaseState : AIStateMachine
{
    // Start is called before the first frame update

    //Declaring variables
    private Transform player;
   

    public ChaseState(Transform player)
    {
        this.player = player;
    }

    public override void EnterState(AiManager ai) 
    {
        Debug.Log("Entered ChaseState");
        ai.Agent.speed = ai.chaseSpeed;
    }

    // Update is called once per frame
    public override void UpdateState(AiManager ai)
    {
        //Player still in AI Detection
        Transform detectedPlayer;

        if (ai.sightDetection.CanSeePlayer(out detectedPlayer))
        {
            Debug.Log("AI sees Player");
            ai.Agent.SetDestination(detectedPlayer.position);
            //ai.swarmBehaviour.Update();
            return;
        }

        else if (!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            Debug.Log("Player not found near waypoint, going to Search");
            Vector3 lastknowposition = ai.Agent.destination;
            ai.SwitchState(new SearchState(lastknowposition));
        }
    }
}
