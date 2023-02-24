using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class KSH_Player : MonoBehaviour
{
    Animator playerAnim;
    Transform playerTr;
    public bool isTurn;
    public enum PlayerJop { warrior, archor, magician };
    public enum PlayerAnimStat { attack, die, hit };

    float power, dex, luk, mp, hp;
    public bool playerAttackEnd;

    PlayerJop playerJop;
    Animator playerAnimator;
    public int damage;

    GameObject targetMonster;

    UserInfo player;

    [SerializeField]
    AudioSource knifeBgm;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        player=GameObject.Find("Player").GetComponent<UserInfo>();
        damage = 10;
    }

    void Update(){
        if(targetMonster==null){
            targetMonster = GameObject.Find("Monster1(Clone)").gameObject;
            Debug.Log(targetMonster);
        }
    }
    
    void PlayerBaseSetting()
    {
        switch (playerJop)
        {
            case PlayerJop.warrior:
                break;
            case PlayerJop.archor:
                break;
            case PlayerJop.magician:
                break;
        }
    }
    public void PlayerAttack( )
    {
        StartCoroutine(IEPlayerAttack());
    }



    IEnumerator IEPlayerAttack()
    {
        float moveMaxTime = 1f;
        float moveTime = 0;
        Vector3 originPos = transform.position;
        Vector3 monsterPos = new Vector3(0, -0.05f, 3.5f);
        playerAnimator.SetTrigger("Jump");
        while (moveTime <= moveMaxTime)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, monsterPos, Time.deltaTime*7f);
            moveTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.33f);
        playerAnimator.SetTrigger("Attack");
        StartCoroutine(HitDamage(player.Damage));
        knifeBgm.Play();
        moveTime = 0;
        yield return new WaitForSeconds(1f);
        playerAnimator.SetTrigger("Jump");
 
        while (moveTime <= moveMaxTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, Time.deltaTime * 7f);
            moveTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
        playerAttackEnd = true;
    }


    public IEnumerator OnDamage(int damage, GameObject target)
    {
        while (true)
        {
            yield return null;
        }
    }

    public IEnumerator HitDamage(int damage)
    {
        bool isKilling=false;
        Debug.Log("1");
        StartCoroutine(targetMonster.GetComponent<MonsterInfo>().HitMotion(player.Damage)); 
        yield return null;
    }

}
