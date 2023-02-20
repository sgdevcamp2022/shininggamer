using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KSH_FightManager : MonoBehaviour
{
    public GameObject[] ksh_Player;
    public GameObject[] ksh_Monster;

    public GameObject playerActionStat;
    public GameObject diceLayout;

    public Transform[] playerStartPos;
    public Transform[] monsterStartPos;

    public Transform[] turnCameraPos; //0 - start, 1 - player, 2 - monster

    public enum FightStatus {startFight, fighting, playerTurn ,monsterTurn };

    public bool isPlayerTurn;
    public bool settingComplet;

    public Button[] toDoButton;

    private void Start()
    {
        StartCoroutine(FightGameController());
        isPlayerTurn = true;
    }

    private void Update()
    {
        CameraPosSetting();
    }

    public void CameraPosSetting()
    {
        if (!settingComplet) return;
        if (isPlayerTurn)
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
        if(!playerActionStat.activeSelf && settingComplet && isPlayerTurn && !ksh_Player[0].GetComponent<KSH_Player>().playerAttackEnd)
        {
            playerActionStat.SetActive(true);
        }
        if (ksh_Player[0].GetComponent<KSH_Player>().playerAttackEnd)
        {
            isPlayerTurn = false;
        }

    }
    
    IEnumerator FightGameController()
    {
        while (true)
        {
            if (isPlayerTurn)
            {
                PlayerTurn();
            }
            else
            {
                MonsterTurn();
            }

            yield return null;
        }
    }

    void MonsterTurn()
    {
        if (playerActionStat.activeSelf && settingComplet && !isPlayerTurn)
        {
            playerActionStat.SetActive(false);
            
        }
    }

}
