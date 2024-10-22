using KinematicCharacterController;
using UnityEngine;

public class AttackState : AIStateMachine
{
    private Transform player;
    private float pauseduration = 0.3f;
    private float pausetimer;
    public float distancetoPlayer;

    private bool lungedone = false;

    public void SetTarget (Transform targetplayer)
    {
        player = targetplayer;
        pausetimer = 0f;
    }

    public override void EnterState(AiManager ai)
    {
        ai.spiderAnim.Play("Armature|SpiderRun_Anim");
        //Debug.Log("Entering AttackState");
        ai.IsAttackState = true;
        //Debug.Log("AttackState = True");    
        ai.Agent.speed = ai.attackMovementSpeed;
    }

    public override void UpdateState(AiManager ai)
    {
        lungedone = false;
        //Debug.Log("Entered Update of Attack");
        if (ai.sightDetection.CanSeePlayer(out Transform detectedPlayer))
        {
            distancetoPlayer = CheckDistancetoPlayer(ai, player);
            Vector3 directiontoPlayer = (detectedPlayer.position - ai.transform.position).normalized;
            float stopDistance = 2f;
            if(distancetoPlayer > ai.minDistancetoAttackPlayer)
            {
                //Debug.Log("Moving to Attack Position");
                ai.Agent.SetDestination(detectedPlayer.position - directiontoPlayer*stopDistance);
                ai.spiderAnim.Play("Armature|SpiderAttackJump_Anim");
                pausetimer += Time.deltaTime;

                if (pausetimer >= pauseduration)
                {
                    //ai.spiderAnim.Play("Armature|SpiderAttackFull_Anim");
                    Debug.Log("Attacking Player");
                    //ai.Agent.speed = 1f;
                    //pausetimer += Time.deltaTime;
                    ai.Agent.speed = 20f;
                    pausetimer = 0f;
                    //ai.Agent.speed = ai.attackMovementSpeed;
                    //ai.Agent.SetDestination(player.position);
                    Debug.Log("Lunging");
                    
                }
            }

            /*if (distancetoPlayer < ai.minDistancetoAttackPlayer)
            {
                Debug.Log("Attacking Player");
                //ai.Agent.speed = 1f;
                pausetimer += Time.deltaTime;
                if(pausetimer >= pauseduration)
                {
                    ai.Agent.speed = 20f;
                    pausetimer = 0f;
                    ai.Agent.autoBraking = false;
                    //ai.Agent.speed = ai.attackMovementSpeed;
                    ai.spiderAnim.Play("Armature_SpiderIdle_Anim");
                    ai.Agent.SetDestination(player.position);
                    Debug.Log("Lunging");
                    ai.Agent.autoBraking = true;
                }
            } */
        }

        else if (!(ai.sightDetection.CanSeePlayer(out Transform player)))
        {
            //Debug.Log("Lost sight of player, switching to SearchState from AttackState");
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
