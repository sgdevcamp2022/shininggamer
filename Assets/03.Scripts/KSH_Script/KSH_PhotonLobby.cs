using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class KSH_PhotonLobby : MonoBehaviourPunCallbacks
{
    //public Text ConnectionStatus;
    public string IDtext;
    

    void Start()
    {
        IDtext = "ksh";
        Connect(); 
    }

    void Update()
    {
        //ConnectionStatus.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = IDtext;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비 접속 완료");
        OnCreatedRoom();
    }

    public override void OnCreatedRoom()
    {
        print("CreateRoom");
        OnJoinedRoom();
    }

    public override void OnJoinedRoom()
    {
        print("joinedRoom");
        SceneManager.LoadScene("KSH_PhotonFight");
    }

   
}
