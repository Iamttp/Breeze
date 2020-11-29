using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    IPerson p;
    Transform tpCam;

    void Start()
    {
        p = GetComponent<IPerson>();
        tpCam = GameObject.Find("TopCamera").transform;
    }

    void Update()
    {
        p.setMove(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        // 摄像机跟随检测
        if (p.getMove().x != 0 || p.getMove().y != 0)
        {
            int x = Mathf.RoundToInt(transform.position.x / RoomCreator.instance.xOffset);
            int y = Mathf.RoundToInt(transform.position.y / RoomCreator.instance.yOffset);
            CameraFollow.instance.target = new Vector2(x * RoomCreator.instance.xOffset, y * RoomCreator.instance.yOffset);
        }
        p.move();

        // Top摄像机
        tpCam.position = new Vector3(transform.position.x, transform.position.y, tpCam.position.z);

        // 攻击
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) p.attack();
    }
}
