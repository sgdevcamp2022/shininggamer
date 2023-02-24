using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CreateMonsterManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] Monsters;
    [SerializeField]
    GameObject EnemyField;

    [SerializeField]
    Sprite[] imgs;
    
    bool isFirst = false;
    
    Dictionary<int,MonsterType> AllMonsterInfo;
    UserInfo userInfo;

    void Start()
    {
        userInfo = GameObject.Find("Player").GetComponent<UserInfo>();
        Debug.Log(userInfo.CharacterType);
    }

    private void OnEnable() {
        
        //Destroy(GameObject.FindWithTag("Player").GetComponent<HexUnit>());
        AllMonsterInfo=FirebaseLoadManager.AllMonsterInfo;
    }

    void Update(){
        if(AllMonsterInfo==null){
            AllMonsterInfo=FirebaseLoadManager.AllMonsterInfo;
        }
        if(AllMonsterInfo!=null&&!isFirst){
            UserInfo playertmp =GameObject.FindWithTag("Player").GetComponent<UserInfo>();
            foreach(KeyValuePair<int,MonsterType> i in FirebaseLoadManager.AllMonsterInfo){
                Debug.Log(i.Value.Name+" "+playertmp.Monsters);
                if(i.Value.Name==playertmp.Monsters){
                        CreateMonster(i.Key,i.Value);
                }
            }
            isFirst=true;
        }
        
    }

    
   void CreateMonster(int i,MonsterType monster){

       GameObject MonsterIns = Instantiate(Monsters[i]);
       MonsterIns.transform.position=new Vector3(0f,-0.052f,5f);
       MonsterIns.transform.Rotate(new Vector3(0, -180, 0));
       MonsterIns.GetComponent<MonsterInfo>().Name=monster.Name;
       MonsterIns.GetComponent<MonsterInfo>().Type= monster;
       EnemyField.transform.Find("EnemyName").GetComponent<Text>().text=monster.Name;
       EnemyField.transform.Find("LevelText").GetComponent<Text>().text=monster.Level;
       EnemyField.transform.Find("DamageText").GetComponent<Text>().text=monster.Damage;
       float health=Convert.ToSingle(monster.HP);
       EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=health/health;
       EnemyField.transform.Find("EnemyPanel").transform.Find("Image").GetComponent<Image>().sprite=imgs[i];
   
   }
}
