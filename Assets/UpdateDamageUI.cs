using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class UpdateDamageUI : MonoBehaviour
{

    [SerializeField]
    GameObject EnemyField;
    MonsterInfo monster;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(monster==null){
            monster= GameObject.Find("Monster1(Clone)").gameObject.GetComponent<MonsterInfo>();
            if(monster==null)
                monster= GameObject.Find("Monster2(Clone)").gameObject.GetComponent<MonsterInfo>();
            float health=Convert.ToSingle(monster.Type.HP);
            EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=monster.CurrHP/health;
        }else{
            float health=Convert.ToSingle(monster.Type.HP);
            EnemyField.transform.Find("DamageSlider").GetComponent<Slider>().value=monster.CurrHP/health;
        }
    }
}
