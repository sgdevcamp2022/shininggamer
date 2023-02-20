using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField]
    string MonsterName;
    [SerializeField]
    Sprite MonsterImg;
    [SerializeField]
    MonsterType type;

    public string Name{
        get{
            return MonsterName;
        }
        set{
            MonsterName=value;
        }
    }
    
    public MonsterType Type{
        get{
            return type;
        }

        set{
            type=value;
        }
    }
}
