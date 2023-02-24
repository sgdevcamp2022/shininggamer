using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [Header("Start Panel")]
    public GameObject startPanel;
    public Text nickname;
    public Button onlineCoopButton;

    [Header("Lobby Panel")]
    public GameObject lobbyPanel;
    public GameObject roomScrollView;
    public GameObject roomButton;
    static public List<RoomInfo> roomList = new List<RoomInfo>();

    [Header("Room Panel")]
    public GameObject roomPanel;
    public TMP_Text[] players;
    public GameObject startButton;

    [Header("Cannot Find Room Panel")]
    public GameObject cannotFindRoomPanel;

    [Header("Cannot Join Room Panel")]
    public GameObject cannotJoinRoomPanel;

    [Header("Player Selection")]
    public GameObject characterSelectionPanel;
    public PlayerItem[] playerItems;

    public TMP_Text loadingText;

    private void Awake()
    {
        PhotonNetwork.NickName = GameObject.Find("Player").GetComponent<UserInfo>().ID;
        nickname.text = PhotonNetwork.NickName;
        //PhotonNetwork.NickName = "test___";
        PhotonNetwork.AutomaticallySyncScene = true;

        pv = GetComponent<PhotonView>();
    }

    #region Connect to the lobby 
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        startPanel.SetActive(false);
        loadingText.gameObject.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        loadingText.gameObject.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        startPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
    }
    #endregion

    #region Update the room list
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!NetworkManager.roomList.Contains(roomList[i]))
                {
                    NetworkManager.roomList.Add(roomList[i]);
                }
            }
            else if (NetworkManager.roomList.IndexOf(roomList[i]) != -1)
            {
                NetworkManager.roomList.RemoveAt(NetworkManager.roomList.IndexOf(roomList[i]));
            }
        }
        MyRoomListRenewal();
    }

    void MyRoomListRenewal()
    {
        Transform[] roomList = roomScrollView.transform.Find("Viewport").Find("Content").GetComponentsInChildren<Transform>();
        if (roomList != null)
        {
            for (int i = 1; i < roomList.Length; i++)
            {
                if (roomList[i] != transform)
                    Destroy(roomList[i].gameObject);
            }
        }

        for (int i = 0; i < NetworkManager.roomList.Count; i++)
        {
            GameObject newRoomButton = Instantiate(roomButton, roomScrollView.transform.Find("Viewport").Find("Content").transform) as GameObject;
            newRoomButton.transform.Find("GameName").GetComponent<TMP_Text>().text = NetworkManager.roomList[i].Name;
            //newRoomButton.transform.Find("Status").GetComponent<TMP_Text>().text = "Can Join";
            roomScrollView.GetComponent<RoomManager>().CheckRoomsStatus();
        }
    }
    #endregion

    #region Join/Leave the room     
    string GetMasterClientNickname()
    {
        Player masterPlayer;
        PhotonNetwork.CurrentRoom.Players.TryGetValue(PhotonNetwork.CurrentRoom.MasterClientId, out masterPlayer);
        return masterPlayer.NickName;
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            print("PhotonNetwork is not connected.");
            return;
        }

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 3;
        options.EmptyRoomTtl = 0;
        options.BroadcastPropsChangeToAll = true;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "start", false } };
        options.CustomRoomPropertiesForLobby = new string[] { "start" };
        options.PlayerTtl = 0;
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, options, TypedLobby.Default);

    }

    public override void OnCreatedRoom()
    {
        players[0].gameObject.SetActive(true);
        players[1].gameObject.SetActive(false);
        players[2].gameObject.SetActive(false);

        players[0].text = GetMasterClientNickname();

        //print(PhotonNetwork.NickName + " created a new room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print(PhotonNetwork.NickName + " failed to create a new room: " + message);
    }

    public void JoinRoom(TMP_Text gameName)
    {
        //print(PhotonNetwork.NickName + " tried to join the room " + gameName.text + ".");
        PhotonNetwork.JoinRoom(gameName.text);
    }

    public void JoinRoom(TMP_InputField gameName)
    {
        //print(PhotonNetwork.NickName + " tried to join the room " + gameName.text + ".");
        PhotonNetwork.JoinRoom(gameName.text);
    }


    public override void OnJoinedRoom()
    {
        //print(PhotonNetwork.NickName + " joined the room.");
        roomPanel.SetActive(true);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].text = PhotonNetwork.PlayerList[i].NickName;
        }

        int currentRoom = -1;
        for (int i = 0; i < NetworkManager.roomList.Count; i++)
        {
            if (PhotonNetwork.CurrentRoom == NetworkManager.roomList[i])
            {
                currentRoom = i;
                break;
            }
        }

        roomScrollView.GetComponent<RoomManager>().CheckCurrentRoomStatus();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (returnCode == 32758)
            cannotFindRoomPanel.SetActive(true);
        else if (returnCode == 32765)
            cannotJoinRoomPanel.SetActive(true);

        print(PhotonNetwork.NickName + " couldn't join the room: " + message);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //print("Remote Player " + PhotonNetwork.NickName + " enterend the room.");

        int newPlayerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        players[newPlayerIndex].gameObject.SetActive(true);
        players[newPlayerIndex].text = PhotonNetwork.PlayerList[newPlayerIndex].NickName;

        roomScrollView.GetComponent<RoomManager>().CheckCurrentRoomStatus();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(PhotonNetwork.CurrentRoom.PlayerCount == 1 ? true : false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        roomScrollView.GetComponent<RoomManager>().CheckRoomsStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemotePlayerLeftRoom(otherPlayer);
    }

    public void RemotePlayerLeftRoom(Player otherPlayer)
    {
        players[PhotonNetwork.CurrentRoom.PlayerCount].gameObject.SetActive(false);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && players[1].text == otherPlayer.NickName)
            players[1].text = players[2].text;
    }
    #endregion

    #region Character selection
    public void ActivateCharacterSelectionPanel(bool isActive)
    {
        pv.RPC("RPCActivateCharacterSelection", RpcTarget.All, isActive);
        //photonView.RPC("UpdatePlayerItemList", RpcTarget.All);
    }

    [PunRPC]
    void RPCActivateCharacterSelection(bool isActive)
    {
        characterSelectionPanel.SetActive(isActive);
        lobbyPanel.SetActive(!isActive);
        roomPanel.SetActive(!isActive);

        if (isActive)
        {
            // 플레이어마다 누를 수 있는 StartSelectionButton 다르게 하기
            int localPlayerId = PhotonNetwork.CurrentRoom.Players.FirstOrDefault(x => x.Value.NickName == PhotonNetwork.NickName).Key;
            Button[] startSelectionButtons = new Button[playerItems.Length];
            for (int i = 0; i < playerItems.Length; i++)
            {
                startSelectionButtons[i] = playerItems[i].transform.GetChild(0).GetComponent<Button>();
                startSelectionButtons[i].interactable = false;
            }
            startSelectionButtons[localPlayerId - 1].GetComponent<Button>().interactable = true;

            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                playerItems[player.Key - 1].SetPlayerInfo(player.Value);
            }
            playerItems[localPlayerId - 1].ApplyLocalChanges();
        }
    }

    public void ShowPlayerAvatar(bool isActive)
    {
        int localPlayerId = PhotonNetwork.CurrentRoom.Players.FirstOrDefault(x => x.Value.NickName == PhotonNetwork.NickName).Key;
        pv.RPC("RPCActivatePlayerAvatar", RpcTarget.All, isActive, localPlayerId - 1);
    }

    [PunRPC]
    void RPCActivatePlayerAvatar(bool isActive, int playerId)
    {
        if (isActive)
        {
            //PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable());
        }
        playerItems[playerId].transform.GetChild(1).gameObject.SetActive(isActive);
        playerItems[playerId].transform.GetChild(0).gameObject.SetActive(!isActive);

        // 캐릭터 창에서 나갈 때 player의 기존의 properties 폐기

    }
    #endregion

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient /*&& PhotonNetwork.CurrentRoom.PlayerCount == 3*/)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }

        CheckPlayersReadyAndStartGame();
    }

    public void CheckPlayersReadyAndStartGame()
    {
        if (!PhotonNetwork.InRoom || (bool)PhotonNetwork.CurrentRoom.CustomProperties["start"])
            return;

        try
        {
            int playersReady = 0;
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if ((bool)player.Value.CustomProperties["isReady"])
                    playersReady++;
            }
            if (playersReady == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                PhotonNetwork.CurrentRoom.CustomProperties["start"] = true;

                Dictionary<int, CharacterType> characterTypes = GameObject.FindObjectOfType<FirebaseLoadManager>().CharacterOp;
                // 캐릭터를 변경하지 않았을 시 CustomProperties에 avatarIndex가 존재하지 않음(PlayerItem.cs의 128번째 줄 실행 후 SetCustomProperties를 하지 않음)
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("avatarIndex"))
                    GameObject.Find("Player").GetComponent<UserInfo>().CType = characterTypes[(int)PhotonNetwork.LocalPlayer.CustomProperties["avatarIndex"]];
                else
                    GameObject.Find("Player").GetComponent<UserInfo>().CType = characterTypes[0];
                PhotonNetwork.LoadLevel("TilemapScene");
                print("load scene");
            }
        }
        catch (NullReferenceException e)
        {
            //print("need to initialize player custom properties.");
        }

    }


    public void QuitGame()
    {
        Application.Quit();
    }
}