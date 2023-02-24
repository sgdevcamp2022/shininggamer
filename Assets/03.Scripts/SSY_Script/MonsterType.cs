using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterType
{
    string _MonsterName;
    string _Evasion;
    string _PhysicalDefense;
    string _MagicDefense;
    string _Level;
    string _Health;
    string _Speed;
    string _Damage;

    public MonsterType(object MonsterName,object Damage,object Evasion, object MagicDefense, object PhysicalDefense, object Level, object Speed, object Health){
        _MonsterName=MonsterName as string;
        _Damage=Damage as string;
        _Evasion=Evasion as string;
        _MagicDefense = MagicDefense as string;
        _PhysicalDefense=PhysicalDefense as string;
        _Level=Level as string;
        _Speed=Speed as string;
        _Health=Health as string;
    }

    public string Name{
        get{
            return _MonsterName;
        }
        set{
            _MonsterName=value;
        }
    }

    public string Damage{
        get{
            return _Damage;
        }
        set{
            _Damage=value;
        }
    }
    public string HP{
        get{
            return _Health;
        }
        set{
            _Health=value;
        }
    }
    public string Level{
        get{
            return _Level;
        }   
        set{
            _Level=value;
        }
    }

    public string Speed{
        get{
            return _Speed;
        }   
        set{
            _Speed=value;
        }
    }
}
