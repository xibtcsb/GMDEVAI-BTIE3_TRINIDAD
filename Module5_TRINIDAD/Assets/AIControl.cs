using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIControl : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;

    
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    void Pursue(Vector3 location)
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;

        float lookAhead = targetDirection.magnitude/(agent.speed + playerMovement.currentSpeed);

        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    void Evade()
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;

        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);

        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    Vector3 wanderTarget;
    void Wander()
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f)* wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

            int hidingSpotsCount = World.Instance.GetHidingSpots().Length;

            for (int i = 0; i < hidingSpotsCount; i++)
            {
                Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
                Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5; //distance offset

                float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
                if (spotDistance < distance)
                {
                    chosenSpot = hidePosition;
                    distance = spotDistance;
                }
            }
            Seek(chosenSpot);
    }
    
    void Update()
    {
        Hide();
    }
}
