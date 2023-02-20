using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSH_Monster : MonoBehaviour
{
    public int power, dex, luk, mp, hp;
    public int damage;
    private void Awake()
    {
        hp = 30;
    }

    public IEnumerator HitMotion()
    {
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("Hit");
        }
        OnDamage(damage);
        yield return null;
    }

    void OnDamage(int damage)
    {
        hp =- damage;
        if(hp <= 0)
        {
            hp = 0;
            GetComponent<Animator>().SetTrigger("Die");
        }   
    }
}
