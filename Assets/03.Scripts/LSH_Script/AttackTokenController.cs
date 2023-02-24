// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class AttackTokenController : MonoBehaviour
// {
//     int successCount;
//     int strength;
//     int recognition;
//     int intelligence;

//     public GameObject[] attackTokens;
//     List<int> myNum;
//     List<bool> successList;
//     [Tooltip("0: Power, 1: Recognition, 2: Intelligence")]
//     [SerializeField] List<Sprite> successImage;
//     [Tooltip("0: Power, 1: Recognition, 2: Intelligence")]
//     [SerializeField] List<Sprite> failImage;
//     public GameObject fightManager;
//     UserInfo playerInfo;

//     public int Strength
//     {
//         get
//         {
//             return strength;
//         }
//         set
//         {
//             strength = value;
//         }
//     }

//     public int Recognition
//     {
//         get
//         {
//             return recognition;
//         }
//         set
//         {
//             recognition = value;
//         }
//     }

//     public int Intelligence
//     {
//         get
//         {
//             return intelligence;
//         }
//         set
//         {
//             intelligence = value;
//         }
//     }
    
//     public int SuccessCount
//     {
//         get
//         {
//             return successCount;
//         }
//         set
//         {
//             successCount = value;
//         }
//     }

//     void Awake()
//     {
//         successCount = 0;
//         myNum = new List<int>();
//         successList = new List<bool>();
//         attackTokens = new GameObject[4];
//         playerInfo = GameObject.Find("Player").GetComponent<UserInfo>();

//         for (int i = 0; i < attackTokens.Length; i++)
//             attackTokens[i] = transform.GetChild(i).gameObject;
//     }

//     public void OnRandomPositionNumberClick()
//     {
//         PickRandomly();
//         RollTheDice();
//         StopAllCoroutines();
//         StartCoroutine(ShowResult());
//     }

//     void PickRandomly()
//     {
//         myNum.Clear();

//         int attackType = 0;
//         //switch (playerInfo.CType.Name)
//         //{
//         //    case "����":
//         //        attackType = strength;
//         //        break;
//         //    case "�ü�":
//         //        attackType = recognition;
//         //        break;
//         //    case "������":
//         //        attackType = intelligence;
//         //        break;
//         //}
//         attackType = strength;

//         for (int i = 0; i < attackType; i++)
//         {
//             int rand = Random.Range(1, 101);
//             myNum.Add(rand);
//         }
//     }

//     void RollTheDice()
//     {
//         successList.Clear();

//         for (int i = 0; i < attackTokens.Length; i++)
//         {
//             int rand = Random.Range(1, 101);

//             if (myNum.Contains(rand))
//                 successList.Add(true);
//             else
//                 successList.Add(false);
//         }
//     }

//     IEnumerator ShowResult()
//     {
//         yield return new WaitForSeconds(1.5f);

//         int imageIndex = -1;
//         switch (playerInfo.CType.Name)
//         {
//             case "����":
//                 imageIndex = 0;
//                 break;
//             case "�ü�":
//                 imageIndex = 1;
//                 break;
//             case "������":
//                 imageIndex = 2;
//                 break;
//         }

//         for (int i = 0; i < attackTokens.Length; i++)
//         {
//             attackTokens[i].transform.GetChild(imageIndex).GetComponent<Image>().enabled = true;
//         }
            

//         for (int i = 0; i < attackTokens.Length; i++)
//         {
//             GetComponent<AudioSource>().Play();

//             if (successList[i])
//             {
//                 attackTokens[i].transform.GetChild(imageIndex).GetComponent<Image>().sprite = successImage[imageIndex];
//                 successCount++;
//                 yield return new WaitForSeconds(0.3f);
//             }
//             else
//             {
//                 attackTokens[i].transform.GetChild(imageIndex).GetComponent<Image>().sprite = failImage[imageIndex];
//                 yield return new WaitForSeconds(0.3f);
//             }
//         }

//         //gameManager.GetComponent<GameManager>().SetMoveCount(successCount);
//         fightManager.GetComponent<FightManager>().PlayerAttackByToken(4 - successCount);
//         yield return new WaitForSeconds(1.5f);

//         for (int i = 0; i < attackTokens.Length; i++)
//             attackTokens[i].transform.GetChild(imageIndex).GetComponent<Image>().enabled = false;
//     }
// }
