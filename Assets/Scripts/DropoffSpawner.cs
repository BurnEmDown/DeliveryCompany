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
            List<Node> currentDropoffPointsList = new List<Node>();
            for (int i = 0; i < num; i++)
            {
                index = Random.Range(0, availableDropOffPoints.Count);
                Node currentNode = availableDropOffPoints[index];
                currentDropoffPointsList.Add(currentNode);
                availableDropOffPoints.RemoveAt(index);
                if (availableDropOffPoints.Count <= 3)
                {
                    RefreshList();
                }
            }
            recentlyUsedDropOffPoints.AddRange(currentDropoffPointsList);
            return currentDropoffPointsList;
        }
        
        private void RefreshList()
        {
            availableDropOffPoints.AddRange(recentlyUsedDropOffPoints);
            recentlyUsedDropOffPoints.Clear();
        }
    }
}