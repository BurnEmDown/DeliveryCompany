using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PickupSpawner pickupSpawner;
    [SerializeField] private DropoffSpawner dropoffSpawner;

    private void Start()
    {
        List<Node> pickups = pickupSpawner.SpawnCurrentPickupPoints(2);
        List<Node> dropoffs = dropoffSpawner.SpawnCurrentDropOffPoints(2);
        foreach (var node in pickups)
        {
            Debug.Log("pickup point: " + node.ToString());
        }
        foreach (var node in dropoffs)
        {
            Debug.Log("dropoff point: " + node.ToString());
        }
    }
}
