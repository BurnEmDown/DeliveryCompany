using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    public static Driver Instance;


    [SerializeField] private Node startNode;
    
    [SerializeField] private float moveSpeed = 0.002f;

    public bool isMoving = false;

    private IEnumerator driveToDestinationNodeEnumerator;

    private Marker startMarker;
    public Marker StartMarker => startMarker;

    private void Start()
    {
        Instance = this;

        transform.position = startNode.transform.position;
        startMarker = startNode.gameObject.AddComponent<Marker>();
        startMarker.node = startNode;
        startMarker.matchIndex = -1;
        startMarker.name = "Start Node";
        driveToDestinationNodeEnumerator = CreateDriveToDestinationNodeEnumerator();
    }

    private void Update()
    {
        if (isMoving)
            driveToDestinationNodeEnumerator.MoveNext();
    }

    public void StartMoving()
    {
        isMoving = true;
    }


    public IEnumerator CreateDriveToDestinationNodeEnumerator()
    {
        var paths = GameManager.Instance.selectedPaths;
        var pathIndex = 0;

        while (true)
        {
            var path = paths[pathIndex];
            var nodes = path.path;
            var nodeIndex = 0;

            while (nodeIndex < nodes.Count)
            {
                var node = nodes[nodeIndex];
                var destination = node.transform.position;

                if (destination.x > transform.position.x)
                    transform.localRotation = new Quaternion(0, 180, 0, 0);
                else
                {
                    transform.localRotation = new Quaternion(0, 0, 0, 0);
                }

                while (transform.position != destination)
                {
                    transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                nodeIndex++;
                yield return null;
            }

            pathIndex++;
            var isDropoff = !path.destination.isPickup;
            if (isDropoff)
            {
                GameManager.Instance.CreatePickupsAndDropoffs(1);
            }
            path.destination.gameObject.SetActive(false);

            do yield return null;
            while (pathIndex >= paths.Count);
        }
    }

}
