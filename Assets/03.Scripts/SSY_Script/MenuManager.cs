using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    bool _isLogin=false;

    [SerializeField]
    Text userid;
    [SerializeField]
    GameObject LoginUI;
    [SerializeField]
    GameObject MenuUI;
    [SerializeField]
    GameObject Warning;
    [SerializeField]
    GameObject Player;

    string id="";

    void Start(){
        Warning.transform.Find("Text").GetComponent<Text>().text="로그인을 해주세요.";
            
    }
    public string Id{
        set{
            if(id=="")
                id=value;
            Player.GetComponent<UserInfo>().ID=Id;    
        }
        get {
            return id;
        }
    }
    
    public bool isLogin{
        set{
            _isLogin=value;
        }
    }

    public void OnLoginClick(){
        if(!_isLogin){
            MenuUI.SetActive(false);
            LoginUI.SetActive(true);
        }else{
            userid.text=Id;
        }
    }

    public void OnNewGameClick(){
        if(_isLogin){
            DontDestroyOnLoad(Player);
            SceneManager.LoadScene("TitleScene");
            //새로운 씬으로 이동
        }else{
            Warning.SetActive(true);
        }
    }

}
