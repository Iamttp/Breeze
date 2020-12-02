using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO Tree
public class MapObj : MonoBehaviour
{
    public GameObject showF;
    public Text text;
    public string typeName;
    private MapObject obj;

    void Start()
    {
        init();
    }

    void Update()
    {
        if (obj.upMapName != null && obj.upMapName != "")
        {
            text.enabled = true;
            var timeDis = obj.plantTime.AddSeconds(obj.upSeconds) - System.DateTime.Now;
            if (timeDis.TotalSeconds <= 0)
            {
                typeName = obj.upMapName;
                init();
            }
            if (timeDis.Hours == 0) text.text = string.Format("{0:D2}:{1:D2}", timeDis.Minutes, timeDis.Seconds);
            else text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeDis.Hours, timeDis.Minutes, timeDis.Seconds);
            showUpFunc();
        }
        else
        {
            text.enabled = false;
        }
        if (obj.packageName != null && obj.packageName != "") showFFunc();
    }


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
        obj = MapManager.instance.objTable[typeName];
        obj.plantTime = System.DateTime.Now;
        name = typeName;
        GetComponent<SpriteRenderer>().sprite = obj.sprite;
    }

    void showFFunc()
    {
        if (!showF.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
            PackageManager.instance.objTable[obj.packageName].num += obj.packageNum;
        }
    }

    void showUpFunc()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2 + 30);
        text.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}