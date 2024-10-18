using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwarmBehaviour : MonoBehaviour
{
    public List<Transform> swarmbuddies;
    public float spacingdistance = 2f;
    public float cohesionweight = 1f;
    public float spacingweight = 2f;
    public float alignmentweight = 1.5f;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        Vector3 spacing = CalculateSpacing();
        Vector3 alignment = CalculateAlignment();
        Vector3 cohesion = CalculateCohesion();

        //Swarm Behavior Movement

        Vector3 swarmmovement = spacing * spacingweight + alignment * alignmentweight + cohesion * cohesionweight;
        Vector3 newTarget = agent.destination + swarmmovement;

        agent.SetDestination(newTarget);
    }

    private Vector3 CalculateSpacing()
    {
        Vector3 spacingforce = Vector3.zero;
        int neighbours = 0;

        foreach (Transform buddies in swarmbuddies)
        {
            float distance = Vector3.Distance(transform.position, buddies.position);
            if (distance > 0 && distance < spacingdistance)
            {
                //separate swarm
                spacingforce += (transform.position - buddies.position).normalized;
                neighbours++;
            }
        }
        if (neighbours > 0)
        {
            spacingforce /= neighbours;
        }
        return spacingforce;
    }

    private Vector3 CalculateAlignment()
    {
        Vector3 alignmentForce = Vector3.zero;
        int neighbours = 0;

        foreach (Transform buddies in swarmbuddies)
        {
            if(buddies != null)
            {
                alignmentForce += buddies.forward;
                neighbours++;
            }
        }

        if(neighbours > 0)
        {
            alignmentForce /= neighbours;
        }
        return alignmentForce;
    }

    private Vector3 CalculateCohesion()
    {
        Vector3 cohesionForce = Vector3.zero;
        int neighbours = 0;

        foreach(Transform buddies in swarmbuddies)
        {
            if (buddies != null)
            {
                cohesionForce += buddies.position;
                neighbours++;
            }
        }
        if (neighbours > 0)
        {
            cohesionForce /= neighbours;
            cohesionForce = (cohesionForce - transform.position).normalized;
        }
        return cohesionForce;
    }
}
