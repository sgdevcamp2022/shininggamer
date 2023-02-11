using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SignUpController : MonoBehaviour
{
    [SerializeField]
    Text Id;
    [SerializeField]
    InputField passwd;
    [SerializeField]
    InputField rePasswd;
    [SerializeField]
    GameObject WarningPanel;
    [SerializeField]
    GameObject []Warnings;

    string password="";
    // Update is called once per frame
    void Update()
    {
        if(!Id.text.Contains("@")){
            Warnings[0].gameObject.SetActive(true);
        }else{
            Warnings[0].gameObject.SetActive(false);
        }

    }

    public void OnClick(){
        password=(string)passwd.text;
        if(0!=passwd.text.CompareTo(rePasswd.text)){
            Warnings[1].gameObject.SetActive(true);
        }else{
            Warnings[1].gameObject.SetActive(false);
            //FirebaseAuthManager.Instance.Create(Id,password);
        }
        this.gameObject.SetActive(false);
    }

    public void OnCancelClick (){

    }

}
