using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World : MonoBehaviour
{
    private static World instance = new World(); 

    private static GameObject[] hidingSpots;

    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    private World() {}
   
    public static World Instance
    {
        get { return instance; } 
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
