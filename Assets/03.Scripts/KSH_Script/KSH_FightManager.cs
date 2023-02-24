using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KSH_FightManager : MonoBehaviour
{
    public GameObject[] ksh_Player;
    public GameObject[] ksh_Monster;

    //송수영이 만든 monster;
    List<GameObject> monster;

    [SerializeField]
    GameObject SkillPanel;

    public GameObject playerActionStat;
    public GameObject diceLayout;

    public Transform[] playerStartPos;
    public Transform[] monsterStartPos;

    public Transform[] turnCameraPos; //0 - start, 1 - player, 2 - monster

    public enum FightStatus {startFight, fighting, playerTurn ,monsterTurn };

    public bool isPlayerTurn;
    public bool settingComplet = true;

    public Button[] toDoButton;

    private void Awake()
    {
        monster=new List<GameObject>();
        TurnSetting();
        TurnStart();
    }

    private void Start()
    {
        //TurnSetting();
        //TurnStart();
        PlayerTurnCheck();
        StartCoroutine(FightGameController());
        //TurnEnd();
        //isPlayerTurn = true;
    }

    private void Update()
    {
        if(monster==null){
            GameObject tmp = GameObject.Find("Monster1(Clone)").gameObject;
            monster.Add(tmp);
        }
        CameraPosSetting();
    }


    void PlayerTurnCheck()
    {
        for (int i = 0; i < ksh_Player.Length; i++)
        {
            if (ksh_Player[i].GetComponent<KSH_Player>().isTurn)
            {
                isPlayerTurn = true;
            }
            
        }
        for (int i = 0; i < ksh_Monster.Length; i++)
        {
            if (ksh_Monster[i].GetComponent<KSH_Monster>().isTurn)
            {
                isPlayerTurn = false;
            }
        }
    }

    public void CameraPosSetting()
    {
        //if (!settingComplet) return;
        if (isPlayerTurn)
        {
            turnCameraPos[0].position = Vector3.Lerp(turnCameraPos[0].position, turnCameraPos[1].position,Time.deltaTime);
            turnCameraPos[0].rotation = Quaternion.Lerp(turnCameraPos[0].rotation, turnCameraPos[1].rotation, Time.deltaTime);
            SkillPanel.SetActive(true);   
        }
        else
        {
            SkillPanel.SetActive(false);   
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
        //if(!playerActionStat.activeSelf && settingComplet && isPlayerTurn && !ksh_Player[0].GetComponent<KSH_Player>().playerAttackEnd)
        //{
        //    playerActionStat.SetActive(true);
        //}
        //if (ksh_Player[0].GetComponent<KSH_Player>().playerAttackEnd)
        //{
        //    isPlayerTurn = false;
        //}


        if (isPlayerTurn)
        {
            playerActionStat.SetActive(true);
        }
        else
        {
            playerActionStat.SetActive(false);
        }
    }
    
    IEnumerator FightGameController()
    {
        //while (true)
        //{
            if (isPlayerTurn)
            {
                PlayerTurn();
                StartCoroutine(PlayerAttactEndCheck());
            }
            else
            {
                MonsterTurn();
            }

        //TurnEnd();
        //TurnStart();
            yield return null;
        //}
    }

    void MonsterTurn()
    {
        if (playerActionStat.activeSelf && settingComplet && !isPlayerTurn)
        {
            playerActionStat.SetActive(false);
        }
    }

    public List<GameObject> turnObj = new List<GameObject>();
    public void TurnSetting()
    {
        int playerfirst = Random.Range(0,2);

        if (playerfirst == 1)
        {
            for (int i = 0; i < ksh_Player.Length; i++)
            {
                turnObj.Add(ksh_Player[i]);
            }

            for (int i = 0; i < monster.Count; i++)
            {
                turnObj.Add(monster[i]);
            }
        }
        else
        {
            for (int i = 0; i < monster.Count; i++)
            {
                turnObj.Add(monster[i]);
            }
            for (int i = 0; i < monster.Count; i++)
            {
                turnObj.Add(monster[i]);
            }

        }
    }

    public void TurnStart()
    {
            if (turnObj[0].gameObject.name.Contains("Slime"))
            {
                turnObj[0].GetComponent<KSH_Monster>().isTurn = true;
            }
            else
            {
                turnObj[0].GetComponent<KSH_Player>().isTurn = true;
            }                
    }

    public void TurnEnd()
    {
        turnObj.Add(turnObj[0]);
        turnObj.RemoveAt(0);

        
    }

    IEnumerator PlayerAttactEndCheck()
    {
        while (true)
        {
            if (turnObj[0].GetComponent<KSH_Player>().playerAttackEnd)
            {
                isPlayerTurn = false;
                
            }
            yield return null;
        }
    }
}
