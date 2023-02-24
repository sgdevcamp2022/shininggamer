using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserInfo : MonoBehaviour
{
    string characterName;
    string monsters;
    string _id;
    CharacterType _ctype;
    string findCtype;
    string playerHp;
    [SerializeField]
    int damage;
    bool isFirstLoad;

//삭제 부분
    public UserInfo(string id){
        _id=id;
        _ctype=null;
        findCtype="";
    }

    void OnEnable()
    {
    	  // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public string ID{
        get{
            return _id;
        }
        set{
            _id=value;
        }
    }

    public int Damage{
        get{
            return damage;
        }
        set{
            damage=value;
        }
    }

    public CharacterType CType{
        get{
            return _ctype;
        }
        set{
            _ctype=value;
        }
    } 


    public string CharacterType
    {
        get
        {
            return characterName;
        }
    }

    public string Monsters
    {
        get
        {
            return monsters;
        }
    }

    public bool IsFirstLoad
    {
        get
        {
            return isFirstLoad;
        }
        set
        {
            isFirstLoad = value;
        }
    }

    public void SendToFight(string characterType, string monsterName, string currentHp)
    {
        characterName = characterType;
        monsters = monsterName;
        playerHp = currentHp;
    }

    void OnSceneLoaded(Scene Scene, LoadSceneMode mode){
        if(Scene.name=="KSH_FightScene"){
            this.gameObject.transform.position=new Vector3(0f,0f,0f);
            this.gameObject.transform.Rotate(new Vector3(0f, 0f, 0f));
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

     void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
