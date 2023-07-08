using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    public Edge[] connections = new Edge[Enum.GetNames(typeof(Enums.Directions)).Length];

    public List<Node> FindPathTo(Node destination)
    {
        // Not implemented yet
        return new List<Node>();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 5f);

        foreach (var neighbor in neighbors)
        {
            Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.35f);
    }
}
