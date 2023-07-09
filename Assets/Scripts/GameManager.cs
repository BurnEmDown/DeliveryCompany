using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public class Path
    {
        public List<Node> path;
        public Marker destination;
    }

    [SerializeField] private Button goButton;
    private TMP_Text goButtonText;
    private Image goButtonImage;
    [SerializeField] private PickupSpawner pickupSpawner;
    [SerializeField] private DropoffSpawner dropoffSpawner;
    [SerializeField] private FoodIconContainer foodIconContainer;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Material[] colorMaterials; // 0 is red, 1 is blue, 2 is green

    [SerializeField] private GameObject pickupMarkerPrefab;
    [SerializeField] private GameObject dropoffMarkerPrefab;
    
    [SerializeField] private int numPickups = 3;
    
    [SerializeField] private int timerSeconds = 72;
    private int score = 0;

    private List<(Node, Node)> PickupDropoffNodesList;


    public static List<Marker> selectedMarkers;
    public static List<Path> selectedPaths;

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
            GameObject pickupObject = Instantiate(pickupMarkerPrefab, tuple.Item1.transform.position + Vector3.back, Quaternion.identity);
            pickupObject.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];
            
            GameObject dropoffObject = Instantiate(dropoffMarkerPrefab, tuple.Item2.transform.position + Vector3.back, Quaternion.identity);
            dropoffObject.GetComponent<SpriteRenderer>().material = colorMaterials[materialIndex];

            GameObject foodIcon = Instantiate(foodIconContainer.GetRandomAvailableFoodIcon(), pickupObject.transform);
            
            foodIcon.transform.localPosition = Vector3.back;
            foodIcon.transform.rotation = Quaternion.identity;
            //some food icons aren't centered: IceCream and Waffle

            GameObject obj2 = Instantiate(dropoffMarkerPrefab, tuple.Item2.transform.position + Vector3.back, Quaternion.identity);
            obj2.GetComponent<SpriteRenderer>().material =
                colorMaterials[materialIndex];
            
            materialIndex++;

            var pickupMarker = pickupObject.GetComponent<Marker>();
            var dropoffMarker = dropoffObject.GetComponent<Marker>();

            pickupMarker.node = tuple.Item1;
            dropoffMarker.node = tuple.Item2;
        }
    }

    private void DisableGoButton()
    {
        goButtonText.text = "Set delivery course";
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

    public static void AddMarkerToSelection(Marker marker)
    {
        if (selectedMarkers == null)
            selectedMarkers = new List<Marker>();

        if (!selectedMarkers.Contains(marker))
            selectedMarkers.Add(marker);

        RedrawNumbersOnMarkers();

        CalculateAndAddPathToMarker(marker);
    }

    public static void RedrawNumbersOnMarkers()
    {
        var allMarkers = FindObjectsOfType<Marker>();
        foreach (var marker in allMarkers)
            marker.label.text = "";

        for (int i = 0; i < selectedMarkers.Count; i++)
        {
            var marker = selectedMarkers[i];
            marker.label.text = (i + 1).ToString();
        }
    }

    public static void CalculateAndAddPathToMarker(Marker marker)
    {
        if (!selectedMarkers.Any())
            return;

        var sourceNode = selectedMarkers.Last().node;
        var destinationNode = marker.node;

        var addedPathNodes = Node.FindPath(sourceNode, destinationNode);
        var addedPath = new Path()
        {
            destination = marker,
            path = addedPathNodes
        };

        selectedPaths.Add(addedPath);
    }
}
