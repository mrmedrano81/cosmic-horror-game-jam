using UnityEngine;
using System;


public class WaypointDetection : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;

    public event Action<Transform> OnPlayerDetected;

    void Update()
    {
        DetectPlayerInRadius();
    }

    private void DetectPlayerInRadius()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        if (hits.Length >0 )
        {
            Debug.Log("Waypoint Detected Player, relaying to AI");
            OnPlayerDetected?.Invoke(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
