using UnityEngine;

public class SearchState : AIStateMachine
{
    // Start is called before the first frame update

    private Vector3 LastKnownposition;
    private float searchDuration = 5f;
    private float searchTimer;

    //for 360 sweep
    public float rotationSpeed = 50f;
    private float totalRotation;


    public SearchState(Vector3 lastpos)
    {
        this.LastKnownposition = lastpos;
    }

    public override void EnterState(AiManager ai)
    {
        ai.Agent.SetDestination(LastKnownposition);
        ai.Agent.speed = ai.patrolSpeed;
        searchDuration = 0f;
        totalRotation = 0f;
        Debug.Log("Entering SearchState");
    }

    public override void UpdateState(AiManager ai)
    {
        //when player seen
        Transform detecedPlayer;
        if (ai.sightDetection.CanSeePlayer(out detecedPlayer))
        {
            ai.SwitchState(new ChaseState(detecedPlayer));
            return;
        }

        //increment search timer for sweep duration
        if (!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            searchTimer += Time.deltaTime;

            if (totalRotation < 360f)
            {
                float rotationStep = rotationSpeed * Time.deltaTime;
                ai.transform.Rotate(0, rotationStep, 0);
                totalRotation += rotationStep;
            }
            else if(searchTimer > searchDuration)
            {
                ai.SwitchState(new PatrolState());
                Debug.Log("No players found druing search, returning to Patrol");
            }
        }
    }
}
