using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillPositionActiveController : MonoBehaviour
{
    [SerializeField]
    Text DamageType;
    [SerializeField]
    Text Damage;
    [SerializeField]
    Text AccurPer;

    UserInfo player1;
    [SerializeField]
    GameObject FindSlotGrid;
    Image[] slotChild;
    Color SkillColor;
    
    [SerializeField]
    Sprite[] slotImg;
    [SerializeField]
    Sprite[] originalImg;

    [SerializeField]
    Button AttackSkillBut;
    int CharacterType=-1;
    private void Awake() {
        player1=GameObject.FindWithTag("Player").GetComponent<UserInfo>();
        CharacterType=SwitchPlayerCharacter();
        slotChild=FindSlotGrid.transform.GetComponentsInChildren<Image>();
        AttackSkillBut.gameObject.GetComponent<Image>().sprite=originalImg[CharacterType];
    }


    int SwitchPlayerCharacter(){

        switch(player1.CType.Name){
            case "전사":
                return 0;
            case "마법사":
                return 1;
            case "궁수":
                return 2;
        }

        return -1;
    }

    private void OnEnable() {
        
        if(player1.CType.Name=="전사"||player1.CType.Name=="궁수"){
            DamageType.text="물리 데미지";
            if(ColorUtility.TryParseHtmlString("#93A0D9",out SkillColor))
                Damage.color=SkillColor;
            foreach(Image img in slotChild){
                if(player1.CType.Name=="전사"){
                    img.sprite=slotImg[0];
                    AccurPer.text=player1.CType.Power+"%";
                }
                else{
                    img.sprite=slotImg[2];
                    AccurPer.text=player1.CType.Recognition+"%";
                }
            }
        }else{
            DamageType.text="마법 데미지";
            if(ColorUtility.TryParseHtmlString("#963CB9",out SkillColor))
                Damage.color=SkillColor;
            foreach(Image img in slotChild){
                img.sprite=slotImg[1];
            }
            AccurPer.text=player1.CType.Intellect+"%";
        }
        Damage.text=player1.Damage.ToString();
    }

}
