using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject swordPrefab;
    public GameObject archorPrefab;
    public List<GameObject> enemys = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();
    public static Manager instance;

    void Awake()
    {
        instance = this;
    }

    GameObject createPerson(Vector3 pos, GameObject prefab, Color color, bool isPlayer = false)
    {
        var temp = Instantiate(prefab, pos, Quaternion.identity);
        if (isPlayer) temp.name = "Player";
        if (isPlayer) temp.AddComponent<PlayerControll>();
        else temp.AddComponent<ComputerControll>();
        foreach (Transform child in temp.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = color;
                break;
            }
        }
        return temp;
    }

    void Start()
    {
        const int x = 4;
        const int y = 2;
        // 创建Player
        players.Add(createPerson(new Vector3(-x, -y, 0), archorPrefab, Color.blue, true));
        // 创建Company
        players.Add(createPerson(new Vector3(-x, y, 0), swordPrefab, Color.blue / 2));
        // 创建Enemy
        enemys.Add(createPerson(new Vector3(x, -y, 0), swordPrefab, Color.red));
        // 创建Enemy2
        enemys.Add(createPerson(new Vector3(x, y, 0), archorPrefab, Color.red));

        foreach (var obj in players)
        {
            obj.GetComponent<IPerson>().owner = true;
        }
        foreach (var obj in enemys)
        {
            obj.GetComponent<IPerson>().owner = false;
        }
    }

    void Update()
    {

    }
}
