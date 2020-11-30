using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI
{
    /// <summary>
    /// 选择半径为maxAttack内当前最近的敌人
    /// </summary>
    /// <param name="transform">位置</param>
    /// <param name="owner">归属</param>
    /// <param name="maxAttackDis">半径内</param>
    /// <returns></returns>
    public static GameObject getMinDisEnemy(Transform transform, bool owner, int maxAttackDis)
    {
        GameObject res = null;
        var pos = transform.position;
        List<GameObject> temp;
        if (owner)
        {
            temp = Manager.instance.enemys;
        }
        else
        {
            temp = Manager.instance.players;
        }
        float min_ = maxAttackDis;
        foreach (var obj in temp)
        {
            var dis = Vector2.Distance(obj.transform.position, pos);
            if (dis < min_)
            {
                min_ = dis;
                res = obj;
            }
        }
        return res;
    }

    private Vector2 setNow(Vector2 now, Vector3 tgpos, Vector3 tfpos)
    {
        const float smoothDoor = 0.9f; // 移动速度滑动窗口
        const float smoothVal = 0.2f;

        if (tgpos.x - tfpos.x > smoothVal) now.x = now.x * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.x - tfpos.x < -smoothVal) now.x = now.x * smoothDoor + -1 * (1 - smoothDoor);
        if (tgpos.y - tfpos.y > smoothVal) now.y = now.y * smoothDoor + 1 * (1 - smoothDoor);
        else if (tgpos.y - tfpos.y < -smoothVal) now.y = now.y * smoothDoor + -1 * (1 - smoothDoor);
        return now;
    }

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
        return setNow(now, tgpos, tfpos);
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


    private Vector3 lastPatrolTarget;
    private float timeDelay = 0;
    private int index = 0;
    private static List<Vector3> dir = new List<Vector3>() {
        new Vector3(-2, 0, 0),
        new Vector3(0, 2, 0),
        new Vector3(2, 0, 0),
        new Vector3(0, -2, 0),
    };
    private bool isFirst = true;
    public Vector2 patrol(Vector2 now, Transform transform)
    {
        if (isFirst)
        {
            lastPatrolTarget = transform.position + dir[index++] + Random.insideUnitSphere;
            index = index % dir.Count;
            isFirst = false;
            return Vector2.zero;
        }

        if (Vector3.Distance(lastPatrolTarget, transform.position) < 0.1f || timeDelay > 1)
        {
            timeDelay = 0;
            lastPatrolTarget += dir[index++] + Random.insideUnitSphere;
            index = index % dir.Count;
            return Vector2.zero;
        }
        else
        {
            timeDelay += Time.deltaTime;
            return setNow(now, lastPatrolTarget, transform.position);
        }
    }
}
