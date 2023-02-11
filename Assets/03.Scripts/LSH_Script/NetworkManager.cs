using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    [Header("Start Panel")]
    public GameObject startPanel;
    public Text nickname;
    public Button onlineCoopButton;

    [Header("Lobby Panel")]
    public GameObject lobbyPanel;
    public ScrollRect roomScrollView;
    public GameObject roomButton;
    private List<RoomInfo> myRoomList = new List<RoomInfo>();

    [Header("Room Panel")]
    public GameObject roomPanel;
    public TMP_Text[] players;
    public GameObject startButton;

    [Header("Cannot Find Room Panel")]
    public GameObject cannotFindRoomPanel;

    [Header("Player Selection")]
    public GameObject characterSelectionPanel;
    public PlayerItem[] playerItems;

    public TMP_Text loadingText;

    private bool gameStart;

    private void Awake()
    {
        PhotonNetwork.NickName = GameObject.Find("Player").GetComponent<UserInfo>().ID;
        nickname.text = PhotonNetwork.NickName;
        PhotonNetwork.AutomaticallySyncScene = true;

        pv = GetComponent<PhotonView>();

        gameStart = false;
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
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myRoomList.Contains(roomList[i]))
                {
                    myRoomList.Add(roomList[i]);
                }
            }
            else if (myRoomList.IndexOf(roomList[i]) != -1)
            {
                myRoomList.RemoveAt(myRoomList.IndexOf(roomList[i]));
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

        for (int i = 0; i < myRoomList.Count; i++)
        {
            GameObject newRoomButton = Instantiate(roomButton, roomScrollView.transform.Find("Viewport").Find("Content").transform) as GameObject;
            newRoomButton.transform.Find("GameName").GetComponent<TMP_Text>().text = myRoomList[i].Name;
            newRoomButton.transform.Find("Status").GetComponent<TMP_Text>().text = "Can Join";
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
        PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        players[0].gameObject.SetActive(true);
        players[1].gameObject.SetActive(false);
        players[2].gameObject.SetActive(false);

        players[0].text = GetMasterClientNickname();

        print(PhotonNetwork.NickName + " created a new room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print(PhotonNetwork.NickName + " failed to create a new room: " + message);
    }

    public void JoinRoom(TMP_Text gameName)
    {
        print(PhotonNetwork.NickName + " tried to join the room " + gameName.text + ".");
        PhotonNetwork.JoinRoom(gameName.text);
    }

    public void JoinRoom(TMP_InputField gameName)
    {
        print(PhotonNetwork.NickName + " tried to join the room " + gameName.text + ".");
        PhotonNetwork.JoinRoom(gameName.text);
    }


    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.NickName + " joined the room.");
        roomPanel.SetActive(true);

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        cannotFindRoomPanel.SetActive(true);
        print(PhotonNetwork.NickName + " couldn't join the room: " + message);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Remote Player " + PhotonNetwork.NickName + " enterend the room.");

        int newPlayerIndex = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        print("newPlayerIndex: " + newPlayerIndex);
        players[newPlayerIndex].gameObject.SetActive(true);
        players[newPlayerIndex].text = PhotonNetwork.PlayerList[newPlayerIndex].NickName;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(PhotonNetwork.CurrentRoom.PlayerCount == 1 ? true : false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
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
    public void ShowCharacterSelection(bool isActive)
    {
        pv.RPC("OnActivateCharacterSelection", RpcTarget.All, isActive);
        //photonView.RPC("UpdatePlayerItemList", RpcTarget.All);
    }

    [PunRPC]
    void OnActivateCharacterSelection(bool isActive)
    {
        characterSelectionPanel.SetActive(isActive);
        lobbyPanel.SetActive(!isActive);
        roomPanel.SetActive(!isActive);
        
        if(isActive)
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
        pv.RPC("OnActivatePlayerAvatar", RpcTarget.All, isActive, localPlayerId - 1);
    }

    [PunRPC]
    void OnActivatePlayerAvatar(bool isActive, int playerId)
    {
        playerItems[playerId].transform.GetChild(1).gameObject.SetActive(isActive);
        playerItems[playerId].transform.GetChild(0).gameObject.SetActive(!isActive);
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

        if (characterSelectionPanel.activeSelf && PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady"))
        {
            int playersReady = 0;
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                if ((bool)player.Value.CustomProperties["isReady"] == true)
                    playersReady++;
            }
            if (!gameStart && playersReady == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                gameStart = true;
                LoadScene("KSH_FightScene");
            }
        }
    }

    void LoadScene(string scene)
    {
        PhotonNetwork.LoadLevel(scene);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}