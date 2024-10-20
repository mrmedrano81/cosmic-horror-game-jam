    using UnityEngine;

public class AttackState : AIStateMachine
{
    private Transform player;

    public void SetTarget (Transform targetplayer)
    {
        player = targetplayer;
    }

    public override void EnterState(AiManager ai)
    {
        Debug.Log("Entering AttackState");
        ai.IsAttackState = true;
        Debug.Log("AttackState = True");
        ai.Agent.speed = ai.attackSpeed;
    }

    public override void UpdateState(AiManager ai)
    {
        Debug.Log("Entered Update of Attack");
        if (ai.sightDetection.CanSeePlayer(out Transform detectedPlayer))
        {
            Debug.Log("Attacking Player");
            ai.Agent.SetDestination(detectedPlayer.position);
            
        }

        else if (!(ai.sightDetection.CanSeePlayer(out Transform player)))
        {
            Debug.Log("Lost sight of player, switching to SearchState from AttackState");
            Vector3 lastpos = ai.Agent.destination;
            ai.IsAttackState = false;
            ai.SwitchState(new SearchState(lastpos));
        }
    }
}
