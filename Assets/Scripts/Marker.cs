using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public TextMeshPro label;
    public Node node;

    private void OnMouseDown() => GameManager.AddMarkerToSelection(this);

}
