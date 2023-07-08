using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;

    class Waypoint
    {
        public float distance;
        public bool visited;
        public IEnumerable<Node> way;

        public Waypoint(float distance = Mathf.Infinity, bool visited = false)
        {
            this.distance = distance;
            this.visited = visited;
        }
    }
    public static List<Node> FindPath(Node source, Node destination)
    {
        var nodesToWaypoints = new Dictionary<Node, Waypoint>();

        bool allNeighborsVisited(Node node)
        {
            var allNeighborsCalculated = node.neighbors.All(nodesToWaypoints.Keys.Contains);
            if (!allNeighborsCalculated) return false;

            var allNeighborsVisited = node.neighbors.All(n => nodesToWaypoints[n].visited);
            return allNeighborsVisited;
        }

        Node getNodeWithMinimalDistance()
        {
            var unvisitedNodes = nodesToWaypoints.Keys
                .Where(n => !nodesToWaypoints[n].visited);
            var nodeWithMinimalDistance = unvisitedNodes.First();
            foreach (var unvisited in unvisitedNodes)
            {
                var distance = nodesToWaypoints[unvisited].distance;
                var minimalDistance = nodesToWaypoints[nodeWithMinimalDistance].distance;
                if (distance < minimalDistance)
                    nodeWithMinimalDistance = unvisited;
            }
            return nodeWithMinimalDistance;
        }


        nodesToWaypoints.Add(source, new Waypoint(0) { way = new List<Node>() });
        nodesToWaypoints.Add(destination, new Waypoint());

        while (!allNeighborsVisited(destination))
        {
            var currentNode = getNodeWithMinimalDistance();
            var currentDistance = nodesToWaypoints[currentNode].distance;
            var currentWay = nodesToWaypoints[currentNode].way;
            var nextWay = currentWay.Concat(new[] { currentNode });


            foreach (var neighbor in currentNode.neighbors)
            {
                if (!nodesToWaypoints.ContainsKey(neighbor))
                    nodesToWaypoints.Add(neighbor, new Waypoint());

                var isVisited = nodesToWaypoints[neighbor].visited;
                if (isVisited)
                    continue;

                var originalDistance = nodesToWaypoints[neighbor].distance;

                var currentPosition = currentNode.transform.position;
                var neighborPosition = neighbor.transform.position;
                var wayFromCurrentToNeighbor = neighborPosition - currentPosition;
                var distanceFromCurrentToNeighbor = wayFromCurrentToNeighbor.magnitude;
                var optionalNewDistance = currentDistance + distanceFromCurrentToNeighbor;

                if (optionalNewDistance < originalDistance)
                {
                    var newNeighborWaypoint = new Waypoint(optionalNewDistance);
                    newNeighborWaypoint.way = nextWay;
                    nodesToWaypoints[neighbor] = newNeighborWaypoint;
                }
            }
            var newWaypoint = new Waypoint(currentDistance, true) { way = nodesToWaypoints[currentNode].way };
            nodesToWaypoints[currentNode] = newWaypoint;
        }

        var destinationWaypoint = nodesToWaypoints[destination];
        return destinationWaypoint.way.ToList();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.3f);

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
    public override string ToString()
    {
        return $"{transform.position.x.ToString()}" + " , " + $"{transform.position.y.ToString()}";
    }
}

