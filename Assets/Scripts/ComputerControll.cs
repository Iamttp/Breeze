using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControll : MonoBehaviour
{
    IPerson p;
    Transform player;
    AI ai;
    float orgSpeedVal;

    void Start()
    {
        p = GetComponent<IPerson>();
        player = GameObject.Find("Player").transform;
        ai = new AI();
        orgSpeedVal = p.speedVal;
    }

    void Update()
    {
        p.speedVal = orgSpeedVal; // 每次迭代前保证原速度

        GameObject temp = AI.getMinDisEnemy(transform, p.owner, 4);
        nowState = temp == null ? 0 : 1; // 半径为4的范围内存在敌人，进入攻击状态
        if (p.owner) OwnerAI(temp);
        else EnemyAI(temp);

        p.move();
    }

    int nowState = 0;
    float attackTime = 0.5f;
    float nowAttackTime = 0;

    void OwnerAI(GameObject temp)
    {
        switch (nowState)
        {
            case 0: // 跟随主角
                p.moveVec = ai.follow(p.moveVec, player, transform, 3);
                break;
            case 1: // 攻击敌人
                p.moveVec = ai.follow(p.moveVec, temp.transform, transform, 0.1f, false);
                attackRand();
                break;
        }
    }

    void EnemyAI(GameObject temp)
    {
        switch (nowState)
        {
            case 0: // 巡逻
                p.moveVec = ai.patrol(p.moveVec, transform);
                p.speedVal = orgSpeedVal / 4 + Random.value;
                break;
            case 1: // 攻击主角或其它
                p.moveVec = ai.follow(p.moveVec, temp.transform, transform, 0.1f, false);
                attackRand();
                break;
        }
    }
    void attackRand()
    {
        // TODO 根据类型判断
        nowAttackTime += Time.deltaTime;
        if (nowAttackTime < attackTime) return;
        if (Random.value > 0.5f)
        {
            p.attack();
            nowAttackTime = 0;
        }
    }
}

