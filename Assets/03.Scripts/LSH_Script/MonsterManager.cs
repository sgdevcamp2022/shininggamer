// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MonsterManager : MonoBehaviour
// {
//     [SerializeField] GameObject monsterPosBox;
//     [SerializeField] GameObject monsterFocused;

//     int hp;

//     public int Hp
//     {
//         get { return hp; }
//         set { hp = value; }
//     }

//     private void Awake()
//     {
//         switch (this.ToString())
//         {
//             case "Slime":
//                 this.GetComponent<MonsterInfo>().Type = FirebaseLoadManager.AllMonsterInfo[0];
//                 hp = Int32.Parse(FirebaseLoadManager.AllMonsterInfo[0].Health);
//                 break;
//             case "TurtleShell":
//                 this.GetComponent<MonsterInfo>().Type = FirebaseLoadManager.AllMonsterInfo[1];
//                 hp = Int32.Parse(FirebaseLoadManager.AllMonsterInfo[1].Health);
//                 break;
//             default:
//                 break;
//         }
        
//     }

//     private void OnMouseDown()
//     {
//         for (int i = 0; i < 3; i++)
//         {
//             monsterPosBox.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
//         }

//         print(Camera.main);
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Monster")))
//         {
//             FightManager.focusedMonster = hit.transform.gameObject;
//             print("focus " + FightManager.focusedMonster);
//             monsterFocused.SetActive(true);
//         }
//     }
// }
