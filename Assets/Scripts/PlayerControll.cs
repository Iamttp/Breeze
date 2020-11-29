using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    Person p;

    void Start()
    {
        p = GetComponent<Person>();
    }

    void Update()
    {
        p.moveVec.x = Input.GetAxisRaw("Horizontal");
        p.moveVec.y = Input.GetAxisRaw("Vertical");

        // 摄像机跟随检测
        if (p.moveVec.x != 0 || p.moveVec.y != 0)
        {
            int x = Mathf.RoundToInt(transform.position.x / RoomCreator.instance.xOffset);
            int y = Mathf.RoundToInt(transform.position.y / RoomCreator.instance.yOffset);
            CameraFollow.instance.target = new Vector2(x * RoomCreator.instance.xOffset, y * RoomCreator.instance.yOffset);
        }
        p.move();

        // 攻击检测
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) p.attack();
        if (Input.GetMouseButton(1)) p.hit();
    }
}
