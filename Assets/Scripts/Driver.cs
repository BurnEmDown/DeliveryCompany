using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private Node currentNode;
    [SerializeField] private Node currentDestinationNode;

    public Node GetCurrentNode()
    {
        return currentNode;
    }

    public void SetDestinationNode(Node destNode)
    {
        currentDestinationNode = destNode;
    }

    public IEnumerator DriveToDestinationNodeCoroutine(float seconds)
    {
        for (float time = 0 ; time < seconds ; time += Time.deltaTime )
        {
            transform.position = Vector3.Lerp( currentNode.transform.position, currentDestinationNode.transform.position, Mathf.SmoothStep( 0, 1, time ) );
        }

        currentNode = currentDestinationNode;
        yield return null;
    }
}
