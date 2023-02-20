//using System.Collections;
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//public class TempController : MonoBehaviour
//{
//    [SerializeField]
//    GameObject Monster1;
//    [SerializeField]
//    GameObject Monster2;
//    [SerializeField]
//    GameObject Monster3;
//    [SerializeField]
//    GameObject Monster4;
//    [SerializeField]
//    GameObject EnemyField;

//    [SerializeField]
//    Sprite[] imgs;
    
//    Dictionary<int,MonsterType> AllMonsterInfo;
//    void Start(){
//        AllMonsterInfo=GameObject.Find("GameObject").GetComponent<FirebaseLoadManager>().LoadAllMonsterInfo();
//    }

//    //아래 onclick을 수현님 씬에서 원 내 triggerEnter로 변경하면 된다.
//    //triggerEnter 함수 안에는 씬 이동 함수를 넣으면 되고,
//    //씬을 시작할때 아래 짜놓은 코드를 넣으면 된다.
//    public void On1Click(){
//        CreateMonster1();
//    }

//    void CreateMonster1(){
//        GameObject Monster1Ins = Instantiate(Monster1);
//        Monster1Ins.transform.position=new Vector3(2,0,0);

//        foreach(KeyValuePair<int,MonsterType> tmp in AllMonsterInfo){
//            if(tmp.Value.Name=="Monster1"){
//                Monster1Ins.GetComponent<MonsterInfo>().Name="Monster1";
//                Monster1Ins.GetComponent<MonsterInfo>().Type=tmp.Value;
//            }
//        }
//        EnemyField.transform.Find("EnemyName").GetComponent<Text>().text=Monster1Ins.GetComponent<MonsterInfo>().Type.Name;
//        EnemyField.transform.Find("LevelText").GetComponent<Text>().text=Monster1Ins.GetComponent<MonsterInfo>().Type.Level;
//        EnemyField.transform.Find("DamageText").GetComponent<Text>().text=Monster1Ins.GetComponent<MonsterInfo>().Type.Damage;
//        float health=Convert.ToSingle(Monster1Ins.GetComponent<MonsterInfo>().Type.Health);
//        EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=health/health;
//        EnemyField.transform.Find("EnemyPanel").transform.Find("Img").GetComponent<Image>().sprite=imgs[0];
//    }

//    void CreateMonster2(){
//        GameObject Monster2Ins = Instantiate(Monster2);
//        Monster2Ins.transform.position=new Vector3(2,0,0);

//        foreach(KeyValuePair<int,MonsterType> tmp in AllMonsterInfo){
//            if(tmp.Value.Name=="Monster2"){
//                Monster2Ins.GetComponent<MonsterInfo>().Name="Monster2";
//                Monster2Ins.GetComponent<MonsterInfo>().Type=tmp.Value;
//            }
//        }
//        //위치 조정 
//        EnemyField.transform.Find("EnemyName").GetComponent<Text>().text=Monster2Ins.GetComponent<MonsterInfo>().Type.Name;
//        EnemyField.transform.Find("LevelText").GetComponent<Text>().text=Monster2Ins.GetComponent<MonsterInfo>().Type.Level;
//        EnemyField.transform.Find("DamageText").GetComponent<Text>().text=Monster2Ins.GetComponent<MonsterInfo>().Type.Damage;
//        float health=Convert.ToSingle(Monster2Ins.GetComponent<MonsterInfo>().Type.Health);
//        EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=health/health;
//        EnemyField.transform.Find("EnemyPanel").transform.Find("Img").GetComponent<Image>().sprite=imgs[1];
//    }
//    public void On2Click(){
//        CreateMonster2();
//    }
//    public void On3Click(){
//        CreateMonster3();
//    }
//    public void On4Click(){
//        CreateMonster4();
//    }

//    void CreateMonster3(){
//        GameObject Monster3Ins = Instantiate(Monster3);
//        Monster3Ins.transform.position=new Vector3(2,0,0);

//        foreach(KeyValuePair<int,MonsterType> tmp in AllMonsterInfo){
//            if(tmp.Value.Name=="Monster3"){
//                Monster3Ins.GetComponent<MonsterInfo>().Name="Monster3";
//                Monster3Ins.GetComponent<MonsterInfo>().Type=tmp.Value;
//            }
//        }
//        //위치 조정 
//        EnemyField.transform.Find("EnemyName").GetComponent<Text>().text=Monster3Ins.GetComponent<MonsterInfo>().Type.Name;
//        EnemyField.transform.Find("LevelText").GetComponent<Text>().text=Monster3Ins.GetComponent<MonsterInfo>().Type.Level;
//        EnemyField.transform.Find("DamageText").GetComponent<Text>().text=Monster3Ins.GetComponent<MonsterInfo>().Type.Damage;
//        float health=Convert.ToSingle(Monster3Ins.GetComponent<MonsterInfo>().Type.Health);
//        EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=health/health;
//        EnemyField.transform.Find("EnemyPanel").transform.Find("Img").GetComponent<Image>().sprite=imgs[2];
//    }
//    void CreateMonster4(){
//        GameObject Monster4Ins = Instantiate(Monster4);
//        Monster4Ins.transform.position=new Vector3(2,0,0);

//        foreach(KeyValuePair<int,MonsterType> tmp in AllMonsterInfo){
//            if(tmp.Value.Name=="Monster4"){
//                Monster4Ins.GetComponent<MonsterInfo>().Name="Monster4";
//                Monster4Ins.GetComponent<MonsterInfo>().Type=tmp.Value;
//            }
//        }
//        //위치 조정 
//        EnemyField.transform.Find("EnemyName").GetComponent<Text>().text=Monster4Ins.GetComponent<MonsterInfo>().Type.Name;
//        EnemyField.transform.Find("LevelText").GetComponent<Text>().text=Monster4Ins.GetComponent<MonsterInfo>().Type.Level;
//        EnemyField.transform.Find("DamageText").GetComponent<Text>().text=Monster4Ins.GetComponent<MonsterInfo>().Type.Damage;
//        float health=Convert.ToSingle(Monster4Ins.GetComponent<MonsterInfo>().Type.Health);
//        EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=health/health;
//        EnemyField.transform.Find("EnemyPanel").transform.Find("Img").GetComponent<Image>().sprite=imgs[3];
//    }
//}
