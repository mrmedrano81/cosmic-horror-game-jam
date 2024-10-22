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
            float distancetoPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (Vector3.Angle(transform.forward, directiontoPlayer) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directiontoPlayer, distancetoPlayer, obstacleLayer))
                {
                    playerTransform = player.transform;
                    //Debug.Log("Player Seen in SIGHT");
                    return true;
                }
            }
        }
        playerTransform = null;
        //Debug.Log("Player Not Seen in SIGHT");
        return false;
    }

    // Draw the detection radius and FOV cone in the Scene view for visualization.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);  // Detection radius.

        Vector3 leftLimit = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightLimit = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward *viewDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }

}

//if ai.checkplayerdetected() == true;
// ai.switchstate(chasestate);
