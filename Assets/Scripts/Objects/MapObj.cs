using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Tree Stone and let them anmi when F press
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

    void Start()
    {
        init();
    }

    void Update()
    {
        if (obj == null) return;
        if (obj.upMapName != null && obj.upMapName != "") showUpFunc();
        if (obj.packageName != null && obj.packageName != "") showFFunc();
    }

    // TODO 无采摘obj，关闭Box Trigger提高性能
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject;
        if (otherP.name == "Player")
        {
            showF.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
        Destroy(gameObject);
        PackageManager.instance.objTable[obj.packageName].num += obj.packageNum;
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