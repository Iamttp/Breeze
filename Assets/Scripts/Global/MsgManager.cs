using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgManager : MonoBehaviour
{
    public static MsgManager instance;
    [Header("从底部开始，初始都为false")]
    public List<Text> texts;

    private List<RectTransform> rt = new List<RectTransform>();
    private List<Vector2> orgPos = new List<Vector2>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (var tx in texts)
        {
            tx.text = "";
            rt.Add(tx.GetComponent<RectTransform>());
            orgPos.Add(tx.GetComponent<RectTransform>().anchoredPosition);
        }
    }

    void Update()
    {
    }

    public void AddMsg(string msg, Color color)
    {
        int i;
        for (i = 0; i < orgPos.Count; i++)
        {
            if (rt[i].anchoredPosition == orgPos[i])
                break;
        }
        if (i >= orgPos.Count) return; // TODO 超过容量等待而不是return 富文本
        texts[i].text = msg;
        texts[i].color = color;
        StartCoroutine(moveText(texts[i], i));
    }

    IEnumerator moveText(Text tx, int index)
    {
        tx.color -= new Color(0, 0, 0, 0.02f);
        if (tx.color.a <= 0.2f)
        {
            rt[index].anchoredPosition = orgPos[index];
            texts[index].text = "";
            tx.color += new Color(0, 0, 0, 1);
            yield break;
        }
        rt[index].anchoredPosition += new Vector2(0, 1);
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(moveText(texts[index], index));
    }
}
