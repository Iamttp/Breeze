using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCreator : MonoBehaviour
{
    [HideInInspector]
    public static RoomCreator instance;

    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNum;
    public Color startCol;
    public Color endCol;

    [Header("位置控制")]
    public float xOffset;
    public float yOffset;

    [HideInInspector]
    public List<Room> rooms = new List<Room>();
    private HashSet<Vector3> vis = new HashSet<Vector3>();
    private Transform generatorPoint;

    [HideInInspector]
    public enum Direction { up, down, left, right };
    [HideInInspector]
    public Direction direction;

    void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
        //Screen.SetResolution(1920, 1920 / 2, true);
    }

    void Start()
    {

        generatorPoint = transform;
        for (int i = 0; i < roomNum; i++)
        {
            rooms.Add(Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>());
            ChangePointPos();
        }
        rooms[0].GetComponent<SpriteRenderer>().color = startCol;//改变第1个房间的颜色
        rooms[roomNum - 1].GetComponent<SpriteRenderer>().color = endCol;

        for (int i = 0; i < roomNum; i++)
        {
            SetRoad(i);
        }
    }

    void ChangePointPos()
    {
        vis.Add(generatorPoint.position);
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
        } while (vis.Contains(generatorPoint.position));
    }

    void SetRoad(int index)
    {
        Vector3 pos = rooms[index].transform.position;
        rooms[index].up = vis.Contains(pos + new Vector3(0, yOffset, 0));
        rooms[index].down = vis.Contains(pos + new Vector3(0, -yOffset, 0));
        rooms[index].left = vis.Contains(pos + new Vector3(-xOffset, 0, 0));
        rooms[index].right = vis.Contains(pos + new Vector3(xOffset, 0, 0));
    }

    void Update()
    {
        //if (Input.anyKey)
        //{
        //    SceneManager.LoadScene(0);
        //}
    }
}
