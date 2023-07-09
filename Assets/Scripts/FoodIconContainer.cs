using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class FoodIconContainer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> foodIconList;

        private List<GameObject> availableFoodIconList;
        private List<GameObject> recentlyChosenFoodIconList;

        private void Start()
        {
            availableFoodIconList = new List<GameObject>();
            recentlyChosenFoodIconList = new List<GameObject>();
            availableFoodIconList.AddRange(foodIconList);
        }

        public GameObject GetRandomAvailableFoodIcon()
        {
            int index = Random.Range(0, availableFoodIconList.Count);
            GameObject currentIcon = availableFoodIconList[index];
            recentlyChosenFoodIconList.Add(currentIcon);
            availableFoodIconList.RemoveAt(index);
            if (availableFoodIconList.Count <= 3)
            {
                RefreshList();
            }
            recentlyChosenFoodIconList.Add(currentIcon);
            return currentIcon;
        }

        private void RefreshList()
        {
            availableFoodIconList.AddRange(recentlyChosenFoodIconList);
            recentlyChosenFoodIconList.Clear();
        }
    }
}