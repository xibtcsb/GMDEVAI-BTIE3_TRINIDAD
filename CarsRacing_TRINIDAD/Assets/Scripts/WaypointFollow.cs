using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow : MonoBehaviour
{
    //public GameObject[] waypoints;

    public UnityStandardAssets.Utility.WaypointCircuit circuit;
    int currentWaypointIndex = 0;

    float speed = 20f;
    float rotSpeed = 3f;
    float accuracy = 3f;

    void Start()
    {
        //waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
    }

    void LateUpdate()
    {
        if (circuit.Waypoints.Length == 0) return;

        GameObject currentWaypoint = circuit.Waypoints[currentWaypointIndex].gameObject;

        Vector3 lookAtGoal = new Vector3(currentWaypoint.transform.position.x,
                                         this.transform.position.y,
                                         currentWaypoint.transform.position.z);

        Vector3 direction = lookAtGoal - this.transform.position;

        if (direction.magnitude < accuracy)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= circuit.Waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

        this.transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
}
