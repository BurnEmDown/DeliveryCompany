using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private Node currentNode;
    [SerializeField] private Node currentDestinationNode;
    
    [SerializeField] private float moveSpeed = 0.002f;

    public Node GetCurrentNode()
    {
        return currentNode;
    }

    private void Start()
    {
        transform.position = currentNode.transform.position;
        StartCoroutine(DriveToDestinationNodeCoroutine());
    }

    public void SetDestinationNode(Node destNode)
    {
        currentDestinationNode = destNode;
    }


    public IEnumerator DriveToDestinationNodeCoroutine()
    {

        while(transform.position != currentDestinationNode.transform.position) {
 
            // move driver towards the destination, never moving farther than "moveSpeed" in one frame.
            transform.position = Vector2.MoveTowards(transform.position, currentDestinationNode.transform.position, moveSpeed);
 
            // wait until next frame to continue
            yield return null;
        }

        currentNode = currentDestinationNode;
        Debug.Log("reached destination: " + currentNode.ToString());
        yield return null;
    }
}
