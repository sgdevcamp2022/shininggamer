using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SetAttackPanelUIManager : MonoBehaviour
{

    [SerializeField]
    Sprite[] ActiveImg;
    Sprite originalImg; 

    [SerializeField]
    GameObject canvas;
    GraphicRaycaster m_Raycaster;
    EventSystem m_EventSystem;
    PointerEventData m_PointerEventData;

    bool isFirst=true;
    int type=-1;
    // Start is called before the first frame update

    GameObject skillbut;
    void Start(){
        type=SwitchPlayerCharacter();
        StartCoroutine("TouchUIRaycast");
    }
    
    int SwitchPlayerCharacter(){

        switch(GameObject.FindWithTag("Player").GetComponent<UserInfo>().CType.Name){
            case "전사":
                return 0;
            case "마법사":
                return 1;
            case "궁수":
                return 2;
        }

        return -1;
    }

   IEnumerator TouchUIRaycast(){

        while(true){
            bool isSkilling=false;
            m_PointerEventData= new PointerEventData(m_EventSystem);
            m_PointerEventData.position=Input.mousePosition;

            List<RaycastResult> results= new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData,results);

            foreach(RaycastResult hit in results){
                if(0==hit.gameObject.name.CompareTo("Skill")){
                    skillbut=hit.gameObject;
                    if(isFirst){
                        originalImg=skillbut.GetComponent<Image>().sprite;
                        isFirst=false;
                    }
                    
                    skillbut.GetComponent<Image>().sprite=ActiveImg[type];
                    isSkilling=true;
                }
            }
            if(!isSkilling&&skillbut!=null){
                skillbut.GetComponent<Image>().sprite=originalImg;
            }
            yield return null;
        }
    
   }

    private void OnEnable() {
        
        m_Raycaster=canvas.GetComponent<GraphicRaycaster>();
        m_EventSystem=GetComponent<EventSystem>();
    
    }

}
