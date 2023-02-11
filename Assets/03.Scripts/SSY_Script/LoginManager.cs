using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

    [SerializeField]
    GameObject MenuUI;
    [SerializeField]
    GameObject LoginUI;
    [SerializeField]
    InputField password;
    [SerializeField]
    Text id;
    [SerializeField]
    Text WindowId;
    [SerializeField]
    GameObject SignUpUI;
    string email="@naver.com";
    string userid;
    MenuManager menu=null;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseAuthManager.Instance.LoginState+=OnChangedState;
        FirebaseAuthManager.Instance.Init();
        if(menu==null)
            menu=GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }
    void OnChangedState(bool sign){
        if(userid!=null)
            userid="";
        if(sign){
            userid+=FirebaseAuthManager.Instance.UserId;
            menu.Id=id.text;
            menu.isLogin=true;
            Debug.Log(userid);
            LoginUI.SetActive(false);
            MenuUI.SetActive(true);
        }
    }

    public void Create(){
        SignUpUI.SetActive(true);
    }

    public void Login(){
        FirebaseAuthManager.Instance.Login(id.text+email,password.text);
        WindowId.text=id.text;
    }

    public void Logout(){
        MenuUI.SetActive(true);
        LoginUI.SetActive(false);
    }

    public void OnGoogleClick(){
        email+="@gmail.com";
    }

    public void OnNaverClick(){
        email+="@naver.com";
    }
}
