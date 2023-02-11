using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    string _id;
    CharacterType _ctype;
    string findCtype;

    public UserInfo(string id){
        _id=id;
        _ctype=null;
        findCtype="";
    }
    
    public string ID{
        get{
            return _id;
        }
        set{
            _id=value;
        }
    }

    public CharacterType CType{
        get{
            return _ctype;
        }
        set{
            _ctype=value;
        }
    } 
}
