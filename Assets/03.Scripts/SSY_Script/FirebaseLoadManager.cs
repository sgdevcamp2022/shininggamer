using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;

public class FirebaseLoadManager : MonoBehaviour
{

    private DatabaseReference databaseReference;
    public Dictionary<int,CharacterType> CharacterOp;
    //Dictionary 변수가 여기에 있음 안됌. 총 연결 시 Find로 찾아서 연결로 변경 예정

    private void Start()
    {
        CharacterOp=new Dictionary<int,CharacterType>();
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
       
        LoadAllCharacterInfo();

    }

    void LoadAllCharacterInfo(){
        databaseReference.Child("CharacterType").GetValueAsync().ContinueWith(task =>
        { 
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
            
                int i=0;
                foreach(DataSnapshot data in snapshot.Children){
                    IDictionary types =(IDictionary)data.Value;
                    CharacterType tmp = new CharacterType(types["CharacterName"],types["Evasion"],types["Luck"],types["MagicDefense"],types["PhysicalDefense"],types["Power"],types["Recognition"],types["Speed"],types["Talent"],types["Vitality"],types["Intellect"]);          
                    CharacterOp.Add(i,tmp);
                    i++;
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(CharacterOp.Count>=2){
            for(int i=0;i<CharacterOp.Count;i++){
                Debug.Log(CharacterOp[i].Name);
            }
        }
    }
}
