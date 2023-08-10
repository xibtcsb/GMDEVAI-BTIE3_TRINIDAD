using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent3 : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;

    float distanceThreshold = 15f; // Adjust this threshold as needed
    bool isEvading = false;

    Vector3 wanderTarget;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - transform.position;
        agent.SetDestination(transform.position - fleeDirection);
    }

    void Wander()
    {
        float wanderRadius = 20f;
        float wanderDistance = 10f;
        float wanderJitter = 1f;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= distanceThreshold && !isEvading)
        {
            Evade();
            isEvading = true;
        }
        else if (distanceToTarget > distanceThreshold && isEvading)
        {
            isEvading = false;
        }

        if (isEvading)
        {
            Evade();
        }
        else
        {
            Wander();
        }
    }
    
    void Evade()
    {
        Vector3 targetDirection = transform.position - target.transform.position;

        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }
}
