using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObj : MonoBehaviour
{
    [HideInInspector]
    public System.DateTime plantTime;

    public GameObject showF;
    Animator anim;

    public GameObject canvas;
    public Text text;

    public string typeName;
    private MapObject obj;

    [HideInInspector]
    public bool isDrag = false; // 是否是CellObj中拖动出來的，而不是prefab brush刷的

    void Start()
    {
        if (!isDrag) // TODO 更好的写法，因为MapManager.instance.plants已经在CellObj中写了
        {
            if (MapManager.instance.plants.ContainsKey(transform.position))
            {
                // Prefab brush 有bug 会重复出现，小心画，出现error查看问题
                // Debug.Log("map error ! " + transform.position + ":" + MapManager.instance.plants[transform.position]);

                // Destroy 重复出现的，最新的直接删除
                DestroyImmediate(transform.gameObject);
                return;
            }
            else
                MapManager.instance.plants[transform.position] = gameObject;
        }
        init();
    }

    void Update()
    {
        if (obj == null) return;
        if (obj.upMapName != null && obj.upMapName != "") showUpFunc();
        if (obj.packageName != null && obj.packageName != "") showFFunc();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (obj.isNShowF) return; // 无采摘obj，关闭Box Trigger提高性能
        var otherP = collision.gameObject;
        if (otherP.name == "Player")
        {
            showF.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (obj.isNShowF) return; // TODO 添加一个强制摧毁的功能
        showF.SetActive(false);
    }

    void init()
    {
        plantTime = System.DateTime.Now;
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 1000); // 重叠bug解决

        if (!MapManager.instance.objTable.ContainsKey(typeName)) return;
        obj = MapManager.instance.objTable[typeName];
        name = typeName;
        if (obj.sprite != null) GetComponent<SpriteRenderer>().sprite = obj.sprite;

        var isUp = (obj.upMapName != null && obj.upMapName != "");
        canvas.SetActive(isUp);
        text.enabled = isUp;
    }

    void showFFunc()
    {
        if (!showF.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(dead());
        }
    }

    IEnumerator dead()
    {
        if (anim != null) anim.enabled = true;
        yield return new WaitForSeconds(obj.deadTime);
        Music.instance.playDestory();
        Destroy(gameObject);
        MapManager.instance.plants.Remove(gameObject.transform.position); // 一定记得删除Map Obj的同时删除plants相应数据
        PackageManager.instance.objTable[obj.packageName].num += obj.packageNum;
        MsgManager.instance.AddMsg("---> Get " + obj.packageName + " x" + obj.packageNum + " <---", new Color(0, 0.6f, 0, 1));
    }

    void showUpFunc()
    {
        var timeDis = plantTime.AddSeconds(obj.upSeconds) - System.DateTime.Now;
        if (timeDis.TotalSeconds <= 0)
        {
            typeName = obj.upMapName;
            init();
            return;
        }
        if (timeDis.Hours == 0) text.text = string.Format("{0:D2}:{1:D2}", timeDis.Minutes, timeDis.Seconds);
        else text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeDis.Hours, timeDis.Minutes, timeDis.Seconds);

        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2 + 30);
        text.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}