using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PickupSpawner pickupSpawner;

    private void Start()
    {
        List<Node> pickups = pickupSpawner.SpawnCurrentPickupPoints(2);
        foreach (var node in pickups)
        {
            Debug.Log("pickup point: " + node.ToString());
        }
    }
}
