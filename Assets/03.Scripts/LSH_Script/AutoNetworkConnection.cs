using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AutoNetworkConnection : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = "AUTO"; // 닉네임 설정
        PhotonNetwork.AutomaticallySyncScene = true; // 플레이어 간 씬 동일하게 하는 설정

        PhotonNetwork.ConnectUsingSettings(); // Photon 연결 
    }

    public override void OnConnectedToMaster()
    {
        print("Join lobby automatically");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Join room automatically");
        PhotonNetwork.JoinRandomOrCreateRoom();
        Dictionary<int, CharacterType> characterTypes = GameObject.FindObjectOfType<FirebaseLoadManager>().CharacterOp;
        GameObject.Find("Player").GetComponent<UserInfo>().CType = characterTypes[1];
    }
}
