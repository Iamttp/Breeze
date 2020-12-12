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

    public static string getString(float val)
    {
        return val.ToString("0.00");
    }

    static json3 playerInfo = new json3();
    public static void sendMsg(GameObject gameObject)
    {
        if (!Manager.instance.isNet || gameObject.name != "Player") return;

        var script = gameObject.GetComponent<IPerson>();
        playerInfo.X = getString(gameObject.transform.position.x);
        playerInfo.Y = getString(gameObject.transform.position.y);
        playerInfo.State = script.State;
        playerInfo.MoveVecX = getString(script.MoveVec.x);
        playerInfo.MoveVecY = getString(script.MoveVec.y);
        foreach (var item in Manager.instance.netIdToObj)
            if (item.Value == gameObject)
            {
                playerInfo.Id = item.Key;
                break;
            }
        byte[] data = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(playerInfo));
        Message msg = new Message(201, data);
        if (!NetUtil.instance.Send(msg)) // TODO 服务器崩溃考虑
            Debug.Log("发送失败");
    }

    private int stopFlag = 5; // SendMsg 兴趣点， 当移动后，后5步操作感兴趣
    void updateSendMsg()
    {
        if (MoveVec.magnitude > 0.001f)
        {
            sendMsg(gameObject);
            stopFlag = 5;
        }
        else
        {
            if (stopFlag-- > 0)
            {
                sendMsg(gameObject);
            }
        }
    }

    protected void FixedUpdate()
    {
        updateSendMsg();

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

    public void move()
    {
        anim.SetFloat("speed", moveVec.magnitude);

        if (State == State.dead) return;
        if (moveVec.magnitude <= 0.001f) return;

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
