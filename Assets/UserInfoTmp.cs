using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UserInfoTmp : MonoBehaviour
{
    string _id;
    CharacterType _ctype;
    string findCtype;
    string _fightMonsterName;

//삭제 부분
    public UserInfoTmp(){
        _id="tndud3999";
        _ctype=new CharacterType("전사","0","25","28","9","50","0","1","81","57","61","77","85","45");
        findCtype="전사";
        _fightMonsterName="";
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

    public CharacterType CType{
        get{
            return _ctype;
        }
        set{
            _ctype=value;
        }
    } 

    public string FightMonsterName{
        get{
            return _fightMonsterName;
        }
        set{
            _fightMonsterName=value;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.tag.Substring(0, 7) == "Monster"){
                _fightMonsterName=other.gameObject.tag.Replace(" ","");
                this.gameObject.GetComponent<Animator>().SetBool("IsWalking",false);
                SceneManager.LoadScene("KSH_FightScene");
            }
                
        }
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
