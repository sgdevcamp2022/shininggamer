using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject content;

    public void CheckRoomsStatus()
    {
        // 방장은 방 리스트 업데이트 하지 않음
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        for (int i = 0; i < NetworkManager.roomList.Count; i++)
        {
            if ((bool)NetworkManager.roomList[i].CustomProperties["start"])
                content.transform.GetChild(i).Find("Status").GetComponent<TMP_Text>().text = "In Progress";
            else if (NetworkManager.roomList[i].PlayerCount == NetworkManager.roomList[i].MaxPlayers)
                content.transform.GetChild(i).Find("Status").GetComponent<TMP_Text>().text = "Full";
            else
                content.transform.GetChild(i).Find("Status").GetComponent<TMP_Text>().text = "Can Join";
        }
    }

    public void CheckCurrentRoomStatus()
    {
        // 방장은 방 리스트 업데이트 하지 않음
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            return;

        int currentRoom = -1;
        for(int i = 0; i < NetworkManager.roomList.Count; i++)
        {
            if (PhotonNetwork.CurrentRoom.Name == NetworkManager.roomList[i].Name)
            {
                currentRoom = i;
                break;
            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["start"])
                content.transform.GetChild(currentRoom).Find("Status").GetComponent<TMP_Text>().text = "In Progress";
            else
                content.transform.GetChild(currentRoom).Find("Status").GetComponent<TMP_Text>().text = "Full";
        }
        else
        {
            content.transform.GetChild(currentRoom).Find("Status").GetComponent<TMP_Text>().text = "Can Join";
        }
    }
}
