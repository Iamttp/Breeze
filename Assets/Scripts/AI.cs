using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    private int rateGo = 1;
    private Vector2 followTarget(Vector2 now, Transform target, Transform transform, float followDis)
    {
        if (target == null)
        {
            rateGo = 1;
            return Vector2.zero;
        }

        var tgpos = target.position;
        var tfpos = transform.position;
        if (Vector3.Distance(tgpos, tfpos) < followDis)
        {
            rateGo = 1;
            return Vector2.zero;
        }

        const float smoothDoor = 0.9f; // 移动速度滑动窗口
        const float smoothVal = 0.2f;

        if (tgpos.x - tfpos.x > smoothVal) now.x = now.x * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.x - tfpos.x < -smoothVal) now.x = now.x * smoothDoor + -1 * (1 - smoothDoor);
        if (tgpos.y - tfpos.y > smoothVal) now.y = now.y * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.y - tfpos.y < -smoothVal) now.y = now.y * smoothDoor + -1 * (1 - smoothDoor);
        return now;
    }


    private Vector3 lastPos;
    private float timeOfDelay;

    // 被阻挡随机运动解决，离得太远仍然不行
    private Vector2 forceMove(Vector2 now, Transform transform)
    {
        var tfpos = transform.position;
        if (Vector3.Distance(lastPos, tfpos) < 0.01f)
        {
            timeOfDelay += Time.deltaTime;
            if (timeOfDelay >= 0.1f)
            {
                timeOfDelay = 0;
                var temp = new Vector2(Random.value - 0.5f, Random.value - 0.5f);
                now += temp * rateGo;
                rateGo <<= 1; // 慢启动
                if (rateGo >= 32) rateGo = 1;
                return now;
            }
        }
        else
        {
            timeOfDelay = 0;
        }
        lastPos = tfpos;
        return now;
    }

    public Vector2 follow(Vector2 now, Transform target, Transform transform, float followDis, bool isForce = true)
    {
        now = followTarget(now, target, transform, followDis);
        if (!isForce || now == Vector2.zero) return now;
        now = forceMove(now, transform);
        return now;
    }
}
