using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSH_Player : MonoBehaviour
{
    Animator playerAnim;
    Transform playerTr;

    public enum PlayerStatus {   };
    public enum PlayerJop { warrior, archeor, theif, magicion };
    public enum PlayerAnimStat { attack, die, hit };

    float power, dex, luk, mp, hp;

    PlayerJop playerJop;

    public GameObject playerStatUI;
    Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    


    void PlayerBaseSetting()
    {
        
        switch (playerJop)
        {
            case PlayerJop.warrior:
                break;
            case PlayerJop.archeor:
                break;
            case PlayerJop.theif:
                break;
            case PlayerJop.magicion:
                break;
        }
    }

    public void MyTurn()
    {
        playerStatUI.SetActive(true);
    }



    public void PlayerAttack( )
    {

        StartCoroutine(IEPlayerAttack());
        playerStatUI.SetActive(false);
        
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

        
        moveTime = 0;
        yield return new WaitForSeconds(1f);
        playerAnimator.SetTrigger("Jump");
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        { 


        }
        while (moveTime <= moveMaxTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, Time.deltaTime * 7f);
            moveTime += Time.deltaTime;
            yield return null;
        }
        playerStatUI.SetActive(true);
        yield return null;
    }

    IEnumerator MonsterAttacted()
    {
        //monsterDamageLogic
        yield return null;
    }
}
