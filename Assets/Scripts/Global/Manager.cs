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

    public bool isNet;
    private Queue<Message> msgQ;
    private Queue<Message> importMsgQ;
    public Dictionary<int, GameObject> netIdToObj = new Dictionary<int, GameObject>();

    void Awake()
    {
        instance = this;
        if (isNet)
        {
            if (NetUtil.instance.startConnect())
            {
                Debug.Log("连接成功!");
            }
            else
            {
                Debug.Log("IP或者端口号错误...，服务器程序未启动");
                isNet = false;
            }
        }
    }

    void Start()
    {
        if (!isNet)
        {
            Vector2 playerPos = new Vector2();
            playerPos.x = 4;
            playerPos.y = 2;

            // 创建Player
            createPerson(new Vector3(-playerPos.x, -playerPos.y, 0), swordPrefab, Color.blue, true, true);
            // 创建Company
            createPerson(new Vector3(-playerPos.x, playerPos.y, 0), archorPrefab, Color.blue, true);
            // 创建Enemy
            createPerson(new Vector3(playerPos.x, -playerPos.y, 0), archorPrefab, Color.red, false);
            // 创建Enemy2
            createPerson(new Vector3(playerPos.x, playerPos.y, 0), swordPrefab, Color.red, false);

            StartCoroutine(createEnemy());
        }
        else
        {
            msgQ = NetUtil.instance.msgQ;
            importMsgQ = NetUtil.instance.importMsgQ;
        }
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

    json1 id;
    json2 playerPos;
    json3 otherInfo;
    private Message lastMsg = null;

    private void FixedUpdate()
    {
        if (!isNet) return;

        Message msg = null;
        lock (importMsgQ)
        {
            if (importMsgQ.Count != 0) msg = importMsgQ.Dequeue();
        }

        if (msg == null)
        {
            lock (msgQ)
            {
                if (msgQ.Count != 0) msg = msgQ.Dequeue();
            }
        }
        if (msg == null) return;

        switch (msg.id)
        {
            case 1:
                id = JsonUtility.FromJson<json1>(System.Text.Encoding.UTF8.GetString(msg.data));
                break;
            case 2:
                // 依据id和位置创建人物
                playerPos = JsonUtility.FromJson<json2>(System.Text.Encoding.UTF8.GetString(msg.data));
                id = JsonUtility.FromJson<json1>(System.Text.Encoding.UTF8.GetString(lastMsg.data));
                netIdToObj[id.Id] = createPerson(new Vector3(float.Parse(playerPos.X), float.Parse(playerPos.Y), 0), swordPrefab, Color.blue, true, true);
                break;
            case 3:
                // 创建/更新其他玩家
                otherInfo = JsonUtility.FromJson<json3>(System.Text.Encoding.UTF8.GetString(msg.data));
                Vector3 vec = new Vector3(float.Parse(otherInfo.X), float.Parse(otherInfo.Y), 0);
                if (!netIdToObj.ContainsKey(otherInfo.Id))
                {
                    netIdToObj[otherInfo.Id] = createPerson(vec, swordPrefab, Color.red, false, false, true);
                    netIdToObj[otherInfo.Id].GetComponent<IPerson>().SpeedVal = 0; // 速度强制设置为0 靠transform强制更新
                    Debug.Log("创建other");
                }
                else
                {
                    var scriptIperson = netIdToObj[otherInfo.Id].GetComponent<IPerson>();

                    netIdToObj[otherInfo.Id].transform.position = vec;
                    scriptIperson.State = otherInfo.State;
                    if (otherInfo.State == State.attack)
                    {
                        scriptIperson.attack();
                    }
                    scriptIperson.MoveVec = new Vector2(float.Parse(otherInfo.MoveVecX), float.Parse(otherInfo.MoveVecY));
                    scriptIperson.move();
                }
                break;
            case 4:
                //Debug.Log("destory");
                id = JsonUtility.FromJson<json1>(System.Text.Encoding.UTF8.GetString(msg.data));
                Destroy(netIdToObj[id.Id]);
                netIdToObj.Remove(id.Id);
                break;
        }
        lastMsg = msg;
    }

    void Update()
    {
    }

    GameObject createPerson(Vector3 pos, GameObject prefab, Color color, bool owner, bool isPlayer = false, bool isOtherPlayer = false)
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
            if (isOtherPlayer) temp.AddComponent<OtherControll>();
            else temp.AddComponent<ComputerControll>();
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
