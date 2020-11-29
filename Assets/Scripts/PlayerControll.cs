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

    float attackTime = 0.2f;
    float nowAttackTime = 0;

    void Update()
    {
        p.moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // 摄像机跟随检测
        if (p.moveVec.x != 0 || p.moveVec.y != 0)
        {
            int x = Mathf.RoundToInt(transform.position.x / RoomCreator.instance.xOffset);
            int y = Mathf.RoundToInt(transform.position.y / RoomCreator.instance.yOffset);
            CameraFollow.instance.target = new Vector2(x * RoomCreator.instance.xOffset, y * RoomCreator.instance.yOffset);
        }
        p.move();

        // Top摄像机
        tpCam.position = new Vector3(transform.position.x, transform.position.y, tpCam.position.z);

        // 攻击
        nowAttackTime += Time.deltaTime;
        if (nowAttackTime < attackTime) return;
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            nowAttackTime = 0;
            p.attack();
        }
    }
}
