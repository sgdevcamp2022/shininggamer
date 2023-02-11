using UnityEngine;
using UnityEngine.UI;

public class BattleWindow : MonoBehaviour
{
    public void StartBattle()
    {

    }

    public void RunFromBattle()
    {
        int i = 0;

        while(i < 2)
            Destroy(transform.GetChild(0).gameObject);
    }
}
