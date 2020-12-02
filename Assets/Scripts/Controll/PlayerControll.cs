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

        if (Manager.instance.sceneName == Manager.SceneName.Main)
        {
            CameraFollow.instance.speed = 5;
            CameraFollow.instance.target = transform.position;
        }
    }

    float nowAttackTime = 0;

    void Update()
    {
        p.MoveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Manager.instance.sceneName == Manager.SceneName.Underground)
        {
            // 摄像机跟随检测
            if (p.MoveVec.x != 0 || p.MoveVec.y != 0)
            {
                int x = Mathf.RoundToInt(transform.position.x / RoomCreator.instance.xOffset);
                int y = Mathf.RoundToInt(transform.position.y / RoomCreator.instance.yOffset);
                CameraFollow.instance.target = new Vector2(x * RoomCreator.instance.xOffset, y * RoomCreator.instance.yOffset);
            }
        }
        else if (Manager.instance.sceneName == Manager.SceneName.Main)
        {
            if (p.MoveVec.x != 0 || p.MoveVec.y != 0)
                CameraFollow.instance.target = transform.position;
        }
        p.move();

        // Top摄像机
        tpCam.position = new Vector3(transform.position.x, transform.position.y, tpCam.position.z);

        // 攻击
        nowAttackTime += Time.deltaTime;
        if (nowAttackTime < Manager.attackTime) return;
        if (Input.GetKey(KeyCode.Space))
        {
            nowAttackTime = 0;
            p.attack();
        }
    }
}
