using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillController : MonoBehaviour
{

    [SerializeField]
    GameObject SkillPanel;

    private void OnEnable() {
        SkillPanel.SetActive(true);   
    }
}
