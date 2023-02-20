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
    public static Dictionary<int,MonsterType> AllMonsterInfo;
    
    private void Start()
    {
        CharacterOp=new Dictionary<int,CharacterType>();
        AllMonsterInfo=new Dictionary<int,MonsterType>();
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
       

        //if(CharacterOp==null)
            LoadAllCharacterInfo();
        //if(AllMonsterInfo==null)
            LoadAllMonsterInfo();
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
    public Dictionary<int,MonsterType> LoadAllMonsterInfo(){
        databaseReference.Child("MonsterType").GetValueAsync().ContinueWith(task =>
        { 
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
            
                int i=0;
                foreach(DataSnapshot data in snapshot.Children){
                    IDictionary types =(IDictionary)data.Value;
                    MonsterType tmp = new MonsterType(types["MonsterName"],types["Damage"],types["Evasion"],types["MagicDefense"],types["PhysicalDefense"],types["Level"],types["Speed"],types["Health"]);          
                    AllMonsterInfo.Add(i,tmp);
                    i++;
                }
            }
        });

        return AllMonsterInfo;
    }
}
