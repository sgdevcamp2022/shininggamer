using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UIElements;

public class HexGrid : MonoBehaviourPunCallbacks
{
    public int chunkCountX = 4, chunkCountZ = 3;
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Texture2D noiseSource;
    public HexGridChunk chunkPrefab;
    public int seed;
    public List<HexUnit> unitPrefab;
    public HexCell firstPlayerLocation, secondPlayerLocation, thirdPlayerLocation;
    public int[] playerCells = new int[3];
    public HexCell[] cells;

    HexCell currentCell;
    HexGridChunk[] chunks;
    HexCellPriorityQueue searchFrontier;
    HexCell currentPathFrom, currentPathTo;
    List<HexUnit> units = new List<HexUnit>();
    HexUnit selectedUnit;
    RandomController randomController;
    Concentration concentration;

    int cellCountX, cellCountZ;
    bool currentPathExists;
    int searchFrontierPhase;

    public bool HasPath
    {
        get
        {
            return currentPathExists;
        }   
    }

    public int MoveCount
    {
        get
        {
            return moveCount;
        }
        set
        {
            moveCount = value;
        }
    }
    int moveCount;

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        HexUnit.unitPrefab = unitPrefab;
        
        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;
        randomController = FindObjectOfType<RandomController>();
        concentration = FindObjectOfType<Concentration>();

        CreateChunks();
        CreateCells();
    }

    public override void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexUnit.unitPrefab = unitPrefab;
        }
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if(!selectedUnit)
                DoSelection();
            
            if (selectedUnit)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoMove();
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    moveCount++;
                    ConcentrationChange(concentration, -1);
                }
                else
                {
                    DoPathfinding();
                }
            }
        }
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);

                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        cell.uiRect = label.rectTransform;
        cell.Elevation = 0;

        if (cell.transform.localPosition == firstPlayerLocation.transform.localPosition)
            playerCells[0] = i;
        else if (cell.transform.localPosition == secondPlayerLocation.transform.localPosition)
            playerCells[1] = i;
        else if (cell.transform.localPosition == thirdPlayerLocation.transform.localPosition)
            playerCells[2] = i;

        AddCellToChunk(x, z, cell);
    }

    void CreateCells()
    {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
                CreateCell(x, z, i++);
        }
    }

    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }

    public void AddUnit(HexUnit unit, HexCell location, float orientation, int unitType)
    {
        units.Add(unit);
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
        unit.UnitType = unitType;
    }

    public void AddUnit(HexUnit unit, HexCell location, int unitType, string characterName)
    {
        units.Add(unit);
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.UnitType = unitType;
        unit.CharacterName = characterName;
    }

    public void RemoveUnit(HexUnit unit)
    {
        units.Remove(unit);
        unit.Die();
    }

    public void ShowUI(bool visible)
    {
        for (int i = 0; i < chunks.Length; i++)
            chunks[i].ShowUI(visible);
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }

    public HexCell GetCell(HexCoordinates coordinates)
    {
        int z = coordinates.Z;
        
        if (z < 0 || z >= cellCountZ)
            return null;
        int x = coordinates.X + z / 2;
        
        if (x < 0 || x >= cellCountX)
            return null;
        
        return cells[x + z * cellCountX];
    }

    public HexCell GetCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return GetCell(hit.point);
        
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
            {
                if(current.Unit != null && !(current.Unit.CompareTag("Player")))
                {
                    if (current.Unit.tag.Substring(0, 7) == "Monster")
                        return true;
                }
                return true;
            }

            int currentTurn = (current.Distance - 1) / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);

                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                    continue;

                if (neighbor.Unit)
                {
                    if (neighbor.Unit.tag.Substring(0, 7) != "Monster")
                        continue;
                }
                    
                if (current.GetEdgeType(neighbor) == HexEdgeType.Cliff)
                    continue;
                
                int moveCost;
                moveCost = 1;
                int distance = current.Distance + moveCost;
                int turn = (distance - 1) / speed;

                if (turn > currentTurn)
                    distance = turn * speed + moveCost;

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic =
                        neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].Save(writer);
        
        writer.Write(units.Count);
        
        for (int i = 0; i < units.Count; i++)
            units[i].Save(writer);
    }

    public void Load(BinaryReader reader)
    {
        //ClearPath();
        //ClearUnits();

        for (int i = 0; i < cells.Length; i++)
            cells[i].Load(reader);

        for (int i = 0; i < chunks.Length; i++)
            chunks[i].Refresh();
        
        int unitCount = reader.ReadInt32();
        
        for (int i = 0; i < unitCount; i++)
            HexUnit.Load(reader, this);

        //GameObject tmp= GameObject.FindWithTag("Player").gameObject;
        //DontDestroyOnLoad(tmp);
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
        if(currentPathTo.Elevation == 0)
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
        else if (currentPathExists)
        {
            currentPathFrom.DisableHighlight();
            currentPathTo.DisableHighlight();
        }

        currentPathFrom = currentPathTo = null;
    }

    void ClearUnits()
    {
        for (int i = 0; i < units.Count; i++)
            units[i].Die();

        units.Clear();
    }

    bool UpdateCurrentCell()
    {
        HexCell cell = GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        try
        {
            if (cell != currentCell)
            {
                currentCell = cell;
                return true;
            }
        }
        catch(IndexOutOfRangeException e)
        {
            Debug.Log(e);
            return false;
        }

        return false;
    }

    void DoSelection()
    {
        ClearPath();
        UpdateCurrentCell();
        
        if(currentCell)
        {
            for(int i = 0; i < units.Count; i++) 
            {
                if (units[i].CompareTag("Player"))
                {
                    selectedUnit = units[i];
                }
            }
        }

        return;
    }

    void DoPathfinding()
    {
        if (UpdateCurrentCell())
        {
            if (currentCell && selectedUnit.IsValidDestination(currentCell))
            {
                FindPath(selectedUnit.Location, currentCell, 1);
            }
            else
            {
                ClearPath();
            }
        }
    }

    void DoMove()
    {
        if (HasPath)
        {
            selectedUnit.Travel(GetPath());
            ClearPath();
        }
    }
}