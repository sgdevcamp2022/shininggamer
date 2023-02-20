using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
//using static UnityEditor.FilePathAttribute;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEditor;

public class HexMapEditor : MonoBehaviourPunCallbacks
{
    public HexGrid hexGrid;
    public PhotonView pv;
    
    private int activeTerrainTypeIndex;
    private int activeElevation;

    bool applyElevation = true;
    int brushSize;
    int unitType;

    HexCell previousCell;

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index;
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    public void SetApplyElevation(bool toggle)
    {
        applyElevation = toggle;
    }

    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    public void SetUnit(float type)
    {
        unitType = (int)type;
    }

    void Start()
    {
        Load();    
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    DestroyUnit();
                else
                    CreateUnit();
                
                return;
            }
        }
        previousCell = null;
    }

    void HandleInput()
    {
        HexCell currentCell = GetCellUnderCursor();
        if(currentCell)
        {
            EditCells(currentCell);
            previousCell = currentCell;
        }
        else
        {
            previousCell = null;
        }
    }

    void EditCell(HexCell cell)
    {
        if(cell)
        {
            if(activeTerrainTypeIndex >= 0 )
                cell.TerrainTypeIndex = activeTerrainTypeIndex;
            
            if(applyElevation)
                cell.Elevation = activeElevation;
        }
    }

    void EditCells(HexCell center)
    {
        int centerX = center.coordinates.X;
        int centerZ = center.coordinates.Z;

        for(int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
        }

        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
    }

    HexCell GetCellUnderCursor()
    {
        return hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void CreateUnit()
    {
        HexCell cell = GetCellUnderCursor();

        if (cell && !cell.Unit)
        {
            hexGrid.AddUnit(Instantiate(HexUnit.unitPrefab[unitType - 1]), cell, Random.Range(0f, 360f), unitType - 1);
        }
    }

    void DestroyUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && cell.Unit)
            hexGrid.RemoveUnit(cell.Unit);
    }

    public void Save()
    {
        //pv.RPC("OnSave", RpcTarget.All);
        string path = Path.Combine(Application.persistentDataPath, "test.map");

        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            hexGrid.Save(writer);
        }
    }

    [PunRPC]
    void OnSave()
    {

    }

    public void Load()
    {
        //pv.RPC("OnLoad", RpcTarget.All);

        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            hexGrid.Load(reader);
        }
    }

    [PunRPC]
    private void OnLoad()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            hexGrid.Load(reader);
        }
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");
        Vector3[] position = new Vector3[3];
        Quaternion[] quaternion = new Quaternion[3];
        position[0] = new Vector3(303.1089f, -0.5422111f, 15);
        position[1] = new Vector3(285.7884f, 0.4182683f, 15);
        position[2] = new Vector3(294.4486f, -0.718679f, 15);
        quaternion[0] = new Quaternion(0.00000f, 0.70976f, 0.00000f, -0.70444f);
        quaternion[1] = new Quaternion(0.00000f, 0.70976f, 0.00000f, -0.70444f);
        quaternion[2] = new Quaternion(0.00000f, 0.70976f, 0.00000f, -0.70444f);
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            UserInfo userInfo = GameObject.FindObjectOfType<UserInfo>();
            if (userInfo.CType.Name == "전사")
                PhotonNetwork.Instantiate("Characters/Warrior_Player", position[player.Key - 1], quaternion[player.Key - 1]);
            else if (userInfo.CType.Name == "궁수")
                PhotonNetwork.Instantiate("Characters/Archer_Player", position[player.Key - 1], quaternion[player.Key - 1]);
            else if (userInfo.CType.Name == "마법사")
                PhotonNetwork.Instantiate("Characters/Magician_Player", position[player.Key - 1], quaternion[player.Key - 1]);
        }
    }
}