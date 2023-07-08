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

        foreach (var node in selectedNodes)
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
        var allNodes = Object.FindObjectsOfType<Node>();
        foreach (var node in allNodes)
        {
            foreach (var neighbor in node.neighbors)
            {
                if (!neighbor.neighbors.Contains(node))
                    neighbor.neighbors.Add(node);
            }
        }
    }


    [MenuItem("Nodes/Test")]
    public static void Test()
    {
        var helpers = Object.FindObjectsOfType<TestHelper>();
        if (!helpers.Any())
        {
            Debug.LogWarning("Can't test without TestHelper object in scene.");
            return;
        }
        var helper = helpers.First();

        var source = helper.source;
        var destination = helper.destination;
        if (source == null || destination == null)
        {
            Debug.LogWarning("Can't test without source and target nodes in the TestHelper object.");
            return;
        }

        var way = Node.FindPath(source, destination);
        helper.way = way;
    }
}
