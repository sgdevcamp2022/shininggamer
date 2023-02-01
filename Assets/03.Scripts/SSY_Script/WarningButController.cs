using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningButController : MonoBehaviour
{
    [SerializeField]
    GameObject WarningUI;

    public void OnClick(){
        WarningUI.SetActive(false);
    }
}
