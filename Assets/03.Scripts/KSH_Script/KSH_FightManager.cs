using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KSH_FightManager : MonoBehaviour
{
    public GameObject ksh_Player;
    public GameObject ksh_Monster;

    public GameObject playerActionStat;
    public GameObject diceLayout;

    public Transform playerStartPos;
    public Transform monsterStartPos;

    public Transform[] turnCameraPos; //0 - start, 1 - player, 2 - monster

    public enum FightStatus {startFight, fighting, playerTurn ,monsterTurn };

    public bool playerTurn;
    public bool settingComplet;

    public Button[] toDoButton;


    private void Start()
    {
        StartCoroutine(FightGameController());
        playerTurn = true;
    }

    private void Update()
    {
        CameraPosSetting();
    }

    public void CameraPosSetting()
    {
        if (!settingComplet) return;
        if (playerTurn)
        {
            turnCameraPos[0].position = Vector3.Lerp(turnCameraPos[0].position, turnCameraPos[1].position,Time.deltaTime);
            turnCameraPos[0].rotation = Quaternion.Lerp(turnCameraPos[0].rotation, turnCameraPos[1].rotation, Time.deltaTime);
        }
        else
        {
            turnCameraPos[0].position = Vector3.Lerp(turnCameraPos[0].position, turnCameraPos[2].position, Time.deltaTime);
            turnCameraPos[0].rotation = Quaternion.Lerp(turnCameraPos[0].rotation, turnCameraPos[2].rotation, Time.deltaTime);
        }
    }

    void PlayerInCheck()
    {
        //network connected succsece
    }

    void PlayerTurn()
    {
        if(!playerActionStat.activeSelf && settingComplet)
        {
            playerActionStat.SetActive(true);

        }
        else
        {
            playerActionStat.SetActive(false);
        }
    }

    void TurnSetting()
    {
        
    }

    void GameStatusCheck()
    {
        
    }

    

    IEnumerator FightGameController()
    {
        while (true)
        {
            if (playerTurn)
            {
                PlayerTurn();
            }

            yield return null;
        }
    }


}
