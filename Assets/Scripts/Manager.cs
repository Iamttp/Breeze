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

    void Start()
    {
        // 创建Player
        var tempPlayer = Instantiate(archorPrefab);
        tempPlayer.name = "Player";
        tempPlayer.AddComponent<PlayerControll>();
        foreach (Transform child in tempPlayer.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            }
        }
        players.Add(tempPlayer);

        // 创建Enemy
        var tempEnemy = Instantiate(swordPrefab);
        tempEnemy.AddComponent<ComputerControll>().followDis = 5; // TODO followDis
        foreach (Transform child in tempEnemy.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            }
        }
        enemys.Add(tempEnemy);

        foreach(var obj in players)
        {
            obj.GetComponent<IPerson>().owner = true;
        }
        
        foreach(var obj in enemys)
        {
            obj.GetComponent<IPerson>().owner = false;
        }
    }

    void Update()
    {

    }
}
