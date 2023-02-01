using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour
{
    [SerializeField]
    HexCell[] neighbors;

    public HexCoordinates coordinates;
    public Color color;
    public RectTransform uiRect;

    public HexCell PathFrom { get; set; }
    public int SearchHeuristic { get; set; }
    public HexCell NextWithSamePriority { get; set; }
    public HexUnit Unit { get; set; }

    public int SearchPhase { get; set; }
    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }

    public Vector3 Position
    {
        get 
        {
            return transform.localPosition;
        }
    }

    int distance;

    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
        }
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void SetLabel(string text)
    {
        UnityEngine.UI.Text label = uiRect.GetComponent<Text>();
        label.text = text;
    }

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }
}