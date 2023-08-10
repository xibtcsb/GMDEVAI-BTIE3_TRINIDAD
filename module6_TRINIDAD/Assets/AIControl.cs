using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    GameObject[] goalLocations;
    NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detectionRadius = 5;
    float fleeRadius = 10;

    public GameObject flockingTarget; 

    void ResetAgent()
    {
        speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMult", speedMult);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    void Start()
    {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0, 1));
        ResetAgent();
    }

    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }

        FlockToTarget(); 
    }

    public void DetectNewObstacle(Vector3 location)
    {
        if (Vector3.Distance(location, this.transform.position) < detectionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - location).normalized;
            Vector3 newGoal = this.transform.position + fleeDirection * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    public void FlockToNewObject(GameObject newFlockingObject)
    {
        flockingTarget = newFlockingObject;
    }

    private void FlockToTarget()
    {
        if (flockingTarget != null)
        {
            Vector3 flockingDirection = flockingTarget.transform.position - transform.position;
            agent.SetDestination(transform.position + flockingDirection);
        }
    }
}
