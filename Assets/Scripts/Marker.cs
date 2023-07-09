using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public TextMeshPro label;
    public Node node;
    public int matchIndex;
    public bool isPickup;

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
}
