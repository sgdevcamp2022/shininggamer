using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CharacterType{
    string _CharacterName;
    string _Evasion;
    string _Luck;
    string _MagicDefense;
    string _PhysicalDefense;
    string _Power;
    string _Recognition;
    string _Speed;
    string _Talent;
    string _Vitality;
    string _Intellect;

    public CharacterType(object name,object evasion,object luck,object magicdef, object physicaldef,object power,object rec,object speed, object talent, object vitality, object intellect){
        _CharacterName=name as string;
        _Evasion=evasion as string;
        _Luck=luck as string;
        _MagicDefense=magicdef as string;
        _PhysicalDefense=physicaldef as string;
        _Power=power as string;
        _Recognition=rec as string;
        _Speed=speed as string;
        _Talent=talent as string;
        _Vitality=vitality as string;
        _Intellect=intellect as string;
    }

    public string Name{
        set{
            _CharacterName=value;
        }
        get{
            return _CharacterName;
        }
    }

    public string Luck{
        set{
            _Luck=value;
        }
        get{
            return _Luck;
        }
    }

    public string MagicDefense{
        set{
            _MagicDefense=value;
        }
        get{
            return _MagicDefense;
        }
    }

    public string Recognition{
        set{
            _Recognition=value;
        }
        get{
            return _Recognition;
        }
    }

    public string Talent{
        set{
            _Talent=value;
        }
        get{
            return _Talent;
        }
    }

    public string Speed{
        set{
            _Speed=value;
        }
        get{
            return _Speed;
        }
    }

    public string PhysicalDefense{
        set{
            _PhysicalDefense=value;
        }
        get{
            return _PhysicalDefense;
        }
    }

    public string Evasion{
        set{
            _Evasion=value;
        }
        get{
            return _Evasion;
        }
    }

    public string Vitality{
        set{
            _Vitality=value;
        }
        get{
            return _Vitality;
        }
    }

    public string Power
    {
        set { 
            _Power=value;
        }
        get { 
            return _Power;
        }
    }

    public string Intellect
    {
        set
        {
            _Intellect = value;
        }
        get
        {
            return _Intellect;
        }
    }
}
