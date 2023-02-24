using UnityEngine;
using UnityEngine.UI;

public class MonsterEncoutManager : MonoBehaviour
{
    public Sprite[] monsterSprite = new Sprite[4];
    public Text monsterName;
    public Image monsterImage;

    [SerializeField]
    GameObject tmp;
    HexGrid hexGrid;
    public bool IsFight
    {
        get
        {
            return isFight;
        }
    }
    bool isFight;

    void Start(){
        hexGrid = GameObject.Find("Hex Grid").GetComponent<HexGrid>();
    }

    public void MonsterPopUp(Collider monster)
    {
        if(monster.CompareTag("Monster 1"))
        {
            monsterName.text = monster.name.Replace("(Clone)","");
            monsterImage.sprite = monsterSprite[0];
        }
        else if(monster.CompareTag("Monster 2"))
        {
            monsterName.text = monster.name;
            monsterImage.sprite = monsterSprite[1];
        }
        else if(monster.CompareTag("Monster 3"))
        {
            monsterName.text = monster.name;
            monsterImage.sprite = monsterSprite[2];
        }
        else if(monster.CompareTag("Monster 4"))
        {
            monsterName.text = monster.name;
            monsterImage.sprite = monsterSprite[3];
        }
    }

    public void Fight()
    {
        isFight = true;
    }

    public void Run()
    {
        hexGrid.SelectedUnit.IsMonsterEncount=false;
        tmp.gameObject.SetActive(false);
    }
}
