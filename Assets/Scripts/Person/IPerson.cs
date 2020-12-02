using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypePerson
{
    Sword,
    Anchor
}
public enum State
{
    idle,
    run,
    dead,
    attack,
    hit
}

public interface IPerson
{
    void move();
    void attack();
    void hit(int val);
    void dead();

    TypePerson TypePerson // 按创建时间从0开始编号, 0 Sword 1 Anchor
    {
        set;
        get;
    }

    State State // 当前状态
    {
        set;
        get;
    }

    bool Owner
    {
        set;
        get;
    }

    float SpeedVal
    {
        set;
        get;
    }

    Vector2 MoveVec
    {
        set;
        get;
    }
}
