using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour, IPerson
{
    Animator anim;
    Rigidbody2D rg;
    BoxCollider2D box;

    public int speedVal; // 5
    public int maxLifeVal;
    public int maxAttackVal;
    public int minAttackVal;

    private int lifeVal;

    private Vector2 moveVec;

    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Attack")
            {
                box = child.GetComponent<BoxCollider2D>();
                break;
            }
        }
        lifeVal = maxLifeVal;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        rg.MovePosition(rg.position + moveVec * speedVal * Time.fixedDeltaTime);
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

        // 更新血条位置
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2 + 50);
        red.GetComponent<RectTransform>().anchoredPosition = pos;
        black.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void attack()
    {
        if (anim.GetBool("dead")) return; // 死亡动画时不可攻击

        anim.SetTrigger("attack");
        box.enabled = true;
        box.isTrigger = true;
        StartCoroutine(attackOver());
    }


    public Image red, black;
    public void hit(int val)
    {
        anim.SetTrigger("hit");
        lifeVal -= val;
        if (lifeVal <= 0) dead();

        // 更新血条
        red.fillAmount = (float)(lifeVal) / maxLifeVal;
    }

    public void dead()
    {
        anim.SetBool("dead", true);
        StartCoroutine(deadOver());
    }

    public Vector2 getMove()
    {
        return moveVec;
    }
    
    public void setMove(Vector2 vec)
    {
        moveVec = vec;
    }

    IEnumerator deadOver()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator attackOver()
    {
        yield return new WaitForSeconds(0.1f); // TODO 攻击持续时间
        box.enabled = false;
        box.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject.GetComponent<Person>();
        if (otherP == null) return;
        otherP.hit(Random.Range(minAttackVal, maxAttackVal + 1));
    }
}
