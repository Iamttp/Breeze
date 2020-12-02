using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        createMapObj(Camera.main.ScreenToWorldPoint(rectTransform.position));
        rectTransform.position = orgPos;
    }

    private static Vector2 getRectPos(Vector2 pos)
    {
        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        wpos.x = Mathf.Round(wpos.x - 0.5f) + 0.5f;
        wpos.y = Mathf.Round(wpos.y - 0.5f) + 0.5f;
        return Camera.main.WorldToScreenPoint(wpos);
    }

    void createMapObj(Vector2 worldPos)
    {
        if (MapManager.instance.plants.ContainsKey(worldPos)) return;
        var mapName = PackageManager.instance.objTable[name].mapName;
        if (mapName == null || mapName == "") return;

        var creator = Instantiate(MapManager.instance.mapObj, worldPos, Quaternion.identity);
        MapManager.instance.plants[worldPos] = creator;
        creator.GetComponent<MapObj>().typeName = mapName;
        PackageManager.instance.objTable[name].num--;
    }

    void Update()
    {
        if (!PackageManager.instance.objTable.ContainsKey(name))
        {
            GetComponent<Image>().color = new Color(0, 0, 0, 0); // 透明
            GetComponentInChildren<Text>().enabled = false;
            return;
        }

        GetComponent<Image>().sprite = PackageManager.instance.objTable[name].sprite;

        var num = PackageManager.instance.objTable[name].num;
        gameObject.SetActive(num > 0);
        GetComponentInChildren<Text>().enabled = true;
        GetComponentInChildren<Text>().text = num.ToString();
    }
}