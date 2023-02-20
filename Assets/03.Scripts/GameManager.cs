using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public int maxLife;

    int currentLife;

    GameObject lifeGrid;
    UserInfo playerInfo;
    HexGrid hexGrid;
    HexUnit player;
    Image[] lifeImages;
    GameObject tilemapCanvas;

    [SerializeField] Sprite filledLifePrefab;
    [SerializeField] Sprite unfilledLifePrefab;

    HexMapCamera hexMapCamera;

    void Start()
    {
        playerInfo = GameObject.Find("Player").GetComponent<UserInfo>();
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        player = GameObject.Find("Tile Player").GetComponent<HexUnit>();
        SetCharacter();

        currentLife = 3;
        lifeGrid = GameObject.Find("Life Grid");
        lifeImages = new Image[3];

        tilemapCanvas = GameObject.Find("Hex Map Editor");
        TryMove();
        
        for(int i = 0; i < maxLife; i++)
            lifeImages[i] = lifeGrid.transform.GetChild(i).GetComponent<Image>();

        if(currentLife == 0)
        {
            // 게임 오버
        }
        else
        {
            SetLife();
        }
    }

    void Update()
    {
        // 어느 플레이어의 턴인지 검사해서 해당 플레이어의 movecount 초기화
    }

    void SetCharacter()
    {
        foreach (KeyValuePair<int, Player> currentPlayer in PhotonNetwork.CurrentRoom.Players)
        {
            if (PhotonNetwork.LocalPlayer.NickName == currentPlayer.Value.NickName)
            {
                player.Turn = (int)currentPlayer.Key;
                break;
            }
        }
        player.Orientation = -180f;

        print("character: " + playerInfo.CType.Name);
        string prefabPath = "CSH_Resources/";
        GameObject newUnit;
        switch (playerInfo.CType.Name)
        {
            case "전사":
                prefabPath += "Warrior";
                newUnit = PhotonNetwork.Instantiate(prefabPath,
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]].Position,
                    player.transform.localRotation);
                newUnit.name = "Player " + player.Turn;
                newUnit.tag = newUnit.name;
                hexGrid.AddUnit(newUnit.GetComponent<HexUnit>(), 
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]], 14, playerInfo.CType.Name);
                break;
            case "궁수":
                prefabPath += "Archer";
                newUnit = PhotonNetwork.Instantiate(prefabPath,
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]].Position,
                    player.transform.localRotation);
                newUnit.name = "Player " + player.Turn;
                newUnit.tag = newUnit.name;
                hexGrid.AddUnit(newUnit.GetComponent<HexUnit>(),
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]], 15, playerInfo.CType.Name);
                break;
            case "마법사":
                prefabPath += "Magician";
                newUnit = PhotonNetwork.Instantiate(prefabPath,
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]].Position,
                    player.transform.localRotation);
                newUnit.name = "Player " + player.Turn;
                newUnit.tag = newUnit.name;
                hexGrid.AddUnit(newUnit.GetComponent<HexUnit>(),
                    hexGrid.cells[hexGrid.playerCells[player.Turn - 1]], 16, playerInfo.CType.Name);
                break;
            default:
                Console.WriteLine("잘못된 캐릭터 정보");
                return;
        }
    }

    void SetLife()
    {
        int i = 0;

        if(currentLife == maxLife)
        {
            while(i < maxLife)
            {
                lifeImages[i].sprite = filledLifePrefab;
                i++;
            }

        }
        else
        {
            while(i < currentLife)
            {
                lifeImages[i].sprite = filledLifePrefab;
                i++;
            }

            i = currentLife;

            while (i < maxLife)
            {
                lifeImages[i].sprite = unfilledLifePrefab;
                i++;
            }
        }
    }

    public void CameraToPlayer(HexUnit currentPlayer)
    {

    }

    public void TryMove()
    {
        RandomController randomController = tilemapCanvas.GetComponentInChildren<RandomController>();
        randomController.Speed = 80;
        randomController.OnRandomPositionNumberClick();
    }

    public void SetMoveCount(int successCount)
    {
        hexGrid.MoveCount = successCount;
    }
}