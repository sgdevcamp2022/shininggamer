using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    
    List<GameObject> monster;

    [SerializeField]
    GameObject SkillPanel;

    public Transform[] playerStartPos;
    public Transform[] monsterStartPos;

    public Transform[] turnCameraPos; //0 - start, 1 - player, 2 - monster

    public enum FightStatus {startFight, fighting, playerTurn ,monsterTurn };

    public bool isPlayerTurn;
    public bool settingComplet = true;

    public Button[] toDoButton;


    private void Start()
    {
        monster=new List<GameObject>();
        //TurnSetting();
        //TurnStart();
        StartCoroutine("FindMonster");
        StartCoroutine(FightGameController());
        //TurnEnd();
        //isPlayerTurn = true;
    }

    IEnumerator FindMonster(){
        yield return new WaitForSeconds(3f);
        
        GameObject tmp = GameObject.Find("Monster1(Clone)").gameObject;
        monster.Add(tmp);
        
        Debug.Log(monster.Count);
        yield return new WaitForSeconds(2f);
        TurnSetting();
        TurnStart();
        PlayerTurnCheck();
        Debug.Log(tmp);

        yield break;

    }

    private void Update()
    {
        CameraPosSetting();
        StartCoroutine("SwitchScene");

    }

    IEnumerator SwitchScene(){
        for (int i = 0; i < monster.Count; i++)
        {
            if(monster[0].GetComponent<MonsterInfo>().isDie){
                yield return new WaitForSeconds(3f);
                SceneManager.LoadScene("TilemapScene");
            }
        }
        yield break;
    }


    void PlayerTurnCheck()
    {
        if (Player.GetComponent<KSH_Player>().isTurn)
        {
            isPlayerTurn = true;
        }
    
        for (int i = 0; i < monster.Count; i++)
        {
            if (monster[i].GetComponent<MonsterInfo>().isTurn)
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


    
    IEnumerator FightGameController()
    {
        //while (true)
        //{
            if (isPlayerTurn)
            {
                StartCoroutine(PlayerAttactEndCheck());
            }

        //TurnEnd();
        //TurnStart();
            yield return null;
        //}
    }

    public List<GameObject> turnObj = new List<GameObject>();

    public void TurnSetting()
    {
        turnObj.Add(Player);
        turnObj.Add(Player);
        for(int i=0;i<10;i++){
            int playerfirst = Random.Range(0,2);
            Debug.Log(playerfirst);
            if (playerfirst%2==1)
            {
                turnObj.Add(Player);
            }
            else
            {
                foreach(GameObject tmp in monster){
                    turnObj.Add(tmp);
                }
            }

        }
        // int playerfirst = Random.Range(0,2);

        // if (playerfirst == 1)
        // {
        //     for (int i = 0; i < 2; i++)
        //     {
        //         turnObj.Add(Player);
        //     }

        //     for (int i = 0; i < 2; i++)
        //     {
        //         foreach(GameObject tmp in monster){
        //             turnObj.Add(tmp);
        //         }
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i < 2; i++)
        //     {
        //         foreach(GameObject tmp in monster){
        //             turnObj.Add(tmp);
        //         }
        //     }
        //     for (int i = 0; i < 2; i++)
        //     {
        //         foreach(GameObject tmp in monster){
        //             turnObj.Add(tmp);
        //         }
        //     }

        // }
    }

    public void TurnStart()
    {
        if(turnObj!=null){
            if (turnObj[0].gameObject.name.Contains("Monster"))
            {
                turnObj[0].GetComponent<MonsterInfo>().isTurn = true;
            }
            else
            {
                turnObj[0].GetComponent<KSH_Player>().isTurn = true;
            }        
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
