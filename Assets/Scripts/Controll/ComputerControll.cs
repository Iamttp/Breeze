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
        ai = new AI();
        orgSpeedVal = p.SpeedVal;
    }

    void Update()
    {
        var playerObj = GameObject.Find("Player");
        if (playerObj == null) return;
        player = playerObj.transform;

        p.SpeedVal = orgSpeedVal; // 每次迭代前保证原速度

        GameObject temp = AI.getMinDisEnemy(transform, p.Owner, 4);
        nowState = temp == null ? 0 : 1; // 半径为4的范围内存在敌人，进入攻击状态
        if (p.Owner)
        {
            if (nowState == 0)
                if (player.GetComponent<IPerson>().State == State.attack ||
                    player.GetComponent<IPerson>().State == State.hit)
                    nowState = 2;
            OwnerAI(temp);
        }
        else
        {
            EnemyAI(temp);
        }

        p.move();
    }

    int nowState = 0;
    float nowAttackTime = 0;

    void OwnerAI(GameObject temp)
    {
        switch (nowState)
        {
            case 0: // 跟随主角
                p.MoveVec = ai.follow(p.MoveVec, player, transform, 3);
                break;
            case 1: // 攻击敌人
                attackRand(temp);
                break;
            case 2: // 跟上挨揍的主角 TODO
                p.MoveVec = ai.follow(p.MoveVec, player, transform, 0.1f);
                break;
        }
    }

    void EnemyAI(GameObject temp)
    {
        switch (nowState)
        {
            case 0: // 巡逻
                p.MoveVec = ai.patrol(p.MoveVec, transform);
                p.SpeedVal = orgSpeedVal / 4 + Random.value;
                break;
            case 1: // 攻击主角或其它
                attackRand(temp);
                break;
        }
    }
    void attackRand(GameObject temp)
    {
        // TODO Sword 攻击死角bug
        if (p.TypePerson == TypePerson.Sword)
            p.MoveVec = ai.follow(p.MoveVec, temp.transform, transform, 1f, false);
        else if (p.TypePerson == TypePerson.Anchor)
            p.MoveVec = ai.follow(p.MoveVec, temp.transform, transform, 3.8f, false);

        nowAttackTime += Time.deltaTime;
        if (nowAttackTime < Manager.attackTime) return;
        nowAttackTime = 0;
        if (Random.value > 0.5f) p.attack();
    }
}

