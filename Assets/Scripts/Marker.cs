using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public TextMeshPro label;
    public Node node;
    public int matchIndex;
    public bool isPickup;
    
    private int neutralFaceTime;
    private int sadFaceTime;
    private int angryFaceTime;
    private int gameOverTime;

    private float creationTime;

    private int faceIndex = 0;
    
    private void Start()
    {
        if (!isPickup)
        {
            neutralFaceTime = Random.Range(7, 12);
            sadFaceTime = Random.Range(15, 20);
            angryFaceTime = Random.Range(23, 28);
            gameOverTime = Random.Range(31, 35);

            creationTime = Time.time;
        }
    }

    private void Update()
    {
        if (!isPickup)
        {
            var timeSinceCreation = Time.time - creationTime;
            if (timeSinceCreation < neutralFaceTime)
            {
                // do nothing
            }
            else if (timeSinceCreation < sadFaceTime)
            {
                Debug.Log("trying to set neutral face " + gameObject.name);
                SetNeutralFace();
            }
            else if (timeSinceCreation < angryFaceTime)
            {
                SetSadFace();
            }
            else if (timeSinceCreation < gameOverTime)
            {
                SetAngryFace();
            }
            else if (timeSinceCreation > gameOverTime)
            {
                // gameover
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    
    private void OnMouseDown()
    {
        if (!IsValidDestination())
        {
            Debug.Log("<color=cyan>Color doesn't exist in previous pickups</color>");
            return;
        }
        GameManager.Instance.AddMarkerToSelection(this);
    }


    public bool IsValidDestination()
    {
        var selectedMarkers = GameManager.Instance.SelectedMarkers;
        var hasColorInPrevPickups = selectedMarkers != null && selectedMarkers.Any(marker => marker.isPickup && marker.matchIndex == this.matchIndex);
        var isValidDestination = isPickup || hasColorInPrevPickups;

        return isValidDestination;
    }

    private void SetNeutralFace()
    {
        if (faceIndex != 1)
        {
            faceIndex = 1;
            GetComponent<SpriteRenderer>().sprite = EmojiContainer.Instance.NeutralEmoji;
        }
    }
    
    private void SetSadFace()
    {
        if (faceIndex != 2)
        {
            faceIndex = 2;
            GetComponent<SpriteRenderer>().sprite = EmojiContainer.Instance.SadEmoji;
        }
    }
    
    private void SetAngryFace()
    {
        if (faceIndex != 3)
        {
            faceIndex = 3;
            GetComponent<SpriteRenderer>().sprite = EmojiContainer.Instance.AngryEmoji;
        }
    }
}
