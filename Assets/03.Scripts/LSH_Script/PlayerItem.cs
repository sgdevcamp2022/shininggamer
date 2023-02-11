using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public GameObject leaveRoomButton;
    public Text characterName;

    Hashtable playerProperties;
    public GameObject character;
    public GameObject[] characters;

    Player player;

    Dictionary<int, CharacterType> characterTypes;

    private void Awake()
    {
        // 전사 -> 궁수 -> 마법사 순서
        characterTypes = GameObject.Find("DBManager").GetComponent<FirebaseLoadManager>().CharacterOp;
    }

    public void SetPlayerInfo(Player _player)
    {
        print("setplayerinfo called.");
        playerName.text = _player.NickName;
        player = _player;
        characterName.text = characterTypes[0].Name;
        playerProperties = new Hashtable();
        playerProperties["isReady"] = false;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
        leaveRoomButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = characters.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        print("right arrow clicked.");
        if ((int)playerProperties["playerAvatar"] == characters.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        print("OnPlayerPropertiesUpdate called.");
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }


    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            print("UpdatePlayerItem called. Destroy " + character.transform.GetChild(0).gameObject);
            Destroy(character.transform.GetChild(0).gameObject);

            GameObject newAvatar = characters[(int)player.CustomProperties["playerAvatar"]].gameObject;
            newAvatar.transform.localScale = new Vector3(300, 300, 100);
            newAvatar.transform.localEulerAngles = new Vector3(0, 180, 0);
            newAvatar.transform.position = new Vector3(0, -400, 50);
            ChangeLayerRecursively(newAvatar.transform, "UI"); // 자식 컴포넌트까지 레이어 모두 변경
            Instantiate(newAvatar, character.transform);

            characterName.text = characterTypes[(int)playerProperties["playerAvatar"]].Name;

            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }

    void ChangeLayerRecursively(Transform transform, string name)
    {
        transform.gameObject.layer = LayerMask.NameToLayer(name);
        foreach(Transform child in transform)
        {
            ChangeLayerRecursively(child, name);
        }
    }

    public void SetPlayerReady()
    {
        playerProperties["isReady"] = true;
        player.SetCustomProperties(playerProperties);
        print("player ready: " + player.CustomProperties["isReady"]);
    }
}
