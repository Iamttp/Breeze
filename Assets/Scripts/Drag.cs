using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
        createPlant(Camera.main.ScreenToWorldPoint(rectTransform.position), name);
        rectTransform.position = orgPos;
    }

    private static Vector2 getRectPos(Vector2 pos)
    {
        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        wpos.x = Mathf.Round(wpos.x - 0.5f) + 0.5f;
        wpos.y = Mathf.Round(wpos.y - 0.5f) + 0.5f;
        return Camera.main.WorldToScreenPoint(wpos);
    }

    void createPlant(Vector2 worldPos, string typeName)
    {
        if (PackageManager.instance.plants.ContainsKey(worldPos)) return;
        var creator = Instantiate(PackageManager.instance.plant, worldPos, Quaternion.identity);
        PackageManager.instance.plants[worldPos] = creator;
        creator.GetComponent<SpriteRenderer>().sprite = GetComponent<Image>().sprite;
        
        creator.GetComponent<Plant>().typeName = typeName;
        PackageManager.instance.objNum[typeName]--;
    }
}