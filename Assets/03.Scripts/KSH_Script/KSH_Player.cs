using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSH_Player : MonoBehaviour
{
    Animator playerAnim;
    Transform playerTr;

    public enum PlayerJop { warrior, archor, magician };
    public enum PlayerAnimStat { attack, die, hit };

    float power, dex, luk, mp, hp;
    public bool playerAttackEnd;

    PlayerJop playerJop;
    Animator playerAnimator;

    [SerializeField]
    GameObject targetMonster;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        targetMonster = GameObject.Find("KSH_Slime");
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
        StartCoroutine(HitDamage(damage));

        
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

    public int damage = 10;

    public IEnumerator OnDamage(int damage, GameObject target)
    {
        while (true)
        {
            yield return null;
        }
    }

    public IEnumerator HitDamage(int damage)
    {
        targetMonster.GetComponent<KSH_Monster>().damage = damage;
        StartCoroutine(targetMonster.GetComponent<KSH_Monster>().HitMotion()); 
        yield return null;
    }

}
