using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject swordPrefab;
    public GameObject archorPrefab;
    [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> players = new List<GameObject>();
    public static Manager instance;

    public static float attackTime = 0.5f;
    public const int size = 20;  // TODO enemy 生成位置

    public enum SceneName
    {
        Underground,
        Main
    }
    public SceneName sceneName;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        const int x = 4;
        const int y = 2;
        // 创建Player
        createPerson(new Vector3(-x, -y, 0), swordPrefab, Color.blue, true, true);
        // 创建Company
        createPerson(new Vector3(-x, y, 0), archorPrefab, Color.blue, true);
        // 创建Enemy
        createPerson(new Vector3(x, -y, 0), archorPrefab, Color.red, false);
        // 创建Enemy2
        createPerson(new Vector3(x, y, 0), swordPrefab, Color.red, false);

        StartCoroutine(createEnemy());
    }

    IEnumerator createEnemy()
    {
        yield return new WaitForSeconds(10);

        if (enemys.Count >= 10) StartCoroutine(createEnemy());
        else
        {
            var x = Random.Range(-size, size);
            var y = Random.Range(-size, size);
            // 创建Enemy
            createPerson(new Vector3(x, -y, 0), archorPrefab, Color.red, false);
            // 创建Enemy2
            createPerson(new Vector3(x, y, 0), swordPrefab, Color.red, false);
            StartCoroutine(createEnemy());
        }
    }

    void Update()
    {

    }

    GameObject createPerson(Vector3 pos, GameObject prefab, Color color, bool owner, bool isPlayer = false)
    {
        var temp = Instantiate(prefab, pos, Quaternion.identity);
        if (isPlayer) temp.name = "Player";
        foreach (Transform child in temp.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = color;
                break;
            }
        }

        if (owner) players.Add(temp);
        else enemys.Add(temp);

        temp.GetComponent<IPerson>().Owner = owner;
        if (owner)
        {
            temp.AddComponent<ComputerControll>().enabled = !isPlayer;
            temp.AddComponent<PlayerControll>().enabled = isPlayer;
        }
        else
        {
            temp.AddComponent<ComputerControll>();
        }
        return temp;
    }

    public void playerCheck()
    {
        if (players.Count == 0)
        {
            Debug.Log("Game Over");
            return;
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].name == "Player") return;
        }
        setPlayer(-1, 0);
    }

    public static void setPlayer(int from, int to)
    {
        var players = instance.players;
        players[to].GetComponent<PlayerControll>().enabled = true;
        players[to].GetComponent<ComputerControll>().enabled = false;
        if (from == -1)
        {
            players[to].name = "Player";
        }
        else
        {
            players[from].GetComponent<PlayerControll>().enabled = false;
            players[from].GetComponent<ComputerControll>().enabled = true;

            var temp = players[to].name;
            players[to].name = players[from].name;
            players[from].name = temp;
        }
        // 摄像机锁定
        if (instance.sceneName == Manager.SceneName.Main)
            CameraFollow.instance.target = players[to].transform.position;
    }
}
