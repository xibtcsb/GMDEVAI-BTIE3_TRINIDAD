using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelforwardToGoal : MonoBehaviour
{
    public Transform target;
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float deceleration = 7f;
    public float minDistance = 2f;

    private float currentSpeed = 0f;

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            Vector3 direction = targetPosition - transform.position;
            float distance = direction.magnitude;

            if (distance > minDistance)
            {
                if (currentSpeed < maxSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                    currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
                }

                Vector3 movement = direction.normalized * currentSpeed * Time.deltaTime;
                transform.Translate(movement, Space.World);
            }
            else
            {
                if (currentSpeed > 0f)
                {
                    currentSpeed -= deceleration * Time.deltaTime;
                    currentSpeed = Mathf.Max(currentSpeed, 0f);
                    Vector3 movement = direction.normalized * currentSpeed * Time.deltaTime;
                    transform.Translate(movement, Space.World);
                }
            }
        }
    }
}