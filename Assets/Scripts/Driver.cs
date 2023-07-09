using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private Node currentNode;
    [SerializeField] private Node currentDestinationNode;
    
    [SerializeField] private float moveSpeed = 0.002f;

    private IEnumerator driveToDestinationNodeEnumerator;
    private int currentPathIndex;
    private int currentNodeIndex;

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
        driveToDestinationNodeEnumerator.MoveNext();
    }


    public IEnumerator _CreateDriveToDestinationNodeEnumerator()
    {
        currentPathIndex = 0;
        currentNodeIndex = 0;
        while()
        {
            var nextNode = currentNodeIndex++;
            while(transform.position != currentDestinationNode.transform.position)
            {
                var destinationPosition = currentDestinationNode.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            currentNode = currentDestinationNode;
            Debug.Log("reached node: " + currentNode.ToString());
            yield return null;
        }
    }


    public IEnumerator CreateDriveToDestinationNodeEnumerator()
    {





        yield return null;
    }

}
