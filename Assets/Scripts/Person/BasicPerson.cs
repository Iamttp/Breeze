using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicPerson : MonoBehaviour, IPerson
{
    protected Animator anim;
    protected Rigidbody2D rg;

    public int maxLifeVal;
    public int maxAttackVal;
    public int minAttackVal;

    protected int lifeVal;

    public Image red, black;

    bool owner;
    public bool Owner { get => owner; set => owner = value; }

    Vector2 moveVec;
    public Vector2 MoveVec { get => moveVec; set => moveVec = value; }

    float speedVal = 3;
    public float SpeedVal { get => speedVal; set => speedVal = value; }

    State state;
    public State State { get => state; set => state = value; }

    TypePerson typePerson;
    public TypePerson TypePerson { get => typePerson; set => typePerson = value; }

    protected void FixedUpdate()
    {
        if (State == State.run)
        {
            rg.MovePosition(rg.position + MoveVec * SpeedVal * Time.fixedDeltaTime);
        }
        
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 1000); // 重叠bug解决

        // 更新血条位置
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2 + 50);
        red.GetComponent<RectTransform>().anchoredPosition = pos;
        black.GetComponent<RectTransform>().anchoredPosition = pos;
    }


    private json2 playerPos = new json2();
    public void move()
    {
        anim.SetFloat("speed", moveVec.magnitude);
        
        if (State == State.dead) return;
        if (moveVec.magnitude <= 0.001f) return;

        if (Manager.instance.isNet && name == "Player")
        {
            playerPos.X = transform.position.x;
            playerPos.Y = transform.position.y;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerPos));
            Message msg = new Message(201, data);
            if (!NetUtil.instance.Send(msg))
                Debug.Log("发送失败");
        }

        // 动画翻转
        if (moveVec.x != 0)
        {
            var vec = transform.localScale;
            vec.x = Mathf.Abs(vec.x) * ((moveVec.x > 0) ? 1 : -1);
            transform.localScale = vec; // -1 翻转
        }
        State = State.run;
    }

    public abstract void attack();

    public void hit(int val)
    {
        if (State == State.dead) return;

        State = State.hit;

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
        if (owner) Manager.instance.players.Remove(gameObject);
        else Manager.instance.enemys.Remove(gameObject);
        State = State.dead;
        anim.SetBool("dead", true);
        StartCoroutine(deadOver());
        if (owner)
            MsgManager.instance.AddMsg("---> Kill " + typePerson.ToString() + " <---", Color.red);
        else
            MsgManager.instance.AddMsg("---> Kill " + typePerson.ToString() + " <---", Color.green);
        Manager.instance.playerCheck();
    }

    IEnumerator deadOver()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    IEnumerator hitOver()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    // 仅在遇到墙时启用刚体
    private void OnTriggerStay2D(Collider2D collision)
    {
        var other = collision.gameObject;
        if (other.tag == "wall")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            StartCoroutine(wallOver());
        }
    }

    // 1s后继续isTrigger
    IEnumerator wallOver()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
