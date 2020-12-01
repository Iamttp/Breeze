using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour
{
    public Text text;
    public string typeName;
    public System.DateTime plantTime = System.DateTime.Now;
    public GameObject prefabObj;

    void Start()
    {
        init();
        StartCoroutine(SecondRun());
    }

    void Update()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = new Vector2(pos.x - Screen.width / 2, pos.y - Screen.height / 2 + 30);
        text.GetComponent<RectTransform>().anchoredPosition = pos;
    }


    void init()
    {
        switch (typeName) // properties set ...
        {
            case "Pumpkin":
                plantTime = plantTime.AddSeconds(5);
                break;
            case "PumpkinU":
                plantTime = plantTime.AddSeconds(5);
                transform.localScale *= 2;
                break;
            case "PumpkinUU":
                Destroy(gameObject);
                Instantiate(prefabObj, transform.position, Quaternion.identity).GetComponent<PickUp>().typeName = "Pumpkin";
                break;
            case "Carrot":
                plantTime = plantTime.AddSeconds(2);
                break;
            case "CarrotU":
                plantTime = plantTime.AddSeconds(2);
                transform.localScale *= 2;
                break;
            case "CarrotUU":
                Destroy(gameObject);
                Instantiate(prefabObj, transform.position, Quaternion.identity).GetComponent<PickUp>().typeName = "Carrot";
                break;
        }
    }


    IEnumerator SecondRun()
    {
        var timeDis = plantTime - System.DateTime.Now;
        if (timeDis.TotalSeconds <= 0)
        {
            typeName += "U";
            init();
        }

        if (timeDis.Hours == 0) text.text = string.Format("{0:D2}:{1:D2}", timeDis.Minutes, timeDis.Seconds);
        else text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeDis.Hours, timeDis.Minutes, timeDis.Seconds);

        yield return new WaitForSeconds(1);
        StartCoroutine(SecondRun());
    }
}
