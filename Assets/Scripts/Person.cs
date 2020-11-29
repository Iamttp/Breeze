using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rg;

    public float speed;

    [HideInInspector]
    public Vector2 moveVec;

    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        rg.MovePosition(rg.position + moveVec * speed * Time.fixedDeltaTime);
    }

    public void move()
    {
        // 动画翻转
        if (moveVec.x != 0)
        {
            var vec = transform.localScale;
            vec.x = Mathf.Abs(vec.x) * ((moveVec.x > 0) ? 1 : -1);
            transform.localScale = vec; // -1 翻转
        }
        anim.SetFloat("speed", moveVec.magnitude);
    }

    public void attack()
    {
        anim.SetTrigger("attack");
        // TODO
    }

    public void hit()
    {
        anim.SetTrigger("hit");
        // TODO
    }

    public void dead()
    {
        anim.SetBool("dead", true);
        StartCoroutine(deadOver());
    }

    IEnumerator deadOver()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
