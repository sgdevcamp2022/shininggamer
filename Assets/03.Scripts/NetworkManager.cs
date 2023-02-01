using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Start Panel")]
    public GameObject startPanel;
    public TMP_Text nickname;
    public UnityEngine.UI.Button onlineCoopButton;

    [Header("Lobby Panel")]
    public GameObject lobbyPanel;
    public ScrollRect roomScrollView;
    public GameObject roomButton;
    private List<RoomInfo> myRoomList = new List<RoomInfo>();

    [Header("Room Panel")]
    public GameObject roomPanel;
    public TMP_Text[] players;

    [Header("Cannot Find Room Panel")]
    public GameObject cannotFindRoomPanel;

    public TMP_Text loadingText;

    private void Awake()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerID", "");
        nickname.text = PhotonNetwork.NickName;
    }

    #region          
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

    #region  κ        ӹ      Ʈ       Ʈ
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

    #region    ӹ      /      
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

    public void QuitGame()
    {
        Application.Quit();
    }
}