using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CharacterUIControllerFight : MonoBehaviour
{
    public Sprite[] characterPanels = new Sprite[3];
    public Text CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = value;
        }
    }

    UserInfo player;
    Slider hpBar, expBar;
    Image characterPanel;
    Text currentHp, id, level, physicalDefense, magicDefense, evasion,
        power, vitality, intelligence, recognition, talent, speed, luck,
        hp, exp;
    GameObject textPanel, abilityPanel;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<UserInfo>();
        textPanel = GameObject.Find("TextPanel");
        abilityPanel = GameObject.Find("abilities");
        id = GameObject.Find("ID").GetComponent<Text>();
        physicalDefense = textPanel.transform.Find("PhysicalDefense").GetComponent<Text>();
        magicDefense = textPanel.transform.Find("MagicDefense").GetComponent<Text>();
        evasion = textPanel.transform.Find("Evasion").GetComponent<Text>();
        level = textPanel.transform.Find("Level").GetComponent<Text>();
        power = abilityPanel.transform.Find("Power").GetComponent<Text>();
        vitality = abilityPanel.transform.Find("Vitality").GetComponent<Text>();
        intelligence = abilityPanel.transform.Find("Intelligence").GetComponent<Text>();
        recognition = abilityPanel.transform.Find("Recognition").GetComponent<Text>();
        talent = abilityPanel.transform.Find("Talent").GetComponent<Text>();
        speed = abilityPanel.transform.Find("Speed").GetComponent<Text>();
        luck = abilityPanel.transform.Find("Luck").GetComponent<Text>();
        hpBar = GameObject.Find("HpSlider").GetComponent<Slider>();
        expBar = GameObject.Find("ExpSlider").GetComponent<Slider>();
        hp = hpBar.transform.Find("Hp Text").GetComponent<Text>();
        exp = expBar.transform.Find("Exp Text").GetComponent<Text>();
        currentHp = GameObject.Find("Current Hp").GetComponent<Text>();
        characterPanel = GameObject.Find("CharacterPanel").GetComponent<Image>();
    }

    public void InitializeStat(int charImgIndex)
    {
        try{
            id.text = player.ID;
            physicalDefense.text = player.CType.PhysicalDefense;
            magicDefense.text = player.CType.MagicDefense;
            evasion.text = player.CType.Evasion;
            level.text = player.CType.Level;
            power.text = player.CType.Power;
            vitality.text = player.CType.Vitality;
            intelligence.text = player.CType.Intellect;
            recognition.text = player.CType.Recognition;
            talent.text = player.CType.Talent;
            speed.text = player.CType.Speed;
            luck.text = player.CType.Luck;

            currentHp.text = player.CType.HP;
            hpBar.maxValue = int.Parse(player.CType.HP);
            hpBar.value = int.Parse(player.CType.HP);
            hp.text = currentHp.text + " / " + player.CType.HP;
            expBar.maxValue = int.Parse(player.CType.Exp);
            expBar.value = 0;
            exp.text = "0 / " + player.CType.Exp;
            characterPanel.sprite = characterPanels[charImgIndex];
        }catch(Exception e){

        }
        
    }

}
