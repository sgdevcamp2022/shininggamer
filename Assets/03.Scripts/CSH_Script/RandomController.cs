using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomController : MonoBehaviour
{
    [SerializeField]
    Image[] MoveBox;

    private int movePercentage;
    private int successCount = 0;

    List<int> MyNum=new List<int>();
    List<bool> SuccessList=new List<bool>();

    public Sprite successImg;

    public RandomController(int speed)
    {
        movePercentage = speed;
    }

    public int OnRandomPositionNumberClick(){
        PickRandomly();
        RollTheDice();
        ShowResult();

        return successCount;
    }

    void PickRandomly(){
        MyNum.Clear();
        for(int i=0;i<movePercentage;i++){
            int rand = Random.Range(1,101);
            MyNum.Add(rand);
        }
    }

    void RollTheDice(){
        SuccessList.Clear();
        for(int i=0;i<5;i++){
            int rand = Random.Range(1,101);
            if(MyNum.Contains(rand)){
                SuccessList.Add(true);
            }else{
                SuccessList.Add(false);
            }
        }
    }

    void ShowResult()
    {
        for (int i = 0; i < 5; i++)
        {
            if (SuccessList[i])
            {
                Debug.Log("성공");
                successCount++;
            }
            else
            {
                Debug.Log("실패");
            }
        }
    }
}
