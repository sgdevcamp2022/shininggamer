using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;

public class FirebaseAuthManager
{
    private static FirebaseAuthManager instance = null;

    public static FirebaseAuthManager Instance{
        get{
            if(instance==null)
                instance=new FirebaseAuthManager();
            return instance;
        }
    }

    private FirebaseAuth auth;
    private FirebaseUser user;

    public Action<bool> LoginState; 
    public string UserId => user.UserId;

    public int option = 2;
    // Start is called before the first frame update
    public void Init(){
        auth=FirebaseAuth.DefaultInstance;
        
        if(auth.CurrentUser!=null)
            Logout();
        auth.StateChanged +=OnChanged;
    }

    void OnChanged(object sender,EventArgs e){

        if(auth.CurrentUser != user){
            bool signed = (auth.CurrentUser != user && auth.CurrentUser!=null);

            if(!signed&&user!=null){
                LoginState?.Invoke(false);
                Debug.Log("로그아웃");
            }

            user=auth.CurrentUser;
            if(signed){
                
                LoginState?.Invoke(true);
                Debug.Log("로그인");
            }
        }
    }

    public void Create(string email,string password){
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return ; //회원 가입 취소
            }

            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return ; //회원가입실패
            }

            FirebaseUser newUser = task.Result;
        });
        
    }

    public void Login(string email,string password){
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("로그인 취소");
                return ;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("로그인 실패");
                return ;
            }

            FirebaseUser newUser = task.Result;

        });
    }

    public void Logout(){
        auth.SignOut();
    }
}
