using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControll : MonoBehaviour
{
    IPerson p;
    Transform target;

    public float followDis; // 2

    void Start()
    {
        p = GetComponent<IPerson>();
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        followTarget();
        // 攻击
        if (Random.value > 0.99f) p.attack();
    }


    private Vector3 lastPos;
    private float timeOfDelay;
    private int rateGo = 1;
    void followTarget()
    {
        const float smoothDoor = 0.9f;
        const float smoothVal = 0.2f;

        if (target == null) return;

        var tgpos = target.position;
        var tfpos = transform.position;
        if (Vector3.Distance(tgpos, tfpos) < followDis)
        {
            rateGo = 1;
            p.moveVec = new Vector2(0, 0);
            p.move();
            return;
        }

        var tempMove = p.moveVec;
        if (tgpos.x - tfpos.x > smoothVal) tempMove.x = tempMove.x * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.x - tfpos.x < -smoothVal) tempMove.x = tempMove.x * smoothDoor + -1 * (1 - smoothDoor);
        if (tgpos.y - tfpos.y > smoothVal) tempMove.y = tempMove.y * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.y - tfpos.y < -smoothVal) tempMove.y = tempMove.y * smoothDoor + -1 * (1 - smoothDoor);
        p.moveVec = tempMove;
        p.move();

        // 被阻挡随机运动解决，离得太远仍然不行
        if (Vector3.Distance(lastPos, tfpos) < 0.01f)
        {
            timeOfDelay += Time.deltaTime;
            if (timeOfDelay >= 0.1f)
            {
                timeOfDelay = 0;
                var temp = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
                p.moveVec += temp * rateGo;
                rateGo <<= 1; // 慢启动
                if (rateGo >= 32) rateGo = 1;
                p.move();
            }
        }
        else
        {
            timeOfDelay = 0;
        }
        lastPos = tfpos;
    }
}
