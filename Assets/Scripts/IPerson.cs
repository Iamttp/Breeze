using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerson
{
    void move();
    void attack();
    void hit(int val);
    void dead();

    bool owner
    {
        set;
        get;
    }  
    
    float speedVal
    {
        set;
        get;
    }

    bool isDeath
    {
        set;
        get;
    }

    Vector2 moveVec
    {
        set;
        get;
    }
}
