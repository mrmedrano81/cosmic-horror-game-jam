using UnityEngine;

public class ChaseState : AIStateMachine
{
    // Start is called before the first frame update

    //Declaring variables
    private Transform player;

    public void SetTarget(Transform targetplayer)
    {
        player = targetplayer;
    }

    public override void EnterState(AiManager ai) 
    {
        Debug.Log("Entered ChaseState");
        ai.timeinsight = 0f;
        //ai.Agent.areaMask = ai.GetAreaMaskforChase();
        ai.Agent.areaMask = ai.chaseAreaMask;
        ai.Agent.speed = ai.chaseSpeed;
    }

    // Update is called once per frame
    public override void UpdateState(AiManager ai)
    {
        //Player still in AI Detection
        if (player != null)
        {
            ai.timeinsight += Time.deltaTime;
            float distancetoPlayer = Vector3.Distance(ai.transform.position, player.transform.position);
            Debug.Log("AI sees Player");

            RotateTowards(ai, player.position);

            if (distancetoPlayer > ai.minDistancetoPlayer)
            {
                ai.Agent.SetDestination(player.position);
                Debug.Log("Approaching Player");
            }

            else
            {
                ai.Agent.ResetPath();
                Debug.Log("Maintaining Distance From Player");
            }

            if(ai.timeinsight >= ai.attacktrigger)
            {
                Debug.Log("Contact Time condition fulfilled, going to attack state");
                ai.AttackTarget(player);
            }
            return;
        }
     
        if (!ai.Agent.pathPending && ai.Agent.remainingDistance <= ai.waypointTolerance)
        {
            Debug.Log("Player not found near waypoint, going to Search");
            Vector3 lastknowposition = ai.Agent.destination;
            ai.SwitchState(new SearchState(lastknowposition));
        }
    }

    private void RotateTowards(AiManager ai, Vector3 targetpos)
    {
        Vector3 direction = (targetpos - ai.transform.position).normalized;
        Quaternion targetrotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = Quaternion.Slerp(ai.transform.rotation, targetrotation, ai.airotationspeed *Time.deltaTime);
    }
}
