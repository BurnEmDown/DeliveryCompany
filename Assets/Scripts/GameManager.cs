using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public class Path
    {
        public List<Node> path;
        public Marker destination;
    }

    [SerializeField] private EmojiContainer emojiContainer;

    [SerializeField] private Button goButton;
    [SerializeField] private Button resetRouteButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TMP_Text pauseButtonText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Image pauseImage;
    private TMP_Text goButtonText;
    private Image goButtonImage;
    [SerializeField] private PickupSpawner pickupSpawner;
    [SerializeField] private DropoffSpawner dropoffSpawner;
    [SerializeField] private FoodIconContainer foodIconContainer;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Color[] colors; // 0 is red, 1 is blue, 2 is green
    public Color disabledButtonColor;
    public Color selectedButtonColor;

    [SerializeField] private GameObject pickupMarkerPrefab;
    [SerializeField] private GameObject dropoffMarkerPrefab;

    [SerializeField] private int numPickups = 3;

    [SerializeField] private int timerSeconds = 72;
    private int score = 0;

    public List<Marker> SelectedMarkers => selectedMarkers;
    public List<Path> selectedPaths;

    private bool isPaused;

    // needed for create primitive
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private List<Marker> selectedMarkers;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        goButtonText = goButton.GetComponentInChildren<TMP_Text>();
        goButtonImage = goButton.GetComponent<Image>();
    }

    private void Start()
    {
        DisableGoButton();
        SetTimerText(timerSeconds);
        CreatePickupsAndDropoffs(numPickups);
        DisableResetRouteButton();
    }

    int currentMatchIndex = 0;

    public void CreatePickupsAndDropoffs(int num)
    {
        var pickupDropoffNodesList = new List<(Node, Node)>();
        List<Node> pickups = pickupSpawner.SpawnCurrentPickupPoints(num);
        List<Node> dropoffs = dropoffSpawner.SpawnCurrentDropOffPoints(num);
        foreach (var node in pickups)
        {
            Debug.Log("pickup point: " + node.ToString());
        }

        foreach (var node in dropoffs)
        {
            Debug.Log("dropoff point: " + node.ToString());
        }

        for (int i = 0; i < num; i++)
        {
            pickupDropoffNodesList.Add((pickups[i], dropoffs[i]));
        }

        foreach (var tuple in pickupDropoffNodesList)
        {
            var pickupPosition = tuple.Item1.transform.position + Vector3.back;
            var dropoffPosition = tuple.Item2.transform.position + Vector3.back;

            GameObject pickupObject = Instantiate(pickupMarkerPrefab, pickupPosition, Quaternion.identity);
            GameObject dropoffObject = Instantiate(dropoffMarkerPrefab, dropoffPosition, Quaternion.identity);

            var pickupMarker = pickupObject.GetComponent<Marker>();
            var dropoffMarker = dropoffObject.GetComponent<Marker>();

            pickupMarker.node = tuple.Item1;
            dropoffMarker.node = tuple.Item2;

            pickupMarker.matchIndex = currentMatchIndex;
            dropoffMarker.matchIndex = currentMatchIndex;

            pickupMarker.isPickup = true;
            dropoffMarker.isPickup = false;

            GameObject foodIcon = foodIconContainer.GetRandomAvailableFoodIcon();

            GameObject foodIconPickup = Instantiate(foodIcon, pickupObject.transform);
            foodIconPickup.transform.localPosition = Vector3.back + (Vector3.up * 0.9f);
            foodIconPickup.transform.localScale *= 3f;

            GameObject foodIconDropoff = Instantiate(foodIcon, dropoffObject.transform);
            foodIconDropoff.transform.localPosition = Vector3.back + (Vector3.up * 0.65f);
            foodIconDropoff.transform.localScale *= 0.8f;

            currentMatchIndex++;
        }

        //RedrawAvailabilityColors();
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

    private void EnableResetRouteButton()
    {
        resetRouteButton.interactable = true;
    }

    private void DisableResetRouteButton()
    {
        resetRouteButton.interactable = false;
    }

    public void SetTimerText(int seconds)
    {
        int minutes = seconds / 60;
        int onlySeconds = seconds - (minutes * 60);
        string extraZero = onlySeconds < 10 ? "0" : "";

        timerText.text = minutes + ":" + extraZero + onlySeconds;
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

    public void AddMarkerToSelection(Marker marker)
    {
        if (SelectedMarkers == null)
        {
            selectedMarkers = new List<Marker>();
            selectedMarkers.Add(Driver.Instance.StartMarker);
        }

        if (selectedMarkers.Contains(marker))
            return;


        selectedMarkers.Add(marker);

        RedrawNumbersOnMarkers();
        //RedrawAvailabilityColors();

        CalculateAndAddPathToMarker(marker);
        EnableResetRouteButton();

        if (selectedMarkers.Count >= numPickups)
        {
            EnableGoButton();
        }
    }

    public void RedrawNumbersOnMarkers()
    {
        var allMarkers = FindObjectsOfType<Marker>().Where(m => m.label != null);
        foreach (var marker in allMarkers)
            marker.label.text = "";

        for (int i = 0; i < selectedMarkers.Count; i++)
        {
            var marker = selectedMarkers[i];
            if (marker.label == null) continue;

            marker.label.text = i.ToString();
        }
    }

    public void RedrawAvailabilityColors()
    {
        var allMarkers = FindObjectsOfType<Marker>();

        foreach (var marker in allMarkers)
        {
            var color = colors[marker.matchIndex];
            var isSelected = SelectedMarkers != null && SelectedMarkers.Contains(marker);
            if (isSelected)
            {
                var selectedColorStrength = disabledButtonColor.a;
                var opaqueSelectedColor = selectedButtonColor;
                opaqueSelectedColor.a = 1;
                color = Color.Lerp(color, opaqueSelectedColor, selectedColorStrength);
            }

            if (!marker.IsValidDestination())
            {
                var disableColorStrength = disabledButtonColor.a;
                var opaqueDisabledColor = disabledButtonColor;
                opaqueDisabledColor.a = 1;
                color = Color.Lerp(color, opaqueDisabledColor, disableColorStrength);
            }

            marker.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void CalculateAndAddPathToMarker(Marker marker)
    {
        if (!selectedMarkers.Any(m => m != marker))
            return;

        var sourceNode = selectedMarkers.Last(m => m != marker).node;
        var destinationNode = marker.node;

        var addedPathNodes = Node.FindPath(sourceNode, destinationNode);
        var addedPath = new Path()
        {
            destination = marker,
            path = addedPathNodes
        };

        if (selectedPaths == null)
            selectedPaths = new List<Path>();

        selectedPaths.Add(addedPath);
    }

    public void ResetRoute()
    {
        if (selectedPaths != null)
            selectedPaths.Clear();
        if (selectedMarkers != null)
            selectedMarkers.Clear();
        RedrawNumbersOnMarkers();
        //RedrawAvailabilityColors();
        DisableResetRouteButton();
        DisableGoButton();
    }

    public void OnPauseButtonClick()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseImage.gameObject.SetActive(true);
            pauseButtonText.text = "Unpause";
            resetButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
            DisableResetRouteButton();
            DisableGoButton();
        }
        else
        {
            isPaused = false;
            pauseImage.gameObject.SetActive(false);
            pauseButtonText.text = "Pause";
            resetButton.gameObject.SetActive(false);
            Time.timeScale = 1f;
            if (selectedMarkers.Count > 0)
                EnableResetRouteButton();
            if (selectedMarkers.Count >= numPickups)
                EnableGoButton();
        }

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnResetButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
