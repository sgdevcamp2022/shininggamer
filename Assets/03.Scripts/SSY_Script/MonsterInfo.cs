using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField]
    string MonsterName;
    [SerializeField]
    Sprite MonsterImg;
    [SerializeField]
    MonsterType type;
    [SerializeField]
    AudioSource dieBgm;
    public int CurrHP;
    public bool isDie=false;
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
            CurrHP=int.Parse(type.HP);
        }
    }
    public bool isTurn;

    public IEnumerator HitMotion(int damage)
    {   
        Debug.Log("2");
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("Hit");
            yield return new WaitForSeconds(0.5f);
            dieBgm.Play();
        }
        isDie=OnDamage(damage);
        yield return new WaitForSeconds(2f);
        if(!isDie){
            GetComponent<Animator>().SetTrigger("Idle");
        }
        yield return null;
    }

    bool OnDamage(int damage)
    {
        CurrHP = CurrHP - damage;
        Debug.Log(damage);
        if(CurrHP <= 0)
        {
            CurrHP = 0;
            GetComponent<Animator>().SetBool("isDie",true);
            return true;   
        }

        return false;
    }

}
