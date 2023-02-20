using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class HexUnit : MonoBehaviourPunCallbacks
{
    public PhotonView pv;

    public static List<HexUnit> unitPrefab;

    List<HexCell> pathToTravel;
    const float travelSpeed = 4f;
    const float rotationSpeed = 180f;

    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if (location)
                location.Unit = null;

            location = value;
            value.Unit = this;
            transform.localPosition = value.Position;
        }
    }
    HexCell location;

    public float Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }
    float orientation;

    public int UnitType
    {
        get
        {
            return unitType;
        }
        set
        {
            unitType = value;
        }
    }
    int unitType;

    public string CharacterName
    {
        get
        {
            return characterName;
        }
        set
        {
            characterName = value;
        }
    }
    string characterName;

    public int Turn
    {
        get
        {
            return turn;
        }
        set
        {
            turn = value;
        }
    }
    int turn;

    public override void OnEnable()
    {
        if (location)
            transform.localPosition = location.Position;
    }

    public void Travel(List<HexCell> path)
    {
        Location = path[path.Count - 1];
        pathToTravel = path;
        pv.RPC("RPCTravelCoroutine", RpcTarget.All);
    }

    [PunRPC]
    void RPCTravelCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        GetComponent<Animator>().SetBool("IsWalking", true);
        GetComponent<AudioSource>().Play();

        Vector3 a, b, c = pathToTravel[0].Position;
        transform.localPosition = c;
        yield return LookAt(pathToTravel[1].Position);
        float t = Time.deltaTime * travelSpeed;

        for (int i = 1; i < pathToTravel.Count; i++)
        {
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + pathToTravel[i].Position) * 0.5f;

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                orientation = transform.localRotation.eulerAngles.y;
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }

            t -= 1f;
        }

        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;

        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            orientation = transform.localRotation.eulerAngles.y;
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }

        transform.localPosition = location.Position;
        ListPool<HexCell>.Add(pathToTravel);
        pathToTravel = null;

        GetComponent<Animator>().SetBool("IsWalking", false);
        GetComponent<AudioSource>().Stop();
    }

    IEnumerator LookAt(Vector3 point)
    {
        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);

        float angle = Quaternion.Angle(fromRotation, toRotation);

        if(angle > 0f)
        {
            float speed = rotationSpeed / angle;

            for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed)
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
        }

        transform.LookAt(point);
        orientation = transform.localRotation.eulerAngles.y;
    }

    /*void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("KSH_FightScene");
    }*/

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    public void Die()
    {
        location.Unit = null;
        Destroy(gameObject);
    }

    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
        writer.Write(orientation);
        writer.Write((float)unitType);
    }

    public static void Load(BinaryReader reader, HexGrid grid)
    {
        HexCoordinates coordinates = HexCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        float unitType = reader.ReadSingle();
        grid.AddUnit(Instantiate(unitPrefab[(int)unitType]), grid.GetCell(coordinates), orientation, (int)unitType);
    }

    public bool IsValidDestination(HexCell cell)
    {
        if(cell.Unit != null && !(cell.Unit.CompareTag("Player")))
        {
            if (cell.Unit.tag.Substring(0, 7) == "Monster")
                return true;
            else
                return false;
        }

        return true;
    }
}