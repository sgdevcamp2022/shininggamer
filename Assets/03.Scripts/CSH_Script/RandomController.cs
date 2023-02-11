using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomController : MonoBehaviour
{
    int movePercentage;
    Image[] moveImages;
    int successCount = 0;

    List<int> MyNum;
    List<bool> SuccessList;
    [SerializeField] Sprite successImage;

    void Awake()
    {
        MyNum = new List<int>();
        SuccessList = new List<bool>();
        moveImages = new Image[5];
        movePercentage = 80;

        for(int i = 0; i < 5; i++)
            moveImages[i] = transform.GetChild(i).GetComponent<Image>();
    }

    public int OnRandomPositionNumberClick()
    {
        PickRandomly();
        RollTheDice();
        ShowResult();

        return successCount;
    }

    void PickRandomly()
    {
        MyNum.Clear();

        for(int i = 0; i < movePercentage; i++)
        {
            int rand = Random.Range(1,101);
            MyNum.Add(rand);
        }
    }

    void RollTheDice()
    {
        SuccessList.Clear();

        for(int i = 0; i < 5; i++)
        {
            int rand = Random.Range(1,101);
            
            if(MyNum.Contains(rand))
                SuccessList.Add(true);
            else
                SuccessList.Add(false);
        }
    }

    void ShowResult()
    {
        for (int i = 0; i < 5; i++)
        {
            if (SuccessList[i])
            {
                moveImages[i].sprite = successImage;
                successCount++;
            }
        }
    }
}
