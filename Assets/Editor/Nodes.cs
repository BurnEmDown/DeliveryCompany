using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Nodes : MonoBehaviour
{
    [MenuItem("Nodes/Link Selected")]
    public static void LinkSelected()
    {
        var selectedNodes = Selection.gameObjects
            .Select(go => go.GetComponent<Node>())
            .Where(n => n != null);

        foreach(var node in selectedNodes)
        {
            var newNeighbors = selectedNodes.Where(n =>
                n != node && !node.neighbors.Contains(n)
            );
            node.neighbors.AddRange(newNeighbors);
        }
    }

    [MenuItem("Nodes/Convert All To Two Way")]
    public static void ConvertAllToTwoWay()
    {
        Debug.Log("TODO: Implement");
    }
}
