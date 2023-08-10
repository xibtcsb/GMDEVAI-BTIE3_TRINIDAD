using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject flockingObject; 
    GameObject[] agents;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                GameObject newObstacle = Instantiate(obstacle, hit.point, obstacle.transform.rotation);
                foreach (GameObject a in agents)
                {
                    a.GetComponent<AIControl>().DetectNewObstacle(hit.point);
                }

     
                Destroy(newObstacle, 5f);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                GameObject newFlockingObject = Instantiate(flockingObject, hit.point, Quaternion.identity);
                foreach (GameObject a in agents)
                {
                    a.GetComponent<AIControl>().FlockToNewObject(newFlockingObject);
                }

             
                Destroy(newFlockingObject, 5f);
            }
        }
    }
}
