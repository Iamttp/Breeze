using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControll : MonoBehaviour
{
    IPerson p;
    Transform player;
    AI ai;

    void Start()
    {
        p = GetComponent<IPerson>();
        player = GameObject.Find("Player").transform;
        ai = new AI();
    }

    void Update()
    {
        if (p.owner) OwnerAI();
        else EnemyAI();

        p.move();
    }

    int nowState = 0;
    void OwnerAI()
    {
        switch (nowState)
        {
            case 0: // 跟随主角
                p.moveVec = ai.follow(p.moveVec, player, transform, 5);
                break;
            case 1: // 攻击敌人
                p.moveVec = ai.follow(p.moveVec, player, transform, 0.1f);
                if (Random.value > 0.99f) p.attack();
                break;
        }
    }

    void EnemyAI()
    {
        switch (nowState)
        {
            case 0: // 巡逻
                // TODO
                p.moveVec = ai.follow(p.moveVec, player, transform, 5);
                break;
            case 1: // 攻击主角或其它
                p.moveVec = ai.follow(p.moveVec, player, transform, 0.1f);
                if (Random.value > 0.99f) p.attack();
                break;
        }
    }
}
