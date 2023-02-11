using UnityEngine;
using UnityEngine.UI;

public class Concentration : MonoBehaviour
{
    Image[] concentImages;
    [SerializeField] Sprite filledConcentration;
    [SerializeField] Sprite unFilledConcentration;

    public int totalConcentration;

    void Start()
    {
        totalConcentration = 5;
        concentImages = new Image[5];

        for(int i = 0; i < totalConcentration; i++)
        {
            concentImages[i] = transform.GetChild(i).GetComponent<Image>();
            concentImages[i].sprite = filledConcentration;
        }
    }

    public void ConcentImageChange()
    {
        for (int i = 0; i < totalConcentration; i++)
            concentImages[i].sprite = filledConcentration;

        for (int i = totalConcentration; i < 5; i++)
            concentImages[i].sprite = unFilledConcentration;
    }
}