using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KSH_ButtonOver : MonoBehaviour
{
    enum ToDoType {Nomal, Skill, Run}
    public GameObject actionNameText;
    public GameObject typeText;
    public GameObject damageText;
    public GameObject persentageText;

    ToDoType todo;
    private void Start()
    {
        TypeSetting();
    }

    void OnMouseEnter()
    {
        

    }

    void ActionStatSetText()
    {

    }

    void TypeSetting()
    {
        switch (todo)
        {
            case ToDoType.Nomal:
                break;
            case ToDoType.Skill:
                break;
            case ToDoType.Run:
                break;
        }
    }
}