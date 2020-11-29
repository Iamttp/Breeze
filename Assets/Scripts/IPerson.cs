using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerson
{
    void move();
    void attack();
    void hit(int val);
    void dead();

    Vector2 getMove();
    void setMove(Vector2 vec);
}
