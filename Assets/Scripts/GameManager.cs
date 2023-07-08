using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PickupSpawner pickupSpawner;
    [SerializeField] private DropoffSpawner dropoffSpawner;

    [SerializeField] private Material[] colorMaterials; // 0 is red, 1 is blue, 2 is green

    [SerializeField] private GameObject pickupMarkerPrefab;
    [SerializeField] private GameObject dropoffMarkerPrefab;
    
    [SerializeField] private int numPickups = 3;

    private List<(Node, Node)> PickupDropoffNodesList;
    
    // needed for create primitive
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        PickupDropoffNodesList = new List<(Node, Node)>();
    }

    private void Start()
    {
        List<Node> pickups = pickupSpawner.SpawnCurrentPickupPoints(numPickups);
        List<Node> dropoffs = dropoffSpawner.SpawnCurrentDropOffPoints(numPickups);
        foreach (var node in pickups)
        {
            Debug.Log("pickup point: " + node.ToString());
        }
        foreach (var node in dropoffs)
        {
            Debug.Log("dropoff point: " + node.ToString());
        }

        for (int i = 0; i < numPickups; i++)
        {
            PickupDropoffNodesList.Add((pickups[i],dropoffs[i]));
        }

        int materialIndex = 0;
        foreach (var tuple in PickupDropoffNodesList)
        {
            GameObject obj1 = Instantiate(pickupMarkerPrefab, tuple.Item1.transform.position + Vector3.back, Quaternion.identity);
            obj1.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];

            GameObject obj2 = Instantiate(dropoffMarkerPrefab, tuple.Item2.transform.position + Vector3.back, Quaternion.identity);
            obj2.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];
            
            materialIndex++;
        }
    }
}
