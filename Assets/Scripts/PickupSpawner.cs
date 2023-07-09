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
        availablePickupPoints = new List<Node>();
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
            Node currentNode = availablePickupPoints[index];
            currentPickupPointsList.Add(currentNode);
            availablePickupPoints.RemoveAt(index);
            if (availablePickupPoints.Count <= 3)
            {
                RefreshList();
            }
        }

        recentlyUsedPickupPoints.AddRange(currentPickupPointsList);
        return currentPickupPointsList;
    }

    private void RefreshList()
    {
        availablePickupPoints.AddRange(recentlyUsedPickupPoints);
        recentlyUsedPickupPoints.Clear();
    }
}
