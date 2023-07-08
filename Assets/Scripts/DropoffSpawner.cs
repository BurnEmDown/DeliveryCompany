using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class DropoffSpawner : MonoBehaviour
    {
        [SerializeField] private List<Node> DropOffPoints;

        private List<Node> availableDropOffPoints;
        private List<Node> recentlyUsedDropOffPoints;

        private void Awake()
        {
            availableDropOffPoints = new List<Node>();
            availableDropOffPoints.AddRange(DropOffPoints);
            recentlyUsedDropOffPoints = new List<Node>();
        }

        public List<Node> SpawnCurrentDropOffPoints(int num)
        {
            int index;
            List<Node> currentPickupPointsList = new List<Node>();
            for (int i = 0; i < num; i++)
            {
                index = Random.Range(0, availableDropOffPoints.Count);
                currentPickupPointsList.Add(availableDropOffPoints[index]);
                recentlyUsedDropOffPoints.Add(availableDropOffPoints[index]);
                availableDropOffPoints.RemoveAt(index);
            }

            return currentPickupPointsList;
        }
    }
}