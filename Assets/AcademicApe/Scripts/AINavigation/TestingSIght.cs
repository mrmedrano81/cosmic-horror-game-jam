using KinematicCharacterController;
using UnityEngine;

public class Sight : MonoBehaviour
{
    // Start is called before the first frame update
    //Declare Variables

    public float viewDistance = 10.0f;
    public float viewAngle = 60f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public void CanSeePlayer()
    {
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider player in playersInRange)
        {
            Vector3 directiontoPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (Vector3.Angle(transform.forward, directiontoPlayer) <= viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, directiontoPlayer,distanceToPlayer, obstacleLayer))
                {
                    Debug.Log("Can See Player");
                }
            }
        }

        Debug.Log("Can't See Player");
    }

    // Draw the detection radius and FOV cone in the Scene view for visualization.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);  // Detection radius.

        Vector3 leftLimit = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightLimit = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
    }


    private void Update()
    {
        CanSeePlayer();
    }
}
