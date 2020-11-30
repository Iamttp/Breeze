﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO 人物重叠bug
public class Sword : BasicPerson
{
    BoxCollider2D box;

    void Start()
    {
        TypePerson = TypePerson.Sword;

        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        lifeVal = maxLifeVal;

        if (Owner) red.color = new Color(1, 0, 0, 0.6f);
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Attack")
            {
                box = child.GetComponent<BoxCollider2D>();
                break;
            }
        }
    }

    void Update()
    {
    }
    
    private void FixedUpdate()
    {
        rg.MovePosition(rg.position + MoveVec * SpeedVal * Time.fixedDeltaTime);
    }

    override public void attack()
    {
        if (State == State.dead) return;

        anim.SetTrigger("attack");
        State = State.attack;
        box.enabled = true;
        box.isTrigger = true;
        StartCoroutine(attackOver());
    }

    IEnumerator attackOver()
    {
        yield return new WaitForSeconds(0.1f); // TODO 攻击持续时间
        box.enabled = false;
        box.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject.GetComponent<IPerson>();
        if (otherP == null) return;
        if (otherP.Owner == Owner) return;
        otherP.hit(Random.Range(minAttackVal, maxAttackVal + 1));
    }
}
