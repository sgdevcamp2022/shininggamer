/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIController : MonoBehaviour
{
    UserInfo playertmp;
    Slider hpBar, expBar;
    Image characterPanel;
    Text currentHp, id, level, physicalDefense, magicDefense, evasion,
        power, vitality, intelligence, recognition, talent, speed, luck;
    GameObject textPanel, abilityPanel;

    void Start()
    {
        playertmp = GameObject.Find("Player").GetComponent<UserInfo>();
        textPanel = GameObject.Find("TextPanel");
        abilityPanel = GameObject.Find("abilities");
        physicalDefense = textPanel.transform.Find("PhysicalDefense").GetComponent<Text>();
        magicDefense = textPanel.transform.Find("MagicDefense").GetComponent<Text>();
        evasion = textPanel.transform.Find("Evasion").GetComponent<Text>();
        power = abilityPanel.transform.Find("Power").GetComponent<Text>();
        vitality = abilityPanel.transform.Find("Vitality").GetComponent<Text>();
        intelligence = abilityPanel.transform.Find("Intelligence").GetComponent<Text>();
        recognition = abilityPanel.transform.Find("Recognition").GetComponent<Text>();
        talent = abilityPanel.transform.Find("Talent").GetComponent<Text>();
        speed = abilityPanel.transform.Find("Speed").GetComponent<Text>();
        luck = abilityPanel.transform.Find("Luck").GetComponent<Text>();
        //hpBar = transform.Find("HpSlider").GetComponent<Slider>();
        //expBar = transform.Find("ExpSlider").GetComponent<Slider>();
        //characterPanel = transform.Find("CharacterPanel").GetComponent<Image>();
        //currentHp = transform.Find("Health").GetComponent<Text>();
        //id = transform.Find("ID").GetComponent<Text>();
        //level = transform.Find("Level").GetComponent<Text>();

        Debug.Log(playertmp.CType.PhysicalDefense);

        InitializeStat();
    }

    void InitializeStat()
    {
        physicalDefense.text = playertmp.CType.PhysicalDefense;
        magicDefense.text = playertmp.CType.MagicDefense;
        evasion.text = playertmp.CType.Evasion;
        power.text = playertmp.CType.Power;
        vitality.text = playertmp.CType.Vitality;
        intelligence.text = playertmp.CType.Intellect;
        recognition.text = playertmp.CType.Recognition;
        talent.text = playertmp.CType.Talent;
        speed.text = playertmp.CType.Speed;
        luck.text = playertmp.CType.Luck;
    }


    void Update()
    {
        
    }
}*/
