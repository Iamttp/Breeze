using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archor : BasicPerson
{
    GameObject attackDisShow;

    public int maxAttackDis;
    public int speedArcVal;

    public GameObject ArcPrefab;

    void Start()
    {
        TypePerson = TypePerson.Anchor;

        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        lifeVal = maxLifeVal;

        if (!Owner) red.color = Color.red;
        else red.color = Color.blue;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "AttackDisShow")
            {
                attackDisShow = child.gameObject;
                break;
            }
        }
    }

    void Update()
    {
    }

    override public void attack()
    {
        if (State == State.dead) return;

        GameObject temp = AI.getMinDisEnemy(transform, Owner, maxAttackDis);
        if (temp == null)
        {
            attackDisShow.SetActive(true);
            StartCoroutine(attackOver());
            return;
        }
        State = State.attack;

        if (name == "Player") Music.instance.playAttack();
        anim.SetTrigger("attack");
        GameObject arc = Instantiate(ArcPrefab, transform.position, Quaternion.identity);
        arc.GetComponent<ArcFollow>().speed = speedArcVal;
        arc.GetComponent<ArcFollow>().Owner = Owner;
        arc.GetComponent<ArcFollow>().target = temp.transform.position;
        arc.GetComponent<ArcFollow>().attackVal = Random.Range(minAttackVal, maxAttackVal + 1);
    }

    IEnumerator attackOver()
    {
        yield return new WaitForSeconds(0.1f); // 攻击范围显示持续时间
        attackDisShow.SetActive(false);
        State = State.idle;
    }
}
