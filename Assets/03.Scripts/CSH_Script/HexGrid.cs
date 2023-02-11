using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int width;
    public int height;

    private int searchFrontierPhase;
    private int moveCount;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public HexUnit unitPrefab;
    public BattleWindow battlewindowPrefab;

    HexCell[] cells;
    HexCell currentCell;
    HexMesh hexMesh;
    HexCellPriorityQueue searchFrontier;
    HexCell currentPathFrom, currentPathTo;
    HexUnit selectedUnit;
    RandomController randomController;
    Canvas gridCanvas;
    List<HexUnit> units = new List<HexUnit>();
    Concentration concentration;

    bool currentPathExists;

    public List<HexCell> GetPath()
    {
        if (!currentPathExists)
            return null;

        List<HexCell> path = ListPool<HexCell>.Get();

        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
            path.Add(c);

        path.Add(currentPathFrom);
        path.Reverse();

        return path;
    }

    void Awake()
    {
        cells = new HexCell[height * width];
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        randomController = GetComponentInChildren<RandomController>();
        concentration = GetComponentInChildren<Concentration>();

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

        CreateUnit();
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
        moveCount = randomController.OnRandomPositionNumberClick();
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x > 0)
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);

                if (x > 0)
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);

                if (x < width - 1)
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        cell.uiRect = label.rectTransform;
    }

    void CreateUnit()
    {
        HexCell cell = transform.GetChild(3).GetComponent<HexCell>();

        if (cell && !cell.Unit)
            AddUnit(Instantiate(unitPrefab), cell, 180f);
    }

    public void AddUnit(HexUnit unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(HexUnit unit)
    {
        units.Remove(unit);
        unit.Die();
    }

    void Update()
    {
        if(!selectedUnit)
            DoSelection();

        else if(selectedUnit)
        {
            if (Input.GetMouseButtonDown(0))
                DoMove();
            else if (Input.GetMouseButtonDown(1))
            {
                moveCount++;
                ConcentrationChange(concentration, -1);
            }
            else
                DoPathfinding();
        }
    }

    HexCell UnitCursor()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 position;

        if (Physics.Raycast(inputRay, out hit))
        {
            position = hit.point;
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
            HexCell cell = cells[index];

            return cell;
        }

        return null;
    }

    public void FindPath(HexCell fromCell, HexCell toCell, int speed)
    {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell, speed);
        ShowPath(speed);
    }

    bool Search(HexCell fromCell, HexCell toCell, int speed)
    {
        searchFrontierPhase += 2;

        if (searchFrontier == null)
            searchFrontier = new HexCellPriorityQueue();
        else
            searchFrontier.Clear();

        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);

        while (searchFrontier.Count > 0)
        {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
                return true;

            int currentTurn = (current.Distance - 1) / speed;
            int moveCost;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                moveCost = 1;
                int distance = current.Distance + moveCost;
                int turn = (distance - 1) / speed;

                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                    continue;

                if (turn > currentTurn)
                    distance = turn * speed + moveCost;

                if (neighbor.Unit)
                    continue;

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
            }
        }

        return false;
    }

    void ShowPath(int speed)
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;

            while (current != currentPathFrom)
            {
                int turn = current.Distance / speed;

                if (turn <= moveCount)
                {
                    current.SetLabel(turn.ToString());
                    current.EnableHighlight(Color.white);
                    current = current.PathFrom;
                }
                else
                    return;
            }
        }

        currentPathFrom.EnableHighlight(Color.blue);
        currentPathTo.EnableHighlight(Color.red);
    }

    void ClearPath()
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;

            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.DisableHighlight();
                current = current.PathFrom;
            }

            current.DisableHighlight();
            currentPathExists = false;
        }
        else if (currentPathFrom)
        {
            currentPathFrom.DisableHighlight();
            currentPathTo.DisableHighlight();
        }

        currentPathFrom = currentPathTo = null;
    }

    bool UpdateCurrentCell()
    {
        HexCell cell = UnitCursor();

        if(cell != currentCell)
        {
            currentCell = cell;
            return true;
        }

        return false;
    }

    void DoSelection()
    {
        ClearPath();
        currentCell = units[0].Location;

        if (currentCell)
            selectedUnit = currentCell.Unit;
    }

    void DoPathfinding()
    {
        if(UpdateCurrentCell())
        {
            if (currentCell)
                FindPath(selectedUnit.Location, currentCell, 1);
            else
                ClearPath();
        }
    }

    void DoMove()
    {
        if(currentPathExists)
        {
            selectedUnit.Travel(GetPath());
            ClearPath();
        }
    }

    public void ConcentrationChange(Concentration concentration, int concentChange)
    {
        if (concentration.totalConcentration <= 5 && concentration.totalConcentration >= 0)
        {
            concentration.totalConcentration += concentChange;
            concentration.ConcentImageChange();
        }
        else
            return;
    }
 }