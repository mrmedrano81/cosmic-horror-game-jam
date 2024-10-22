using KinematicCharacterController;
using UnityEngine;

public class AttackState : AIStateMachine
{
    private Transform player;
    private float pauseduration = 1f;
    private float pausetimer;
    public float distancetoPlayer;

    public void SetTarget (Transform targetplayer)
    {
        player = targetplayer;
        pausetimer = 0f;
    }

    public override void EnterState(AiManager ai)
    {
        ai.spiderAnim.CrossFadeInFixedTime("Armature_SpiderAttack_Anim", 0.1f);
        Debug.Log("Entering AttackState");
        ai.IsAttackState = true;
        Debug.Log("AttackState = True");
        ai.Agent.speed = ai.attackMovementSpeed;
    }

    public override void UpdateState(AiManager ai)
    {
        Debug.Log("Entered Update of Attack");
        distancetoPlayer = CheckDistancetoPlayer(ai, player);
        if (ai.sightDetection.CanSeePlayer(out Transform detectedPlayer))
        {
            if(distancetoPlayer > ai.minDistancetoAttackPlayer)
            {
                Debug.Log("Moving to Attack Position");
                ai.Agent.SetDestination(detectedPlayer.position);
                //Put Collider code when collides = game over
                ai.spiderAnim.Play("Armature_SpiderAttack_Anim");
            }

            else if (distancetoPlayer < ai.minDistancetoAttackPlayer)
            {
                Debug.Log("Attacking Player");
                ai.Agent.speed = 0f;
                pausetimer += Time.deltaTime;
                if(pausetimer >= pauseduration)
                {
                    pausetimer = 0f;
                    ai.Agent.speed = ai.attackMovementSpeed;
                    ai.spiderAnim.Play("Armature_SpiderIdle_Anim");
                    ai.Agent.SetDestination(player.position);
                    Debug.Log("Lunging");
                }
            }
        }

        else if (!(ai.sightDetection.CanSeePlayer(out Transform player)))
        {
            Debug.Log("Lost sight of player, switching to SearchState from AttackState");
            Vector3 lastpos = ai.Agent.destination;
            ai.IsAttackState = false;
            ai.SwitchState(new SearchState(lastpos));
        }
    }

    private  float CheckDistancetoPlayer( AiManager ai,Transform player)
    {
        float distancetoPlayer = Vector3.Distance(ai.transform.position, player.transform.position);
        return distancetoPlayer;
    }
}
