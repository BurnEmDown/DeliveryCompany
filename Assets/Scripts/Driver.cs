using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private Node currentNode;
    [SerializeField] private Node currentDestinationNode;
    
    [SerializeField] private float moveSpeed = 0.002f;

    public bool isMoving = false;

    private IEnumerator driveToDestinationNodeEnumerator;

    public Node GetCurrentNode()
    {
        return currentNode;
    }

    private void Start()
    {
        transform.position = currentNode.transform.position;
        driveToDestinationNodeEnumerator = CreateDriveToDestinationNodeEnumerator();
    }

    public void SetDestinationNode(Node destNode)
    {
        currentDestinationNode = destNode;
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

        while (pathIndex < paths.Count)
        {
            var path = paths[pathIndex];
            var nodes = path.path;
            var nodeIndex = 0;

            while (nodeIndex < nodes.Count)
            {
                var node = nodes[nodeIndex];
                var destination = node.transform.position;

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
            yield return null;
        }

        isMoving = false;
        yield return null;
    }

}
