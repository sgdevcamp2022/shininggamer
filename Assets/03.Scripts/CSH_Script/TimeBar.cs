using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimeBar : MonoBehaviour
{
    Image[] timeImages;
    [SerializeField] Image dayImagePrefab;
    [SerializeField] Image nightImagePrefab;
    PhotonView pv;

    int timeBlock = 0;
    int dayNightCount = 0;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        timeImages = new Image[15];

        for (int i = 0; i < 15; i++)
        {
            if(i % 5 == 0)
                timeBlock++;

            if (timeBlock % 2 != 0)
            {
                timeImages[i] = dayImagePrefab;
                CreateDayImage();
            }
            else if (timeBlock % 2 == 0)
            {
                timeImages[i] = nightImagePrefab;
                CreateNightImage();
            }
        }
    }

    void CreateDayImage()
    {
        Image timeBlock = Instantiate<Image>(dayImagePrefab);
        timeBlock.transform.SetParent(transform);
    }

    void CreateNightImage()
    {
        Image timeBlock = Instantiate<Image>(nightImagePrefab);
        timeBlock.transform.SetParent(transform);
    }

    public void TimePass()
    {
        pv.RPC("RPCTimePass", RpcTarget.All);
    }

    [PunRPC]
    void RPCTimePass()
    {
        Destroy(transform.GetChild(0).gameObject);

        if (dayNightCount % 5 == 0)
            timeBlock++;

        if (timeBlock % 2 != 0)
        {
            CreateDayImage();
            dayNightCount++;
        }
        else if (timeBlock % 2 == 0)
        {
            CreateNightImage();
            dayNightCount++;
        }
    }
}
