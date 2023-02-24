using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanelSetting : MonoBehaviour
{

    [SerializeField]
    GameObject CharUI;

    UserInfo player;

    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.Find("Player").GetComponent<UserInfo>();
        CharUI.GetComponent<CharacterUIControllerFight>().InitializeStat(1);
    }


}
