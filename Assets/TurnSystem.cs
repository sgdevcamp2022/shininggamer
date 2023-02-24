using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    
    List<string> turn;

    UserInfo player;
    MonsterInfo monster;
    // Start is called before the first frame update
    void Start()
    {
        turn=new List<string>();
        player=GameObject.Find("Player").GetComponent<UserInfo>();
        monster=GameObject.Find("Monster1").GetComponent<MonsterInfo>();
    }

    void Turn(){
        //int fir=player.CType.Speed>monster.Type.Speed? 1:0;
    }

    
}
