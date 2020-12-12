using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    IPerson p;
    Transform tpCam;
    float orgSpeedVal;

    void Start()
    {
        p = GetComponent<IPerson>();
        tpCam = GameObject.Find("TopCamera").transform;
        orgSpeedVal = p.SpeedVal;

        if (Manager.instance.sceneName == Manager.SceneName.Main)
        {
            CameraFollow.instance.speed = 5;
            CameraFollow.instance.target = transform.position;
        }
    }


    void Update()
    {
        p.SpeedVal = orgSpeedVal; // 每次迭代前保证原速度

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

        attack();
        tab();

        if (Input.GetKey(KeyCode.LeftShift)) // TODO 体力条
        {
            p.SpeedVal *= 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PackageManager.instance.setPage(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PackageManager.instance.setPage(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PackageManager.instance.setPage(2);
        }
    }

    float nowAttackTime = 0;
    void attack()
    {
        // 攻击
        nowAttackTime += Time.deltaTime;
        if (nowAttackTime < Manager.attackTime) return;
        if (Input.GetKey(KeyCode.Space))
        {
            nowAttackTime = 0;
            p.attack();
            BasicPerson.sendMsg(gameObject);
        }
    }

    void tab()
    {
        // 人物切换
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            var players = Manager.instance.players;
            if (players.Count <= 1) return;
            int num = players.FindIndex(a => a == gameObject);
            Manager.setPlayer(num, (num + 1) % players.Count);
        }
    }
}
