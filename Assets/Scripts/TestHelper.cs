using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestHelper : MonoBehaviour
{
    public Node source;
    public Node destination;
    public List<Node> way;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < way.Count() - 1; i ++)
        {
            var current = way[i];
            var next = way[i + 1];
            Gizmos.DrawLine(current.transform.position, next.transform.position);
        }

        foreach (var node in way)
        {
            if (node == source || node == destination)
                continue;

            Gizmos.DrawSphere(node.transform.position, 0.35f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(source.transform.position, 0.35f);
        Gizmos.DrawSphere(destination.transform.position, 0.35f);

    }
}
