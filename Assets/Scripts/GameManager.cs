using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button goButton;
    private TMP_Text goButtonText;
    private Image goButtonImage;
    [SerializeField] private PickupSpawner pickupSpawner;
    [SerializeField] private DropoffSpawner dropoffSpawner;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Material[] colorMaterials; // 0 is red, 1 is blue, 2 is green

    [SerializeField] private GameObject pickupMarkerPrefab;
    [SerializeField] private GameObject dropoffMarkerPrefab;
    
    [SerializeField] private int numPickups = 3;
    
    [SerializeField] private int timerSeconds = 72;
    private int score = 0;

    private List<(Node, Node)> PickupDropoffNodesList;
    
    // needed for create primitive
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        PickupDropoffNodesList = new List<(Node, Node)>();
        goButtonText = goButton.GetComponentInChildren<TMP_Text>();
        goButtonImage = goButton.GetComponent<Image>();
    }

    private void Start()
    {
        DisableGoButton();
        SetTimerText(timerSeconds);
        List<Node> pickups = pickupSpawner.SpawnCurrentPickupPoints(numPickups);
        List<Node> dropoffs = dropoffSpawner.SpawnCurrentDropOffPoints(numPickups);
        foreach (var node in pickups)
        {
            Debug.Log("pickup point: " + node.ToString());
        }
        foreach (var node in dropoffs)
        {
            Debug.Log("dropoff point: " + node.ToString());
        }

        for (int i = 0; i < numPickups; i++)
        {
            PickupDropoffNodesList.Add((pickups[i],dropoffs[i]));
        }

        int materialIndex = 0;
        foreach (var tuple in PickupDropoffNodesList)
        {
            GameObject obj1 = Instantiate(pickupMarkerPrefab, tuple.Item1.transform.position + Vector3.back, Quaternion.identity);
            obj1.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];

            GameObject obj2 = Instantiate(dropoffMarkerPrefab, tuple.Item2.transform.position + Vector3.back, Quaternion.identity);
            obj2.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];
            
            materialIndex++;
        }
    }

    private void DisableGoButton()
    {
        goButtonText.text = "Set complete delivery course";
        goButtonText.fontSize = 18;
        goButton.interactable = false;
        var tempColor = goButtonImage.color;
        tempColor.a = 0.5f;
        goButtonImage.color = tempColor;
    }

    private void EnableGoButton()
    {
        goButtonText.text = "Go";
        goButtonText.fontSize = 22;
        goButton.interactable = true;
        var tempColor = goButtonImage.color;
        tempColor.a = 1f;
        goButtonImage.color = tempColor;
    }

    public void SetTimerText(int seconds)
    {
        int minutes = seconds / 60;
        int onlySeconds = seconds - (minutes * 60);
        string extraZero = onlySeconds < 10 ? "0" : "";
        
        timerText.text = $"{minutes}" + ":" + extraZero + $"{onlySeconds}";
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
