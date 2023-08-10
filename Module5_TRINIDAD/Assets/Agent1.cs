using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent1 : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;

    private enum AIState
    {
        Wander,
        Pursue
    }

    private AIState currentState = AIState.Wander;
    private float pursueRange = 10f; // Adjust this range as needed

    private Vector3 wanderTarget;
    private float wanderRadius = 20f;
    private float wanderDistance = 10f;
    private float wanderJitter = 1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        switch (currentState)
        {
            case AIState.Wander:
                if (distanceToTarget <= pursueRange)
                {
                    currentState = AIState.Pursue;
                    break;
                }

                Wander();
                break;

            case AIState.Pursue:
                if (distanceToTarget > pursueRange)
                {
                    currentState = AIState.Wander;
                    break;
                }

                Pursue(target.transform.position);
                break;
        }
    }

    private void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = transform.TransformVector(targetLocal);

        Seek(targetWorld);
    }

    private void Pursue(Vector3 location)
    {
        Vector3 targetDirection = location - transform.position;

        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);

        Seek(location + targetDirection.normalized * lookAhead);
    }

    private void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }
}
