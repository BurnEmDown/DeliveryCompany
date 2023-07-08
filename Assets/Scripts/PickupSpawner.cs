using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private List<Node> PickupPoints;

    private List<Node> availablePickupPoints;
    private List<Node> recentlyUsedPickupPoints;

    private void Awake()
    {
        availablePickupPoints.AddRange(PickupPoints);
        recentlyUsedPickupPoints = new List<Node>();
    }

    public List<Node> SpawnCurrentPickupPoints(int num)
    {
        int index;
        List<Node> currentPickupPointsList = new List<Node>();
        for (int i = 0; i < num; i++)
        {
            index = Random.Range(0, availablePickupPoints.Count);
            currentPickupPointsList.Add(availablePickupPoints[index]);
            recentlyUsedPickupPoints.Add(availablePickupPoints[index]);
            availablePickupPoints.RemoveAt(index);
        }

        return currentPickupPointsList;
    }
}
