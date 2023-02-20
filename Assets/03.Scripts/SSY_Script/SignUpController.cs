using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SignUpController : MonoBehaviour
{
    [SerializeField]
    Text Id;
    [SerializeField]
    InputField password;
    [SerializeField]
    InputField rePasswd;
    [SerializeField]
    GameObject WarningPanel;
    [SerializeField]
    GameObject []Warnings;

    void Start(){
        FirebaseAuthManager.Instance.LoginState+=OnChangedState;
        WarningPanel.gameObject.transform.Find("Text").GetComponent<Text>().text="빈칸을 채워주시길 바랍니다.";
    }
    // Update is called once per frame
    void Update()
    {
        try{
            if(!Id.text.Contains("@")){
                Warnings[0].gameObject.SetActive(true);
            }else{
                Warnings[0].gameObject.SetActive(false);
            }

            if(0!=password.text.CompareTo(rePasswd.text)){
                Warnings[1].gameObject.SetActive(true);
            }else{
                Warnings[1].gameObject.SetActive(false);
            }
        }catch(AggregateException e){
            Debug.Log("에러잡기");
        }

    }

    void OnChangedState(bool sign){
        if(sign){
            this.gameObject.SetActive(false);
        }
    }
    public void OnClick(){

        if(password.text==""||Id.text==""){
            WarningPanel.gameObject.SetActive(true);
            return;
        }

       if(0==password.text.CompareTo(rePasswd.text)){
            Warnings[1].gameObject.SetActive(false);
            FirebaseAuthManager.Instance.Create(Id.text,password.text);
        }
        this.gameObject.SetActive(false);
    }

    public void OnCancelClick (){
        this.gameObject.SetActive(false);
    }

}
