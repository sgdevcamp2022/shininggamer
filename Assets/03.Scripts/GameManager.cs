using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public int maxLife;
    public int gameTurn = 1;
    public GameObject monsterEncount;

    int playerTurn;
    int currentLife;

    GameObject lifeGrid;
    UserInfo playerInfo;
    HexGrid hexGrid;
    HexUnit myPlayer;
    HexUnit encountedMonster;
    Image[] lifeImages;
    GameObject tilemapCanvas;
    GameObject mapEditor;
    CharacterUIController[] charUiController;

    [SerializeField] Sprite filledLifePrefab;
    [SerializeField] Sprite unfilledLifePrefab;


    [Tooltip("Used to add units")]
    public PhotonView hexGridPv;


    void Start()
    {
        playerInfo = GameObject.Find("Player").GetComponent<UserInfo>();
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
        myPlayer = GameObject.Find("Tile Player").GetComponent<HexUnit>();
        mapEditor = GameObject.Find("Hex Map Editor");

        charUiController = new CharacterUIController[3];
        for (int i = 0; i < 3; i++)
            charUiController[i] = mapEditor.transform.GetChild(i + 5).GetComponent<CharacterUIController>();

        if(!playerInfo.IsFirstLoad)
        {
            Debug.Log("first");
            mapEditor.GetComponent<HexMapEditor>().Load();
            SetCharacter();    
        }
        else
        {
            Debug.Log("second");
            mapEditor.GetComponent<HexMapEditor>().LoadAfterFight();
        }

        currentLife = 3;
        lifeGrid = GameObject.Find("Life Grid");
        lifeImages = new Image[3];
        tilemapCanvas = GameObject.Find("Hex Map Editor");
        //if (myPlayer.Turn == gameTurn)
        TryMove(myPlayer.Turn == gameTurn);

        for (int i = 0; i < maxLife; i++)
            lifeImages[i] = lifeGrid.transform.GetChild(i).GetComponent<Image>();
        SetLife();
    }

    void Update()
    {
        if (hexGrid.SelectedUnit != null)
        {
            if (hexGrid.IsUseConcentration)
            {
                charUiController[myPlayer.Turn - 1].Concentration.ConcentrationChange(-1);
                hexGrid.IsUseConcentration = false;
            }

            if (hexGrid.SelectedUnit.IsMonsterEncount)
            {
                monsterEncount.SetActive(true);
                monsterEncount.GetComponent<MonsterEncoutManager>().MonsterPopUp(hexGrid.SelectedUnit.Monster);

                if (monsterEncount.GetComponent<MonsterEncoutManager>().IsFight)
                {
                    FindEncountMonster(hexGrid.SelectedUnit.Monster.tag);
                    mapEditor.GetComponent<HexMapEditor>().DestroyUnit(encountedMonster);
                    mapEditor.GetComponent<HexMapEditor>().SaveAfterFight();

                    playerInfo.SendToFight(
                        playerInfo.CType.Name,
                        hexGrid.SelectedUnit.Monster.name.Replace("(Clone)", ""),
                        "50"
                        );
                    SceneManager.LoadScene("KSH_FightScene");
                }
            }
        }
    }

    void SetCharacter()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            charUiController[i].gameObject.SetActive(true);
            charUiController[i].InitializeStat(i);
            if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.CurrentRoom.Players[i + 1].ActorNumber)
            {
                myPlayer.Turn = i + 1;
            }
        }


        myPlayer.Orientation = -180f;
        string prefabPath = "CSH_Resources/";
        GameObject newUnit;
        switch (playerInfo.CType.Name)
        {
            case "전사":
                prefabPath += "Warrior";
                newUnit = PhotonNetwork.Instantiate(prefabPath, hexGrid.cells[hexGrid.playerCells[myPlayer.Turn - 1]].Position, myPlayer.transform.localRotation);
                newUnit.name = "Player " + myPlayer.Turn;
                newUnit.tag = newUnit.name;
                hexGridPv.RPC("AddUnit", RpcTarget.All, newUnit.GetComponent<PhotonView>().ViewID, "Player " + myPlayer.Turn,
                    myPlayer.Turn, 14, playerInfo.CType.Name, myPlayer.Turn == 1 ? true : false);
                hexGrid.AddLocalInfo(newUnit.GetComponent<HexUnit>(), myPlayer.Turn, gameTurn == myPlayer.Turn);
                break;
            case "궁수":
                prefabPath += "Archer";
                newUnit = PhotonNetwork.Instantiate(prefabPath, hexGrid.cells[hexGrid.playerCells[myPlayer.Turn - 1]].Position, myPlayer.transform.localRotation);
                newUnit.name = "Player " + myPlayer.Turn;
                newUnit.tag = newUnit.name;
                hexGridPv.RPC("AddUnit", RpcTarget.All, newUnit.GetComponent<PhotonView>().ViewID, "Player " + myPlayer.Turn,
                    myPlayer.Turn, 15, playerInfo.CType.Name, myPlayer.Turn == 1 ? true : false);
                hexGrid.AddLocalInfo(newUnit.GetComponent<HexUnit>(), myPlayer.Turn, gameTurn == myPlayer.Turn);
                break;
            case "마법사":
                prefabPath += "Magician";
                newUnit = PhotonNetwork.Instantiate(prefabPath, hexGrid.cells[hexGrid.playerCells[myPlayer.Turn - 1]].Position, myPlayer.transform.localRotation);
                newUnit.name = "Player " + myPlayer.Turn;
                newUnit.tag = newUnit.name;
                hexGridPv.RPC("AddUnit", RpcTarget.All, newUnit.GetComponent<PhotonView>().ViewID, "Player " + myPlayer.Turn,
                    myPlayer.Turn, 16, playerInfo.CType.Name, myPlayer.Turn == 1 ? true : false);
                hexGrid.AddLocalInfo(newUnit.GetComponent<HexUnit>(), myPlayer.Turn, gameTurn == myPlayer.Turn);
                break;
            default:
                Console.WriteLine("잘못된 캐릭터 선택");
                break;
        }

        playerInfo.IsFirstLoad = true;
    }

    [PunRPC]
    public void TryMove(bool isMyTurn)
    {
        print("try move");
        RandomController randomController = tilemapCanvas.GetComponentInChildren<RandomController>();
        randomController.IsPlayerTurn = isMyTurn;
        randomController.Speed = int.Parse(playerInfo.CType.Speed);
        randomController.OnRandomPositionNumberClick();
    }

    public void SetMoveCount(int successCount)
    {
        hexGrid.MoveCount = successCount;
    }

    void SetLife()
    {
        int i = 0;

        if (currentLife == maxLife)
        {
            while (i < maxLife)
            {
                lifeImages[i].sprite = filledLifePrefab;
                i++;
            }
        }
        else
        {
            while (i < currentLife)
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

    void FindEncountMonster(string monsterTag)
    {
        for(int i = 0; i < hexGrid.Units.Count; i++)
        {
            if (hexGrid.Units[i].CompareTag(monsterTag))
            {
                encountedMonster = hexGrid.Units[i];
            }
        }
    }
}