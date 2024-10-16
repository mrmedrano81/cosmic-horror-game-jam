using KinematicCharacterController;
using UnityEngine;

public class AISight : MonoBehaviour
{
    // Start is called before the first frame update
    //Declare Variables

    public float viewDistance = 10.0f;
    public float viewAngle = 60f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public bool CanSeePlayer(out Transform playerTransform)
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider player in playersInRange)
        {
            Vector3 directiontoPlayer = (player.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directiontoPlayer) <= viewAngle/2)
            {
                if (!Physics.Raycast(transform.position, directiontoPlayer, viewDistance, obstacleLayer))
                {
                    playerTransform = player.transform;
                    return true;
                }
            }
        }
        playerTransform = null;
        return false;
    }
  
}
