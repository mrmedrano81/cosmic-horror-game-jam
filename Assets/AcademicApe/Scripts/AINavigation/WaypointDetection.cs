using UnityEngine;


public class WaypointDetection : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public float detectionCD = 120f;

    private float lastdetectionTime = -Mathf.Infinity;

    public bool IsPlayerNearby()
    {
        //Check Cooldown Timer
        if (Time.time < lastdetectionTime + detectionCD)
            return false;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (hits.Length > 0)
        {
            //Debug.Log("Waypoint Detected Player");
            lastdetectionTime = Time.time;
            return true;
        }
        //Debug.Log("Waypoint Did Not Detect Player");
        return false;
       

    }
   
    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
