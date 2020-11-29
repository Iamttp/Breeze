using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControll : MonoBehaviour
{
    Person p;
    Transform target;

    public float followDis; // 2

    void Start()
    {
        p = GetComponent<Person>();
        target = GameObject.Find("Player").transform;
    }

    void Update()
    {
        followTarget();
        // 攻击检测
        if (Random.value > 0.99) p.attack();
    }

    void followTarget()
    {
        const float smoothDoor = 0.9f;
        const float smoothVal = 0.2f;

        if (target == null) return;

        var tgpos = target.position;
        var tfpos = transform.position;
        if (Vector3.Distance(tgpos, tfpos) < followDis)
        {
            p.moveVec.x = p.moveVec.y = 0;
            p.move();
            return;
        }
        if (tgpos.x - tfpos.x > smoothVal) p.moveVec.x = p.moveVec.x * smoothDoor + 1 * (1 - smoothDoor);
        else if(tgpos.x - tfpos.x < -smoothVal) p.moveVec.x = p.moveVec.x * smoothDoor + -1 * (1 - smoothDoor);
        if (tgpos.y - tfpos.y > smoothVal) p.moveVec.y = p.moveVec.y * smoothDoor + 1 * (1 - smoothDoor);
        else if(tgpos.y - tfpos.y < -smoothVal) p.moveVec.y = p.moveVec.y * smoothDoor + -1 * (1 - smoothDoor);
        p.move();
    }
}
