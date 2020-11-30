using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archor : MonoBehaviour, IPerson
{
    Animator anim;
    Rigidbody2D rg;
    GameObject attackDisShow;

    public int speedVal; // 5
    public int maxLifeVal;
    public int maxAttackVal;
    public int minAttackVal;
    public int maxAttackDis;
    public int speedArcVal;

    private int lifeVal;

    public GameObject ArcPrefab;

    void Start()
    {
        anim = GetComponent<Animator>();
        rg = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "AttackDisShow")
            {
                attackDisShow = child.gameObject;
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

        GameObject temp = getMinDisP();
        if (temp == null)
        {
            attackDisShow.SetActive(true);
            StartCoroutine(attackOver());
            return;
        }

        anim.SetTrigger("attack");
        GameObject arc = Instantiate(ArcPrefab, transform.position, Quaternion.identity);
        arc.GetComponent<ArcFollow>().speed = speedArcVal;
        arc.GetComponent<ArcFollow>().p = temp.GetComponent<IPerson>();
        arc.GetComponent<ArcFollow>().target = temp.transform.position;
        arc.GetComponent<ArcFollow>().attackVal = Random.Range(minAttackVal, maxAttackVal + 1);
        arc.GetComponent<ArcFollow>().targetNew = temp.transform;
    }

    private GameObject getMinDisP()
    {
        GameObject res = null;
        var pos = transform.position;
        List<GameObject> temp;
        if (owner)
        {
            temp = Manager.instance.enemys;
        }
        else
        {
            temp = Manager.instance.players;
        }
        float min_ = maxAttackDis;
        foreach (var obj in temp)
        {
            var dis = Vector2.Distance(obj.transform.position, pos);
            if (dis < min_)
            {
                min_ = dis;
                res = obj;
            }
        }
        return res;
    }

    public Image red, black;

    private bool owner;
    bool IPerson.owner { get { return owner; } set { owner = value; } }

    private bool isDeath;
    bool IPerson.isDeath { get { return isDeath; } set { isDeath = value; } }

    private Vector2 moveVec;
    Vector2 IPerson.moveVec { get { return moveVec; } set { moveVec = value; } }

    public void hit(int val)
    {
        if (anim.GetBool("dead")) return; // 死亡动画时不可攻击

        anim.SetTrigger("hit");
        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(hitOver());
        lifeVal -= val;
        if (lifeVal <= 0) dead();

        // 更新血条
        red.fillAmount = (float)(lifeVal) / maxLifeVal;
    }

    public void dead()
    {
        isDeath = true;
        if (owner) Manager.instance.players.Remove(gameObject);
        else Manager.instance.enemys.Remove(gameObject);
        anim.SetBool("dead", true);
        StartCoroutine(deadOver());
    }

    IEnumerator deadOver()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator attackOver()
    {
        yield return new WaitForSeconds(0.1f); // TODO 攻击范围显示持续时间
        attackDisShow.SetActive(false);
    }

    IEnumerator hitOver()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject.GetComponent<IPerson>();
        if (otherP == null) return;
        otherP.hit(Random.Range(minAttackVal, maxAttackVal + 1));
    }
}
