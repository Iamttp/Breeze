using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;

    private RectTransform rectTransform;
    private Vector2 orgPos;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orgPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.red;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, getRectPos(eventData.position), eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(rectTransform.position);
        Instantiate(Manager.instance.prefabs[index], worldPos, Quaternion.identity);
        rectTransform.position = orgPos;
    }

    private static Vector2 getRectPos(Vector2 pos)
    {
        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        wpos.x = Mathf.Round(wpos.x - 0.5f) + 0.5f;
        wpos.y = Mathf.Round(wpos.y - 0.5f) + 0.5f;
        return Camera.main.WorldToScreenPoint(wpos);
    }
}